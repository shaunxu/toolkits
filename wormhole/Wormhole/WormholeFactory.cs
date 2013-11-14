using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Wormhole
{
    public class WormholeFactory
    {
        private static WormholeFactory _instance;

        public static WormholeFactory Current
        {
            get
            {
                return _instance;
            }
        }

        public static void Configure(IEndpointProvider endpointProvider)
        {
            _instance = new WormholeFactory(endpointProvider);
        }

        static WormholeFactory()
        {
            Configure(new StubEndpointProvider());
        }

        private ConcurrentDictionary<string, object> _factories;
        private IEndpointProvider _endpointProvider;

        protected WormholeFactory(IEndpointProvider endpointProvider)
        {
            _factories = new ConcurrentDictionary<string, object>();
            _endpointProvider = endpointProvider;
        }

        private TFactory GetChannelFactory<TFactory, TContract>(Func<ServiceEndpoint, TFactory> factoryInitializer, string hostName, string serviceTypeName)
            where TFactory : ChannelFactory<TContract>
            where TContract : class
        {
            var key = string.Format("{0}|{1}|{2}|{3}", typeof(TContract).AssemblyQualifiedName, hostName, serviceTypeName, 0);
            var factory = _factories.GetOrAdd(key, (k) =>
            {
                var endpoint = _endpointProvider.GetEndpoint<TContract>(hostName, serviceTypeName);
                var value = factoryInitializer.Invoke(endpoint);
                if (_endpointProvider.EndpointBehaviorType != null && !value.Endpoint.EndpointBehaviors.Contains(_endpointProvider.EndpointBehaviorType))
                {
                    value.Endpoint.EndpointBehaviors.Add(_endpointProvider.GetBehavior());
                }
                return value;
            });
            return factory as TFactory;
        }

        #region Simple ChannelFactory, Channel, Invoke

        public ChannelFactory<T> GetSimpleChannelFactory<T>(string hostName = "", string serviceTypeName = "") where T : class
        {
            return GetChannelFactory<ChannelFactory<T>, T>((ep) => new SmartChannelFactory<T>(ep), hostName, serviceTypeName);
        }

        public T GetSimpleChannel<T>(string hostName = "", string serviceTypeName = "") where T : class
        {
            var factory = GetSimpleChannelFactory<T>(hostName, serviceTypeName);
            return factory.CreateChannel();
        }

        public void Invoke<T>(Action<T> action, string hostName = "", string serviceTypeName = "") where T : class
        {
            var channel = GetSimpleChannel<T>(hostName, serviceTypeName);
            action.Invoke(channel);
        }

        public TResult Invoke<TContract, TResult>(Func<TContract, TResult> func, string hostName = "", string serviceTypeName = "") where TContract : class
        {
            var channel = GetSimpleChannel<TContract>(hostName, serviceTypeName);
            return func.Invoke(channel);
        }

        public Task InvokeAsync<T>(Action<T> action, string hostName = "", string serviceTypeName = "") where T : class
        {
            return Task.Run(() => Invoke<T>(action, hostName, serviceTypeName));
        }

        public Task<TResult> InvokeAsync<TContract, TResult>(Func<TContract, TResult> func, string hostName = "", string serviceTypeName = "") where TContract : class
        {
            return Task.Run(() => Invoke<TContract, TResult>(func, hostName, serviceTypeName));
        }

        #endregion

        #region Duplex ChannelFactory, Channel, Invoke

        public DuplexChannelFactory<T> GetDuplexChannelFactory<T>(InstanceContext instanceContext, string hostName = "", string serviceTypeName = "") where T : class
        {
            return GetChannelFactory<DuplexChannelFactory<T>, T>((ep) => new SmartDuplexChannelFactory<T>(instanceContext, ep), hostName, serviceTypeName);
        }

        public T GetDuplexChannel<T>(InstanceContext instanceContext, string hostName = "", string serviceTypeName = "") where T : class
        {
            var factory = GetDuplexChannelFactory<T>(instanceContext, hostName, serviceTypeName);
            return factory.CreateChannel();
        }

        public void Invoke<T>(InstanceContext instanceContext, Action<T> action, string hostName = "", string serviceTypeName = "") where T : class
        {
            var channel = GetDuplexChannel<T>(instanceContext, hostName, serviceTypeName);
            action.Invoke(channel);
        }

        public TResult Invoke<TContract, TResult>(InstanceContext instanceContext, Func<TContract, TResult> func, string hostName = "", string serviceTypeName = "") where TContract : class
        {
            var channel = GetDuplexChannel<TContract>(instanceContext, hostName, serviceTypeName);
            return func.Invoke(channel);
        }

        public Task InvokeAsync<T>(InstanceContext instanceContext, Action<T> action, string hostName = "", string serviceTypeName = "") where T : class
        {
            return Task.Run(() => Invoke<T>(instanceContext, action, hostName, serviceTypeName));
        }

        public Task<TResult> InvokeAsync<TContract, TResult>(InstanceContext instanceContext, Func<TContract, TResult> func, string hostName = "", string serviceTypeName = "") where TContract : class
        {
            return Task.Run(() => Invoke<TContract, TResult>(instanceContext, func, hostName, serviceTypeName));
        }

        #endregion
    }
}
