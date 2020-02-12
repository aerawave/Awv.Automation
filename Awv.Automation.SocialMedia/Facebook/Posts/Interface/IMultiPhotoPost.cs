using Awv.Automation.SocialMedia.Interface.Posts;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Interface
{
    public interface IMultiPhotoPost : IFacebookPost,
        IPostWithImages<IFacebookClient, IPostStatus>
    {
        public IEnumerable<IPhotoPost> GetChildPosts();
    }
}
