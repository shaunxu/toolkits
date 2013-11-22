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
    public class RetryCachedDuplexChannelFactory<TContract> : CustomizableDuplexChannelFactory<TContract, RetryCachedChannelContext> where TContract : class
    {
        public RetryCachedDuplexChannelFactory(RetryCachedChannelContext context, InstanceContext callbackInstance, Binding binding, string remoteAddress)
            : base(context, callbackInstance, binding, remoteAddress)
        {
        }

        public RetryCachedDuplexChannelFactory(RetryCachedChannelContext context, InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(context, callbackInstance, binding, remoteAddress)
        {
        }

        public RetryCachedDuplexChannelFactory(RetryCachedChannelContext context, InstanceContext callbackInstance, string endpointConfigurationName)
            : base(context, callbackInstance, endpointConfigurationName)
        {
        }

        public RetryCachedDuplexChannelFactory(RetryCachedChannelContext context, InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(context, callbackInstance, endpoint)
        {
        }

        protected override CustomizableChannel<TContract> GetCustomizableChannel(RetryCachedChannelContext context)
        {
            return new RetryCachedChannel<TContract>(this, context);
        }
    }
}
