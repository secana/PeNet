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
            Assert.AreEqual((ulong) 0x221100ffeeddccbb, metaDataTablesHdr.MaskValid);
            Assert.AreEqual((ulong) 0xaa99887766554433, metaDataTablesHdr.MaskSorted);
        }
    }
}