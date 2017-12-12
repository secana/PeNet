using System;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace PeNet.Test.Utilities
{
    public class SignatureInformation_Test
    {
        [Fact]
        void IsSigned_PathToSignedBinary_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/firefox.exe"));
        }

        [Fact]
        void IsSigned_PathToUnsignedBinary_ReturnsFalse()
        {
            Assert.False(PeNet.Utilities.SignatureInformation.IsSigned(@"../../../Binaries/TLSCallback_x86.exe"));
        }

        /// <summary>
        /// Testing "offline" makes no sense since the first test would be "false". After testing the cert online
        /// the cert chain will be cached and the second try to check offline would return "true".
        /// </summary>
        [Fact]
        void IsValidCertChain_PathToSignedBinaryWithValidChain_Online_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox.exe", true));
        }

        [Fact]
        void IsValidCertChain_PathToSignedBinaryWithValidChain_CheckRoot_ReturnsTrue()
        {
            Assert.True(PeNet.Utilities.SignatureInformation.IsValidCertChain(@"../../../Binaries/firefox.exe", new TimeSpan(0,0,0,10), false));
        }
    }
}