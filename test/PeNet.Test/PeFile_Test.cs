using System;
using System.Linq;
using Xunit;

namespace PeNet.Test
{
    public class PeFileTest
    {
        [Fact]
        public void ExportedFunctions_WithForwardedFunctions_ParsedForwardedFunctions()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            var forwardExports = peFile.ExportedFunctions.Where(e => e.HasForward).ToList();

            Assert.Equal(180, forwardExports.Count);
            Assert.Equal("NTDLL.RtlEnterCriticalSection", forwardExports.First(e => e.Name == "EnterCriticalSection").ForwardName);
        }

        [Fact]
        public void NetGuidModuleVersionId_NotClrPE_Empty()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Empty(peFile.ClrModuleVersionIds);
        }

        [Fact]
        public void NetGuidModuleVersionId_ClrPE_NotEmpty()
        {
            var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), peFile.ClrModuleVersionIds.First());
        }

        [Fact]
        public void NetGuidComTypeLibId_NotClrPE_Empty()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Equal(string.Empty, peFile.ClrComTypeLibId);
        }

        [Fact]
        public void NetGuidComTypeLibId_ClrPE_NotEmpty()
        {
            var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal("a782d109-aa8f-427b-8dcf-1c786054c7e0", peFile.ClrComTypeLibId);
        }


        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_DifferentFiles_TrueOrFalse(string file, bool expected)
        {
            Assert.Equal(expected, PeFile.IsPEFile(file));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", false)]
        [InlineData(@"Binaries/TLSCallback_x86.exe", false)]
        [InlineData(@"Binaries/NetCoreConsole.dll", false)]
        [InlineData(@"Binaries/win_test.dll", false)]
        [InlineData(@"Binaries/krnl_test.sys", true)]
        public void IsDriver_GivenAPeFile_ReturnsDriverOrNot(string file, bool isDriver)
        {
            var peFile = new PeFile(file);

            Assert.Equal(isDriver, peFile.IsDriver);
        }

        [Fact]
        public void MALWRE()
        {
            var file =
                @"C:\Users\stefan.hausotte\source\repos\PeNet-Analyzer\test\PeNet.Analyzer.Test\Binaries\4d5bc8f3311079eadcc8031f5a648e7e1ec68b9d2aed0342d9ec426259603e96";
            var peFile = new PeFile(file);

            var rsc = peFile.ImageResourceDirectory;
            var vs = peFile.Resources.VsVersionInfo;
        }
    }
}