using System;
using System.IO;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"C:\Users\Stefa\Downloads\Issues");
            foreach (var file in files)
            {
                Console.WriteLine($"{file}, valid: {PeFile.IsValidPEFile(file)}");
            }
            Console.ReadKey();
        }
    }
}