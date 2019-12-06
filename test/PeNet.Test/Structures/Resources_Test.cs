using Xunit;

namespace PeNet.Test.Structures
{
    public class Resources_Test
    {
        [Fact]
        public void Resources_GivenAPEFile_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/firefox_invalid_x64.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal((ushort) 0x03E8, vsVersionInfo.wLength);
            Assert.Equal((ushort) 0x0034, vsVersionInfo.wValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.wType);
        }
    }
}