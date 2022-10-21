using PeNet.FileParser;
using System;
using System.IO;
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
        public void NetGuidComTypeLibId_NotClrPE_Null()
        {
            var peFile = new PeFile(@"Binaries/win_test.dll");
            Assert.Null(peFile.ClrComTypeLibId);
        }

        [Fact]
        public void NetGuidComTypeLibId_ClrPE_NotEmpty()
        {
            var peFile = new PeFile(@"Binaries/NetFrameworkConsole.exe");
            Assert.Equal(new Guid("a782d109-aa8f-427b-8dcf-1c786054c7e0"), peFile.ClrComTypeLibId);
        }


        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_GivenString_TrueOrFalse(string file, bool expected)
        {
            Assert.Equal(expected, PeFile.IsPeFile(file));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_GivenStream_TrueOrFalse(string file, bool expected)
        {
            using var fs = File.OpenRead(file);
            Assert.Equal(expected, PeFile.IsPeFile(fs));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86_2.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_GivenBuffer_TrueOrFalse(string file, bool expected)
        {
            var buff = File.ReadAllBytes(file);
            Assert.Equal(expected, PeFile.IsPeFile(buff));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void IsPEFile_GivenMMFile_TrueOrFalse(string file, bool expected)
        {
            using var mmf = new MMFile(file);
            Assert.Equal(expected, PeFile.IsPeFile(mmf));
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void TryParse_GivenString_TrueOrFalse(string file, bool expected)
        {
            var actual = PeFile.TryParse(file, out var peFile);

            Assert.Equal(expected, actual);
            if (expected)
                Assert.NotNull(peFile);
            else
                Assert.Null(peFile);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void TryParse_GivenBuffer_TrueOrFalse(string file, bool expected)
        {
            var buff = File.ReadAllBytes(file);
            var actual = PeFile.TryParse(buff, out var peFile);

            Assert.Equal(expected, actual);
            if (expected)
                Assert.NotNull(peFile);
            else
                Assert.Null(peFile);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void TryParse_GivenStream_TrueOrFalse(string file, bool expected)
        {
            using var fs = File.OpenRead(file);
            var actual = PeFile.TryParse(fs, out var peFile);

            Assert.Equal(expected, actual);
            if (expected)
                Assert.NotNull(peFile);
            else
                Assert.Null(peFile);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", true)]
        [InlineData(@"Binaries/firefox_x86.exe", true)]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", true)]
        [InlineData(@"Binaries/notPeFile.txt", false)]
        public void TryParse_GivenMMF_TrueOrFalse(string file, bool expected)
        {
            using var mmf = new MMFile(file);
            var actual = PeFile.TryParse(mmf, out var peFile);

            Assert.Equal(expected, actual);
            if (expected)
                Assert.NotNull(peFile);
            else
                Assert.Null(peFile);
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
        public void Sha256_GivenAPeFile1_ReturnsCorrectHash()
        {
            var peFile = new PeFile(@"Binaries/firefox_x64.exe");

            Assert.Equal("377d3b741d8447b9bbd5f6fa700151a6ce8412ca15792ba4eaaa3174b1763ba4", peFile.Sha256);
        }

        [Fact]
        public void Sha256_GivenAPeFile2_ReturnsCorrectHash()
        {
            using var fs = File.OpenRead(@"Binaries/firefox_x64_copy1.exe");
            var peFile = new PeFile(fs);

            Assert.Equal("377d3b741d8447b9bbd5f6fa700151a6ce8412ca15792ba4eaaa3174b1763ba4", peFile.Sha256);
        }

        [Fact]
        public void Sha256_GivenAPeFile3_ReturnsCorrectHash()
        {
            using var fs = File.OpenRead(@"Binaries/firefox_x64_copy2.exe");
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
            var peFile = new PeFile(ms);

            Assert.Equal("377d3b741d8447b9bbd5f6fa700151a6ce8412ca15792ba4eaaa3174b1763ba4", peFile.Sha256);
        }

        [Fact]
        public void Sha1_GivenAPeFile_ReturnsCorrectHash()
        {
            var peFile = new PeFile(@"Binaries/firefox_x64.exe");

            Assert.Equal("5faf53976b7a4c2ffaf96581803c72cd09484b39", peFile.Sha1);
        }

        [Fact]
        public void Md5_GivenAPeFile_ReturnsCorrectHash()
        {
            var peFile = new PeFile(@"Binaries/firefox_x64.exe");

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

            var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.IsSigned);
        }

        [SkippableTheory]
        [InlineData(@"../../../Binaries/firefox_x86.exe", false, true)]
        [InlineData(@"../../../Binaries/firefox_x86.exe", false, false)]
        public void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsIfValidOrNot(string file, bool expected, bool online)
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.HasValidCertChain(online));
        }

        [Theory]
        [InlineData(@"Binaries/remove-section.exe", true, "1e556b0da4925dec2b923de11d6806f8514021d4a8d4dcb741a210f19cbec567")]
        [InlineData(@"Binaries/remove-section.exe", false, "e046941f6cf3a8c7905d0837400bf3d0527e24312d900f7bba94521da2c4ac8e")]
        public void RemoveSection_GivenPeFile_ReturnsPeWithRemovedSection(string file, bool removeContent, string expectedSha256)
        {
            var buff = File.ReadAllBytes(file);
            var peFile = new PeFile(buff);

            peFile.RemoveSection(".rsrc", removeContent);
            var actual = peFile.Sha256;

            Assert.Equal(expectedSha256, actual);
        }

        [Theory]
        [InlineData(@"Binaries/arm_binary.dll", true)]
        [InlineData(@"Binaries/arm_dotnet_binary.dll", true)]
        [InlineData(@"Binaries/old_firefox_x86.exe", true)]
        [InlineData(@"Binaries/firefox_invalid_x64.exe", false)]
        [InlineData(@"Binaries/dotnet_x64.dll", false)]
        [InlineData(@"Binaries/osx_vb_netcore.dll", false)]
        public void Is32Bit_GivenPeFiles_ReturnsCorrectBitness(string file, bool is32Bit)
        {
            var peFile = new PeFile(file);

            Assert.Equal(is32Bit, peFile.Is32Bit);
        }
    }
}
