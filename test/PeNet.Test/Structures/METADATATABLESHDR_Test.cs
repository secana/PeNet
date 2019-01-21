using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class METADATATABLESHDR_Test
    {
        [Fact]
        public void MetaDataTablesHdrConstructorWorks_Test()
        {
            var metaDataTablesHdr = new METADATATABLESHDR(RawDotNetStructures.RawMetaDataTablesHdr, 0x2);

            Assert.Equal((uint) 0x55443322, metaDataTablesHdr.Reserved1);
            Assert.Equal((byte) 0x66, metaDataTablesHdr.MajorVersion);
            Assert.Equal((byte) 0x77, metaDataTablesHdr.MinorVersion);
            Assert.Equal((byte) 0x88, metaDataTablesHdr.HeapSizes);
            Assert.Equal((byte) 0x99, metaDataTablesHdr.Reserved2);
            Assert.Equal((ulong) 0xffffffffffffffff, metaDataTablesHdr.Valid);
            Assert.Equal((ulong) 0xaa99887766554433, metaDataTablesHdr.MaskSorted);
        }
    }
}