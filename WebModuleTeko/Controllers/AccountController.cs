using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using WebModuleTeko.Database;

namespace WebModuleTeko.Controllers;

[ApiController]
[OpenApiTag("Account")]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly WmtContext _wmtContext;

    public AccountController(ILogger<AccountController> logger, WmtContext context)
    {
        _logger = logger;
        _wmtContext = context;
    }

    [HttpGet("[action]")]
    public async Task<string> GetAllAccounts()
    {
        var test = await _wmtContext.Posts.ToListAsync();

        return "";
    }
}