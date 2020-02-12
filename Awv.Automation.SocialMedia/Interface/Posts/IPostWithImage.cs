using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Awv.Automation.SocialMedia.Interface.Posts
{
    public interface IPostWithImage<TClientType, TSendReturnType> : IPost<TClientType, TSendReturnType> where TClientType : ISocialMediaClient
    {
        public string GetImageKey();
        public Image<Rgba32> GetImage();
    }
}
