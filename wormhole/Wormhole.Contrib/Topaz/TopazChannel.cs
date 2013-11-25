using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib.Topaz
{
    public class TopazChannel<TContract> : CustomizableChannel<TContract> where TContract : class
    {
        private RetryPolicy _policy;

        public TopazChannel(ICustomizableChannelFactory<TContract> factory, TopazChannelContext context)
            : base(factory, context)
        {
            _policy = context.Policy;
            _policy.Retrying += (sender, e) =>
                {
                    Console.WriteLine("Retry - Count: {0}, Delay: {1}, Exception: {2}", e.CurrentRetryCount, e.Delay, e.LastException.Message);
                };
        }

        public override IMessage Invoke(IMessage msg)
        {
            try
            {
                return _policy.ExecuteAction<IMessage>(() =>
                    {
                        return base.Invoke(msg);
                    });
            }
            catch
            {
                // all the retries failed
                throw;
            }
        }
    }
}
