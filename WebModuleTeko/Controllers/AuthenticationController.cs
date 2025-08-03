using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;
using WebModuleTeko.Database.Entities;
using WebModuleTeko.Helpers;
using WebModuleTeko.Models;
using WebModuleTeko.Models.Authentication;
using WebModuleTeko.Services.Authentication;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly WmtContext _wmtContext;
    private readonly KeycloakService _keycloakService;
    private readonly LoginLimiterService _loginLimiterService;
    private readonly IValidator<RegisterUserModel> _registerUserValidator;

    public AuthenticationController(
        ILogger<UserController> logger, 
        WmtContext context,
        KeycloakService keycloakService,
        LoginLimiterService loginLimiterService,
        IValidator<RegisterUserModel> registerUserValidator)
    {
        _logger = logger;
        _wmtContext = context;
        _keycloakService = keycloakService;
        _loginLimiterService = loginLimiterService;
        _registerUserValidator = registerUserValidator;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticatedUserModel>> GetToken(string username, string password, string tfaCode)
    {
        _logger.LogInformation($"{nameof(AuthenticationController)}: User {username} is trying to log in.");

        await _keycloakService.EnsureReady();

        if (_loginLimiterService.ShouldLoginBeRestricted(username))
        {
            return Forbid();
        }

        try
        {
            var token = await _keycloakService.LoginUser(username, password);
            var user = await _wmtContext.Users.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null || !TotpHelper.ValidateSecret(user.TfaKey, tfaCode))
            {
                return Forbid();
            }

            return Ok(new AuthenticatedUserModel
            {
                Email = user.Email,
                UserId = user.Id,
                Username = user.Username,
                Token = token,
            });
        }
        catch(Exception ex) 
        {
            _loginLimiterService.RecordLoginFailure(username);
            _logger.LogError($"{nameof(AuthenticationController)}: Login for user {username} failed {ex.Message}");
            throw;
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<TotpModel>> RegisterNewUser(RegisterUserModel model)
    {
        _logger.LogInformation($"{nameof(AuthenticationController)}: Registering new user {model.Username}");

        var validationResult = await _registerUserValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return BadRequest("Invalid model");
        }

        try
        {
            await _keycloakService.EnsureReady();

            await _keycloakService.CreateNewUser(model);

            _logger.LogInformation($"{nameof(AuthenticationController)}: Keycloak user created for {model.Username}");

            var secret = TotpHelper.NewSecret();
            var totpUri = TotpHelper.NewTotpUri("WebModule", secret, model.Email);

            _wmtContext.Add(new UserEntity()
            {
                Email = model.Email,
                Username = model.Username,
                TfaKey = secret
            });

            await _wmtContext.SaveChangesAsync();

            _logger.LogInformation($"{nameof(AuthenticationController)}: Successfully registered user {model.Username}");

            return Ok(new TotpModel
            {
                Uri = totpUri,
                SetupCode = secret,
                Image = $"data:image/png;base64,{TotpHelper.Base64QrCodeFromTotpUri(totpUri)}"
            });
        }
        catch(Exception ex)
        {
            _logger.LogError($"{nameof(AuthenticationController)}: Registering user {model.Username} failed {ex.Message}");
            throw;
        }
    }
}