using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2df40cb1cc8b3ad9688921ae3227762006947c2e28bb430fe8fdd52f298c26fe
//  4d0a82277068a34de34737857e8e9511e520aa9a4ada2fe54f58db47cba461c6



          var file = new PeNet.PeFile(@"C:\Local Virus Copies\7c1dba942ca01313b5f4fbcf085d92ce65d1c0c69587fa64207c040179bf23fc");
            Console.WriteLine($"IsValidPeFile: {file.IsValidPeFile.ToString()}");
            Console.WriteLine($"HasValidExceptionDir.: {file.HasValidExceptionDir.ToString()}");
            Console.WriteLine($"HasValidExportDir.: {file.HasValidExportDir.ToString()}");
            Console.WriteLine($"HasValidImportDir.: {file.HasValidImportDir.ToString()}");
            Console.WriteLine($"HasValidResourceDir.: {file.HasValidResourceDir.ToString()}");
            Console.WriteLine($"HasValidSecurityDir.: {file.HasValidSecurityDir.ToString()}");
            Console.ReadKey();
        }
    }
}
