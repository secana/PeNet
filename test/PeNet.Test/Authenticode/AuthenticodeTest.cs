using System.IO;
using PeNet.Asn1;
using Xunit;

namespace PeNet.Test.Authenticode
{
    public class AuthenticodeTest
    {
        [Fact]
        public void IsSignatureValid_SigendBinary_x86_ReturnsTrue()
        {
            var peFile = new PeFile(@"../../../Binaries/firefox_x86.exe");
            Assert.NotNull(peFile.PKCS7);
            Assert.True(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_InvalidSigendBinary_x86_ReturnsFalse()
        {
            var peFile = new PeFile(@"../../../Binaries/firefox_invalid_x86.exe");
            Assert.NotNull(peFile.PKCS7);
            Assert.False(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_InvalidSigendBinary_x64_ReturnsFalse()
        {
            var peFile = new PeFile(@"../../../Binaries/firefox_invalid_x64.exe");
            Assert.NotNull(peFile.PKCS7);
            Assert.False(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_SigendBinary_x64_ReturnsTrue()
        {
            var peFile = new PeFile(@"../../../Binaries/firefox_x64.exe");
            Assert.NotNull(peFile.PKCS7);
            Assert.True(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_SigendBinary_x86Other_ReturnsTrue()
        {
            var peFile = new PeFile(@"../../../Binaries/pidgin.exe");
            Assert.NotNull(peFile.PKCS7);
            Assert.True(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_UnsigendBinary_ReturnsFalse()
        {
            var peFile = new PeFile(@"../../../Binaries/TLSCallback_x86.exe");
            Assert.False(peFile.IsSignatureValid);
        }

        [Fact]
        public void PreventOverflow()
        {
            var cert = File.ReadAllBytes(@"../../../Authenticode/pkcs7.bin");
            var asn1 = Asn1Node.ReadNode(cert);
            Assert.NotNull(asn1);
        }

        [Fact]
        public void Asn1ShouldSupportIa5String()
        {
            var cert = File.ReadAllBytes(@"../../../Authenticode/pidgin.pkcs7");
            var asn1 = Asn1Node.ReadNode(cert);
            Assert.NotNull(asn1);
        }
    }
}