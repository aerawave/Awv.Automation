using Awv.Automation.SocialMedia.Interface.PostStatuses;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter.Tweets.Statuses
{
    public class TweetSuccess : TweetStatus, IPostSuccess
    {
        public TweetSuccess(ITweet tweet, TwitterStatus status)
            : base(tweet, status) { }

        public override string ToString()
            => $"{nameof(TweetSuccess)}";
    }
}
