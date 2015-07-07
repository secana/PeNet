using PeNet2;
using PeParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var peHeader = new PeFile(@"C:\Users\stefan.hausotte@gdata.de\Documents\tmp\kernel32.dll");

            Console.WriteLine(peHeader.ImageDosHeader.ToString());
            Console.WriteLine(peHeader.ImageNtHeaders.ToString());
        }
    }
}
