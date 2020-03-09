using System;
using System.Runtime.InteropServices;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class SignatureInformationTest
    {
        [Fact]
        public void IsSigned_PathToSignedBinary1_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/firefox_x86.exe"));
        }

        [Fact]
        public void IsSigned_PathToSignedBinary2_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/firefox_x64.exe"));
        }

        [SkippableFact]
        public void IsSigned_PathToSignedBinary3_ReturnsTrue()
        {
            Skip.IfNot(System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows));

            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"C:\Windows\System32\kernel32.dll"));
        }

        [SkippableFact]
        public void IsSigned_PathToSignedBinary4_ReturnsTrue()
        {
            Skip.IfNot(System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows));

            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"C:\Windows\explorer.exe"));
        }


        [Fact]
        public void IsSigned_PathToUnsignedBinary_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/TLSCallback_x86.exe"));
        }

        [Fact]
        public void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox_x86.exe", new TimeSpan(0, 0, 0, 10), true));
        }

        [Fact]
        public void IsValidCertChain_PathToSignedBinaryWithValidChain_Offline_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox_x86.exe", false));
        }
    }
}