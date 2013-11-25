using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample.Contract
{
    public class Consts
    {
        public static readonly string SimpleServiceAddress = "net.tcp://localhost:32768/SimpleService";
        public static readonly string DuplexServiceAddress = "net.tcp://localhost:32768/DuplexService";

        public static Binding Binding
        {
            get
            {
                return new NetTcpBinding();
            }
        }
    }
}
