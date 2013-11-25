using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Wormhole
{
    public class CustomizableChannel<TContract> : RealProxy where TContract : class
    {
        private ICustomizableChannelFactory<TContract> _factory;
        private CustomizableChannelContext _context;

        protected ICustomizableChannelFactory<TContract> ChannelFactory
        {
            get
            {
                return _factory;
            }
        }

        protected TContext GetCustomizableChannelContext<TContext>() where TContext : CustomizableChannelContext
        {
            return _context as TContext;
        }

        public CustomizableChannel(ICustomizableChannelFactory<TContract> factory, CustomizableChannelContext context)
            : base(typeof(TContract))
        {
            _factory = factory;
            _context = context;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var channel = ChannelFactory.GetBaseChannel();
            return Invoke(msg, channel);
        }

        protected IMessage Invoke(IMessage msg, TContract channel)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodBase = methodCall.MethodBase;
            var result = methodBase.Invoke(channel, methodCall.Args);
            return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
        }
    }

    //public class CustomizableChannel<TContract> : RealProxy where TContract : class
    //{
    //    private int _retryCount;
    //    private TimeSpan _retryInterval;
    //    private ICustomizableChannelFactory<TContract> _factory;

    //    private TContract _channel;
    //    private object _channelLock;

    //    public CustomizableChannel(ICustomizableChannelFactory<TContract> factory, int retryCount, TimeSpan retryInterval)
    //        : base(typeof(TContract))
    //    {
    //        _factory = factory;
    //        _retryCount = retryCount;
    //        _retryInterval = retryInterval;

    //        _channel = null;
    //        _channelLock = new object();
    //    }

    //    private TContract GetChannel(bool refresh)
    //    {
    //        if (refresh)
    //        {
    //            lock (_channelLock)
    //            {
    //                _channel = _factory.GetBaseChannel();
    //            }
    //        }
    //        else
    //        {
    //            if (_channel == null)
    //            {
    //                lock (_channelLock)
    //                {
    //                    if (_channel == null)
    //                    {
    //                        _channel = _factory.GetBaseChannel();
    //                    }
    //                }
    //            }
    //        }

    //        return _channel;
    //    }

    //    public override IMessage Invoke(IMessage msg)
    //    {
    //        var methodCall = msg as IMethodCallMessage;
    //        var methodBase = methodCall.MethodBase;

    //        var retries = _retryCount;
    //        var refresh = false;
    //        while (true)
    //        {
    //            retries--;
    //            var channel = GetChannel(refresh);
    //            try
    //            {
    //                var result = methodBase.Invoke(channel, methodCall.Args);
    //                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
    //            }
    //            catch (Exception ex)
    //            {
    //                if (ex.IsOrHasInnerException<FaultException>() != null)
    //                {
    //                    throw;
    //                }
    //                else if (ex.IsOrHasInnerException<CommunicationException>() != null)
    //                {
    //                    if (retries > 0)
    //                    {
    //                        Console.WriteLine("retry {0}: retry", retries);
    //                        refresh = true;
    //                        Thread.Sleep(_retryInterval);
    //                    }
    //                    else
    //                    {
    //                        Console.WriteLine("retry {0}: failed", retries);
    //                        throw;
    //                    }
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //        }
    //    }
    //}
}
