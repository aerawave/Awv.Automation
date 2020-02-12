using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Facebook
{
    public interface IFacebookClient : ISocialMediaClient
    {
        string GetTarget();
        IPostStatus Send(IFacebookPost post);
        Task<IPostStatus> SendAsync(IFacebookPost post);
    }
}
