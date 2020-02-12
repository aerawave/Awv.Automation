using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Statuses
{
    public class PostPartialSuccess : PostStatus, IPostPartialSuccess
    {
        public PostPartialSuccess(IFacebookPost post, PostData? data)
            : base(post, data) { }

        public override string ToString()
            => $"{nameof(PostPartialSuccess)}";
    }
}
