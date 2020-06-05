using PeNet.Header.Pe;
using System.Linq;
using Xunit;

namespace PeNet.Test.Editor
{
    public class SectionTest
    {
        [Fact]
        public void AddSection_NewSectionAtEndOfFile()
        {
            var peFile = new PeFile(@"Binaries/add-section.exe");

            // Before adding the new section
            Assert.Equal(27_648, peFile.RawFile.Length);
            Assert.Equal(6, peFile.ImageNtHeaders.FileHeader.NumberOfSections);
            Assert.Equal(0xb000u, peFile.ImageNtHeaders.OptionalHeader.SizeOfImage);
            Assert.Equal(6, peFile.ImageSectionHeaders.Length);


            peFile.AddSection(".newSec", 100, (ScnCharacteristicsType)0x40000040);

            // After adding the new section
            Assert.Equal(27_748, peFile.RawFile.Length);
            Assert.Equal(7, peFile.ImageNtHeaders.FileHeader.NumberOfSections);
            Assert.Equal(0xc000u, peFile.ImageNtHeaders.OptionalHeader.SizeOfImage);
            Assert.Equal(7, peFile.ImageSectionHeaders.Length);
        }

        [Fact]
        public void RemoveSection_SectionRemoved()
        {
            var peFile = new PeFile(@"Binaries/remove-section.exe");
            peFile.RemoveSection(".rsrc");

            Assert.Equal(5, peFile.ImageSectionHeaders.Length);
            Assert.False(peFile.ImageSectionHeaders.ToList().Exists(s => s.Name == ".rsrc"));
        }
    }
}
