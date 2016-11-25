using System.Linq;
using Xunit;

namespace PeNet.UnitTest.Binaries
{
    public class TLSCallback_x86_Test
    {
        [Fact]
        public void TLSCallback_x86_Works_Test()
        {
            // Given
            var peFile = new PeFile(@"../../Binaries/TLSCallback_x86.exe");

            // When
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.Equal(1, callbacks.Length);
            Assert.Equal((ulong) 0x004111CC, callbacks.First().Callback);
        }
    }
}