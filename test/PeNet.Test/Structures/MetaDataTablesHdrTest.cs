using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class MetaDataTablesHdrTest
    {
        [Fact]
        public void MetaDataTablesHdrConstructorWorks_Test()
        {
            var metaDataTablesHdr = new MetaDataTablesHdr(new BufferFile(RawDotNetStructures.RawMetaDataTablesHdr), 0x2);

            Assert.Equal((uint) 0x55443322, metaDataTablesHdr.Reserved1);
            Assert.Equal((byte) 0x66, metaDataTablesHdr.MajorVersion);
            Assert.Equal((byte) 0x77, metaDataTablesHdr.MinorVersion);
            Assert.Equal((byte) 0x88, metaDataTablesHdr.HeapSizes);
            Assert.Equal((byte) 0x99, metaDataTablesHdr.Reserved2);
            Assert.Equal(0xffffffffffffffff, (ulong) metaDataTablesHdr.MaskValid);
            Assert.Equal(0xaa99887766554433, metaDataTablesHdr.MaskSorted);
        }
    }
}