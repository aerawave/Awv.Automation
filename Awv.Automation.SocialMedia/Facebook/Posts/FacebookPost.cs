using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia.Facebook.Posts
{
    public class FacebookPost : IFacebookPost
    {
        public bool Published { get; set; }
        public string Caption { get; set; }

        public FacebookPost()
        {
            Published = true;
        }

        public FacebookPost(string caption)
            : this()
        {
            Caption = caption;
        }


        public string GetCaption() => Caption;

        public Func<IEnumerable<IPostStatus>, bool> GetPartialSuccessFunction()
            => OnPartialSuccess.Throw;
    }
}
