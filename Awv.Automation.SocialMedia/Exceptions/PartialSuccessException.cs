using Awv.Automation.SocialMedia.Interface.PostStatuses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Awv.Automation.SocialMedia.Exceptions
{
    public class PartialSuccessException : Exception
    {
        public IPostStatus[] Statuses { get; set; }

        public PartialSuccessException(IEnumerable<IPostStatus> statuses)
        {
            Statuses = statuses.ToArray();
        }
    }
}
