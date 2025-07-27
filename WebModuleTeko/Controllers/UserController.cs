using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;
using WebModuleTeko.Services;
using WebModuleTeko.Services.Authentication;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly WmtContext _wmtContext;

    public UserController(
        ILogger<UserController> logger, 
        WmtContext context,
        KeycloakService keycloakService)
    {
        _logger = logger;
        _wmtContext = context;
    }
}