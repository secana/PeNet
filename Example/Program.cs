using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2df40cb1cc8b3ad9688921ae3227762006947c2e28bb430fe8fdd52f298c26fe

            var file = new PeNet.PeFile(@"C:\Local Virus Copies\2df40cb1cc8b3ad9688921ae3227762006947c2e28bb430fe8fdd52f298c26fe");
            Console.ReadKey();
        }
    }
}
