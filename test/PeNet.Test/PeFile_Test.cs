using System.Linq;
using Xunit;

namespace PeNet.Test.PeFile_Test
{
    public class PeFile
    {
        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_DifferentFiles_TrueOrFalse(string file, bool expected)
        {
            Assert.Equal(expected, PeNet.PeFile.IsPEFile(file));
        }

        [Fact]
        public void ExportedFunctions_WithForwardedFunctions_ParsedFordwardedFunctions()
        {
            var peFile = new PeNet.PeFile(@"Binaries/win_test.dll");
            var forwardExports = peFile.ExportedFunctions.Where(e => e.HasForwad).ToList();

            Assert.Equal(180, forwardExports.Count);
            Assert.Equal("NTDLL.RtlEnterCriticalSection", forwardExports.First(e => e.Name == "EnterCriticalSection").ForwardName);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", false)]
        [InlineData(@"Binaries/TLSCallback_x86.exe", false)]
        [InlineData(@"Binaries/NetCoreConsole.dll", false)]
        [InlineData(@"Binaries/win_test.dll", false)]
        [InlineData(@"Binaries/krnl_test.sys", true)]
        public void IsDriver_GivenAPeFile_ReturnsDriverOrNot(string file, bool isDriver)
        {
            var peFile = new PeNet.PeFile(file);

            Assert.Equal(isDriver, peFile.IsDriver);
        }
    }
}