using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebModuleTeko.Configuration;
using WebModuleTeko.Database;
using WebModuleTeko.Extensions;
using WebModuleTeko.Models.Authentication;
using WebModuleTeko.Models.Authentication.Validators;
using WebModuleTeko.Services.Authentication;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection(nameof(ApiConfiguration)));
var apiConfiguration = builder.Configuration.GetSection(nameof(ApiConfiguration)).Get<ApiConfiguration>()!;

// Authentication
builder.Services.AddHttpClient<KeycloakService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.Authority = apiConfiguration.TokenAuthority;
    options.Audience = apiConfiguration.ClientId;
    options.RequireHttpsMetadata = false; // Idk man??

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = apiConfiguration.ClientId,
        ValidateIssuer = true,
        ValidIssuer = apiConfiguration.TokenAuthority,
        ValidateLifetime = true
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRateLimiter();
builder.Services.AddSingleton<LoginLimiterService>();
builder.Services.AddScoped<IValidator<RegisterUserModel>, RegisterUserModelValidator>();

// Add db.
builder.Services.AddDbContext<WmtContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(WmtContext))));

// Swagger Setup.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();

builder.Services.AddSpaStaticFiles(config =>
{
    config.RootPath = "wwwroot";
});

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
app.UseAuthorization();

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

if(!app.Environment.IsDevelopment())
{
    Console.WriteLine("Starting WebServer");

    app.UseStaticFiles();
    app.UseSpaStaticFiles();
    app.UseSpa(builder =>
    {
        //builder.Options.SourcePath = "ClientApp";
        //builder.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        //builder.UseAngularCliServer(npmScript: "start");
    });
}

if (!app.Environment.IsEnvironment("NSwag"))
{
    app.EnsureDatabaseMigrated();
}

app.Run();
