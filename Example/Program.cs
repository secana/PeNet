using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\Windows\System32\kernel32.dll";
            var pe = new PeNet.PeFile(file);
            Console.WriteLine(PeNet.Utility.IsSigned(file));
            Console.WriteLine(PeNet.Utility.IsValidCertChain(file, true));
            Console.WriteLine(pe.IsSigned);
            Console.WriteLine(pe.IsValidCertChain(true));
            Console.WriteLine(PeNet.Utility.IsSignatureValid(file));
            Console.ReadKey(true);
        }
    }
}
