using System.IO;
using PeNet.Asn1;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Authenticode
{
    public class AuthenticodeTest
    {
        [Fact]
        public void GetAuthenticodeInfo_FromArmDotNetBinary_WithBuffer()
        {
            var peFile = new PeFile(@"./Binaries/arm_dotnet_binary.dll");

            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].VirtualAddress);
            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].Size);
            Assert.Null(peFile.Pkcs7);
            Assert.NotNull(peFile.Authenticode);
        }

        [Fact]
        public void GetAuthenticodeInfo_FromArmDotNetBinary_WithMMF()
        {
            using var mmf = new PeNet.FileParser.MMFile(@"./Binaries/arm_dotnet_binary.dll");
            var peFile = new PeFile(mmf);

            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].VirtualAddress);
            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].Size);
            Assert.Null(peFile.Pkcs7);
            Assert.NotNull(peFile.Authenticode);
        }

        [Fact]
        public void GetAuthenticodeInfo_FromArmBinary_WithBuffer()
        {
            var peFile = new PeFile(@"./Binaries/arm_binary.dll");

            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].VirtualAddress);
            Assert.Equal((uint)0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].Size);
            Assert.Null(peFile.Pkcs7);
            Assert.NotNull(peFile.Authenticode);
        }

        [Fact]
        public void GetAuthenticodeInfo_FromArmBinary_WithMMF()
        {
            using var mmf = new PeNet.FileParser.MMFile(@"./Binaries/arm_binary.dll");
            var peFile = new PeFile(mmf);

            Assert.Equal((uint) 0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].VirtualAddress);
            Assert.Equal((uint) 0, peFile.ImageNtHeaders.OptionalHeader.DataDirectory[(int)DataDirectoryType.Security].Size);
            Assert.Null(peFile.Pkcs7);
            Assert.NotNull(peFile.Authenticode);
        }

        [Fact]
        public void IsSignatureValid_ManipulatedSignature_x64_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/firefox_x64_manipulated.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        /// <summary>
        /// Firefox is signed by a broken certificate.
        /// The Windows CAPI (and thus .Net Framework) shows it's valid, but its not!
        /// https://github.com/dotnet/corefx/issues/34202
        /// </summary>
        [Fact]
        public void IsSignatureValid_SigendBinaryOld_x86_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/old_firefox_x86.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        /// <summary>
        /// Firefox is signed by a broken certificate.
        /// The Windows CAPI (and thus .Net Framework) shows it's valid, but its not!
        /// https://github.com/dotnet/corefx/issues/34202
        /// </summary>
        [Fact]
        public void IsSignatureValid_SigendBinary_x86_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/firefox_x86.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        [Fact]
        public void IsSignatureValid_InvalidSigendBinary_x86_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/firefox_invalid_x86.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        [Fact]
        public void IsSignatureValid_InvalidSigendBinary_x64_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/firefox_invalid_x64.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        /// <summary>
        /// Firefox is signed by a broken certificate.
        /// The Windows CAPI (and thus .Net Framework) shows it's valid, but its not!
        /// https://github.com/dotnet/corefx/issues/34202
        /// </summary>
        [Fact]
        public void IsSignatureValid_SigendBinary_x64_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/firefox_x64.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.False(peFile.HasValidSignature);
        }

        [Fact]
        public void IsSignatureValid_SigendBinary_x86Other_ReturnsTrue()
        {
            var peFile = new PeFile(@"./Binaries/pidgin.exe");
            Assert.NotNull(peFile.Pkcs7);
            Assert.True(peFile.HasValidSignature);
        }

        [Fact]
        public void IsSignatureValid_UnsigendBinary_ReturnsFalse()
        {
            var peFile = new PeFile(@"./Binaries/TLSCallback_x86.exe");
            Assert.False(peFile.HasValidSignature);
        }

        [Fact]
        public void PreventOverflow()
        {
            var cert = File.ReadAllBytes(@"./Header/Authenticode/pkcs7.bin");
            var asn1 = Asn1Node.ReadNode(cert);
            Assert.NotNull(asn1);
        }

        [Fact]
        public void Asn1ShouldSupportIa5String()
        {
            var cert = File.ReadAllBytes(@"./Header/Authenticode/pidgin.pkcs7");
            var asn1 = Asn1Node.ReadNode(cert);
            Assert.NotNull(asn1);
        }
    }
}