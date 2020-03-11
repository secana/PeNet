using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageOptionalHeaderTest
    {
        [Fact]
        public void OptionalHeaderConstructor64BitWorks_Test()
        {
            var optHeader = new ImageOptionalHeader(new BufferFile(RawStructures.RawImageOptionalHeader64Bit), 2, true);

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
            var optHeader = new ImageOptionalHeader(new BufferFile(RawStructures.RawImageOptionalHeader32Bit), 2, false);

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

        private void AssertCommonOptHeaderProperties(ImageOptionalHeader optHeader)
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
            Assert.Equal(SubsystemType.WindowsCui, optHeader.Subsystem);
            Assert.Equal("Windows CUI", optHeader.SubsystemResolved);
            Assert.Equal((ushort) 0xaa99, (ushort) optHeader.DllCharacteristics);
            Assert.Equal(0xaa998822, optHeader.LoaderFlags);
            Assert.Equal((uint) 0x00000005, optHeader.NumberOfRvaAndSizes);
        }

        private void AssertDataDirectories(ImageDataDirectory[] dataDirectories)
        {
            Assert.Equal(16, dataDirectories.Length);

            Assert.Equal((uint) 0x44332211, dataDirectories[(int) DataDirectoryType.Export].VirtualAddress);
            Assert.Equal((uint) 0x44332233, dataDirectories[(int) DataDirectoryType.Export].Size);
            Assert.Equal((uint) 0x44332244, dataDirectories[(int) DataDirectoryType.Import].VirtualAddress);
            Assert.Equal((uint) 0x44332255, dataDirectories[(int) DataDirectoryType.Import].Size);
            Assert.Equal((uint) 0x44887766,
                dataDirectories[(int) DataDirectoryType.Resource].VirtualAddress);
            Assert.Equal((uint) 0x4433aa99, dataDirectories[(int) DataDirectoryType.Resource].Size);
            Assert.Equal((uint) 0x44882211,
                dataDirectories[(int) DataDirectoryType.Exception].VirtualAddress);
            Assert.Equal((uint) 0x44334433, dataDirectories[(int) DataDirectoryType.Exception].Size);
            Assert.Equal((uint) 0x443322bb,
                dataDirectories[(int) DataDirectoryType.Security].VirtualAddress);
            Assert.Equal((uint) 0x443322cc, dataDirectories[(int) DataDirectoryType.Security].Size);
            Assert.Equal((uint) 0x443322dd,
                dataDirectories[(int) DataDirectoryType.BaseReloc].VirtualAddress);
            Assert.Equal((uint) 0x443322ee, dataDirectories[(int) DataDirectoryType.BaseReloc].Size);
            Assert.Equal((uint) 0x443322ff, dataDirectories[(int) DataDirectoryType.Debug].VirtualAddress);
            Assert.Equal((uint) 0x44333311, dataDirectories[(int) DataDirectoryType.Debug].Size);
            Assert.Equal((uint) 0x44334411,
                dataDirectories[(int) DataDirectoryType.Copyright].VirtualAddress);
            Assert.Equal((uint) 0x44335511, dataDirectories[(int) DataDirectoryType.Copyright].Size);
            Assert.Equal((uint) 0x44336611,
                dataDirectories[(int) DataDirectoryType.Globalptr].VirtualAddress);
            Assert.Equal((uint) 0x44337711, dataDirectories[(int) DataDirectoryType.Globalptr].Size);
            Assert.Equal((uint) 0x44338811, dataDirectories[(int) DataDirectoryType.TLS].VirtualAddress);
            Assert.Equal((uint) 0x44339911, dataDirectories[(int) DataDirectoryType.TLS].Size);
            Assert.Equal((uint) 0x4433aa11,
                dataDirectories[(int) DataDirectoryType.LoadConfig].VirtualAddress);
            Assert.Equal((uint) 0x4433bb11, dataDirectories[(int) DataDirectoryType.LoadConfig].Size);
            Assert.Equal((uint) 0x443399ee,
                dataDirectories[(int) DataDirectoryType.BoundImport].VirtualAddress);
            Assert.Equal((uint) 0x44330099, dataDirectories[(int) DataDirectoryType.BoundImport].Size);
            Assert.Equal((uint) 0x4433cc11, dataDirectories[(int) DataDirectoryType.IAT].VirtualAddress);
            Assert.Equal((uint) 0x4433dd11, dataDirectories[(int) DataDirectoryType.IAT].Size);
            Assert.Equal((uint) 0x4433ee11,
                dataDirectories[(int) DataDirectoryType.DelayImport].VirtualAddress);
            Assert.Equal((uint) 0x4433ff11, dataDirectories[(int) DataDirectoryType.DelayImport].Size);
            Assert.Equal((uint) 0x44442211,
                dataDirectories[(int) DataDirectoryType.ComDescriptor].VirtualAddress);
            Assert.Equal((uint) 0x44552211, dataDirectories[(int) DataDirectoryType.ComDescriptor].Size);
            Assert.Equal(0xffcc2211, dataDirectories[(int) DataDirectoryType.Reserved].VirtualAddress);
            Assert.Equal((uint) 0x44cc2211, dataDirectories[(int) DataDirectoryType.Reserved].Size);
        }

        [Fact]
        public void DllCharacteristics_Missing_NoSEH_Flag()
        {
            var peFile = new PeFile("./Binaries/firefox_x64.exe");
            var fileCharacteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;

            Assert.Equal((ushort)0xC160, (ushort) peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics);
            Assert.False(fileCharacteristics.HasFlag(DllCharacteristicsType.NoSeh));
        }

        [Fact]
        public void DllCharacteristics_Set_NoSEH_Flag()
        {
            var peFile = new PeFile("./Binaries/No_SEH.exe");
            var fileCharacteristics = peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics;

            Assert.Equal((ushort)0x08540, (ushort) peFile.ImageNtHeaders.OptionalHeader.DllCharacteristics);
            Assert.True(fileCharacteristics.HasFlag(DllCharacteristicsType.NoSeh));
        }
    }
}