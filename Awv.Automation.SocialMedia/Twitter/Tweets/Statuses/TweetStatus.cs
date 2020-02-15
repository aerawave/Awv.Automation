using Awv.Automation.SocialMedia.Interface.PostStatuses;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Tweets.Statuses
{
    public class TweetStatus : IPostStatus
    {
        public ITweet Tweet { get; private set; }
        public TwitterStatus TweetSharpStatus { get; private set; }

        public TweetStatus(ITweet tweet)
        {
            Tweet = tweet;
        }

        public TweetStatus(ITweet tweet, TwitterStatus status)
            : this(tweet)
        {
            TweetSharpStatus = status;
        }
    }
}
