using System;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = @"E:\Git\GAIA\Clustering\bin\x64\Release\Clustering.dll";
            var pe = new PeFile(file);
            

            Console.WriteLine(pe.MetaDataStreamTablesHeader);
            
            foreach (var tableDefinition in pe.MetaDataStreamTablesHeader.TableDefinitions)
            {
                Console.WriteLine(tableDefinition);
            }

            Console.WriteLine(pe.MetaDataStreamTablesHeader.MetaDataTables.ModuleTable);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}