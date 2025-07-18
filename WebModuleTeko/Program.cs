using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebModuleTeko.Configuration;
using WebModuleTeko.Database;
using WebModuleTeko.Extensions;


var builder = WebApplication.CreateBuilder(args);

var apiConfiguration = builder.Configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>()!;

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = apiConfiguration.TokenAuthority;
    options.Audience = apiConfiguration.TokenAudience;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRateLimiter();

// Add db.
builder.Services.AddDbContext<WmtContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(WmtContext))));

// Swagger Setup.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseRateLimiter();
app.MapControllers();

// Authentication
app.UseAuthentication();

// Use Swagger
app.UseSwagger();
app.UseOpenApi();

// Cors
app.UseCors(options => options
    .WithOrigins(apiConfiguration.AllowedOrigin
        .Split(',')
        .Select(origin => origin.Trim())
        .ToArray())
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());


if (!app.Environment.IsEnvironment("NSwag"))
{
    app.EnsureDatabaseMigrated();
}

app.Run();
