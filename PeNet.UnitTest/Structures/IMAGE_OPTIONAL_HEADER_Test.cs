using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class IMAGE_OPTIONAL_HEADER_Test
    {
        private readonly byte[] _rawImageOptionalHeader64Bit =
        {
            0xff, // Crap for offset
            0xff, // Crap

            0x0b, // Magic (32 Bit)
            0x01,

            0x11, // MajorLinkerVersion

            0x33, // MinorLinkerVersion

            0x44, // SizeOfCode
            0x55,
            0x11,
            0x22,

            0x77, // SizeOfInitializedData
            0x88,
            0x99,
            0xaa,

            0x44, // SizeOfUnitializedData
            0x55,
            0x11,
            0x22,

            0x22, // AddressOfEntryPoint
            0x88,
            0x77,
            0xaa,

            0x77, // BaseOfCode
            0x88,
            0xff,
            0xaa,

            0x44, // ImageBase
            0xcc,
            0x99,
            0xaa,
            0x33,
            0x22,
            0x11,
            0x00,

            0x77, // SectionAlignment
            0xcc,
            0x99,
            0xaa,

            0xdd, // FileAlignment
            0xff,
            0x99,
            0xaa,

            0x77, // MajorOperatingSystemVersion
            0x88,

            0x99, // MinorOperatingSystemVersion
            0xaa,

            0xff, // MajorImageVersion
            0x44,

            0xdd, // MinorImageVersion
            0xee,

            0xbb, // MajorSubsystemVersion
            0x88,

            0xee, // MinorSubsystemVersion
            0xaa,

            0x77, // Win32VerionValue
            0x88,
            0x99,
            0xaa,

            0xaa, // SizeOfImage
            0xbb,
            0x99,
            0xaa,

            0x77, // SizeOfHeaders
            0x88,
            0xff,
            0xaa,

            0x77, // Checksum
            0x88,
            0x99,
            0xcc,

            0x77, // Subsystem
            0x88,

            0x99, // DllCharacteristics
            0xaa,

            0xff, // SizeOfStackReserve
            0xdd,
            0x99,
            0xaa,
            0x22,
            0x33,
            0x44,
            0x55,


            0x88, // SizeOfStackCommit
            0x88,
            0x99,
            0xaa,
            0x88,
            0x88,
            0x99,
            0xaa,

            0xee, // SizeOfHeapReserve
            0xcc,
            0x99,
            0xaa,
            0xee,
            0xcc,
            0x99,
            0xaa,

            0x77, // SizeOfHeapCommit
            0x11,
            0x99,
            0xaa,
            0x77,
            0x11,
            0x99,
            0xaa,

            0x22, // LoaderFlags
            0x88,
            0x99,
            0xaa,

            0x05, // NumberOfRvaAndSizes
            0x00,
            0x00,
            0x00,

            // Data Directories

            0x11, // Virtual Address Export
            0x22,
            0x33,
            0x44,

            0x33, // Size of Export
            0x22,
            0x33,
            0x44,

            0x44, // Virtual Address Import
            0x22,
            0x33,
            0x44,

            0x55, // Size of Import
            0x22,
            0x33,
            0x44,

            0x66, // Virtual Address Resource
            0x77,
            0x88,
            0x44,

            0x99, // Size of Resource
            0xaa,
            0x33,
            0x44,

            0x11, // Virtual Address Exception
            0x22,
            0x88,
            0x44,

            0x33, // Size of Exception
            0x44,
            0x33,
            0x44,

            0xbb, // Virtual Address Security
            0x22,
            0x33,
            0x44,

            0xcc, // Size of Security
            0x22,
            0x33,
            0x44,

            0xdd, // Virtual Address Basereloc
            0x22,
            0x33,
            0x44,

            0xee, // Size of Basereloc
            0x22,
            0x33,
            0x44,

            0xff, // Virtual Address Debug
            0x22,
            0x33,
            0x44,

            0x11, // Size of Debug
            0x33,
            0x33,
            0x44,

            0x11, // Virtual Address Copyright
            0x44,
            0x33,
            0x44,

            0x11, // Size of Copyright
            0x55,
            0x33,
            0x44,

            0x11, // Virtual Address Globalprt
            0x66,
            0x33,
            0x44,

            0x11, // Size of Globalprt
            0x77,
            0x33,
            0x44,

            0x11, // Virtual Address TLS
            0x88,
            0x33,
            0x44,

            0x11, // Size of TLS
            0x99,
            0x33,
            0x44,

            0x11, // Virtual Address Load_Config
            0xaa,
            0x33,
            0x44,

            0x11, // Size of Load_Config
            0xbb,
            0x33,
            0x44,

            0xee, // Virtual Address Bound_Import
            0x99,
            0x33,
            0x44,

            0x99, // Size of Bound_Import
            0x00,
            0x33,
            0x44,

            0x11, // Virtual Address IAT
            0xcc,
            0x33,
            0x44,

            0x11, // Size of IAT
            0xdd,
            0x33,
            0x44,

            0x11, // Virtual Address Delay_Import
            0xee,
            0x33,
            0x44,

            0x11, // Size of Delay_Import
            0xff,
            0x33,
            0x44,

            0x11, // Virtual Address Com_Descriptor
            0x22,
            0x44,
            0x44,

            0x11, // Size of Com_Descriptor
            0x22,
            0x55,
            0x44,

            0x11, // Virtual Address of Reserved
            0x22,
            0xcc,
            0xff,

            0x11, // Size of Reserved
            0x22,
            0xcc,
            0x44
        };

        private readonly byte[] _rawImageOptionalHeader32Bit =
        {
            0xff, // Crap for offset
            0xff, // Crap

            0x0b, // Magic (32 Bit)
            0x01,

            0x11, // MajorLinkerVersion

            0x33, // MinorLinkerVersion

            0x44, // SizeOfCode
            0x55,
            0x11,
            0x22,

            0x77, // SizeOfInitializedData
            0x88,
            0x99,
            0xaa,

            0x44, // SizeOfUnitializedData
            0x55,
            0x11,
            0x22,

            0x22, // AddressOfEntryPoint
            0x88,
            0x77,
            0xaa,

            0x77, // BaseOfCode
            0x88,
            0xff,
            0xaa,

            0x44, // BaseOfData
            0x88,
            0x99,
            0xaa,

            0x44, // ImageBase
            0xcc,
            0x99,
            0xaa,

            0x77, // SectionAlignment
            0xcc,
            0x99,
            0xaa,

            0xdd, // FileAlignment
            0xff,
            0x99,
            0xaa,

            0x77, // MajorOperatingSystemVersion
            0x88,

            0x99, // MinorOperatingSystemVersion
            0xaa,

            0xff, // MajorImageVersion
            0x44,

            0xdd, // MinorImageVersion
            0xee,

            0xbb, // MajorSubsystemVersion
            0x88,

            0xee, // MinorSubsystemVersion
            0xaa,

            0x77, // Win32VerionValue
            0x88,
            0x99,
            0xaa,

            0xaa, // SizeOfImage
            0xbb,
            0x99,
            0xaa,

            0x77, // SizeOfHeaders
            0x88,
            0xff,
            0xaa,

            0x77, // Checksum
            0x88,
            0x99,
            0xcc,

            0x77, // Subsystem
            0x88,

            0x99, // DllCharacteristics
            0xaa,

            0xff, // SizeOfStackReserve
            0xdd,
            0x99,
            0xaa,

            0x88, // SizeOfStackCommit
            0x88,
            0x99,
            0xaa,

            0xee, // SizeOfHeapReserve
            0xcc,
            0x99,
            0xaa,

            0x77, // SizeOfHeapCommit
            0x11,
            0x99,
            0xaa,

            0x22, // LoaderFlags
            0x88,
            0x99,
            0xaa,

            0x05, // NumberOfRvaAndSizes
            0x00,
            0x00,
            0x00,

            // Data Directories

            0x11, // Virtual Address Export
            0x22,
            0x33,
            0x44,

            0x33, // Size of Export
            0x22,
            0x33,
            0x44,

            0x44, // Virtual Address Import
            0x22,
            0x33,
            0x44,

            0x55, // Size of Import
            0x22,
            0x33,
            0x44,

            0x66, // Virtual Address Resource
            0x77,
            0x88,
            0x44,

            0x99, // Size of Resource
            0xaa,
            0x33,
            0x44,

            0x11, // Virtual Address Exception
            0x22,
            0x88,
            0x44,

            0x33, // Size of Exception
            0x44,
            0x33,
            0x44,

            0xbb, // Virtual Address Security
            0x22,
            0x33,
            0x44,

            0xcc, // Size of Security
            0x22,
            0x33,
            0x44,

            0xdd, // Virtual Address Basereloc
            0x22,
            0x33,
            0x44,

            0xee, // Size of Basereloc
            0x22,
            0x33,
            0x44,

            0xff, // Virtual Address Debug
            0x22,
            0x33,
            0x44,

            0x11, // Size of Debug
            0x33,
            0x33,
            0x44,

            0x11, // Virtual Address Copyright
            0x44,
            0x33,
            0x44,

            0x11, // Size of Copyright
            0x55,
            0x33,
            0x44,

            0x11, // Virtual Address Globalprt
            0x66,
            0x33,
            0x44,

            0x11, // Size of Globalprt
            0x77,
            0x33,
            0x44,

            0x11, // Virtual Address TLS
            0x88,
            0x33,
            0x44,

            0x11, // Size of TLS
            0x99,
            0x33,
            0x44,

            0x11, // Virtual Address Load_Config
            0xaa,
            0x33,
            0x44,

            0x11, // Size of Load_Config
            0xbb,
            0x33,
            0x44,

            0xee, // Virtual Address Bound_Import
            0x99,
            0x33,
            0x44,

            0x99, // Size of Bound_Import
            0x00,
            0x33,
            0x44,

            0x11, // Virtual Address IAT
            0xcc,
            0x33,
            0x44,

            0x11, // Size of IAT
            0xdd,
            0x33,
            0x44,

            0x11, // Virtual Address Delay_Import
            0xee,
            0x33,
            0x44,

            0x11, // Size of Delay_Import
            0xff,
            0x33,
            0x44,

            0x11, // Virtual Address Com_Descriptor
            0x22,
            0x44,
            0x44,

            0x11, // Size of Com_Descriptor
            0x22,
            0x55,
            0x44,

            0x11, // Virtual Address of Reserved
            0x22,
            0xcc,
            0xff,

            0x11, // Size of Reserved
            0x22,
            0xcc,
            0x44
        };

        [TestMethod]
        public void Constructor64BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(_rawImageOptionalHeader64Bit, 2, true);

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
        public void Constructor32BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(_rawImageOptionalHeader32Bit, 2, false);

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