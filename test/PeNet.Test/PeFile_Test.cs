using System;
using System.Linq;
using Xunit;

namespace PeNet.Test
{
    public class PeFileTest
    {
        private readonly PeFile _peFile = new PeFile(@"Binaries/win_test.dll");

        [Fact]
        public void ExportedFunctions_WithForwardedFunctions_ParsedFordwardedFunctions()
        {
            var forwardExports = _peFile.ExportedFunctions.Where(e => e.HasForward).ToList();

            Assert.Equal(180, forwardExports.Count);
            Assert.Equal("NTDLL.RtlEnterCriticalSection", forwardExports.First(e => e.Name == "EnterCriticalSection").ForwardName);
        }

        [Fact]
        public void NetGuidModuleVersionId_NotClrPE_Empty()
        {
            Assert.Empty(_peFile.ClrModuleVersionIds);
        }

        [Fact]
        public void NetGuidComTypeLibId_NotClrPE_Empty()
        {
            Assert.Equal(string.Empty, _peFile.ClrComTypeLibId);
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
    }
}