using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageSectionHeaderTest
    {
        [Fact]
        public void ImageSectionHeaderConstructorWorks_Test()
        {
            var sectionHeader = new ImageSectionHeader(new BufferFile(RawStructures.RawSectionHeader), 2, 0);

            Assert.Equal(".data", sectionHeader.Name);
            Assert.Equal((uint) 0x33221100, sectionHeader.VirtualSize);
            Assert.Equal((uint) 0x77665544, sectionHeader.VirtualAddress);
            Assert.Equal(0xbbaa9988, sectionHeader.SizeOfRawData);
            Assert.Equal(0xffeeddcc, sectionHeader.PointerToRawData);
            Assert.Equal((uint) 0x44332211, sectionHeader.PointerToRelocations);
            Assert.Equal(0x88776655, sectionHeader.PointerToLinenumbers);
            Assert.Equal((ushort) 0xaa99, sectionHeader.NumberOfRelocations);
            Assert.Equal((ushort) 0xccbb, sectionHeader.NumberOfLinenumbers);
            Assert.Equal((ScnCharacteristicsType) 0x00000820, sectionHeader.Characteristics);
            Assert.Equal(2, sectionHeader.CharacteristicsResolved.Count);
            Assert.Contains("LnkRemove", sectionHeader.CharacteristicsResolved);
            Assert.Contains("CntCode", sectionHeader.CharacteristicsResolved);
        }

        [Fact]
        public void ImageSectionHeader_CorrectSectionNames()
        {
            var peFile = new PeFile(@"./Binaries/pdb_guid.exe");
            var snc = peFile.ImageSectionHeaders;

            Assert.Equal(".text", snc[0].Name);
            Assert.Equal("ERRATA", snc[1].Name);
            Assert.Equal("INITKDBG", snc[2].Name);
            Assert.Equal("POOLCODE", snc[3].Name);
            Assert.Equal(".rdata", snc[4].Name);
            Assert.Equal(".data", snc[5].Name);
            Assert.Equal(".pdata", snc[6].Name);
            Assert.Equal("ALMOSTRO", snc[7].Name);
            Assert.Equal("CACHEALI", snc[8].Name);
            Assert.Equal("PAGELK", snc[9].Name);
            Assert.Equal("PAGE", snc[10].Name);
            Assert.Equal("PAGEKD", snc[11].Name);
            Assert.Equal("PAGEVRFY", snc[12].Name);
            Assert.Equal("PAGEHDLS", snc[13].Name);
            Assert.Equal("PAGEBGFX", snc[14].Name);
            Assert.Equal("PAGEVRFB", snc[15].Name);
            Assert.Equal(".edata", snc[16].Name);
            Assert.Equal("PAGEDATA", snc[17].Name);
            Assert.Equal("PAGEVRFD", snc[18].Name);
            Assert.Equal("INITDATA", snc[19].Name);
            Assert.Equal("INIT", snc[20].Name);
            Assert.Equal(".rsrc", snc[21].Name);
            Assert.Equal(".reloc", snc[22].Name);
        }
    }
}
