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
    }
}