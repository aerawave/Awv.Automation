using Awv.Automation.SocialMedia.Interface.PostStatuses;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Tweets.Statuses
{
    public class TweetPartialSuccess : TweetStatus, IPostPartialSuccess
    {
        public bool Posted { get; set; } = false;
        public TweetPartialSuccess(ITweet tweet, TwitterStatus status)
            : base(tweet, status) { }

        public TweetPartialSuccess(ITweet tweet)
            : base(tweet) { }

        public override string ToString()
            => $"{nameof(TweetPartialSuccess)}";
    }
}
