using Awv.Automation.SocialMedia.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using Awv.Automation.SocialMedia.Twitter.Tweets;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Twitter
{
    public interface ITwitterClient : ISocialMediaClient
    {
        IPostStatus Send(ITweet post);
        Task<IPostStatus> SendAsync(ITweet tweet);
    }
}
