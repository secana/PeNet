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
            var peHeader = new PeNet2.PeFile(@"C:\Windows\System32\oleaut32.dll");
            var peOld = new PeParser.PEHeader(@"C:\Windows\System32\oleaut32.dll");

            Console.WriteLine("Old Hash:{0}", peOld.GetImpHash());
            Console.WriteLine("New Hash:{0}", peHeader.GetImpHash());
            Console.ReadKey();
        }
    }
}
