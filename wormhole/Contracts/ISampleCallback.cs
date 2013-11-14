using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISampleCallback
    {
        [OperationContract]
        string ConvertToString(int x, int y, int result);
    }
}
