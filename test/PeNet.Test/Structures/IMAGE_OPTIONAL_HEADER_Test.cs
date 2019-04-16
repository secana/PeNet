using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_OPTIONAL_HEADER_Test
    {
        [Fact]
        public void OptionalHeaderConstructor64BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(RawStructures.RawImageOptionalHeader64Bit, 2, true);

            AssertCommonOptHeaderProperties(optHeader);
            Assert.Equal((ulong) 0x00112233aa99cc44, optHeader.ImageBase);
            Assert.Equal((ulong) 0x55443322aa99ddff, optHeader.SizeOfStackReserve);
            Assert.Equal(0xaa998888aa998888, optHeader.SizeOfStackCommit);
            Assert.Equal(0xaa99cceeaa99ccee, optHeader.SizeOfHeapReserve);
            Assert.Equal(0xaa991177aa991177, optHeader.SizeOfHeapCommit);

            // Data directories
            AssertDataDirectories(optHeader.DataDirectory);
        }

        [Fact]
        public void OptionalHeaderConstructor32BitWorks_Test()
        {
            var optHeader = new IMAGE_OPTIONAL_HEADER(RawStructures.RawImageOptionalHeader32Bit, 2, false);

            AssertCommonOptHeaderProperties(optHeader);
            Assert.Equal(0xaa998844, optHeader.BaseOfData);
            Assert.Equal(0xaa99cc44, optHeader.ImageBase);
            Assert.Equal(0xaa99ddff, optHeader.SizeOfStackReserve);
            Assert.Equal(0xaa998888, optHeader.SizeOfStackCommit);
            Assert.Equal(0xaa99ccee, optHeader.SizeOfHeapReserve);
            Assert.Equal(0xaa991177, optHeader.SizeOfHeapCommit);

            // Data directories
            AssertDataDirectories(optHeader.DataDirectory);
        }

        private void AssertCommonOptHeaderProperties(IMAGE_OPTIONAL_HEADER optHeader)
        {
            Assert.Equal((ushort) 0x010b, optHeader.Magic);
            Assert.Equal((ushort) 0x11, optHeader.MajorLinkerVersion);
            Assert.Equal((ushort) 0x33, optHeader.MinorLinkerVersion);
            Assert.Equal((uint) 0x22115544, optHeader.SizeOfCode);
            Assert.Equal(0xaa998877, optHeader.SizeOfInitializedData);
            Assert.Equal((uint) 0x22115544, optHeader.SizeOfUninitializedData);
            Assert.Equal(0xaa778822, optHeader.AddressOfEntryPoint);
            Assert.Equal(0xaaff8877, optHeader.BaseOfCode);
            Assert.Equal(0xaa99cc77, optHeader.SectionAlignment);
            Assert.Equal(0xaa99ffdd, optHeader.FileAlignment);
            Assert.Equal((ushort) 0x8877, optHeader.MajorOperatingSystemVersion);
            Assert.Equal((ushort) 0xaa99, optHeader.MinorOperatingSystemVersion);
            Assert.Equal((ushort) 0x44ff, optHeader.MajorImageVersion);
            Assert.Equal((ushort) 0xeedd, optHeader.MinorImageVersion);
            Assert.Equal((ushort) 0x88bb, optHeader.MajorSubsystemVersion);
            Assert.Equal((ushort) 0xaaee, optHeader.MinorSubsystemVersion);
            Assert.Equal(0xaa998877, optHeader.Win32VersionValue);
            Assert.Equal(0xaaff8877, optHeader.SizeOfHeaders);
            Assert.Equal(0xcc998877, optHeader.CheckSum);
            Assert.Equal((ushort) 0x8877, optHeader.Subsystem);
            Assert.Equal((ushort) 0xaa99, optHeader.DllCharacteristics);
            Assert.Equal(0xaa998822, optHeader.LoaderFlags);
            Assert.Equal((uint) 0x00000005, optHeader.NumberOfRvaAndSizes);
        }

        private void AssertDataDirectories(IMAGE_DATA_DIRECTORY[] dataDirectories)
        {
            Assert.Equal(16, dataDirectories.Length);

            Assert.Equal((uint) 0x44332211, dataDirectories[(int) Constants.DataDirectoryIndex.Export].VirtualAddress);
            Assert.Equal((uint) 0x44332233, dataDirectories[(int) Constants.DataDirectoryIndex.Export].Size);
            Assert.Equal((uint) 0x44332244, dataDirectories[(int) Constants.DataDirectoryIndex.Import].VirtualAddress);
            Assert.Equal((uint) 0x44332255, dataDirectories[(int) Constants.DataDirectoryIndex.Import].Size);
            Assert.Equal((uint) 0x44887766,
                dataDirectories[(int) Constants.DataDirectoryIndex.Resource].VirtualAddress);
            Assert.Equal((uint) 0x4433aa99, dataDirectories[(int) Constants.DataDirectoryIndex.Resource].Size);
            Assert.Equal((uint) 0x44882211,
                dataDirectories[(int) Constants.DataDirectoryIndex.Exception].VirtualAddress);
            Assert.Equal((uint) 0x44334433, dataDirectories[(int) Constants.DataDirectoryIndex.Exception].Size);
            Assert.Equal((uint) 0x443322bb,
                dataDirectories[(int) Constants.DataDirectoryIndex.Security].VirtualAddress);
            Assert.Equal((uint) 0x443322cc, dataDirectories[(int) Constants.DataDirectoryIndex.Security].Size);
            Assert.Equal((uint) 0x443322dd,
                dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].VirtualAddress);
            Assert.Equal((uint) 0x443322ee, dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].Size);
            Assert.Equal((uint) 0x443322ff, dataDirectories[(int) Constants.DataDirectoryIndex.Debug].VirtualAddress);
            Assert.Equal((uint) 0x44333311, dataDirectories[(int) Constants.DataDirectoryIndex.Debug].Size);
            Assert.Equal((uint) 0x44334411,
                dataDirectories[(int) Constants.DataDirectoryIndex.Copyright].VirtualAddress);
            Assert.Equal((uint) 0x44335511, dataDirectories[(int) Constants.DataDirectoryIndex.Copyright].Size);
            Assert.Equal((uint) 0x44336611,
                dataDirectories[(int) Constants.DataDirectoryIndex.Globalptr].VirtualAddress);
            Assert.Equal((uint) 0x44337711, dataDirectories[(int) Constants.DataDirectoryIndex.Globalptr].Size);
            Assert.Equal((uint) 0x44338811, dataDirectories[(int) Constants.DataDirectoryIndex.TLS].VirtualAddress);
            Assert.Equal((uint) 0x44339911, dataDirectories[(int) Constants.DataDirectoryIndex.TLS].Size);
            Assert.Equal((uint) 0x4433aa11,
                dataDirectories[(int) Constants.DataDirectoryIndex.LoadConfig].VirtualAddress);
            Assert.Equal((uint) 0x4433bb11, dataDirectories[(int) Constants.DataDirectoryIndex.LoadConfig].Size);
            Assert.Equal((uint) 0x443399ee,
                dataDirectories[(int) Constants.DataDirectoryIndex.BoundImport].VirtualAddress);
            Assert.Equal((uint) 0x44330099, dataDirectories[(int) Constants.DataDirectoryIndex.BoundImport].Size);
            Assert.Equal((uint) 0x4433cc11, dataDirectories[(int) Constants.DataDirectoryIndex.IAT].VirtualAddress);
            Assert.Equal((uint) 0x4433dd11, dataDirectories[(int) Constants.DataDirectoryIndex.IAT].Size);
            Assert.Equal((uint) 0x4433ee11,
                dataDirectories[(int) Constants.DataDirectoryIndex.DelayImport].VirtualAddress);
            Assert.Equal((uint) 0x4433ff11, dataDirectories[(int) Constants.DataDirectoryIndex.DelayImport].Size);
            Assert.Equal((uint) 0x44442211,
                dataDirectories[(int) Constants.DataDirectoryIndex.COM_Descriptor].VirtualAddress);
            Assert.Equal((uint) 0x44552211, dataDirectories[(int) Constants.DataDirectoryIndex.COM_Descriptor].Size);
            Assert.Equal(0xffcc2211, dataDirectories[(int) Constants.DataDirectoryIndex.Reserved].VirtualAddress);
            Assert.Equal((uint) 0x44cc2211, dataDirectories[(int) Constants.DataDirectoryIndex.Reserved].Size);
        }

        [Fact]
        public void DllCharacteristics_Missing_NoSEH_Flag()
        {
            var peFile = new PeFile("./Binaries/firefox_x64.exe");
            var File_Characteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;

            Constants.OptionalHeaderDllCharacteristics safe_seh = (Constants.OptionalHeaderDllCharacteristics)File_Characteristics;

            Assert.Equal((ushort)0xC160, peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics);
            Assert.False(safe_seh.HasFlag(Constants.OptionalHeaderDllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_SEH));
        }

        [Fact]
        public void DllCharacteristics_Set_NoSEH_Flag()
        {
            var peFile = new PeFile("./Binaries/No_SEH.exe");
            var File_Characteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;

            Constants.OptionalHeaderDllCharacteristics safe_seh = (Constants.OptionalHeaderDllCharacteristics)File_Characteristics;

            Assert.Equal((ushort)0x08540, peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics);
            Assert.True(safe_seh.HasFlag(Constants.OptionalHeaderDllCharacteristics.IMAGE_DLLCHARACTERISTICS_NO_SEH));
        }
    }
}