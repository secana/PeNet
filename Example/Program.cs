using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\Local Virus Copies\a0aa14a840157acf5109f9a6a3018ed895d56fc5690d71415a5fbccc3e6795b7";
            var pe = new PeNet.PeFile(file);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
