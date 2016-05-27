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
            //var pe = new PeNet.PeFile(@"C:\Users\Stefa\Desktop\Virus\343f010f7ba4a5a3f2701da7d56d10fca711e9d5993897c73eb91b1b1b4213f7");
            var pe = new PeNet.PeFile(@"C:\Users\Stefa\Desktop\Virus\c44bdb8648a29be8cceaa2a6c0b13c7a279ca67a762f8c4896291b374dec14d5");
            //var pe = new PeNet.PeFile(@"C:\windows\system32\kernel32.dll");
            pe.ImageRelocationDirectory?.ToList().ForEach(r => Console.WriteLine(r.TypeOffsets.Length));

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
