using System;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = @"filePath";
            var pe = new PeFile(file);

            Console.WriteLine(pe.MetaDataHdr);
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}