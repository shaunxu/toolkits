using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Sample.Contract
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IDuplexService
    {
        [OperationContract]
        string AddAndEcho(int x, int y);
    }

    public interface ICallback
    {
        [OperationContract]
        string Echo(string message);
    }
}
