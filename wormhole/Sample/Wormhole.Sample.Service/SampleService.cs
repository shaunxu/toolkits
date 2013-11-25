using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wormhole.Sample.Contract;

namespace Wormhole.Sample.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SampleService : ISimpleService, IDuplexService
    {
        public int Add(int x, int y)
        {
            Console.WriteLine("SimpleService.Add({0}, {1}) invoking...", x, y);
            var result = x + y;
            Console.WriteLine("SimpleService.Add({0}, {1}) = {2}", x, y, result);
            return result;
        }

        public string AddAndEcho(int x, int y)
        {
            Console.WriteLine("DuplexService.Add({0}, {1}) invoking...", x, y);
            var result = x + y;
            Console.WriteLine("DuplexService.Add({0}, {1}) = {2}", x, y, result);

            Console.WriteLine("ICallback.Echo({0}) invoking...", result);
            var callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            var value = callback.Echo(result.ToString());
            Console.WriteLine("ICallback.Echo({0}) = {1}", result, value);

            return value;
        }
    }
}
