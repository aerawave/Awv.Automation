using Awv.Automation.SocialMedia.Interface.PostStatuses;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Tweets.Statuses
{
    public class MediaPosted : IPostSuccess
    {
        public TwitterUploadedMedia Media { get; set; }

        public MediaPosted(TwitterUploadedMedia media)
        {
            Media = media;
        }

        public override string ToString()
            => $"{nameof(MediaPosted)}";
    }
}
