namespace WebModuleTeko.Database.Entities.Base;

public class EditableEntity : BaseEntity
{
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
}