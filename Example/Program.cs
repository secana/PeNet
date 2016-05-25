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
            var pe = new PeNet.PeFile(@"c:\Local Virus Copies\c44bdb8648a29be8cceaa2a6c0b13c7a279ca67a762f8c4896291b374dec14d5");

            pe.ImageRelocationDirectory?.ToList().ForEach(r => Console.WriteLine(r.TypeOffsets.Length));

            Console.ReadKey();
        }
    }
}
