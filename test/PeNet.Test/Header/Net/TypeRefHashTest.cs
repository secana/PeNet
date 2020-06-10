using System;
using Xunit;

namespace PeNet.Test.Header.Net
{
    public class TypeRefHashTest
    {
        [Theory]
        [InlineData(@"Binaries/NetCoreConsole.dll", "684f53132ca9be3869bfe1afff7e18566d388c457234958064c95978f21ace99")]
        [InlineData(@"Binaries/NetFrameworkConsole.exe", "042eb0c6bbbeea6662be43239b1c340839e0a0d7f12dfbd03d5822f3e5020b90")]
        [InlineData(@"Binaries/osx_vb_netcore.dll", "bff778f71cc7e3f900bd404369d0493bc33dc8cb01853cb4fbc32b14d20a2940")]
        [InlineData(@"Binaries/arm_dotnet_binary.dll", "c97419457f48a4e48b38c59e218de5adcf335f24cb6ea33391742a489c428447")]
        public void DotNetFile_ReturnsCorrectTypeRefHash(string file, string hash)
        {
            var peFile = new PeFile(file);

            Assert.Equal(hash, peFile.TypeRefHash);
        }

        [Theory]
        [InlineData(@"Binaries/krnl_test.sys")]
        [InlineData(@"Binaries/No_SEH.exe")]
        [InlineData(@"Binaries/win_test.dll")]
        public void NotDotNetFile_ReturnsNull(string file)
        {
            var peFile = new PeFile(file);

            Assert.Null(peFile.TypeRefHash);
        }
    }
}