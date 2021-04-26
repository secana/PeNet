using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace PeNet.Test.Editor
{
    public class ImportTest
    {
        [Fact]
        public void AddImport_RDataSection()
        {
            var peFile = new PeFile(@"Binaries/chrome_elf.dll");
            peFile.AddImport("gdi32.dll", "StartPage");

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
                i.DLL == "gdi32.dll"
                && i.Name == "StartPage"));
        }

        [Fact]
        public void AddImport_ToNotExistingImpDesc_64BitExecutable()
        {
            var peFile = new PeFile(@"Binaries/add-import.exe");
            peFile.AddImport("gdi32.dll", "StartPage");

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
                i.DLL == "gdi32.dll"
                && i.Name == "StartPage"));
        }

        [Fact]
        public void AddImport_ToNotExistingImpDescMultiple_64BitExecutable()
        {
            var peFile = new PeFile(@"Binaries/add-import.exe");
            var ai1 = new AdditionalImport("gdi32.dll", new List<string> { "StartPage", "EndDoc" });
            var ai2 = new AdditionalImport("api-ms-win-core-errorhandling-l1-1-0.dll", new List<string> { "SetUnhandledExceptionFilter" });
            peFile.AddImports(new List<AdditionalImport> { ai1, ai2 });

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
                i.DLL == "gdi32.dll"
                && i.Name == "StartPage"));

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
                i.DLL == "gdi32.dll"
                && i.Name == "EndDoc"));

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
                i.DLL == "api-ms-win-core-errorhandling-l1-1-0.dll"
                && i.Name == "SetUnhandledExceptionFilter"));
        }

        [Fact]
        public void AddImport_ToExistingImpDesc_64BitExecutable()
        {
            var peFile = new PeFile(@"Binaries/add-import.exe");
            var ai = new AdditionalImport("ADVAPI32.dll", new List<string> { "RegCloseKey" });

            peFile.AddImports(new List<AdditionalImport> { ai });

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
            i.DLL == "ADVAPI32.dll"
            && i.Name == "RegCloseKey"));
        }

        [Fact]
        public void AddImport_Regression_1()
        {
            // This is a regression test against an issue introduced in version 2.4.1.

            //Message: 
            //    System.ArgumentOutOfRangeException : Specified argument was out of the range of valid values. (Parameter 'length')
            //  Stack Trace: 
            //    BufferFile.WriteULong(Int64 offset, UInt64 value) line 51
            //    ImageThunkData.set_AddressOfData(UInt64 value) line 37
            //    PeFile.<AddImports>g__AddThunkDatas|1_4(UInt32& offset, List`1 adrList, <>c__DisplayClass1_0& ) line 106
            //    PeFile.<AddImports>g__AddImportWithNewImpDesc|1_5(UInt32& tmpOffset, Int64& paIdesc, AdditionalImport ai, <>c__DisplayClass1_0& ) line 129
            //    PeFile.AddImports(List`1 additionalImports) line 148
            //    ImportTest.AddImport_Regression() line 71

            var peFile = new PeFile(@"Binaries/AddImportsRegression.exe");
            peFile.AddImports(new List<PeNet.AdditionalImport> { new PeNet.AdditionalImport("test.dll", new List<string> 
            {
                "fn0000000140001000","fn0000000140001C50","fn0000000140001C54","fn0000000140001C5C","fn0000000140001C74","fn0000000140001C7C","fn0000000140001C84","fn0000000140001CB4","fn0000000140001CBC","fn0000000140001E18"
            })});

            Assert.NotNull(peFile.ImportedFunctions.FirstOrDefault(i =>
           i.DLL == "test.dll"
           && i.Name == "fn0000000140001000"));
        }
    }
}
