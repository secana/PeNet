using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class Copyright_Test
    {
        [Fact]
        public void CopyrightConstructorWorks_Test()
        {
            var copyright = new Copyright(RawStructures.RawCopyright, 2, 9);
            Assert.Equal("copyright", copyright.CopyrightString);
        }
    }
}