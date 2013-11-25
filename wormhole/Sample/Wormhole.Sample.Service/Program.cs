using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wormhole.Sample.Contract;

namespace Wormhole.Sample.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(SampleService)))
            {
                host.AddServiceEndpoint(typeof(ISimpleService), Consts.Binding, Consts.SimpleServiceAddress);
                host.AddServiceEndpoint(typeof(IDuplexService), Consts.Binding, Consts.DuplexServiceAddress);

                host.Opened += (sender, e) =>
                    {
                        host.Description.Endpoints.All((ep) =>
                            {
                                Console.WriteLine("[{0}] opened at {1}", ep.Contract.ContractType.FullName, ep.ListenUri.AbsoluteUri);
                                return true;
                            });
                    };
                host.Open();

                Console.WriteLine("press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
