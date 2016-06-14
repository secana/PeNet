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
    public class IMAGE_DATA_DIRECTORY_Test
    {
        private readonly byte[] _rawDataDirectory =
        {
            0xff, // Junk
            0xff,

            0x11, // VirtualAddress
            0x22,
            0x33,
            0x44,

            0x55, // Size
            0x66,
            0x77,
            0x88,

            0xff, // Junk
            0xff
        };

        [TestMethod]
        public void ConstructorWorks_Test()
        {
            var dataDirectory = new IMAGE_DATA_DIRECTORY(_rawDataDirectory, 2);

            Assert.AreEqual((uint) 0x44332211, dataDirectory.VirtualAddress);
            Assert.AreEqual((uint) 0x88776655, dataDirectory.Size);
        }
    }
}