using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Automation.SocialMedia.Facebook.Posts
{
    public class FacebookPhotoPost : FacebookPost, IPhotoPost
    {
        public string ImageKey { get; set; }
        public Image<Rgba32> Image { get; set; }

        public FacebookPhotoPost()
            : base()
        {

        }

        public FacebookPhotoPost(string caption)
            : base(caption)
        {

        }
        public FacebookPhotoPost(string caption, string key, Image<Rgba32> image)
            : this(caption)
        {
            ImageKey = key;
            Image = image;
        }

        public FacebookPhotoPost(string key, Image<Rgba32> image)
            : this(null, key, image)
        {
        }


        public void SetImage(string key, Image<Rgba32> image)
        {
            ImageKey = key;
            Image = image;
        }

        public bool IsPublished() => Published;
        public string GetImageKey() => ImageKey;
        public Image<Rgba32> GetImage() => Image;
    }
}
