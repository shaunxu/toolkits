using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib.RetryCached
{
    public class RetryCachedChannelFactory<TContract> : CustomizableChannelFactory<TContract, RetryCachedChannelContext> where TContract : class
    {
        public RetryCachedChannelFactory(RetryCachedChannelContext context, Binding binding, string remoteAddress)
            : base(context, binding, remoteAddress)
        {
        }

        public RetryCachedChannelFactory(RetryCachedChannelContext context, Binding binding, EndpointAddress remoteAddress)
            : base(context, binding, remoteAddress)
        {
        }

        public RetryCachedChannelFactory(RetryCachedChannelContext context, string endpointConfigurationName)
            : base(context, endpointConfigurationName)
        {
        }

        public RetryCachedChannelFactory(RetryCachedChannelContext context, ServiceEndpoint endpoint)
            : base(context, endpoint)
        {
        }

        protected override CustomizableChannel<TContract> GetCustomizableChannel(RetryCachedChannelContext context)
        {
            return new RetryCachedChannel<TContract>(this, context);
        }
    }
}
