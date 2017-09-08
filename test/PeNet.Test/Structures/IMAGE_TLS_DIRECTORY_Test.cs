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
    
    public class IMAGE_TLS_DIRECTORY_Test
    {
        [Fact]
        public void ImageTlsDirectory64ConstructorWorks_Test()
        {
            var tlsDirectory = new IMAGE_TLS_DIRECTORY(RawStructures.RawTlsDirectory64, 2, true);

            Assert.Equal((ulong) 0x7766554433221100, tlsDirectory.StartAddressOfRawData);
            Assert.Equal((ulong) 0xbbaa998877665544, tlsDirectory.EndAddressOfRawData);
            Assert.Equal((ulong) 0x221100ffeeddccbb, tlsDirectory.AddressOfIndex);
            Assert.Equal((ulong) 0xaa99887766554433, tlsDirectory.AddressOfCallBacks);
            Assert.Equal((uint) 0x44332211, tlsDirectory.SizeOfZeroFill);
            Assert.Equal((uint) 0x99887766, tlsDirectory.Characteristics);
        }

        [Fact]
        public void ImageTlsDirectory32ConstructorWorks_Test()
        {
            var tlsDirectory = new IMAGE_TLS_DIRECTORY(RawStructures.RawTlsDirectory32, 2, false);

            Assert.Equal((ulong)0x33221100, tlsDirectory.StartAddressOfRawData);
            Assert.Equal((ulong)0x77665544, tlsDirectory.EndAddressOfRawData);
            Assert.Equal((ulong)0xeeddccbb, tlsDirectory.AddressOfIndex);
            Assert.Equal((ulong)0x66554433, tlsDirectory.AddressOfCallBacks);
            Assert.Equal((uint)0x44332211, tlsDirectory.SizeOfZeroFill);
            Assert.Equal((uint)0x99887766, tlsDirectory.Characteristics);
        }
    }
}