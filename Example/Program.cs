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
            Console.WriteLine(pe.ImageResourceDirectory);
            Console.ReadKey(true);
        }
    }
}
