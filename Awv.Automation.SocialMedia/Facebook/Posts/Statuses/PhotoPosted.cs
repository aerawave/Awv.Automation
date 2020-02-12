using Awv.Automation.SocialMedia.Facebook.Posts.Interface;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Statuses
{
    public class PhotoPosted : PostSuccess
    {
        public IPhotoPost PhotoPost => Post as IPhotoPost;
        public PhotoPosted(IPhotoPost post, PostData data) : base(post, data)
        {
        }

        public override string ToString()
            => $"{nameof(PhotoPosted)}";
    }
}
