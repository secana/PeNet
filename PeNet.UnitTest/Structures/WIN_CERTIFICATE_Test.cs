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
    public class WIN_CERTIFICATE_Test
    {
        [TestMethod]
        public void WinCertificateConstructorWorks_Test()
        {
            var winCertifiacte = new WIN_CERTIFICATE(RawStructures.RawWinCertificate, 2);
            Assert.AreEqual((uint) 0x0000000b, winCertifiacte.dwLength);
            Assert.AreEqual((ushort) 0x5544, winCertifiacte.wRevision);
            Assert.AreEqual((ushort) 0x7766, winCertifiacte.wCertificateType);
            Assert.AreEqual((byte) 0x11, winCertifiacte.bCertificate[0]);
            Assert.AreEqual((byte) 0x22, winCertifiacte.bCertificate[1]);
            Assert.AreEqual((byte) 0x33, winCertifiacte.bCertificate[2]);
        }
    }
}