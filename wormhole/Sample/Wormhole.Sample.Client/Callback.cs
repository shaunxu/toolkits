using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wormhole.Sample.Contract;

namespace Wormhole.Sample.Client
{
    public class Callback : ICallback
    {
        public string Echo(string message)
        {
            Console.WriteLine("Callback.Echo({0}) invoking...", message);
            var value = string.Format("{0} - {1}", message, DateTime.Now.ToLongTimeString());
            Console.WriteLine("Callback.Echo({0}) = {1}", message, value);
            return value;
        }
    }
}
