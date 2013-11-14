using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole
{
    public interface IEndpointProvider
    {
        ServiceEndpoint GetEndpoint<T>(string hostName, string serviceTypeName) where T : class;

        Type EndpointBehaviorType { get; }

        IEndpointBehavior GetBehavior();
    }

    public class StubEndpointProvider : IEndpointProvider
    {
        public ServiceEndpoint GetEndpoint<T>(string hostName, string serviceTypeName) where T : class
        {
            var factory = new ChannelFactory<T>("BasicHttpBinding_ISamepleService");
            return factory.Endpoint;
        }

        public Type EndpointBehaviorType
        {
            get 
            {
                return null; 
            }
        }

        public IEndpointBehavior GetBehavior()
        {
            throw new NotImplementedException();
        }
    }

}
