using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wormhole.Contrib
{
    internal static class Extensions
    {
        public static T IsOrHasInnerException<T>(this Exception ex) where T : Exception
        {
            var ex1 = ex as T;
            if (ex1 == null)
            {
                if (ex.InnerException == null)
                {
                    return null;
                }
                else
                {
                    return ex.InnerException.IsOrHasInnerException<T>();
                }
            }
            else
            {
                return ex1;
            }
        }
    }
}
