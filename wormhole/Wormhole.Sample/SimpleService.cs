using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample
{
    public class SimpleService : ISimpleService
    {
        public int Add(int x, int y)
        {
            Console.WriteLine("SimpleService.Add({0}, {1}) invoking...", x, y);
            var result = x + y;
            Console.WriteLine("SimpleService.Add({0}, {1}) = {2}", x, y, result);
            return result;
        }
    }
}
