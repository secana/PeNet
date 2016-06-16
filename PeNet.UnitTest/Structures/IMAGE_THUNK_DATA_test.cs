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
    public class IMAGE_THUNK_DATA_Test
    {
        [TestMethod]
        public void ImageThunkData64ConstructorWorks_Test()
        {
            var thunkData64 = new IMAGE_THUNK_DATA(RawStructures.RawThunkData64, 2, true);

            Assert.AreEqual((ulong) 0x7766554433221100, thunkData64.AddressOfData);
            Assert.AreEqual((ulong) 0x7766554433221100, thunkData64.ForwarderString);
            Assert.AreEqual((ulong) 0x7766554433221100, thunkData64.Function);
            Assert.AreEqual((ulong) 0x7766554433221100, thunkData64.Ordinal);
        }

        [TestMethod]
        public void ImageThunkData32ConstructorWorks_Test()
        {
            var thunkData32 = new IMAGE_THUNK_DATA(RawStructures.RawThunkData32, 2, false);

            Assert.AreEqual((ulong) 0x33221100, thunkData32.AddressOfData);
            Assert.AreEqual((ulong) 0x33221100, thunkData32.ForwarderString);
            Assert.AreEqual((ulong) 0x33221100, thunkData32.Function);
            Assert.AreEqual((ulong) 0x33221100, thunkData32.Ordinal);
        }
    }
}