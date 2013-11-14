using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(
        //CallbackContract = typeof(ISampleCallback)
    )]
    public interface ISamepleService
    {
        [OperationContract]
        int Add(int x, int y);

        [OperationContract]
        string AddToString(int x, int y);
    }
}
