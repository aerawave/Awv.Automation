using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Requests;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using System.IO;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Twitter
{
    public class TwitterFileClient : ITwitterClient
    {
        public string Output { get; set; }

        public TwitterFileClient(string output = ".")
        {
            Output = Path.GetFullPath(output);
        }

        public IPostStatus Send(ITweet tweet)
            => SendAsync(tweet).Result;

        public async Task<IPostStatus> SendAsync(ITweet tweet) => await new WriteTweetCommand(this, tweet).SendAsync();
    }
}
