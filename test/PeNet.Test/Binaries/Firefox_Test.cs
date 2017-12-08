using Xunit;

namespace PeNet.Test.Binaries
{
    public class Firefox_Test
    {
        private readonly PeFile _peFile = new PeFile(@"../../../Binaries/firefox.exe");

        [Fact]
        public void Firefox_IsSignatureValid()
        {
            Assert.Equal(true, _peFile.IsSignatureValid);
        }
    }
}