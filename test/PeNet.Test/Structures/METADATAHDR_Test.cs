using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class METADATAHDR_Test
    {
        [Fact]
        public void MetaDataHdrConstructorWorks_Test()
        {
            var metaDataHdr = new METADATAHDR(RawDotNetStructures.RawMetaDataHeader, 2);
            Assert.Equal((uint) 0x55443322, metaDataHdr.Signature);
            Assert.Equal((ushort) 0x7766, metaDataHdr.MajorVersion);
            Assert.Equal((ushort) 0x9988, metaDataHdr.MinorVersion);
            Assert.Equal((uint) 0xddccbbaa, metaDataHdr.Reserved);
            Assert.Equal((uint) 0x0000000C, metaDataHdr.VersionLength);
            Assert.Equal("v4.0.30319", metaDataHdr.Version);
            Assert.Equal((ushort) 0x2211, metaDataHdr.Flags);
            Assert.Equal((ushort) 0x0002, metaDataHdr.Streams);

            Assert.Equal(2, metaDataHdr.MetaDataStreamsHdrs.Length);
            Assert.Equal((uint) 0x6C, metaDataHdr.MetaDataStreamsHdrs[0].offset);
            Assert.Equal((uint) 0x1804, metaDataHdr.MetaDataStreamsHdrs[0].size);
            Assert.Equal("#~", metaDataHdr.MetaDataStreamsHdrs[0].streamName);
            Assert.Equal((uint) 0x1870, metaDataHdr.MetaDataStreamsHdrs[1].offset);
            Assert.Equal((uint) 0x1468, metaDataHdr.MetaDataStreamsHdrs[1].size);
            Assert.Equal("#Strings", metaDataHdr.MetaDataStreamsHdrs[1].streamName);
        }
    }
}