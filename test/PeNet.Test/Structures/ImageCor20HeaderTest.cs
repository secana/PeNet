using PeNet.FileParser;
using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class ImageCor20HeaderTest
    {
        [Fact]
        public void ImageCor20HeaderConstructorWorks_Test()
        {
            var imageCor20Header = new ImageCor20Header(new BufferFile(RawDotNetStructures.RawImageCor20Header), 3);

            Assert.Equal((uint) 0x33221100, imageCor20Header.Cb);
            Assert.Equal((uint) 0x5544, imageCor20Header.MajorRuntimeVersion);
            Assert.Equal((uint) 0x7766, imageCor20Header.MinorRuntimeVersion);
            Assert.Equal((uint) 0xbbaa9988, imageCor20Header.MetaData.VirtualAddress);
            Assert.Equal((uint) 0xffeeddcc, imageCor20Header.MetaData.Size);
            Assert.Equal((uint) 0x44332211, imageCor20Header.Flags);
            Assert.Equal((uint) 0x88776655, imageCor20Header.EntryPointToken);
            Assert.Equal((uint) 0x88776655, imageCor20Header.EntryPointRVA);
            Assert.Equal((uint) 0xccbbaa99, imageCor20Header.Resources.VirtualAddress);
            Assert.Equal((uint) 0x11ffeedd, imageCor20Header.Resources.Size);
            Assert.Equal((uint) 0xddccbbaa, imageCor20Header.StrongNameSignature.VirtualAddress);
            Assert.Equal((uint) 0x2211ffee, imageCor20Header.StrongNameSignature.Size);
            Assert.Equal((uint) 0xeeddccbb, imageCor20Header.CodeManagerTable.VirtualAddress);
            Assert.Equal((uint) 0x332211ff, imageCor20Header.CodeManagerTable.Size);
            Assert.Equal((uint) 0xffeeddcc, imageCor20Header.VTableFixups.VirtualAddress);
            Assert.Equal((uint) 0x44332211, imageCor20Header.VTableFixups.Size);
            Assert.Equal((uint) 0x11ffeedd, imageCor20Header.ExportAddressTableJumps.VirtualAddress);
            Assert.Equal((uint) 0x55443322, imageCor20Header.ExportAddressTableJumps.Size);
            Assert.Equal((uint) 0x2211ffee, imageCor20Header.ManagedNativeHeader.VirtualAddress);
            Assert.Equal((uint) 0x66554433, imageCor20Header.ManagedNativeHeader.Size);
        }
    }
}