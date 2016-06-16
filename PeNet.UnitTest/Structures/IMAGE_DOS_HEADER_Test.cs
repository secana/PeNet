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
    public class IMAGE_DOS_HEADER_Test
    {
        [TestMethod]
        public void ImageDosHeaderConstructorWorks_Test()
        {
            var idh = new IMAGE_DOS_HEADER(RawStructures.RawDosHeader, 0);
            Assert.AreEqual((uint) 0x1100, idh.e_magic);
            Assert.AreEqual((uint) 0x3322, idh.e_cblp);
            Assert.AreEqual((uint) 0x5544, idh.e_cp);
            Assert.AreEqual((uint) 0x7766, idh.e_crlc);
            Assert.AreEqual((uint) 0x9988, idh.e_cparhdr);
            Assert.AreEqual((uint) 0xbbaa, idh.e_minalloc);
            Assert.AreEqual((uint) 0xddcc, idh.e_maxalloc);
            Assert.AreEqual((uint) 0x00ff, idh.e_ss);
            Assert.AreEqual((uint) 0x2211, idh.e_sp);
            Assert.AreEqual((uint) 0x4433, idh.e_csum);
            Assert.AreEqual((uint) 0x6655, idh.e_ip);
            Assert.AreEqual((uint) 0x8877, idh.e_cs);
            Assert.AreEqual((uint) 0xaa99, idh.e_lfarlc);
            Assert.AreEqual((uint) 0xccbb, idh.e_ovno);
            AssertEqual(new ushort[]
            {
                0xeedd,
                0x00ff,
                0x2211,
                0x4433
            }, idh.e_res);
            Assert.AreEqual((uint) 0x6655, idh.e_oemid);
            Assert.AreEqual((uint) 0x8877, idh.e_oeminfo);
            AssertEqual(new ushort[]
            {
                0xaa99,
                0xccbb,
                0xeedd,
                0x11ff,
                0x3322,
                0x5544,
                0x7766,
                0x9988,
                0xbbaa,
                0xbbcc
            }, idh.e_res2);
        }

        private void AssertEqual(ushort[] expected, ushort[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}