using WebModuleTeko.Database.Entities.Base;

namespace WebModuleTeko.Database.Entities;

public class ForumPostEntity : EditableEntity
{
    public string Content { get; set; }
    
    public int UserId { get; set; }
    public UserEntity User { get; set; }
}