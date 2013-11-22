using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample
{
    [ServiceContract]
    public interface ISimpleService
    {
        [OperationContract]
        int Add(int x, int y);
    }
}
