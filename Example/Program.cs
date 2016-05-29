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
            var files = Directory.GetFiles(@"C:\windows\system32", "*.dll");

            foreach (var file in files)
            {
                Console.WriteLine(file);
                var pe = new PeNet.PeFile(file);
                Console.WriteLine(pe.ToString());

                foreach (var exception in pe.Exceptions)
                {
                    Console.WriteLine(exception.Message);
                }
                Console.WriteLine("\n\n");
            }


            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
