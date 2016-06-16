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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class RUNTIME_FUNCTION_Test
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RuntimeFunctionConstructorWorks_Test()
        {
            var runtimeFunction = new RUNTIME_FUNCTION(RawStructures.RawRuntimeFunction, 2, null);

            Assert.AreEqual((uint) 0x33221100, runtimeFunction.FunctionStart);
            Assert.AreEqual((uint) 0x77665544, runtimeFunction.FunctionEnd);
            Assert.AreEqual(0xbbaa9988, runtimeFunction.UnwindInfo);
                // No valid UnwindInfo -> Throws ArgumentNullException
        }
    }
}