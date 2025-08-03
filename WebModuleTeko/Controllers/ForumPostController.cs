using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumPostController(ILogger<ForumPostController> logger, WmtContext context) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<ActionResult<string>> GetTest()
    {
        var test = await context.Posts.ToListAsync();

        return Ok("");
    }
}