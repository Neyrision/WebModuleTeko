using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly WmtContext _wmtContext;

    public PostController(ILogger<PostController> logger, WmtContext context)
    {
        _logger = logger;
        _wmtContext = context;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<string>> GetTest()
    {
        var test = await _wmtContext.Posts.ToListAsync();

        return Ok("");
    }
}