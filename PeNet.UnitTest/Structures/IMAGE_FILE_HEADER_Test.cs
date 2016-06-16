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
    public class IMAGE_FILE_HEADER_Test
    {
        [TestMethod]
        public void ImageFileHeaderConstructorWorks_Test()
        {
            var fileHeader = new IMAGE_FILE_HEADER(RawStructures.RawFileHeader, 2);
            Assert.AreEqual((ushort) 0x1100, fileHeader.Machine);
            Assert.AreEqual((ushort) 0x3322, fileHeader.NumberOfSections);
            Assert.AreEqual((uint) 0x77665544, fileHeader.TimeDateStamp);
            Assert.AreEqual(0xbbaa9988, fileHeader.PointerToSymbolTable);
            Assert.AreEqual(0xffeeddcc, fileHeader.NumberOfSymbols);
            Assert.AreEqual((ushort) 0x2211, fileHeader.SizeOfOptionalHeader);
            Assert.AreEqual((ushort) 0x4433, fileHeader.Characteristics);
        }
    }
}