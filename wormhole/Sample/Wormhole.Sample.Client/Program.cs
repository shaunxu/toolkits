using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wormhole.Contrib.RetryCached;
using Wormhole.Sample.Contract;

namespace Wormhole.Sample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new RetryCachedChannelContext(3, TimeSpan.FromSeconds(3), true);

            Console.WriteLine("simple ...");
            var simpleFactory = new RetryCachedChannelFactory<ISimpleService>(context, Consts.Binding, Consts.SimpleServiceAddress);
            var simpleProxy = simpleFactory.CreateChannel();
            using (simpleProxy as IDisposable)
            {
                while (true)
                {
                    Console.WriteLine("invoke? (y|n)");
                    if (string.Compare(Console.ReadLine(), "y", true) == 0)
                    {
                        Console.WriteLine("x = ?");
                        var x = int.Parse(Console.ReadLine());
                        Console.WriteLine("y = ?");
                        var y = int.Parse(Console.ReadLine());
                        var result = simpleProxy.Add(x, y);
                        Console.WriteLine("simpleProxy.Add({0}, {1}) = {2}", x, y, result);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("duplex ...");
            var instanceContext = new InstanceContext(new Callback());
            var duplexFactory = new RetryCachedDuplexChannelFactory<IDuplexService>(context, instanceContext, Consts.Binding, Consts.DuplexServiceAddress);
            var duplexProxy = duplexFactory.CreateChannel();
            using (duplexProxy as IDisposable)
            {
                while (true)
                {
                    Console.WriteLine("invoke? (y|n)");
                    if (string.Compare(Console.ReadLine(), "y", true) == 0)
                    {
                        Console.WriteLine("x = ?");
                        var x = int.Parse(Console.ReadLine());
                        Console.WriteLine("y = ?");
                        var y = int.Parse(Console.ReadLine());
                        var result = duplexProxy.AddAndEcho(x, y);
                        Console.WriteLine("duplexProxy.Add({0}, {1}) = {2}", x, y, result);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("finished");
            Console.ReadKey();
        }
    }
}
