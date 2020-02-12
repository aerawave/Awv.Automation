using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia.Interface.Posts
{
    public interface IPostWithImages<TClientType, TSendReturnType> : IPost<TClientType, TSendReturnType> where TClientType : ISocialMediaClient
    {
        IDictionary<string, Image<Rgba32>> GetImages();
    }
}
