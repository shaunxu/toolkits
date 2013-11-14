using Contracts;
using Wormhole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("press any key to start");
            Console.ReadKey();

            //var client = new Proxy.SamepleServiceClient();

            //var factory = new ChannelFactory<Proxy.ISamepleService>("BasicHttpBinding_ISamepleService");
            //var contract = new ContractDescription(typeof(ISamepleService).Name);
            //var binding = new NetTcpBinding();
            //var address = new EndpointAddress("net.tcp://localhost:19999/");
            //var endpoint = new ServiceEndpoint(contract, binding, address);

            //SmartChannelFactory<ISamepleService> factory = null;

            //var x = 0;
            //var y = 0;

            //while (true)
            //{
            //    Console.WriteLine("x = ?");
            //    x = int.Parse(Console.ReadLine());
            //    Console.WriteLine("y = ?");
            //    y = int.Parse(Console.ReadLine());

            //    if (x == 0 || y == 0)
            //        break;

            //    var client = factory.CreateChannel();
            //    var result = client.Add(x, y);

            //    Console.WriteLine(result);
            //}

            //var factory0 = new SmartChannelFactory<ISamepleService>("BasicHttpBinding_ISamepleService");
            //var channel0 = factory0.CreateChannel();
            //channel0.Add(0, 0);

            //var times = 500;
            //var sw = new Stopwatch();

            //var factory2 = new SmartChannelFactory<ISamepleService>("BasicHttpBinding_ISamepleService");
            //var channel = factory2.CreateChannel();
            //sw.Restart();
            //Run(null, channel, times);
            //sw.Stop();
            //Console.WriteLine("ChannelFactory: NULL; Channel: Cache; Times: {0}; Elapsed: {1}s", times, (sw.ElapsedMilliseconds / (double)1000).ToString("0.###"));

            //var factory1 = new SmartChannelFactory<ISamepleService>("BasicHttpBinding_ISamepleService");
            //sw.Restart();
            //Run(factory1, null, times);
            //sw.Stop();
            //Console.WriteLine("ChannelFactory: Cache; Channel: No Cache; Times: {0}; Elapsed: {1}s", times, (sw.ElapsedMilliseconds / (double)1000).ToString("0.###"));

            var wormhole_elapsed = RunMe(() => WormholeFactory.Current.Invoke<Contracts.ISamepleService, int>(proxy => proxy.Add(1, 1)), 10, true, false);
            Console.WriteLine("WOW!!!");
            Console.WriteLine("WormholeFactory.Current.Invoke, elapsed {0}s", wormhole_elapsed.ToString("0.000"));

            //var instanceContext = new InstanceContext(new SampleCallback());
            //var wormhole_elapsed = RunMe(() =>
            //    {
            //        var result = WormholeFactory.Current.Invoke<ISamepleService, string>(instanceContext, proxy => proxy.AddToString(1, 1));
            //        Console.WriteLine(result);
            //    }, 3, false, true);
            //Console.WriteLine("WOW!!!");
            //Console.WriteLine("WormholeFactory.Current.Invoke, elapsed {0}s", wormhole_elapsed.ToString("0.000"));

            Console.ReadKey();
        }

        static double RunMe(Action action, int times, bool parallel, bool pause)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (parallel)
            {
                Parallel.For(0, times, (i) =>
                {
                    action.Invoke();
                });
            }
            else
            {
                for (var i = 0; i < times; i++)
                {
                    action.Invoke();

                    Console.WriteLine("paused, press any key to resume.");
                    Console.ReadKey();
                }
            }

            sw.Stop();
            return sw.ElapsedMilliseconds / (double)1000;
        }

        //static void Run(ChannelFactory<Proxy.ISamepleService> factory, Proxy.ISamepleService channel, int times)
        //{
        //    Parallel.For(0, times, (j) =>
        //    {
        //        Proxy.ISamepleService __channel = null;
        //        if (channel == null)
        //        {
        //            __channel = factory.CreateChannel();
        //        }
        //        else
        //        {
        //            __channel = channel;
        //        }
        //        __channel.Add(1, 1);
        //    });

        //    //for (var i = 0; i < times; i++)
        //    //{
        //    //    Proxy.ISamepleService __channel = null;
        //    //    if (channel == null)
        //    //    {
        //    //        __channel = factory.CreateChannel();
        //    //    }
        //    //    else
        //    //    {
        //    //        __channel = channel;
        //    //    }
        //    //    __channel.Add(1, 1);
        //    //}
        //}

        public class SampleCallback : ISampleCallback
        {
            public string ConvertToString(int x, int y, int result)
            {
                var value = string.Format("[{0}] {1} + {2} = {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), x, y, result);
                return value;
            }
        }

    }
}
