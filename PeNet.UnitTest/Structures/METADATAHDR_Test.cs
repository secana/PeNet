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
    public class METADATAHDR_Test
    {
        [TestMethod]
        public void MetaDataHdrConstructorWorks_Test()
        {
            var metaDataHdr = new METADATAHDR(RawDotNetStructures.RawMetaDataHeader, 2);
            Assert.AreEqual((uint) 0x55443322, metaDataHdr.Signature);
            Assert.AreEqual((ushort) 0x7766, metaDataHdr.MajorVersion);
            Assert.AreEqual((ushort) 0x9988, metaDataHdr.MinorVersion);
            Assert.AreEqual((uint) 0xddccbbaa, metaDataHdr.Reserved);
            Assert.AreEqual((uint) 0x0000000C, metaDataHdr.VersionLength);
            Assert.AreEqual("v4.0.30319", metaDataHdr.Version);
            Assert.AreEqual((ushort) 0x2211, metaDataHdr.Flags);
            Assert.AreEqual((ushort) 0x0002, metaDataHdr.Streams);

            Assert.AreEqual(2, metaDataHdr.MetaDataStreamsHdrs.Length);
            Assert.AreEqual((uint) 0x6C, metaDataHdr.MetaDataStreamsHdrs[0].offset);
            Assert.AreEqual((uint) 0x1804, metaDataHdr.MetaDataStreamsHdrs[0].size);
            Assert.AreEqual("#~", metaDataHdr.MetaDataStreamsHdrs[0].streamName);
            Assert.AreEqual((uint) 0x1870, metaDataHdr.MetaDataStreamsHdrs[1].offset);
            Assert.AreEqual((uint) 0x1468, metaDataHdr.MetaDataStreamsHdrs[1].size);
            Assert.AreEqual("#Strings", metaDataHdr.MetaDataStreamsHdrs[1].streamName);
        }
    }
}