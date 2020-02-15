using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Requests;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using System.Threading.Tasks;
using TweetSharp;

namespace Awv.Automation.SocialMedia.Twitter
{
    public class TwitterClient : ITwitterClient
    {
        internal TwitterService Service { get; private set; }
        public TwitterClient(string apiKey, string apiSecret, string apiAccessToken, string apiAccessTokenSecret)
        {
            Service = new TwitterService(apiKey, apiSecret);
            Service.AuthenticateWith(apiAccessToken, apiAccessTokenSecret);
        }

        public IPostStatus Send(ITweet post) => SendAsync(post).Result;

        public async Task<IPostStatus> SendAsync(ITweet tweet) => await new SendTweetRequest(this, tweet).SendAsync();
    }
}
