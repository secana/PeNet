using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{

    public class ImageResourceDirectoryEntryTest
    {
        [Fact]
        public void ResourceDirectoryEntryByNameConstructorWorks_Test()
        {
            var fakeParent = new ImageResourceDirectory(null, null, 0, 0, 0);

            var resourceDirectoryEntry =
                new ImageResourceDirectoryEntry(new BufferFile(RawStructures.RawResourceDirectoryEntryByName), fakeParent, 2, 2);

            Assert.True(resourceDirectoryEntry.IsNamedEntry);
            Assert.False(resourceDirectoryEntry.IsIdEntry);
            Assert.Equal(0x80332211, resourceDirectoryEntry.Name);
            Assert.Equal((uint) 0x55443322, resourceDirectoryEntry.OffsetToData);
            Assert.Equal(fakeParent, resourceDirectoryEntry.Parent);
        }

        [Fact]
        public void ResourceDirectoryEntryByIdConstructorWorks_Test()
        {
            var fakeParent = new ImageResourceDirectory(null, null, 0, 0, 0);

            var resourceDirectoryEntry =
                new ImageResourceDirectoryEntry(new BufferFile(RawStructures.RawResourceDirectoryEntryById), fakeParent, 2, 2);

            Assert.True(resourceDirectoryEntry.IsIdEntry);
            Assert.False(resourceDirectoryEntry.IsNamedEntry);
            Assert.Equal((uint) 0x00332211 & 0xFFFF, resourceDirectoryEntry.ID);
            Assert.Equal((uint) 0x55443322, resourceDirectoryEntry.OffsetToData);
            Assert.Equal(fakeParent, resourceDirectoryEntry.Parent);
        }
    }
}
