using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rngf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("Usage: rngf [file size in mb] [file name]");
                return;
            }

            var sizeInMB = int.Parse(args[0]);
            var filename = args[1];

            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            using (var fs = File.OpenWrite(filename))
            {
                for (var i = 0; i < sizeInMB; i++)
                {
                    var buffer = new byte[1024 * 1024];
                    rng.GetBytes(buffer);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            Console.WriteLine(string.Format("Output '{0}' ({1}MB).", filename, sizeInMB));
        }
    }
}
