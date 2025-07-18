namespace WebModuleTeko.Configuration;

public class ApiConfiguration
{
    public string AllowedOrigin { get; set; }
    public string TokenAuthority { get; set; }
    public string TokenAudience { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}