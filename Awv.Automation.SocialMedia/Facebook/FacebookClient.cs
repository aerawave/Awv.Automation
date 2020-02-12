using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Facebook.Requests;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;
using System.Threading.Tasks;

namespace Awv.Automation.SocialMedia.Facebook
{
    public class FacebookClient : IFacebookClient
    {
        public string AccessToken { get; set; }
        public string Target { get; set; }
        public Uri BaseAddress { get; set; } = new Uri("https://graph.facebook.com/v5.0/");

        public FacebookClient(string accessToken, string target)
        {
            AccessToken = accessToken;
            Target = target;
        }

        public async Task<IPostStatus> SendAsync(IFacebookPost post) => await new SendPostRequest(this, post).SendAsync();
        public IPostStatus Send(IFacebookPost post) => SendAsync(post).Result;

        public Uri GetUri(string target, string endpoint, string args = null)
            => new Uri(BaseAddress, $"{target}/{endpoint}?access_token={AccessToken}&{args}");

        public string GetTarget() => Target;
    }
}
