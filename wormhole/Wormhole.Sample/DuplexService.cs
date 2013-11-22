using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DuplexService : IDuplexService
    {
        public string Add(int x, int y)
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
