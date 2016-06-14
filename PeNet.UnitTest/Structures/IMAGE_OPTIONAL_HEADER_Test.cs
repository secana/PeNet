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
    public class IMAGE_OPTIONAL_HEADER_Test
    {
        [TestMethod]
        public void OptionalHeaderConstructor64BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(RawStructures.RawImageOptionalHeader64Bit, 2, true);

            AssertCommonOptHeaderProperties(optHeader);
            Assert.AreEqual((ulong)0x00112233aa99cc44, optHeader.ImageBase);
            Assert.AreEqual((ulong)0x55443322aa99ddff, optHeader.SizeOfStackReserve);
            Assert.AreEqual((ulong)0xaa998888aa998888, optHeader.SizeOfStackCommit);
            Assert.AreEqual((ulong)0xaa99cceeaa99ccee, optHeader.SizeOfHeapReserve);
            Assert.AreEqual((ulong)0xaa991177aa991177, optHeader.SizeOfHeapCommit);

            // Data directories
            AssertDataDirectories(optHeader.DataDirectory);
        }

        [TestMethod]
        public void OptionalHeaderConstructor32BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(RawStructures.RawImageOptionalHeader32Bit, 2, false);

            AssertCommonOptHeaderProperties(optHeader);
            Assert.AreEqual((uint)0xaa998844, optHeader.BaseOfData);
            Assert.AreEqual((uint)0xaa99cc44, optHeader.ImageBase);
            Assert.AreEqual((uint)0xaa99ddff, optHeader.SizeOfStackReserve);
            Assert.AreEqual((uint)0xaa998888, optHeader.SizeOfStackCommit);
            Assert.AreEqual((uint)0xaa99ccee, optHeader.SizeOfHeapReserve);
            Assert.AreEqual((uint)0xaa991177, optHeader.SizeOfHeapCommit);

            // Data directories
            AssertDataDirectories(optHeader.DataDirectory);
        }

        private void AssertCommonOptHeaderProperties(IMAGE_OPTIONAL_HEADER optHeader)
        {
            Assert.AreEqual((ushort)0x010b, optHeader.Magic);
            Assert.AreEqual((ushort)0x11, optHeader.MajorLinkerVersion);
            Assert.AreEqual((ushort)0x33, optHeader.MinorLinkerVersion);
            Assert.AreEqual((uint)0x22115544, optHeader.SizeOfCode);
            Assert.AreEqual((uint)0xaa998877, optHeader.SizeOfInitializedData);
            Assert.AreEqual((uint)0x22115544, optHeader.SizeOfUninitializedData);
            Assert.AreEqual((uint)0xaa778822, optHeader.AddressOfEntryPoint);
            Assert.AreEqual((uint)0xaaff8877, optHeader.BaseOfCode);
            Assert.AreEqual((uint)0xaa99cc77, optHeader.SectionAlignment);
            Assert.AreEqual((uint)0xaa99ffdd, optHeader.FileAlignment);
            Assert.AreEqual((ushort)0x8877, optHeader.MajorOperatingSystemVersion);
            Assert.AreEqual((ushort)0xaa99, optHeader.MinorOperatingSystemVersion);
            Assert.AreEqual((ushort)0x44ff, optHeader.MajorImageVersion);
            Assert.AreEqual((ushort)0xeedd, optHeader.MinorImageVersion);
            Assert.AreEqual((ushort)0x88bb, optHeader.MajorSubsystemVersion);
            Assert.AreEqual((ushort)0xaaee, optHeader.MinorSubsystemVersion);
            Assert.AreEqual((uint)0xaa998877, optHeader.Win32VersionValue);
            Assert.AreEqual((uint)0xaaff8877, optHeader.SizeOfHeaders);
            Assert.AreEqual((uint)0xcc998877, optHeader.CheckSum);
            Assert.AreEqual((ushort)0x8877, optHeader.Subsystem);
            Assert.AreEqual((ushort)0xaa99, optHeader.DllCharacteristics);
            Assert.AreEqual((uint)0xaa998822, optHeader.LoaderFlags);
            Assert.AreEqual((uint)0x00000005, optHeader.NumberOfRvaAndSizes);

        }
        private void AssertDataDirectories(IMAGE_DATA_DIRECTORY[] dataDirectories)
        {
            Assert.AreEqual(16, dataDirectories.Length);

            Assert.AreEqual((uint)0x44332211, dataDirectories[(int)Constants.DataDirectoryIndex.Export].VirtualAddress);
            Assert.AreEqual((uint)0x44332233, dataDirectories[(int)Constants.DataDirectoryIndex.Export].Size);
            Assert.AreEqual((uint)0x44332244, dataDirectories[(int)Constants.DataDirectoryIndex.Import].VirtualAddress);
            Assert.AreEqual((uint)0x44332255, dataDirectories[(int)Constants.DataDirectoryIndex.Import].Size);
            Assert.AreEqual((uint)0x44887766, dataDirectories[(int)Constants.DataDirectoryIndex.Resource].VirtualAddress);
            Assert.AreEqual((uint)0x4433aa99, dataDirectories[(int)Constants.DataDirectoryIndex.Resource].Size);
            Assert.AreEqual((uint)0x44882211, dataDirectories[(int)Constants.DataDirectoryIndex.Exception].VirtualAddress);
            Assert.AreEqual((uint)0x44334433, dataDirectories[(int)Constants.DataDirectoryIndex.Exception].Size);
            Assert.AreEqual((uint)0x443322bb, dataDirectories[(int)Constants.DataDirectoryIndex.Security].VirtualAddress);
            Assert.AreEqual((uint)0x443322cc, dataDirectories[(int)Constants.DataDirectoryIndex.Security].Size);
            Assert.AreEqual((uint)0x443322dd, dataDirectories[(int)Constants.DataDirectoryIndex.BaseReloc].VirtualAddress);
            Assert.AreEqual((uint)0x443322ee, dataDirectories[(int)Constants.DataDirectoryIndex.BaseReloc].Size);
            Assert.AreEqual((uint)0x443322ff, dataDirectories[(int)Constants.DataDirectoryIndex.Debug].VirtualAddress);
            Assert.AreEqual((uint)0x44333311, dataDirectories[(int)Constants.DataDirectoryIndex.Debug].Size);
            Assert.AreEqual((uint)0x44334411, dataDirectories[(int)Constants.DataDirectoryIndex.Copyright].VirtualAddress);
            Assert.AreEqual((uint)0x44335511, dataDirectories[(int)Constants.DataDirectoryIndex.Copyright].Size);
            Assert.AreEqual((uint)0x44336611, dataDirectories[(int)Constants.DataDirectoryIndex.Globalptr].VirtualAddress);
            Assert.AreEqual((uint)0x44337711, dataDirectories[(int)Constants.DataDirectoryIndex.Globalptr].Size);
            Assert.AreEqual((uint)0x44338811, dataDirectories[(int)Constants.DataDirectoryIndex.TLS].VirtualAddress);
            Assert.AreEqual((uint)0x44339911, dataDirectories[(int)Constants.DataDirectoryIndex.TLS].Size);
            Assert.AreEqual((uint)0x4433aa11, dataDirectories[(int)Constants.DataDirectoryIndex.LoadConfig].VirtualAddress);
            Assert.AreEqual((uint)0x4433bb11, dataDirectories[(int)Constants.DataDirectoryIndex.LoadConfig].Size);
            Assert.AreEqual((uint)0x443399ee, dataDirectories[(int)Constants.DataDirectoryIndex.BoundImport].VirtualAddress);
            Assert.AreEqual((uint)0x44330099, dataDirectories[(int)Constants.DataDirectoryIndex.BoundImport].Size);
            Assert.AreEqual((uint)0x4433cc11, dataDirectories[(int)Constants.DataDirectoryIndex.IAT].VirtualAddress);
            Assert.AreEqual((uint)0x4433dd11, dataDirectories[(int)Constants.DataDirectoryIndex.IAT].Size);
            Assert.AreEqual((uint)0x4433ee11, dataDirectories[(int)Constants.DataDirectoryIndex.DelayImport].VirtualAddress);
            Assert.AreEqual((uint)0x4433ff11, dataDirectories[(int)Constants.DataDirectoryIndex.DelayImport].Size);
            Assert.AreEqual((uint)0x44442211, dataDirectories[(int)Constants.DataDirectoryIndex.COM_Descriptor].VirtualAddress);
            Assert.AreEqual((uint)0x44552211, dataDirectories[(int)Constants.DataDirectoryIndex.COM_Descriptor].Size);
            Assert.AreEqual((uint)0xffcc2211, dataDirectories[(int)Constants.DataDirectoryIndex.Reserved].VirtualAddress);
            Assert.AreEqual((uint)0x44cc2211, dataDirectories[(int)Constants.DataDirectoryIndex.Reserved].Size);
        }
    }
}