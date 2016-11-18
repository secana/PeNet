using System;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = @"filepath";
            var pe = new PeFile(file);

            var comHeader = pe.ImageComDescriptor;
            Console.WriteLine(comHeader);
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}