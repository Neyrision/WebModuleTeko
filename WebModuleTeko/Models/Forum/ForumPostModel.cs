using WebModuleTeko.Models.Authentication;

namespace WebModuleTeko.Models.Forum;

public class ForumPostModel
{
    public string Content { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public UserModel User { get; set; }
    public ForumEntryModel _posts;
}