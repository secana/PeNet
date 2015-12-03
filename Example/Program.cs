using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\Local Virus Copies\57\fc\57fc8a235e096d3612ff714514fb01c27f5c60f845955f2fb64bb2382e7f33c6";
            var file2 = @"c:\local virus copies\eb5a1e13aa108843b68b7a14b4bb53dd521e12a12925e16f76e64e5d8b98e115";

            //var pe = new PeNet.PeFile(file);
            if (PeNet.PeFile.IsValidPEFile(file2))
            {
                Console.WriteLine("VALID");
            }
            else
            {
                Console.WriteLine("INVALID");
            }
            Console.ReadKey();
        }
    }
}
