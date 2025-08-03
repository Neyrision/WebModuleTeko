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

    public AuthenticationController(
        ILogger<UserController> logger, 
        WmtContext context,
        KeycloakService keycloakService)
    {
        _logger = logger;
        _wmtContext = context;
        _keycloakService = keycloakService;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<AuthenticatedUserModel>> GetToken(string username, string password, string tfaCode)
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

    [HttpPost("[action]")]
    public async Task<TotpModel> RegisterNewUser(RegisterUserModel model)
    {
        var secret = TotpHelper.NewSecret();
        var totpUri = TotpHelper.NewTotpUri("WebModule", secret, model.Email);

        _wmtContext.Add(new UserEntity()
        {
            Email = model.Email,
            Username = model.Username,
            TfaKey = secret
        });

        await _wmtContext.SaveChangesAsync();

        await _keycloakService.CreateNewUser(model);

        return new TotpModel
        {
            Uri = totpUri,
            SetupCode = secret,
            Image = $"data:image/png;base64,{TotpHelper.Base64QrCodeFromTotpUri(totpUri)}"
        };
    }
}