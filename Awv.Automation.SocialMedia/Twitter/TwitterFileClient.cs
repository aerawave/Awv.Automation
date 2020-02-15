using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Requests;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Twitter
{
    public class TwitterFileClient : ITwitterClient
    {
        public string TargetDirectory { get; set; }

        public TwitterFileClient(string targetDirectory = ".")
        {
            TargetDirectory = targetDirectory;
        }

        public IPostStatus Send(ITweet tweet)
            => SendAsync(tweet).Result;

        public async Task<IPostStatus> SendAsync(ITweet tweet) => await new WriteTweetCommand(this, tweet).SendAsync();
    }
}
