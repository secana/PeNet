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

using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class WIN_CERTIFICATE_Test
    {
        [Fact]
        public void WinCertificateConstructorWorks_Test()
        {
            var winCertifiacte = new WIN_CERTIFICATE(RawStructures.RawWinCertificate, 2);
            Assert.Equal((uint) 0x0000000b, winCertifiacte.dwLength);
            Assert.Equal((ushort) 0x5544, winCertifiacte.wRevision);
            Assert.Equal((ushort) 0x7766, winCertifiacte.wCertificateType);
            Assert.Equal((byte) 0x11, winCertifiacte.bCertificate[0]);
            Assert.Equal((byte) 0x22, winCertifiacte.bCertificate[1]);
            Assert.Equal((byte) 0x33, winCertifiacte.bCertificate[2]);
        }
    }
}