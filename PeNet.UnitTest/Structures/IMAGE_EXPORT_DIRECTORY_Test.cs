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
    public class IMAGE_EXPORT_DIRECTORY_Test
    {
        private readonly byte[] _rawExportDirectory =
        {
            0xff, // Junk
            0xff,

            0x00, // Characteristics
            0x11,
            0x22,
            0x33,

            0x44, // TimeDataStamp
            0x55,
            0x66,
            0x77,

            0x88, // MajorVersion
            0x99,

            0xaa, // MinorVersion
            0xbb,

            0xcc, // Name
            0xdd,
            0xee,
            0xff,

            0x22, // Base
            0x33,
            0x44,
            0x55,

            0x11, // NumberOfFunctions
            0x22,
            0x33,
            0x44,

            0x55, // NumberOfNames
            0x66,
            0x77,
            0x88,

            0x99, // AddressOfFunctions
            0xaa,
            0xbb,
            0xcc,

            0xdd, // AddressOfNames
            0xee,
            0xff,
            0x00,

            0x22, // AddressOfNameOrdinals
            0x33,
            0x44,
            0x55
        };

        [TestMethod]
        public void ImageExportDirectoryConstructorWorks_Test()
        {
            var exportDirectory = new IMAGE_EXPORT_DIRECTORY(_rawExportDirectory, 2);

            Assert.AreEqual((uint) 0x33221100, exportDirectory.Characteristics);
            Assert.AreEqual((uint) 0x77665544, exportDirectory.TimeDateStamp);
            Assert.AreEqual((ushort) 0x9988, exportDirectory.MajorVersion);
            Assert.AreEqual((ushort) 0xbbaa, exportDirectory.MinorVersion);
            Assert.AreEqual((uint) 0xffeeddcc, exportDirectory.Name);
            Assert.AreEqual((uint) 0x55443322, exportDirectory.Base);
            Assert.AreEqual((uint) 0x44332211, exportDirectory.NumberOfFunctions);
            Assert.AreEqual((uint) 0x88776655, exportDirectory.NumberOfNames);
            Assert.AreEqual((uint) 0xccbbaa99, exportDirectory.AddressOfFunctions);
            Assert.AreEqual((uint) 0x00ffeedd, exportDirectory.AddressOfNames);
            Assert.AreEqual((uint) 0x55443322, exportDirectory.AddressOfNameOrdinals);
        }
    }
}