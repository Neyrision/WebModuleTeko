using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebModuleTeko.Database;
using WebModuleTeko.Database.Entities;
using WebModuleTeko.Services.Forum;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class ForumEntryController(ILogger<ForumEntryController> logger, ForumEntryService forumEntryService) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        return Ok("");
    }

    [Route("{userName}")]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetByUserName(string userName)
    {
        return Ok("");
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] ForumPostEntity model)
    {
        return Ok("");
    }
}