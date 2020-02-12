using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Automation.SocialMedia.Facebook.Posts
{
    public class FacebookMultiPhotoPost : FacebookPost, IMultiPhotoPost
    {
        public List<IPhotoPost> ChildPosts { get; set; } = new List<IPhotoPost>();

        public FacebookPhotoPost AddChildPost(string caption, string key, Image<Rgba32> image)
        {
            var post = new FacebookPhotoPost(caption, key, image);
            post.Published = false;
            ChildPosts.Add(post);
            return post;
        }

        public FacebookPhotoPost AddChildPost(string key, Image<Rgba32> image)
            => AddChildPost(null, key, image);

        public IEnumerable<IPhotoPost> GetChildPosts() => ChildPosts;

        public IDictionary<string, Image<Rgba32>> GetImages()
        {
            var entries = ChildPosts.Select(post =>
                    new KeyValuePair<string, Image<Rgba32>>(post.GetImageKey(), post.GetImage())
            );
            var dictionary = new Dictionary<string, Image<Rgba32>>(entries);
            return dictionary;
        }
    }
}
