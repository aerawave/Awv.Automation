using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;

namespace Awv.Automation.SocialMedia.Twitter.Tweets.Statuses
{
    public class TweetFailure : TweetStatus, IPostFailure
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public TweetFailure(ITweet tweet, string message)
            : base(tweet)
        {
            Message = message;
        }

        public TweetFailure(ITweet tweet, Exception exception)
            : this(tweet, exception.Message)
        {
            Exception = exception;
        }


        public override string ToString()
            => $"{nameof(TweetFailure)}: {Message}";
    }
}
