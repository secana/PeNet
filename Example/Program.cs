using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"c:\windows\sysnative\", "*.dll").ToList();
            files.AddRange(Directory.GetFiles(@"c:\windows\system32\", "*.dll"));
            foreach(var f in files)
            {
                try
                {
                    var peheader = new PeNet.PeFile(f);
                    if (peheader.PKCS7 != null)
                        peheader.GetCrlUrlList().Urls.ForEach(x => Console.WriteLine(x));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
        }
    }
}
