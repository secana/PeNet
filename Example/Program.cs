using System;
using System.IO;
using PeNet;

namespace Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dotNetFile = @"D:\BitbucketRepos\GAIA\DataEntities\bin\x64\Release\DataEntities.dll";
            var peFile = new PeFile(dotNetFile);

            Console.WriteLine(peFile.MetaDataStreamTablesHeader);
            var x = peFile.MetaDataStreamTablesHeader.MetaDataTables;
            Console.WriteLine(x.ModuleTable);
        }
    }
}