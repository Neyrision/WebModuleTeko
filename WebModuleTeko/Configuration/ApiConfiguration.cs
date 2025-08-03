namespace WebModuleTeko.Configuration;

public class ApiConfiguration
{
    public string AllowedOrigin { get; set; } = null!;
    public string KeycloakApiUrl { get; set; } = null!;
    public string TokenAuthority { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}