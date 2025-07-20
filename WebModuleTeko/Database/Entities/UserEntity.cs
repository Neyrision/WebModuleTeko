namespace WebModuleTeko.Database.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string TfaKey { get; set; } = null!;
}