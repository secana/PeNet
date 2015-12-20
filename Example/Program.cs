using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
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
