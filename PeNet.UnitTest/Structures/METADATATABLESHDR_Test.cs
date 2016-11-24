/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class METADATATABLESHDR_Test
    {
        [TestMethod]
        public void MetaDataTablesHdrConstructorWorks_Test()
        {
            var metaDataTablesHdr = new METADATATABLESHDR(RawDotNetStructures.RawMetaDataDataTablesHdr, 0x2);

            Assert.AreEqual((uint) 0x55443322, metaDataTablesHdr.Reserved1);
            Assert.AreEqual((byte) 0x66, metaDataTablesHdr.MajorVersion);
            Assert.AreEqual((byte) 0x77, metaDataTablesHdr.MinorVersion);
            Assert.AreEqual((byte) 0x88, metaDataTablesHdr.HeapOffsetSizes);
            Assert.AreEqual((byte) 0x99, metaDataTablesHdr.Reserved2);
            Assert.AreEqual((ulong) 0xffffffffffffffff, metaDataTablesHdr.MaskValid);
            Assert.AreEqual((ulong) 0xaa99887766554433, metaDataTablesHdr.MaskSorted);
        }

        [TestMethod]
        public void MetaDataTablesHdrTableDefinitionParser_Test()
        {
            var metaDataTablesHdr = new METADATATABLESHDR(RawDotNetStructures.RawMetaDataDataTablesHdr, 0x2);

            Assert.AreEqual("Module", metaDataTablesHdr.TableDefinitions[0].Name);
            Assert.AreEqual((uint) 0x00, metaDataTablesHdr.TableDefinitions[0].NumOfRows);
            Assert.AreEqual("TypeRef", metaDataTablesHdr.TableDefinitions[1].Name);
            Assert.AreEqual((uint) 0x01, metaDataTablesHdr.TableDefinitions[1].NumOfRows);
            Assert.AreEqual("TypeDef", metaDataTablesHdr.TableDefinitions[2].Name);
            Assert.AreEqual((uint) 0x02, metaDataTablesHdr.TableDefinitions[2].NumOfRows);
            Assert.AreEqual("Field", metaDataTablesHdr.TableDefinitions[3].Name);
            Assert.AreEqual((uint) 0x04, metaDataTablesHdr.TableDefinitions[3].NumOfRows);
            Assert.AreEqual("MethodDef", metaDataTablesHdr.TableDefinitions[4].Name);
            Assert.AreEqual((uint) 0x06, metaDataTablesHdr.TableDefinitions[4].NumOfRows);
            Assert.AreEqual("Param", metaDataTablesHdr.TableDefinitions[5].Name);
            Assert.AreEqual((uint) 0x08, metaDataTablesHdr.TableDefinitions[5].NumOfRows);
            Assert.AreEqual("InterfaceImpl", metaDataTablesHdr.TableDefinitions[6].Name);
            Assert.AreEqual((uint) 0x09, metaDataTablesHdr.TableDefinitions[6].NumOfRows);
            Assert.AreEqual("MemberRef", metaDataTablesHdr.TableDefinitions[7].Name);
            Assert.AreEqual((uint) 0x10, metaDataTablesHdr.TableDefinitions[7].NumOfRows);
            Assert.AreEqual("Constant", metaDataTablesHdr.TableDefinitions[8].Name);
            Assert.AreEqual((uint) 0x11, metaDataTablesHdr.TableDefinitions[8].NumOfRows);
            Assert.AreEqual("CustomAttribute", metaDataTablesHdr.TableDefinitions[9].Name);
            Assert.AreEqual((uint) 0x12, metaDataTablesHdr.TableDefinitions[9].NumOfRows);
            Assert.AreEqual("FieldMarshal", metaDataTablesHdr.TableDefinitions[10].Name);
            Assert.AreEqual((uint) 0x13, metaDataTablesHdr.TableDefinitions[10].NumOfRows);
            Assert.AreEqual("DeclSecurity", metaDataTablesHdr.TableDefinitions[11].Name);
            Assert.AreEqual((uint) 0x14, metaDataTablesHdr.TableDefinitions[11].NumOfRows);
            Assert.AreEqual("ClassLayout", metaDataTablesHdr.TableDefinitions[12].Name);
            Assert.AreEqual((uint) 0x15, metaDataTablesHdr.TableDefinitions[12].NumOfRows);
            Assert.AreEqual("FieldLayout", metaDataTablesHdr.TableDefinitions[13].Name);
            Assert.AreEqual((uint) 0x16, metaDataTablesHdr.TableDefinitions[13].NumOfRows);
            Assert.AreEqual("StandAloneSig", metaDataTablesHdr.TableDefinitions[14].Name);
            Assert.AreEqual((uint) 0x17, metaDataTablesHdr.TableDefinitions[14].NumOfRows);
            Assert.AreEqual("EventMap", metaDataTablesHdr.TableDefinitions[15].Name);
            Assert.AreEqual((uint) 0x18, metaDataTablesHdr.TableDefinitions[15].NumOfRows);
            Assert.AreEqual("Event", metaDataTablesHdr.TableDefinitions[16].Name);
            Assert.AreEqual((uint) 0x20, metaDataTablesHdr.TableDefinitions[16].NumOfRows);
            Assert.AreEqual("PropertyMap", metaDataTablesHdr.TableDefinitions[17].Name);
            Assert.AreEqual((uint) 0x21, metaDataTablesHdr.TableDefinitions[17].NumOfRows);
            Assert.AreEqual("Property", metaDataTablesHdr.TableDefinitions[18].Name);
            Assert.AreEqual((uint) 0x23, metaDataTablesHdr.TableDefinitions[18].NumOfRows);
            Assert.AreEqual("MethodSemantics", metaDataTablesHdr.TableDefinitions[19].Name);
            Assert.AreEqual((uint) 0x24, metaDataTablesHdr.TableDefinitions[19].NumOfRows);
            Assert.AreEqual("MethodImpl", metaDataTablesHdr.TableDefinitions[20].Name);
            Assert.AreEqual((uint) 0x25, metaDataTablesHdr.TableDefinitions[20].NumOfRows);
            Assert.AreEqual("ModuleRef", metaDataTablesHdr.TableDefinitions[21].Name);
            Assert.AreEqual((uint) 0x26, metaDataTablesHdr.TableDefinitions[21].NumOfRows);
            Assert.AreEqual("TypeSpec", metaDataTablesHdr.TableDefinitions[22].Name);
            Assert.AreEqual((uint) 0x27, metaDataTablesHdr.TableDefinitions[22].NumOfRows);
            Assert.AreEqual("ImplMap", metaDataTablesHdr.TableDefinitions[23].Name);
            Assert.AreEqual((uint) 0x28, metaDataTablesHdr.TableDefinitions[23].NumOfRows);
            Assert.AreEqual("FieldRVA", metaDataTablesHdr.TableDefinitions[24].Name);
            Assert.AreEqual((uint) 0x29, metaDataTablesHdr.TableDefinitions[24].NumOfRows);
            Assert.AreEqual("Assembly", metaDataTablesHdr.TableDefinitions[25].Name);
            Assert.AreEqual((uint) 0x32, metaDataTablesHdr.TableDefinitions[25].NumOfRows);
            Assert.AreEqual("AssemblyProcessor", metaDataTablesHdr.TableDefinitions[26].Name);
            Assert.AreEqual((uint) 0x33, metaDataTablesHdr.TableDefinitions[26].NumOfRows);
            Assert.AreEqual("AssemblyOS", metaDataTablesHdr.TableDefinitions[27].Name);
            Assert.AreEqual((uint) 0x34, metaDataTablesHdr.TableDefinitions[27].NumOfRows);
            Assert.AreEqual("AssemblyRef", metaDataTablesHdr.TableDefinitions[28].Name);
            Assert.AreEqual((uint) 0x35, metaDataTablesHdr.TableDefinitions[28].NumOfRows);
            Assert.AreEqual("AssemblyRefProcessor", metaDataTablesHdr.TableDefinitions[29].Name);
            Assert.AreEqual((uint) 0x36, metaDataTablesHdr.TableDefinitions[29].NumOfRows);
            Assert.AreEqual("AssemblyRefOS", metaDataTablesHdr.TableDefinitions[30].Name);
            Assert.AreEqual((uint) 0x37, metaDataTablesHdr.TableDefinitions[30].NumOfRows);
            Assert.AreEqual("File", metaDataTablesHdr.TableDefinitions[31].Name);
            Assert.AreEqual((uint) 0x38, metaDataTablesHdr.TableDefinitions[31].NumOfRows);
            Assert.AreEqual("ExportedType", metaDataTablesHdr.TableDefinitions[32].Name);
            Assert.AreEqual((uint) 0x39, metaDataTablesHdr.TableDefinitions[32].NumOfRows);
            Assert.AreEqual("ManifestResource", metaDataTablesHdr.TableDefinitions[33].Name);
            Assert.AreEqual((uint) 0x40, metaDataTablesHdr.TableDefinitions[33].NumOfRows);
            Assert.AreEqual("NestedClass", metaDataTablesHdr.TableDefinitions[34].Name);
            Assert.AreEqual((uint) 0x41, metaDataTablesHdr.TableDefinitions[34].NumOfRows);
            Assert.AreEqual("GenericParam", metaDataTablesHdr.TableDefinitions[35].Name);
            Assert.AreEqual((uint) 0x42, metaDataTablesHdr.TableDefinitions[35].NumOfRows);
            Assert.AreEqual("MethodSpec", metaDataTablesHdr.TableDefinitions[36].Name);
            Assert.AreEqual((uint) 0x43, metaDataTablesHdr.TableDefinitions[36].NumOfRows);
            Assert.AreEqual("GenericParamConstraint", metaDataTablesHdr.TableDefinitions[37].Name);
            Assert.AreEqual((uint) 0x44, metaDataTablesHdr.TableDefinitions[37].NumOfRows);
        }
    }
}