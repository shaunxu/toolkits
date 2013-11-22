using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new NetTcpBinding();
            var address = "net.tcp://localhost:32768";

            // simple service
            var simpleHost = new ServiceHost(typeof(SimpleService));
            simpleHost.AddServiceEndpoint(typeof(ISimpleService), binding, address);
            simpleHost.Opened += (sender, e) =>
                {
                    Console.WriteLine("service opened at {0}", simpleHost.Description.Endpoints[0].ListenUri.AbsoluteUri);
                };
            simpleHost.Open();

            var context = new CustomizableChannelContext();

            // simple client
            var simpleFactory = new CustomizableChannelFactory<ISimpleService, CustomizableChannelContext>(context, binding, address);
            var simpleProxy = simpleFactory.CreateChannel();
            using (simpleProxy as IDisposable)
            {
                Console.WriteLine("x = ?");
                var x = int.Parse(Console.ReadLine());
                Console.WriteLine("y = ?");
                var y = int.Parse(Console.ReadLine());
                var result = simpleProxy.Add(x, y);
                Console.WriteLine("{0} + {1} = {2}", x, y, result);
            }

            // terminate
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
            simpleFactory.Close();
            simpleHost.Close();

            // simple service
            var duplexHost = new ServiceHost(typeof(DuplexService));
            duplexHost.AddServiceEndpoint(typeof(IDuplexService), binding, address);
            duplexHost.Opened += (sender, e) =>
            {
                Console.WriteLine("service opened at {0}", duplexHost.Description.Endpoints[0].ListenUri.AbsoluteUri);
            };
            duplexHost.Open();

            // simple client
            var callback = new InstanceContext(new Callback());
            var duplexFactory = new CustomizableDuplexChannelFactory<IDuplexService, CustomizableChannelContext>(context, callback, binding, address);
            var duplexProxy = duplexFactory.CreateChannel();
            using (duplexProxy as IDisposable)
            {
                Console.WriteLine("x = ?");
                var x = int.Parse(Console.ReadLine());
                Console.WriteLine("y = ?");
                var y = int.Parse(Console.ReadLine());
                var result = duplexProxy.Add(x, y);
                Console.WriteLine("{0} + {1} = {2}", x, y, result);
            }

            // terminate
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
            duplexFactory.Close();
            duplexHost.Close();
        }
    }
}
