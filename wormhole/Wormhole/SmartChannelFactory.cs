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
    internal class SmartChannelFactory<TContract> : ChannelFactory<TContract>, ISmartChannelFactory<TContract> where TContract : class
    {
        private SmartChannel<TContract> _proxy;

        public SmartChannelFactory(ServiceEndpoint endpoint)
            : base(endpoint)
        {
            _proxy = new SmartChannel<TContract>(this, 3, TimeSpan.FromSeconds(5));
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
            return _proxy.GetTransparentProxy() as TContract;
        }
    }
}
