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
            var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");

            pe.ImageRelocationDirectory?.ToList().ForEach(r => Console.WriteLine(r.TypeOffsets.Length));

            Console.ReadKey();
        }
    }
}
