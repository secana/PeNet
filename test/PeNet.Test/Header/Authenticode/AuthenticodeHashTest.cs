using Xunit;
using System.Linq;

namespace PeNet.Test.Header.Authenticode
{
    public class AuthenticodeHashTest
    {
        [Theory]
        [InlineData(@"./Binaries/add-import.exe","68555fb55b4d974628d429eda7f5e282d647b426")]
        [InlineData(@"./Binaries/add-section.exe","68555fb55b4d974628d429eda7f5e282d647b426")]
        [InlineData(@"./Binaries/arm_binary.dll","c722f439a2630f31b993917190c29604b8aad3b8")]
        [InlineData(@"./Binaries/arm_dotnet_binary.dll","9bca7cc604e504475e46c2a2192a24c4c11abb43")]
        [InlineData(@"./Binaries/chrome_elf.dll","33ea37b6958a008549dc395e2932095f34bcbf8b")]
        [InlineData(@"./Binaries/dotnet_x64.dll","3a482db1ed60fdab5fa7862b414797cad4f00524")]
        [InlineData(@"./Binaries/firefox_invalid_x64.exe","79da1b80964d0ef4e661b74d09e4d01dfb71fa95")]
        [InlineData(@"./Binaries/firefox_invalid_x86.exe","90dfa709c0ad470a44a6155aa6af7b136318967b")]
        [InlineData(@"./Binaries/firefox_x64_copy1.exe","f9e7919f2e19cacc6af2cdd5919a31999c9d2fb6")]
        [InlineData(@"./Binaries/firefox_x64_copy2.exe","f9e7919f2e19cacc6af2cdd5919a31999c9d2fb6")]
        [InlineData(@"./Binaries/firefox_x64_manipulated.exe","6dece04e93fca237197f53d31b18bb3bda72b15d")]
        [InlineData(@"./Binaries/firefox_x64.exe","f9e7919f2e19cacc6af2cdd5919a31999c9d2fb6")]
        [InlineData(@"./Binaries/firefox_x86_2.exe","0b4ba5ddad3dedef928db97c674703f5bf3e5f81")]
        [InlineData(@"./Binaries/firefox_x86.exe","0b4ba5ddad3dedef928db97c674703f5bf3e5f81")]
        [InlineData(@"./Binaries/krnl_test.sys","b0020506b5b26ba8327d3dcce41ca134c633824b")]
        [InlineData(@"./Binaries/NetCoreConsole.dll","61420d9b7cf60b37caab6918c4ce89ce5a029bfc")]
        [InlineData(@"./Binaries/NetFrameworkConsole.exe","5728a7e8d46bbccb3b46804678d32e89000cf97c")]
        [InlineData(@"./Binaries/No_SEH.exe","796ffb54e4df71bbba8802f514aefb609f6eb901")]
        [InlineData(@"./Binaries/old_firefox_x86.exe","c8d6c881cdd701f8630ca25ed6624ace69010ca0")]
        [InlineData(@"./Binaries/osx_vb_netcore.dll","73297a8a0693b2024c4800d38fa67d1fae9fa2e1")]
        [InlineData(@"./Binaries/pdb_guid.exe","1deaa6bf0c338973d81742e0a4930be35348f813")]
        [InlineData(@"./Binaries/pidgin.exe","5cf1e042d80711ec0bc3ce05be8c34a280e5f512")]
        [InlineData(@"./Binaries/remove-section.exe","68555fb55b4d974628d429eda7f5e282d647b426")]
        [InlineData(@"./Binaries/TLSCallback_x64.dll","92e1156b6a0b77d863388ad0a08e766b91a11aeb")]
        [InlineData(@"./Binaries/TLSCallback_x86.exe","4546c11743b919ce740d40de71369493ae1dde46")]
        [InlineData(@"./Binaries/win_test.dll","314d1a82f923679d76446bcbeb557de2a390519c")]
        public void AuthenticodeHash_GivenABinary_ComputesCorrectHash(string file, string expected)
        {
            var peFile = new PeFile(file);
            var hash = System.Security.Cryptography.SHA1.Create();
            var bytes = peFile.AuthenticodeInfo.ComputeAuthenticodeHashFromPeFile(hash);
            var digest = bytes.ToList().ToHexString().Substring(2);
            
            Assert.Equal(expected, digest);
        }
    }
}