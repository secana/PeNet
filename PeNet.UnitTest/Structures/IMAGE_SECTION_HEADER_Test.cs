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
    public class IMAGE_SECTION_HEADER_Test
    {
        [TestMethod]
        public void ImageSectionHeaderConstructorWorks_Test()
        {
            var sectionHeader = new IMAGE_SECTION_HEADER(RawStructures.RawSectionHeader, 2);

            Assert.AreEqual(".data", Utility.ResolveSectionName(sectionHeader.Name));
            Assert.AreEqual((uint) 0x33221100, sectionHeader.VirtualSize);
            Assert.AreEqual((uint) 0x77665544, sectionHeader.VirtualAddress);
            Assert.AreEqual((uint) 0xbbaa9988, sectionHeader.SizeOfRawData);
            Assert.AreEqual((uint) 0xffeeddcc, sectionHeader.PointerToRawData);
            Assert.AreEqual((uint) 0x44332211, sectionHeader.PointerToRelocations);
            Assert.AreEqual((uint) 0x88776655, sectionHeader.PointerToLinenumbers);
            Assert.AreEqual((ushort) 0xaa99, sectionHeader.NumberOfRelocations);
            Assert.AreEqual((ushort) 0xccbb, sectionHeader.NumberOfLinenumbers);
            Assert.AreEqual((uint) 0x00ffeedd, sectionHeader.Characteristics);
        }
    }
}