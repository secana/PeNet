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

using System.Linq;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_TLS_CALLBACK_Test
    {
        [Fact]
        public void ImageTlsCallback64ConstructorWorks_Test()
        {
            var tlsCallback = new IMAGE_TLS_CALLBACK(RawStructures.RawTlsCallback64, 2, true);
            Assert.Equal((ulong) 0x7766554433221100, tlsCallback.Callback);
        }

        [Fact]
        public void ImageTlsCallback32ConstructorWorks_Test()
        {
            var tlsCallback = new IMAGE_TLS_CALLBACK(RawStructures.RawTlsCallback32, 2, false);
            Assert.Equal((ulong)0x33221100, tlsCallback.Callback);
        }

        [Fact]
        public void TLSCallback_x86_Works_Test()
        {
            // Given
            var peFile = new PeFile(@"../../../Binaries/TLSCallback_x86.exe");

            // When
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.Equal(1, callbacks.Length);
            Assert.Equal((ulong) 0x004111CC, callbacks.First().Callback);
        }
    }
}