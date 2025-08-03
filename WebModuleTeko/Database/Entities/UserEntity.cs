using WebModuleTeko.Database.Entities.Base;

namespace WebModuleTeko.Database.Entities;

public class UserEntity : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string TfaKey { get; set; } = null!;
    
    public ICollection<ForumPostEntity> Posts { get; set; }
    public ICollection<ForumEntryEntity> ForumEntries { get; set; }
}