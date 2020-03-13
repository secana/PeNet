using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class CopyrightTest
    {
        [Fact]
        public void CopyrightConstructorWorks_Test()
        {
            var copyright = new Copyright(new BufferFile(RawStructures.RawCopyright), 2);
            Assert.Equal("copyright", copyright.CopyrightString);
        }
    }
}