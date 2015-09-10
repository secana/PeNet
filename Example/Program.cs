using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"E:\tmp\x64");

            foreach(var f in files)
            {
                if (Path.GetExtension(f) == ".dll" || Path.GetExtension(f) == ".exe")
                {
                    var peHeader = new PeNet.PeFile(f);
                    if(peHeader.WinCertificate != null)
                    {
                        if(peHeader.PKCS7 == null)
                        {
                            Console.WriteLine($"{f} has a WIN_CERTIFICATE but no PKCS7.");
                        }
                        else
                        {
                            Console.WriteLine($"{f} signed by {peHeader.PKCS7.Subject}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{f} has no WIN_CERTIFICATE.");
                    }
                }
            }
        }
    }
}
