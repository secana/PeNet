using Xunit;

namespace PeNet.Test.Header.ImpHash
{
    public class ImpHashTest
    {
        [Theory]
        [InlineData(@"./Binaries/win_test.dll", "f9f97e60cfcd78be051d9570c88ffb6f")]
        [InlineData(@"./Binaries/NetCoreConsole.dll", "f34d5f2d4577ed6d9ceec516c1f5a744")]
        [InlineData(@"./Binaries/pidgin.exe", "91f96ce80cb6ee2b5e5fb6cc19bac72b")]
        public void ImpHash_GivenABinary_ComputesCorrectHash(string file, string expected)
        {
            var peFile = new PeFile(file);
            Assert.Equal(expected, peFile.ImpHash);
        }
    }
}