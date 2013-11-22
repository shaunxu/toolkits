using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole
{
    public class CustomizableDuplexChannelFactory<TContract, TContext> : DuplexChannelFactory<TContract>, ICustomizableChannelFactory<TContract>
        where TContract : class
        where TContext : CustomizableChannelContext
    {
        private InstanceContext _callbackInstance;
        protected CustomizableChannel<TContract> Proxy { get; set; }

        public CustomizableDuplexChannelFactory(TContext context, InstanceContext callbackInstance, Binding binding, string remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
            _callbackInstance = callbackInstance;
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableDuplexChannelFactory(TContext context, InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
            _callbackInstance = callbackInstance;
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableDuplexChannelFactory(TContext context, InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
            _callbackInstance = callbackInstance;
            Proxy = GetCustomizableChannel(context);
        }

        public CustomizableDuplexChannelFactory(TContext context, InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
            _callbackInstance = callbackInstance;
            Proxy = GetCustomizableChannel(context);
        }

        protected virtual CustomizableChannel<TContract> GetCustomizableChannel(TContext context)
        {
            return new CustomizableChannel<TContract>(this, context);
        }

        public TContract GetBaseChannel()
        {
            var channel = base.CreateChannel(_callbackInstance, Endpoint.Address, null);
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
