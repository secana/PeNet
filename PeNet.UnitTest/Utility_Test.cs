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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeNet.UnitTest
{
    [TestClass]
    public class Utility_Test
    {
        [TestMethod]
        public void ResolveCOMImageFlagsSingleFlags_Test()
        {
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", Utility.ResolveCOMImageFlags(0x00000001).First());
            Assert.AreEqual("COMIMAGE_FLAGS_32BITREQUIRED", Utility.ResolveCOMImageFlags(0x00000002).First());
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", Utility.ResolveCOMImageFlags(0x000000004).First());
            Assert.AreEqual("COMIMAGE_FLAGS_STRONGNAMESIGNED", Utility.ResolveCOMImageFlags(0x00000008).First());
            Assert.AreEqual("COMIMAGE_FLAGS_NATIVE_ENTRYPOINT", Utility.ResolveCOMImageFlags(0x00000010).First());
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", Utility.ResolveCOMImageFlags(0x00010000).First());
        }

        [TestMethod]
        public void ResolveCOMIMagesFlagsMultipleFlags_Test()
        {
            uint flags = 0x00010005;
            var resolved = Utility.ResolveCOMImageFlags(flags);

            Assert.AreEqual(3, resolved.Count);
            Assert.AreEqual("COMIMAGE_FLAGS_ILONLY", resolved[0]);
            Assert.AreEqual("COMIMAGE_FLAGS_IL_LIBRARY", resolved[1]);
            Assert.AreEqual("COMIMAGE_FLAGS_TRACKDEBUGDATA", resolved[2]);
        }
    }
}