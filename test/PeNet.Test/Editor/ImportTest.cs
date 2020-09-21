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

            File.WriteAllBytes("patched-chrome_elf.dll", peFile.RawFile.ToArray());

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
    }
}
