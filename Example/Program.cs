using System;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = @"C:\Users\stefan.hausotte\Downloads\FilesWithErrors\These all have the same issue\ds16gt.dLL";
            var pe = new PeFile(file);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}