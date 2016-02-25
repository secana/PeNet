using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PeNet.PeFile(@"c:\windows\system32\calc.exe");
            Console.WriteLine($"SHA-256: {pe.SHA256}");
            Console.WriteLine($"ImpHash: {pe.ImpHash}");
            Console.WriteLine($"SHA-1: {pe.SHA1}");
            Console.WriteLine($"MD5: {pe.MD5}");
            Console.WriteLine($"File Size {pe.FileSize}");

            foreach (var resdir in pe.ImageResourceDirectory)
            {
                Console.WriteLine(resdir.ToString());
                foreach (var entry in resdir.DirectoryEntries)
                {
                    Console.WriteLine(entry.ToString());
                }
                Console.WriteLine("------------------------------------------");
            }
            Console.ReadKey(true);
        }
    }
}
