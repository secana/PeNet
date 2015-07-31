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
            if(args.Count() != 2)
            {
                Console.WriteLine("Usage: Program InputFile OutputFile");
                return;
            }

            var peHeader = new PeNet.PeFile(args[0]);

            if(!peHeader.Is64Bit)
            {
                Console.WriteLine("Not a 64 bit binary.");
                return;
            }
            
            using(var file = new StreamWriter(args[1]))
            {
                foreach (var ef in peHeader.ExportedFunctions)
                {
                    var rf = peHeader.RuntimeFunctions.Where(r => r.FunctionStart == ef.Address).FirstOrDefault();
                    if (rf == null)
                        continue;

                    var uw = peHeader.GetUnwindInfo(rf);
                    if (uw == null)
                        continue;

                    file.WriteLine(ef.ToString());
                    file.WriteLine(rf.ToString());
                    file.WriteLine(uw.ToString());
                    file.WriteLine("------------------------");
                }
            }
        }
    }
}
