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
            var peHeader = new PEHeader(@"C:\Windows\System32\kernel32.dll");

            File.WriteAllText("peheader.txt", peHeader.ToString());
           
            Console.ReadLine();
        }
    }
}
