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
            var peHeader = new PeNet.PeFile(@"C:\Windows\System32\calc.exe");

            Console.WriteLine("Import Hash:{0}", peHeader.GetImpHash());
            Console.ReadKey();
        }
    }
}
