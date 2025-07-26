using Microsoft.AspNetCore.Mvc;
using WebModuleTeko.Database;
using WebModuleTeko.Helpers;
using WebModuleTeko.Models;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly WmtContext _wmtContext;

    public AuthenticationController(ILogger<UserController> logger, WmtContext context)
    {
        _logger = logger;
        _wmtContext = context;
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Login()
    {
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<TotpModel> Register(RegisterUserModel model)
    {
        var secret = TotpHelper.NewSecret();
        var totpUri = TotpHelper.NewTotpUri("WebModule", secret, model.Email);

        //var totp = new Totp(secret);
        //totp.ComputeTotp();

        //_wmtContext.Add(new UserEntity()
        //{
        //    Email = model.Email,
        //    Username = model.Username,
        //    TfaKey = tfaKey
        //});

        //await _wmtContext.SaveChangesAsync();

        return new TotpModel
        {
            Uri = totpUri,
            SetupCode = secret,
            Image = $"data:image/png;base64,{TotpHelper.Base64QrCodeFromTotpUri(totpUri)}"
        };
    }
}