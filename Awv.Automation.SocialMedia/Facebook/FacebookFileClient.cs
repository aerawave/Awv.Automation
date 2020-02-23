using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Facebook.Requests;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System.IO;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Facebook
{
    public class FacebookFileClient : IFacebookClient
    {
        public string Output { get; set; }
        public string Target { get; set; }

        public FacebookFileClient(string target, string output = ".")
        {
            Output = Path.GetFullPath(output);
            Target = target;
        }

        public async Task<IPostStatus> SendAsync(IFacebookPost post) => await new WritePostCommand(this, post).SendAsync();
        public IPostStatus Send(IFacebookPost post) => SendAsync(post).Result;

        public string GetTarget() => Target;
    }
}
