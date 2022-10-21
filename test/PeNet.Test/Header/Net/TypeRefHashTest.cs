using Xunit;

namespace PeNet.Test.Header.Net
{
    public class TypeRefHashTest
    {
        [Theory]
        [InlineData(@"./Binaries/NetCoreConsole.dll", "9b435fef12d55da7073890330a9a4d7f6e02194aa63e6093429db574407458ba")]
        [InlineData(@"./Binaries/NetFrameworkConsole.exe", "c4bc255f816ae338fba805256b078bb023d339d2b80dc84a21444367539038cb")]
        [InlineData(@"./Binaries/osx_vb_netcore.dll", "5fda416c838f34364173ed8c3cd7181dad676aaf7e8b2b37e54953405462f42d")]
        [InlineData(@"./Binaries/arm_dotnet_binary.dll", "d633db771449e2c37e1689a8c291a4f4646ce156652a9dad5f67394c0d92a8c4")]
        public void DotNetFile_ReturnsCorrectTypeRefHash(string file, string hash)
        {
            var peFile = new PeFile(file);

            Assert.Equal(hash, peFile.TypeRefHash);
        }

        [Theory]
        [InlineData(@"./Binaries/krnl_test.sys")]
        [InlineData(@"./Binaries/No_SEH.exe")]
        [InlineData(@"./Binaries/win_test.dll")]
        public void NotDotNetFile_ReturnsNull(string file)
        {
            var peFile = new PeFile(file);

            Assert.Null(peFile.TypeRefHash);
        }
    }
}