using WebModuleTeko.Database.Entities.Base;

namespace WebModuleTeko.Database.Entities;

public class ForumEntryEntity : EditableEntity
{
    public string Title { get; set; }
    
    public UserEntity User { get; set; }
    public ICollection<ForumPostEntity> Posts;
}