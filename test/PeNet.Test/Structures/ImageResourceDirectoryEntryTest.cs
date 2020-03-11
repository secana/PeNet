using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageResourceDirectoryEntryTest
    {
        [Fact]
        public void ResourceDirectroyEntryByNameConstructorWorks_Test()
        {
            var resourceDirectroyEntry =
                new ImageResourceDirectoryEntry(new BufferFile(RawStructures.RawResourceDirectoryEntryByName), 2, 2);

            Assert.True(resourceDirectroyEntry.IsNamedEntry);
            Assert.False(resourceDirectroyEntry.IsIdEntry);
            Assert.Equal(0x80332211, resourceDirectroyEntry.Name);
            Assert.Equal((uint) 0x55443322, resourceDirectroyEntry.OffsetToData);
        }

        [Fact]
        public void ResourceDirectroyEntryByIdConstructorWorks_Test()
        {
            var resourceDirectroyEntry =
                new ImageResourceDirectoryEntry(new BufferFile(RawStructures.RawResourceDirectoryEntryById), 2, 2);

            Assert.True(resourceDirectroyEntry.IsIdEntry);
            Assert.False(resourceDirectroyEntry.IsNamedEntry);
            Assert.Equal((uint) 0x00332211 & 0xFFFF, resourceDirectroyEntry.ID);
            Assert.Equal((uint) 0x55443322, resourceDirectroyEntry.OffsetToData);
        }
    }
}