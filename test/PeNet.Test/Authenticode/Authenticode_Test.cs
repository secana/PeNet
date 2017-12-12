using Xunit;

namespace PeNet.Test.Authenticode
{
    public class Authenticode_Test
    {
        [Fact]
        public void IsSignatureValid_SigendBinary_ReturnsTrue()
        {
            var peFile = new PeFile(@"../../../Binaries/firefox.exe");
            Assert.True(peFile.IsSignatureValid);
        }

        [Fact]
        public void IsSignatureValid_UnsigendBinary_ReturnsFalse()
        {
            var peFile = new PeFile(@"../../../Binaries/TLSCallback_x86.exe");
            Assert.False(peFile.IsSignatureValid);
        }
    }
}