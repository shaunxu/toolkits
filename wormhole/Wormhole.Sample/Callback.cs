using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample
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
