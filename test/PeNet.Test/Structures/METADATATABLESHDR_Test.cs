using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class METADATATABLESHDR_Test
    {
        [Fact]
        public void MetaDataTablesHdrConstructorWorks_Test()
        {
            var metaDataTablesHdr = new METADATATABLESHDR(RawDotNetStructures.RawMetaDataDataTablesHdr, 0x2);

            Assert.Equal((uint) 0x55443322, metaDataTablesHdr.Reserved1);
            Assert.Equal((byte) 0x66, metaDataTablesHdr.MajorVersion);
            Assert.Equal((byte) 0x77, metaDataTablesHdr.MinorVersion);
            Assert.Equal((byte) 0x88, metaDataTablesHdr.HeapOffsetSizes);
            Assert.Equal((byte) 0x99, metaDataTablesHdr.Reserved2);
            Assert.Equal((ulong) 0xffffffffffffffff, metaDataTablesHdr.MaskValid);
            Assert.Equal((ulong) 0xaa99887766554433, metaDataTablesHdr.MaskSorted);
        }

        [Fact]
        public void MetaDataTablesHdrTableDefinitionParser_Test()
        {
            var metaDataTablesHdr = new METADATATABLESHDR(RawDotNetStructures.RawMetaDataDataTablesHdr, 0x2);

            Assert.Equal("Module", metaDataTablesHdr.TableDefinitions[0].Name);
            Assert.Equal((uint) 0x00, metaDataTablesHdr.TableDefinitions[0].NumOfRows);
            Assert.Equal("TypeRef", metaDataTablesHdr.TableDefinitions[1].Name);
            Assert.Equal((uint) 0x01, metaDataTablesHdr.TableDefinitions[1].NumOfRows);
            Assert.Equal("TypeDef", metaDataTablesHdr.TableDefinitions[2].Name);
            Assert.Equal((uint) 0x02, metaDataTablesHdr.TableDefinitions[2].NumOfRows);
            Assert.Equal("Field", metaDataTablesHdr.TableDefinitions[3].Name);
            Assert.Equal((uint) 0x04, metaDataTablesHdr.TableDefinitions[3].NumOfRows);
            Assert.Equal("MethodDef", metaDataTablesHdr.TableDefinitions[4].Name);
            Assert.Equal((uint) 0x06, metaDataTablesHdr.TableDefinitions[4].NumOfRows);
            Assert.Equal("Param", metaDataTablesHdr.TableDefinitions[5].Name);
            Assert.Equal((uint) 0x08, metaDataTablesHdr.TableDefinitions[5].NumOfRows);
            Assert.Equal("InterfaceImpl", metaDataTablesHdr.TableDefinitions[6].Name);
            Assert.Equal((uint) 0x09, metaDataTablesHdr.TableDefinitions[6].NumOfRows);
            Assert.Equal("MemberRef", metaDataTablesHdr.TableDefinitions[7].Name);
            Assert.Equal((uint) 0x10, metaDataTablesHdr.TableDefinitions[7].NumOfRows);
            Assert.Equal("Constant", metaDataTablesHdr.TableDefinitions[8].Name);
            Assert.Equal((uint) 0x11, metaDataTablesHdr.TableDefinitions[8].NumOfRows);
            Assert.Equal("CustomAttribute", metaDataTablesHdr.TableDefinitions[9].Name);
            Assert.Equal((uint) 0x12, metaDataTablesHdr.TableDefinitions[9].NumOfRows);
            Assert.Equal("FieldMarshal", metaDataTablesHdr.TableDefinitions[10].Name);
            Assert.Equal((uint) 0x13, metaDataTablesHdr.TableDefinitions[10].NumOfRows);
            Assert.Equal("DeclSecurity", metaDataTablesHdr.TableDefinitions[11].Name);
            Assert.Equal((uint) 0x14, metaDataTablesHdr.TableDefinitions[11].NumOfRows);
            Assert.Equal("ClassLayout", metaDataTablesHdr.TableDefinitions[12].Name);
            Assert.Equal((uint) 0x15, metaDataTablesHdr.TableDefinitions[12].NumOfRows);
            Assert.Equal("FieldLayout", metaDataTablesHdr.TableDefinitions[13].Name);
            Assert.Equal((uint) 0x16, metaDataTablesHdr.TableDefinitions[13].NumOfRows);
            Assert.Equal("StandAloneSig", metaDataTablesHdr.TableDefinitions[14].Name);
            Assert.Equal((uint) 0x17, metaDataTablesHdr.TableDefinitions[14].NumOfRows);
            Assert.Equal("EventMap", metaDataTablesHdr.TableDefinitions[15].Name);
            Assert.Equal((uint) 0x18, metaDataTablesHdr.TableDefinitions[15].NumOfRows);
            Assert.Equal("Event", metaDataTablesHdr.TableDefinitions[16].Name);
            Assert.Equal((uint) 0x20, metaDataTablesHdr.TableDefinitions[16].NumOfRows);
            Assert.Equal("PropertyMap", metaDataTablesHdr.TableDefinitions[17].Name);
            Assert.Equal((uint) 0x21, metaDataTablesHdr.TableDefinitions[17].NumOfRows);
            Assert.Equal("Property", metaDataTablesHdr.TableDefinitions[18].Name);
            Assert.Equal((uint) 0x23, metaDataTablesHdr.TableDefinitions[18].NumOfRows);
            Assert.Equal("MethodSemantics", metaDataTablesHdr.TableDefinitions[19].Name);
            Assert.Equal((uint) 0x24, metaDataTablesHdr.TableDefinitions[19].NumOfRows);
            Assert.Equal("MethodImpl", metaDataTablesHdr.TableDefinitions[20].Name);
            Assert.Equal((uint) 0x25, metaDataTablesHdr.TableDefinitions[20].NumOfRows);
            Assert.Equal("ModuleRef", metaDataTablesHdr.TableDefinitions[21].Name);
            Assert.Equal((uint) 0x26, metaDataTablesHdr.TableDefinitions[21].NumOfRows);
            Assert.Equal("TypeSpec", metaDataTablesHdr.TableDefinitions[22].Name);
            Assert.Equal((uint) 0x27, metaDataTablesHdr.TableDefinitions[22].NumOfRows);
            Assert.Equal("ImplMap", metaDataTablesHdr.TableDefinitions[23].Name);
            Assert.Equal((uint) 0x28, metaDataTablesHdr.TableDefinitions[23].NumOfRows);
            Assert.Equal("FieldRVA", metaDataTablesHdr.TableDefinitions[24].Name);
            Assert.Equal((uint) 0x29, metaDataTablesHdr.TableDefinitions[24].NumOfRows);
            Assert.Equal("Assembly", metaDataTablesHdr.TableDefinitions[25].Name);
            Assert.Equal((uint) 0x32, metaDataTablesHdr.TableDefinitions[25].NumOfRows);
            Assert.Equal("AssemblyProcessor", metaDataTablesHdr.TableDefinitions[26].Name);
            Assert.Equal((uint) 0x33, metaDataTablesHdr.TableDefinitions[26].NumOfRows);
            Assert.Equal("AssemblyOS", metaDataTablesHdr.TableDefinitions[27].Name);
            Assert.Equal((uint) 0x34, metaDataTablesHdr.TableDefinitions[27].NumOfRows);
            Assert.Equal("AssemblyRef", metaDataTablesHdr.TableDefinitions[28].Name);
            Assert.Equal((uint) 0x35, metaDataTablesHdr.TableDefinitions[28].NumOfRows);
            Assert.Equal("AssemblyRefProcessor", metaDataTablesHdr.TableDefinitions[29].Name);
            Assert.Equal((uint) 0x36, metaDataTablesHdr.TableDefinitions[29].NumOfRows);
            Assert.Equal("AssemblyRefOS", metaDataTablesHdr.TableDefinitions[30].Name);
            Assert.Equal((uint) 0x37, metaDataTablesHdr.TableDefinitions[30].NumOfRows);
            Assert.Equal("File", metaDataTablesHdr.TableDefinitions[31].Name);
            Assert.Equal((uint) 0x38, metaDataTablesHdr.TableDefinitions[31].NumOfRows);
            Assert.Equal("ExportedType", metaDataTablesHdr.TableDefinitions[32].Name);
            Assert.Equal((uint) 0x39, metaDataTablesHdr.TableDefinitions[32].NumOfRows);
            Assert.Equal("ManifestResource", metaDataTablesHdr.TableDefinitions[33].Name);
            Assert.Equal((uint) 0x40, metaDataTablesHdr.TableDefinitions[33].NumOfRows);
            Assert.Equal("NestedClass", metaDataTablesHdr.TableDefinitions[34].Name);
            Assert.Equal((uint) 0x41, metaDataTablesHdr.TableDefinitions[34].NumOfRows);
            Assert.Equal("GenericParam", metaDataTablesHdr.TableDefinitions[35].Name);
            Assert.Equal((uint) 0x42, metaDataTablesHdr.TableDefinitions[35].NumOfRows);
            Assert.Equal("MethodSpec", metaDataTablesHdr.TableDefinitions[36].Name);
            Assert.Equal((uint) 0x43, metaDataTablesHdr.TableDefinitions[36].NumOfRows);
            Assert.Equal("GenericParamConstraint", metaDataTablesHdr.TableDefinitions[37].Name);
            Assert.Equal((uint) 0x44, metaDataTablesHdr.TableDefinitions[37].NumOfRows);
        }
    }
}