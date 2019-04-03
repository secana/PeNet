using System.Linq;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_TLS_CALLBACK_Test
    {
        [Fact]
        public void ImageTlsCallback64ConstructorWorks_Test()
        {
            var tlsCallback = new IMAGE_TLS_CALLBACK(RawStructures.RawTlsCallback64, 2, true);
            Assert.Equal((ulong) 0x7766554433221100, tlsCallback.Callback);
        }

        [Fact]
        public void ImageTlsCallback32ConstructorWorks_Test()
        {
            var tlsCallback = new IMAGE_TLS_CALLBACK(RawStructures.RawTlsCallback32, 2, false);
            Assert.Equal((ulong)0x33221100, tlsCallback.Callback);
        }

        [Fact]
        public void TLSCallback_x86_Works_Test()
        {
            // Given
            var peFile = new PeFile(@"../../../Binaries/TLSCallback_x86.exe");

            // When
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.Single(callbacks);
            Assert.Equal((ulong) 0x004111CC, callbacks.First().Callback);
        }

        [Fact]
        public void TLSCallback_x64_Works1_Test()
        {
            // Given
            var peFile = new PeFile(@"../../../Binaries/TLSCallback_x64.dll");

            // When
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.Single(callbacks);
            Assert.Equal((ulong) 0x0000000180001000, callbacks.First().Callback);
        }

        [Fact]
        public void TLSCallback_x64_Works2_Test()
        {
            // Given
            var peFile = new PeFile(@"../../../Binaries/firefox_x64.exe");

            // When
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.Single(callbacks);
            Assert.Equal((ulong) 0x00000001400044a0, callbacks.First().Callback);
        }
    }
}