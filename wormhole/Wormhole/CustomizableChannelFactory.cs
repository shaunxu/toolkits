using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Net.Security;

namespace Wormhole
{
    public class CustomizableChannelFactory<TContract, TContext> : ChannelFactory<TContract>, ICustomizableChannelFactory<TContract>
        where TContract : class
        where TContext : CustomizableChannelContext
    {
        protected CustomizableChannel<TContract> Proxy { get; set; }

        public CustomizableChannelFactory(TContext context, Binding binding, string remoteAddress)
            : base(binding, remoteAddress)
        {
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableChannelFactory(TContext context, Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableChannelFactory(TContext context, string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableChannelFactory(TContext context, ServiceEndpoint endpoint)
            : base(endpoint)
        {
            Proxy = GetCustomizableChannel(context);
        }

        protected virtual CustomizableChannel<TContract> GetCustomizableChannel(TContext context) 
        {
            return new CustomizableChannel<TContract>(this, context);
        }

        public TContract GetBaseChannel()
        {
            var channel = base.CreateChannel(Endpoint.Address, null);
            // force the channel to be opened so that it can perform request through the same channel in parallel
            // ref: http://blogs.msdn.com/b/wenlong/archive/2007/10/26/best-practice-always-open-wcf-client-proxy-explicitly-when-it-is-shared.aspx
            ((IClientChannel)channel).Open();
            return channel;
        }

        public override TContract CreateChannel(EndpointAddress address, Uri via)
        {
            return Proxy.GetTransparentProxy() as TContract;
        }
    }
}
