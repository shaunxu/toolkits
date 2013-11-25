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
    public class TopazChannelFactory<TContract> : CustomizableChannelFactory<TContract, TopazChannelContext> where TContract : class
    {
        public TopazChannelFactory(TopazChannelContext context, Binding binding, string remoteAddress)
            : base(context, binding, remoteAddress)
        {
        }

        public TopazChannelFactory(TopazChannelContext context, Binding binding, EndpointAddress remoteAddress)
            : base(context, binding, remoteAddress)
        {
        }

        public TopazChannelFactory(TopazChannelContext context, string endpointConfigurationName)
            : base(context, endpointConfigurationName)
        {
        }

        public TopazChannelFactory(TopazChannelContext context, ServiceEndpoint endpoint)
            : base(context, endpoint)
        {
        }

        protected override CustomizableChannel<TContract> GetCustomizableChannel(TopazChannelContext context)
        {
            return new TopazChannel<TContract>(this, context);
        }
    }
}
