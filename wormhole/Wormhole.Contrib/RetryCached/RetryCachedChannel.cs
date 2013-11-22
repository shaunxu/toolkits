using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wormhole.Contrib.RetryCached
{
    public class RetryCachedChannel<TContract> : CustomizableChannel<TContract> where TContract : class
    {
        private TContract _channel;
        private object _channelLock;

        public RetryCachedChannel(ICustomizableChannelFactory<TContract> factory, RetryCachedChannelContext context)
            : base(factory, context)
        {
            _channel = null;
            _channelLock = new object();
        }

        private TContract GetChannel(bool refresh)
        {
            if (refresh || !GetCustomizableChannelContext<RetryCachedChannelContext>().EnableCache)
            {
                lock (_channelLock)
                {
                    _channel = ChannelFactory.GetBaseChannel();
                }
            }
            else
            {
                if (_channel == null)
                {
                    lock (_channelLock)
                    {
                        if (_channel == null)
                        {
                            _channel = ChannelFactory.GetBaseChannel();
                        }
                    }
                }
            }

            return _channel;
        }

        protected override IMessage OnInvoke(IMessage msg)
        {
            var interval = GetCustomizableChannelContext<RetryCachedChannelContext>().RetryInterval;

            var retries = GetCustomizableChannelContext<RetryCachedChannelContext>().RetryCount;
            var refresh = false;
            while (true)
            {
                retries--;
                var channel = GetChannel(refresh);
                try
                {
                    Console.WriteLine("channel {0} is working...", channel.GetHashCode());
                    return base.OnInvoke(msg);
                }
                catch (Exception ex)
                {
                    if (ex.IsOrHasInnerException<FaultException>() != null)
                    {
                        throw;
                    }
                    else if (ex.IsOrHasInnerException<CommunicationException>() != null)
                    {
                        if (retries > 0)
                        {
                            Console.WriteLine("retry {0}: retry", retries);
                            refresh = true;
                            Thread.Sleep(interval);
                        }
                        else
                        {
                            Console.WriteLine("retry {0}: failed", retries);
                            throw;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
