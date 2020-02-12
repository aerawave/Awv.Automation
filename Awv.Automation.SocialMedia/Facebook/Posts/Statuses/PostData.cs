using Newtonsoft.Json;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Statuses
{
    public struct PostData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("post_id")]
        public string PostId { get; set; }

        public PostData(string id, string postId)
            : this()
        {
            Id = id;
            PostId = postId;
        }
    }
}
