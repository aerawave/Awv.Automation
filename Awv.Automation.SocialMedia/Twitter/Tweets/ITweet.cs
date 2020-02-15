using Awv.Automation.SocialMedia.Interface.Posts;
using Awv.Automation.SocialMedia.Interface.PostStatuses;

namespace Awv.Automation.SocialMedia.Twitter.Tweets
{
    public interface ITweet :
        IPostWithCaption<ITwitterClient, IPostStatus>,
        IPostWithImages<ITwitterClient, IPostStatus>
    {
    }
}
