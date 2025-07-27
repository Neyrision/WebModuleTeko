using WebModuleTeko.Models.Authentication;

namespace WebModuleTeko.Models.Forum;

public class ForumEntryModel
{
    public string Title { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public UserModel User { get; set; }
    public ICollection<ForumPostModel> _posts;
}