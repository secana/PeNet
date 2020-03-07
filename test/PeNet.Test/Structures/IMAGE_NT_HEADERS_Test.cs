using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_NT_HEADERS_Test
    {
        [Fact]
        public void ImageNtHeadersConstructorWorks_Test()
        {
            var ntHeaders = new IMAGE_NT_HEADERS(new BufferFile(RawStructures.RawImageNtHeaders64), 2);
            Assert.Equal((uint) 0x33221100, ntHeaders.Signature);
        }
    }
}