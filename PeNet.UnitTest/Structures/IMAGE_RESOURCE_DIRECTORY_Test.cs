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
    public class IMAGE_RESOURCE_DIRECTORY_Test
    {
        [TestMethod]
        public void ImageResourceDirectoryConstructorWorks_Test()
        {
            var resourceDirectory = new IMAGE_RESOURCE_DIRECTORY(RawStructures.RawResourceDirectory, 2, 2);
            Assert.AreEqual((uint) 0x33221100, resourceDirectory.Characteristics);
            Assert.AreEqual((uint) 0x77665544, resourceDirectory.TimeDateStamp);
            Assert.AreEqual((ushort) 0x9988, resourceDirectory.MajorVersion);
            Assert.AreEqual((ushort) 0xbbaa, resourceDirectory.MinorVersion);
            Assert.AreEqual((ushort) 0x0001, resourceDirectory.NumberOfNameEntries);
            Assert.AreEqual((ushort) 0x0001, resourceDirectory.NumberOfIdEntries);
            Assert.AreEqual((uint) 0x44332211, resourceDirectory.DirectoryEntries[0].Name);
            Assert.AreEqual((uint) 0x88776655, resourceDirectory.DirectoryEntries[0].OffsetToData);
            Assert.AreEqual((uint) 0x44332222 & 0xFFFF, resourceDirectory.DirectoryEntries[1].ID);
            Assert.AreEqual((uint) 0x88776622, resourceDirectory.DirectoryEntries[1].OffsetToData);
        }
    }
}