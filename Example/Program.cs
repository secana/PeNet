using System;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = @"E:\Git\GAIA\Clustering\bin\x64\Release\Clustering.dll";
            var pe = new PeFile(file);

            var list = pe.MetaDataStreamUS;

            foreach (var s in list)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}