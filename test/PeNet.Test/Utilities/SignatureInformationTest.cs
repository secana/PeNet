using System.Runtime.InteropServices;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class SignatureInformationTest
    {
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
        [InlineData(@"C:\Windows\System32\kernel32.dll", true, true)]
        public void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsIfValidOrNot(string file, bool expected, bool online)
        {
            var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.IsValidCertChain(online));
        }
    }
}