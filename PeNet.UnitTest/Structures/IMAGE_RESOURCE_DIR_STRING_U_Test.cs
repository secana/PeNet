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
    public class IMAGE_RESOURCE_DIR_STRING_U_Test
    {
        private readonly byte[] _rawResourceDirStringU =
        {
            0xff, // Junk
            0xff,

            0x0b, // Length
            0x00,

            // Resource name
            72, // H
            0,
            101, // e
            0,
            108, // l
            0,
            108, // l
            0,
            111, // o
            0,
            32, // ' '
            0,
            87, // W
            0,
            111, // o
            0,
            114, // r
            0,
            108, // l
            0,
            100, // d
            0
        };

        [TestMethod]
        public void ImageResourceDirStringUConstructorWorks_Test()
        {
            var resourceDirStringU = new IMAGE_RESOURCE_DIR_STRING_U(_rawResourceDirStringU, 2);
            Assert.AreEqual((uint) 0x000b, resourceDirStringU.Length);
            Assert.AreEqual("Hello World", resourceDirStringU.NameString);
        }
    }
}