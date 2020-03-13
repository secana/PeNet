using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace PeNet.Test
{
    public class PeFileTest
    {
        [Fact]
        public void ExportedFunctions_WithForwardedFunctions_ParsedForwardedFunctions()
        {
            using var peFile = new PeFile(@"Binaries/win_test.dll");
            var forwardExports = peFile.ExportedFunctions.Where(e => e.HasForward).ToList();

            Assert.Equal(180, forwardExports.Count);
            Assert.Equal("NTDLL.RtlEnterCriticalSection", forwardExports.First(e => e.Name == "EnterCriticalSection").ForwardName);
        }

        [Fact]
        public void NetGuidModuleVersionId_NotClrPE_Empty()
        {
            using var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Empty(peFile.ClrModuleVersionIds);
        }

        [Fact]
        public void NetGuidModuleVersionId_ClrPE_NotEmpty()
        {
            using var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal(new Guid("5250e853-c17a-4e76-adb3-0a716ec8af5d"), peFile.ClrModuleVersionIds.First());
        }

        [Fact]
        public void NetGuidComTypeLibId_NotClrPE_Empty()
        {
            using var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Equal(string.Empty, peFile.ClrComTypeLibId);
        }

        [Fact]
        public void NetGuidComTypeLibId_ClrPE_NotEmpty()
        {
            using var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal("a782d109-aa8f-427b-8dcf-1c786054c7e0", peFile.ClrComTypeLibId);
        }


        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_DifferentFiles_TrueOrFalse(string file, bool expected)
        {
            Assert.Equal(expected, PeFile.IsPeFile(file));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", false)]
        [InlineData(@"Binaries/TLSCallback_x86.exe", false)]
        [InlineData(@"Binaries/NetCoreConsole.dll", false)]
        [InlineData(@"Binaries/win_test.dll", false)]
        [InlineData(@"Binaries/krnl_test.sys", true)]
        public void IsDriver_GivenAPeFile_ReturnsDriverOrNot(string file, bool isDriver)
        {
            using var peFile = new PeFile(file);

            Assert.Equal(isDriver, peFile.IsDriver);
        }

        [Fact]
        public void Sha256_GivenAPeFile_ReturnsCorrectHash()
        {
            using var peFile = new PeFile(@"Binaries/firefox_x64.exe");

            Assert.Equal("377d3b741d8447b9bbd5f6fa700151a6ce8412ca15792ba4eaaa3174b1763ba4", peFile.Sha256);
        }

        [Fact]
        public void Sha1_GivenAPeFile_ReturnsCorrectHash()
        {
            using var peFile = new PeFile(@"Binaries/firefox_x64.exe");

            Assert.Equal("5faf53976b7a4c2ffaf96581803c72cd09484b39", peFile.Sha1);
        }

        [Fact]
        public void Md5_GivenAPeFile_ReturnsCorrectHash()
        {
            using var peFile = new PeFile(@"Binaries/firefox_x64.exe");

            Assert.Equal("fa64b4aeb420a6c292f877e90d0670a5", peFile.Md5);
        }

        [SkippableTheory]
        [InlineData(@"../../../Binaries/firefox_x86.exe", true)]
        [InlineData(@"../../../Binaries/firefox_x64.exe", true)]
        [InlineData(@"C:\Windows\System32\kernel32.dll", true)]
        [InlineData(@"C:\Windows\explorer.exe", true)]
        [InlineData(@"../../../Binaries/TLSCallback_x86.exe", false)]
        public void IsSigned_PathToSignedBinary_ReturnsSignedOrNot(string file, bool expected)
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            using var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.IsSigned);
        }

        [SkippableTheory]
        [InlineData(@"../../../Binaries/firefox_x86.exe", false, true)]
        [InlineData(@"../../../Binaries/firefox_x86.exe", false, false)]
        [InlineData(@"C:\Windows\System32\kernel32.dll", true, true)]
        public void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsIfValidOrNot(string file, bool expected, bool online)
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            using var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.HasValidCertChain(online));
        }
    }
}