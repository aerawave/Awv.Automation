using Awv.Automation.SocialMedia.Exceptions;
using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;
using System.Collections.Generic;

namespace Awv.Automation.SocialMedia
{
    public static class OnPartialSuccess
    {
        public static readonly Func<IEnumerable<IPostStatus>, bool> PostAnyway = statuses => true;
        public static readonly Func<IEnumerable<IPostStatus>, bool> Throw = statuses => throw new PartialSuccessException(statuses);
    }
}
