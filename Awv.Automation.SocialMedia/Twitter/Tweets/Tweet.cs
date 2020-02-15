using Awv.Automation.SocialMedia.Interface.PostStatuses;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia.Twitter.Tweets
{
    public class Tweet : ITweet
    {
        public string Caption { get; set; }
        private Dictionary<string, Image<Rgba32>> Images { get; set; } = new Dictionary<string, Image<Rgba32>>();

        public void AddImage(string key, Image<Rgba32> image) => Images.Add(key, image);
        public string GetCaption() => Caption;
        public IDictionary<string, Image<Rgba32>> GetImages() => Images;

        public Func<IEnumerable<IPostStatus>, bool> GetPartialSuccessFunction()
            => OnPartialSuccess.Throw;
    }
}
