using System;
using System.Runtime.InteropServices;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class SignatureInformation_Test
    {
        [Fact]
        void IsSigned_PathToSignedBinary1_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/firefox_x86.exe"));
        }

        [Fact]
        void IsSigned_PathToSignedBinary2_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/firefox_x64.exe"));
        }

        [SkippableFact]
        void IsSigned_PathToSignedBinary3_ReturnsTrue()
        {
            Skip.IfNot(System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows));

            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"C:\Windows\System32\kernel32.dll"));
        }

        [SkippableFact]
        void IsSigned_PathToSignedBinary4_ReturnsTrue()
        {
            Skip.IfNot(System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows));

            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"C:\Windows\explorer.exe"));
        }


        [Fact]
        void IsSigned_PathToUnsignedBinary_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/TLSCallback_x86.exe"));
        }

        [Fact]
        void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox_x86.exe", new TimeSpan(0, 0, 0, 10), true));
        }

        [Fact]
        void IsValidCertChain_PathToSignedBinaryWithValidChain_Offline_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox_x86.exe", false));
        }
    }
}