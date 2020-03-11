using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class METADATATABLESHDR_Test
    {
        [Fact]
        public void MetaDataTablesHdrConstructorWorks_Test()
        {
            var MetaDataTablesHdr = new MetaDataTablesHdr(new BufferFile(RawDotNetStructures.RawMetaDataTablesHdr), 0x2);

            Assert.Equal((uint) 0x55443322, MetaDataTablesHdr.Reserved1);
            Assert.Equal((byte) 0x66, MetaDataTablesHdr.MajorVersion);
            Assert.Equal((byte) 0x77, MetaDataTablesHdr.MinorVersion);
            Assert.Equal((byte) 0x88, MetaDataTablesHdr.HeapSizes);
            Assert.Equal((byte) 0x99, MetaDataTablesHdr.Reserved2);
            Assert.Equal((ulong) 0xffffffffffffffff, MetaDataTablesHdr.Valid);
            Assert.Equal((ulong) 0xaa99887766554433, MetaDataTablesHdr.MaskSorted);
        }
    }
}