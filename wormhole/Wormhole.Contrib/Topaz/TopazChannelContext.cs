using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib.Topaz
{
    public class TopazChannelContext : CustomizableChannelContext
    {
        public RetryPolicy Policy { get; set; }

        public TopazChannelContext(RetryPolicy policy)
        {
            Policy = policy;
        }

        public TopazChannelContext(RetryStrategy strategy)
            : this(new RetryPolicy(new WcfInvokeErrorDetectionStrategy(), strategy))
        {
        }

        public class WcfInvokeErrorDetectionStrategy : ITransientErrorDetectionStrategy
        {
            public bool IsTransient(Exception ex)
            {
                return ex.IsOrHasInnerException<CommunicationException>() != null;
            }
        }
    }
}
