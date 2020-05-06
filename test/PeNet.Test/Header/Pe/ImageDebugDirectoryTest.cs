using System;
using PeNet.FileParser;
using PeNet.Header.Pe;
using Xunit;

namespace PeNet.Test.Header.Pe
{
    
    public class ImageDebugDirectoryTest
    {
        [Fact]
        public void ImageDebugDirectoryConstructorWorks_Test()
        {
            var idd = new ImageDebugDirectory(new BufferFile(RawStructures.RawDebugDirectory), 2);

            Assert.Equal((uint) 0x44332211, idd.Characteristics);
            Assert.Equal(0x88776655, idd.TimeDateStamp);
            Assert.Equal((ushort) 0xaa99, idd.MajorVersion);
            Assert.Equal((ushort) 0xccbb, idd.MinorVersion);
            Assert.Equal((uint) 0x11ffeedd, idd.Type);
            Assert.Equal((uint) 0x55443322, idd.SizeOfData);
            Assert.Equal(0x99887766, idd.AddressOfRawData);
            Assert.Equal(0xddccbbaa, idd.PointerToRawData);
        }

        [Fact]
        public void ParseDebugDirectory_MultipleEntries_ReturnsAllEntries()
        {
            var file = @"Binaries/TLSCallback_x64.dll";

            var debugEntries = new PeFile(file).ImageDebugDirectory;

            Assert.Equal(4, debugEntries.Length);
            Assert.Equal((uint)0x000023B0, debugEntries[0].AddressOfRawData);
            Assert.Equal((uint)0x0000241C, debugEntries[1].AddressOfRawData);
            Assert.Equal((uint)0x00002430, debugEntries[2].AddressOfRawData);
            Assert.Equal((uint)0x00000000, debugEntries[3].AddressOfRawData);
        }

        [Fact]
        public void ImageDebugDirectory_PeFileWithDebugInfo1_ParseCvHeader()
        {
            var file = @"Binaries/pdb_guid.exe";

            var peFile = new PeFile(file);
            var cv = peFile.ImageDebugDirectory[0].CvInfoPdb70;

            Assert.Equal(0x53445352u, cv.CvSignature);
            Assert.Equal(Guid.Parse("0de6dc23-8e19-4bb7-8608-d54b1e6fa379"), cv.Signature);
            Assert.Equal((ushort) 0x0001, cv.Age);
            Assert.Equal("ntkrnlmp.pdb", cv.PdbFileName);
        }

        [Fact]
        public void ImageDebugDirectory_PeFileWithDebugInfo2_ParseCvHeader()
        {
            var file = @"Binaries/firefox_x86_2.exe";

            var peFile = new PeFile(file);
            var cv = peFile.ImageDebugDirectory[0].CvInfoPdb70;

            Assert.Equal(0x53445352u, cv.CvSignature);
            Assert.Equal(Guid.Parse("37bca9fb-2576-469b-8575-0a210e895e31"), cv.Signature);
            Assert.Equal((ushort)0x0002, cv.Age);
            Assert.Equal(@"z:\build\build\src\obj-firefox\browser\app\firefox.pdb", cv.PdbFileName);
        }
    }
}