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

        private TFactory GetChannelFactory<TFactory, TContract>(Func<ServiceEndpoint, TFactory> factoryInitializer)
            where TFactory : ChannelFactory<TContract>
            where TContract : class
        {
            var key = typeof(TContract).AssemblyQualifiedName;
            var factory = _factories.GetOrAdd(key, (k) =>
            {
                var endpoint = _endpointProvider.GetEndpoint<TContract>();
                var value = factoryInitializer.Invoke(endpoint);
                return value;
            });
            return factory as TFactory;
        }

        #region Simple ChannelFactory, Channel, Invoke

        public ChannelFactory<T> GetSimpleChannelFactory<T>() where T : class
        {
            var context = new CustomizableChannelContext();
            return GetChannelFactory<ChannelFactory<T>, T>((ep) => new CustomizableChannelFactory<T, CustomizableChannelContext>(context, ep));
        }

        public T GetSimpleChannel<T>() where T : class
        {
            var factory = GetSimpleChannelFactory<T>();
            return factory.CreateChannel();
        }

        public void Invoke<T>(Action<T> action) where T : class
        {
            var channel = GetSimpleChannel<T>();
            action.Invoke(channel);
        }

        public TResult Invoke<TContract, TResult>(Func<TContract, TResult> func) where TContract : class
        {
            var channel = GetSimpleChannel<TContract>();
            return func.Invoke(channel);
        }

        public Task InvokeAsync<T>(Action<T> action) where T : class
        {
            return Task.Run(() => Invoke<T>(action));
        }

        public Task<TResult> InvokeAsync<TContract, TResult>(Func<TContract, TResult> func) where TContract : class
        {
            return Task.Run(() => Invoke<TContract, TResult>(func));
        }

        #endregion

        #region Duplex ChannelFactory, Channel, Invoke

        public DuplexChannelFactory<T> GetDuplexChannelFactory<T>(InstanceContext instanceContext) where T : class
        {
            var context = new CustomizableChannelContext();
            return GetChannelFactory<DuplexChannelFactory<T>, T>((ep) => new CustomizableDuplexChannelFactory<T, CustomizableChannelContext>(context, instanceContext, ep));
        }

        public T GetDuplexChannel<T>(InstanceContext instanceContext) where T : class
        {
            var factory = GetDuplexChannelFactory<T>(instanceContext);
            return factory.CreateChannel();
        }

        public void Invoke<T>(InstanceContext instanceContext, Action<T> action) where T : class
        {
            var channel = GetDuplexChannel<T>(instanceContext);
            action.Invoke(channel);
        }

        public TResult Invoke<TContract, TResult>(InstanceContext instanceContext, Func<TContract, TResult> func) where TContract : class
        {
            var channel = GetDuplexChannel<TContract>(instanceContext);
            return func.Invoke(channel);
        }

        public Task InvokeAsync<T>(InstanceContext instanceContext, Action<T> action) where T : class
        {
            return Task.Run(() => Invoke<T>(instanceContext, action));
        }

        public Task<TResult> InvokeAsync<TContract, TResult>(InstanceContext instanceContext, Func<TContract, TResult> func) where TContract : class
        {
            return Task.Run(() => Invoke<TContract, TResult>(instanceContext, func));
        }

        #endregion
    }
}
