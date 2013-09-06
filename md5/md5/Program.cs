using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md5
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Usage: md5 [file name]");
                return;
            }

            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            using (var fs = File.OpenRead(args[0]))
            {
                var hash = md5.ComputeHash(fs);
                var value = Convert.ToBase64String(hash);
                Console.WriteLine(string.Format("{0} > {1}", args[0], value));
            }
        }
    }
}
