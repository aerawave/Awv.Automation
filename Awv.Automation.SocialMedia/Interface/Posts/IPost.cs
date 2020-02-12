using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia.Interface.Posts
{
    public interface IPost
    {
        Func<IEnumerable<IPostStatus>, bool> GetPartialSuccessFunction();
    }
    public interface IPost<TClientType, TSendReturnType> : IPost where TClientType : ISocialMediaClient
    {
    }
}
