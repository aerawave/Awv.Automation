using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Statuses
{
    public class PostStatus : IPostStatus
    {
        public IFacebookPost Post { get; private set; }
        public PostData? Data { get; private set; }
        public PostStatus(IFacebookPost post, PostData? data)
        {
            Post = post;
            Data = data;
        }
    }
}
