using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageNtHeadersTest
    {
        [Fact]
        public void ImageNtHeadersConstructorWorks_Test()
        {
            var ntHeaders = new ImageNtHeaders(new BufferFile(RawStructures.RawImageNtHeaders64), 2);
            Assert.Equal((uint) 0x33221100, ntHeaders.Signature);
        }
    }
}