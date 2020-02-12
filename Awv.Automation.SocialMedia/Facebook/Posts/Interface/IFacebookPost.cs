using Awv.Automation.SocialMedia.Interface.Posts;
using Awv.Automation.SocialMedia.Interface.PostStatuses;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Interface
{
    public interface IFacebookPost :
        IPostWithCaption<IFacebookClient, IPostStatus>
    {
    }
}
