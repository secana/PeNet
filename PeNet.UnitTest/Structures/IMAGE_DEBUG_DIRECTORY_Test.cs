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
    public class IMAGE_DEBUG_DIRECTORY_Test
    {
        [TestMethod]
        public void ImageDebugDirectoryConstructorWorks_Test()
        {
            var idd = new IMAGE_DEBUG_DIRECTORY(RawStructures.RawDebugDirectory, 2);

            Assert.AreEqual((uint) 0x44332211, idd.Characteristics);
            Assert.AreEqual(0x88776655, idd.TimeDateStamp);
            Assert.AreEqual((ushort) 0xaa99, idd.MajorVersion);
            Assert.AreEqual((ushort) 0xccbb, idd.MinorVersion);
            Assert.AreEqual((uint) 0x11ffeedd, idd.Type);
            Assert.AreEqual((uint) 0x55443322, idd.SizeOfData);
            Assert.AreEqual(0x99887766, idd.AddressOfRawData);
            Assert.AreEqual(0xddccbbaa, idd.PointerToRawData);
        }
    }
}