using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PeNet.PeFile(@"c:\local virus copies\19bf12dba089c11e491a5d995d94febfd72856bec01e133c4a47e680e9e23af7 ");

            var bytes = pe.PKCS7.RawData;
            var urls = new PeNet.PeFile.CrlUrlList(bytes).Urls;

            foreach (var url in urls)
            {
                Console.WriteLine(url);
            }
            Console.ReadKey(true);
        }
    }
}
