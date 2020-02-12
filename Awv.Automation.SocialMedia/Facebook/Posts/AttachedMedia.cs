using Awv.Automation.SocialMedia.Facebook.Posts.Statuses;
using Newtonsoft.Json;

namespace Awv.Automation.SocialMedia.Facebook.Posts
{
    public struct AttachedMedia
    {
        [JsonProperty("media_fbid")]
        public string Id { get; set; }

        public AttachedMedia(string id)
            : this ()
        {
            Id = id;
        }

        public AttachedMedia(PostData data)
            : this (data.Id)
        {

        }
    }
}
