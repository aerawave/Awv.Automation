using Awv.Automation.SocialMedia.Facebook.Posts.Interface;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;

namespace Awv.Automation.SocialMedia.Facebook.Posts.Statuses
{
    public class PostFailure : PostStatus, IPostFailure
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public PostFailure(IFacebookPost post, string message)
            : base(post, default(PostData))
        {
            Message = message;
        }

        public PostFailure(IFacebookPost post, Exception exception)
            : this(post, exception.Message)
        {
            Exception = exception;
        }


        public override string ToString()
            => $"{nameof(PostFailure)}: {Message}";
    }
}
