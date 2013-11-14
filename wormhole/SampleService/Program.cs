using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleService
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(SamepleService));
            host.Opened += (sender, e) =>
            {
                Console.WriteLine("service opened at {0}", host.Description.Endpoints[0].ListenUri.AbsoluteUri);
            };
            host.Open();

            while(true)
            {
                Thread.Sleep(15 * 1000);
            }

            //Console.WriteLine("press any key to exit");
            //Console.ReadKey();
            //host.Close();
        }
    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SamepleService : ISamepleService
    {
        public int Add(int x, int y)
        {
            Console.WriteLine("[{0}] {1} + {2} = ?", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), x, y);

            var result = x + y;
            Thread.Sleep(2000);

            Console.WriteLine("[{0}] {1} + {2} = {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), x, y, result);
            return result;
        }

        public string AddToString(int x, int y)
        {
            var result = x + y;

            Console.WriteLine("press any key to invoke callback...");
            Console.ReadKey();

            var retries = 3;

            while (true)
            {
                retries--;
                try
                {
                    var callback = OperationContext.Current.GetCallbackChannel<ISampleCallback>();
                    var value = callback.ConvertToString(x, y, result);
                    return value;
                }
                catch
                {
                    if (retries > 0)
                    {
                        Console.WriteLine("retry {0}: retry", retries);
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                    }
                    else
                    {
                        Console.WriteLine("retry {0}: failed", retries);
                        throw;
                    }
                }
            }
        }
    }
}
