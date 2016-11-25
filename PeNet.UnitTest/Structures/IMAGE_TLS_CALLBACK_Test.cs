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

using Xunit;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
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
    }
}