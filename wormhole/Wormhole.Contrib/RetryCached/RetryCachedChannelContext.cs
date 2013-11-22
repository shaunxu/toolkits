using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib.RetryCached
{
    public class RetryCachedChannelContext : CustomizableChannelContext
    {
        public int RetryCount { get; set; }

        public TimeSpan RetryInterval { get; set; }

        public bool EnableCache { get; set; }

        public RetryCachedChannelContext(int retryCount, TimeSpan retryInterval, bool enableCache)
        {
            RetryCount = retryCount;
            RetryInterval = retryInterval;
            EnableCache = enableCache;
        }
    }
}
