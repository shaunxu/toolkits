using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole
{
    internal class SmartDuplexChannelFactory<TContract> : DuplexChannelFactory<TContract>, ISmartChannelFactory<TContract> where TContract : class
    {
        private SmartChannel<TContract> _proxy;
        private InstanceContext _callbackInstance;

        public SmartDuplexChannelFactory(InstanceContext callbackInstance, ServiceEndpoint endpoint)
            : base(callbackInstance, endpoint)
        {
            _callbackInstance = callbackInstance;
            _proxy = new SmartChannel<TContract>(this, 3, TimeSpan.FromSeconds(5));
        }

        public TContract GetBaseChannel()
        {
            return base.CreateChannel(_callbackInstance, Endpoint.Address, null);
        }

        public override TContract CreateChannel(EndpointAddress address, Uri via)
        {
            return _proxy.GetTransparentProxy() as TContract;
        }
    }
}
