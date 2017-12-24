using System.Linq;
using Moq;
using PeNet.Structures;
using PeNet.Structures.MetaDataTables;
using Xunit;

namespace PeNet.Test.Structures.MetaDataTables
{
    public class ModuleTable_Test
    {
        [Fact]
        public void ModuleTableConstructorWorksSmallIndexes_Test()
        {
            var fakeStreamString = new Mock<IMETADATASTREAM_STRING>();
            var fakeStreamGuid = new Mock<IMETADATASTREAM_GUID>();
            var heapOffsets = new PeNet.Structures.MetaDataTables.Indices.HeapOffsetSizes(0x00);
            var moduleTable = new ModuleTable(
                RawDotNetStructures.RawModuleTableSmall, 
                0x02, 
                1, 
                fakeStreamString.Object, 
                fakeStreamGuid.Object,
                heapOffsets);

            Assert.Equal((uint) 1, moduleTable.NumberOfRows);
            Assert.Equal(1, moduleTable.Rows.Count);
            Assert.Equal((ushort) 0x2211, moduleTable.Rows.First().Generation);
            Assert.Equal((uint) 0x4433, moduleTable.Rows.First().Name);
            Assert.Equal((uint) 0x6655, moduleTable.Rows.First().Mvid);
            Assert.Equal((uint) 0x8877, moduleTable.Rows.First().EncId);
            Assert.Equal((uint) 0xaa99, moduleTable.Rows.First().EncBaseId);
            Assert.Equal((uint) 0x0a, moduleTable.Rows.First().Length);
            Assert.Equal((uint) 0x0a, moduleTable.Length);
        }

        [Fact]
        public void ModuleTableConstructorWorksBigIndexes_Test()
        {
            var fakeStreamString = new Mock<IMETADATASTREAM_STRING>();
            var fakeStreamGuid = new Mock<IMETADATASTREAM_GUID>();
            var heapOffsets = new PeNet.Structures.MetaDataTables.Indices.HeapOffsetSizes(0x07);
            var moduleTable = new ModuleTable(
                RawDotNetStructures.RawModuleTableBig, 
                0x02, 
                1, 
                fakeStreamString.Object, 
                fakeStreamGuid.Object,
                heapOffsets);

            Assert.Equal((uint) 1, moduleTable.NumberOfRows);
            Assert.Equal(1, moduleTable.Rows.Count);
            Assert.Equal((ushort) 0x2211, moduleTable.Rows.First().Generation);
            Assert.Equal((uint) 0xbbaa4433, moduleTable.Rows.First().Name);
            Assert.Equal((uint) 0xbbaa6655, moduleTable.Rows.First().Mvid);
            Assert.Equal((uint) 0xbbaa8877, moduleTable.Rows.First().EncId);
            Assert.Equal((uint) 0xbbaaaa99, moduleTable.Rows.First().EncBaseId);
            Assert.Equal((uint) 0x12, moduleTable.Rows.First().Length);
            Assert.Equal((uint) 0x12, moduleTable.Length);
        }
    }
}