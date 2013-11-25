using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib.Topaz
{
    public class TopazDuplexChannelFactory<TContract> : CustomizableDuplexChannelFactory<TContract, TopazChannelContext> where TContract : class
    {
        public TopazDuplexChannelFactory(TopazChannelContext context, InstanceContext callbackInstance, Binding binding, string remoteAddress)
            : base(context, callbackInstance, binding, remoteAddress)
        {
        }

        public TopazDuplexChannelFactory(TopazChannelContext context, InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(context, callbackInstance, binding, remoteAddress)
        {
        }

        public TopazDuplexChannelFactory(TopazChannelContext context, InstanceContext callbackInstance, string endpointConfigurationName)
            : base(context, callbackInstance, endpointConfigurationName)
        {
        }

        public TopazDuplexChannelFactory(TopazChannelContext context, InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(context, callbackInstance, endpoint)
        {
        }

        protected override CustomizableChannel<TContract> GetCustomizableChannel(TopazChannelContext context)
        {
            return new TopazChannel<TContract>(this, context);
        }
    }
}
