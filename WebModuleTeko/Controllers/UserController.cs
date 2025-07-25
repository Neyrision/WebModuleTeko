﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Database;

namespace WebModuleTeko.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly WmtContext _wmtContext;

    public UserController(ILogger<UserController> logger, WmtContext context)
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