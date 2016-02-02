using System;
using System.IO;
using System.Linq;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dlls = Directory.GetFiles(@"c:\windows\system32\", "*.dll");
            var exes = Directory.GetFiles(@"c:\windows\system32\", "*.exe");

            foreach (var exe in exes)
            {
                try
                {
                    var pe = new PeNet.PeFile(exe);
                    var ch = PeNet.Utility.ResolveCharacteristics(pe.ImageNtHeaders.FileHeader.Characteristics);
                    if(ch != "EXE")
                        Console.WriteLine("Not an exe: " + exe);
                }
                catch(Exception)
                { }
            }

            foreach (var dll in dlls)
            {
                try
                {
                    var pe = new PeNet.PeFile(dll);
                    var ch = PeNet.Utility.ResolveCharacteristics(pe.ImageNtHeaders.FileHeader.Characteristics);
                    if (ch != "DLL")
                        Console.WriteLine("Not an dll: " + dll);
                }
                catch (Exception)
                { }
            }
            Console.WriteLine("FINISH");
            Console.ReadKey(true);
        }
    }
}
