namespace WebModuleTeko.Models;

public class AuthenticatedUserModel
{

    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Token { get; set; } = null!;

}