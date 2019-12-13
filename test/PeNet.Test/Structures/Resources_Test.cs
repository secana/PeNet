using Xunit;

namespace PeNet.Test.Structures
{
    public class Resources_Test
    {
        [Fact]
        public void Resources_GivenAPEFile1_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/firefox_invalid_x64.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal((ushort) 0x03E8, vsVersionInfo.wLength);
            Assert.Equal((ushort) 0x0034, vsVersionInfo.wValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.wType);
            Assert.Equal("VS_VERSION_INFO", vsVersionInfo.szKey);

            Assert.Equal(0xFEEF04BD, vsVersionInfo.VsFixedFileInfo.dwSignature);
            Assert.Equal((uint) 0x00010000, vsVersionInfo.VsFixedFileInfo.dwStrucVersion);
            Assert.Equal((uint) 0x00390000, vsVersionInfo.VsFixedFileInfo.dwFileVersionMS);
            Assert.Equal((uint) 0x00021995, vsVersionInfo.VsFixedFileInfo.dwFileVersionLS);
            Assert.Equal((uint) 0x00390000, vsVersionInfo.VsFixedFileInfo.dwProductVersionMS);
            Assert.Equal((uint) 0x00020000, vsVersionInfo.VsFixedFileInfo.dwProductVersionLS);
            Assert.Equal((uint) 0x0000003F, vsVersionInfo.VsFixedFileInfo.dwFileFlagsMask);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileFlags);
            Assert.Equal((uint) 0x00000004, vsVersionInfo.VsFixedFileInfo.dwFileOS);
            Assert.Equal((uint) 0x00000002, vsVersionInfo.VsFixedFileInfo.dwFileType);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileSubType);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileDateMS);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileDateLS);

            Assert.Equal((ushort) 0x0346, vsVersionInfo.StringFileInfo.wLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.wType);
            Assert.Equal("StringFileInfo", vsVersionInfo.StringFileInfo.szKey);

            Assert.Equal((ushort) 0x0322, vsVersionInfo.StringFileInfo.StringTable[0].wLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.StringTable[0].wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].wType);
            Assert.Equal("000004b0", vsVersionInfo.StringFileInfo.StringTable[0].szKey);

            Assert.Equal((ushort) 0x0018, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wType);
            Assert.Equal("Comments", vsVersionInfo.StringFileInfo.StringTable[0].String[0].szKey);


            Assert.Equal((ushort)0x00AC, vsVersionInfo.StringFileInfo.StringTable[0].String[1].wLength);
            Assert.Equal((ushort)0x0044, vsVersionInfo.StringFileInfo.StringTable[0].String[1].wValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[1].wType);
            Assert.Equal("LegalCopyright", vsVersionInfo.StringFileInfo.StringTable[0].String[1].szKey);
            Assert.Equal("\u00a9Firefox and Mozilla Developers; available under the MPL 2 license.", vsVersionInfo.StringFileInfo.StringTable[0].String[1].Value);

            // ... more String entries ...

            Assert.Equal((ushort)0x0036, vsVersionInfo.StringFileInfo.StringTable[0].String[10].wLength);
            Assert.Equal((ushort)0x000F, vsVersionInfo.StringFileInfo.StringTable[0].String[10].wValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[10].wType);
            Assert.Equal("BuildID", vsVersionInfo.StringFileInfo.StringTable[0].String[10].szKey);
            Assert.Equal("20171206182557", vsVersionInfo.StringFileInfo.StringTable[0].String[10].Value);

            Assert.Equal("\u00a9Firefox and Mozilla Developers; available under the MPL 2 license.", vsVersionInfo.StringFileInfo.StringTable[0].LegalCopyright);
            Assert.Equal("Mozilla Corporation", vsVersionInfo.StringFileInfo.StringTable[0].CompanyName);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].FileDescription);
            Assert.Equal("57.0.2", vsVersionInfo.StringFileInfo.StringTable[0].FileVersion);
            Assert.Equal("57.0.2", vsVersionInfo.StringFileInfo.StringTable[0].ProductVersion);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].InternalName);
            Assert.Equal("Firefox is a Trademark of The Mozilla Foundation.", vsVersionInfo.StringFileInfo.StringTable[0].LegalTrademarks);
            Assert.Equal("firefox.exe", vsVersionInfo.StringFileInfo.StringTable[0].OriginalFilename);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].ProductName);

            Assert.Equal((ushort) 0x0044, vsVersionInfo.VarFileInfo.wLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.VarFileInfo.wType);
            Assert.Equal("VarFileInfo", vsVersionInfo.VarFileInfo.szKey);

            Assert.Equal((ushort) 0x0024, vsVersionInfo.VarFileInfo.Children[0].wLength);
            Assert.Equal((ushort) 0x0004, vsVersionInfo.VarFileInfo.Children[0].wValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.Children[0].wType);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].szKey);
            Assert.Equal((uint) 0x04b00000, vsVersionInfo.VarFileInfo.Children[0].Value[0]);
        }

        [Fact]
        public void Resources_GivenAPEFile2_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/pidgin.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal((ushort)0x0374, vsVersionInfo.wLength);
            Assert.Equal((ushort)0x0034, vsVersionInfo.wValueLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.wType);
            Assert.Equal("VS_VERSION_INFO", vsVersionInfo.szKey);

            Assert.Equal(0xFEEF04BD, vsVersionInfo.VsFixedFileInfo.dwSignature);
            Assert.Equal((uint)0x00010000, vsVersionInfo.VsFixedFileInfo.dwStrucVersion);
            Assert.Equal((uint)0x0002000A, vsVersionInfo.VsFixedFileInfo.dwFileVersionMS);
            Assert.Equal((uint)0x000B0000, vsVersionInfo.VsFixedFileInfo.dwFileVersionLS);
            Assert.Equal((uint)0x0002000A, vsVersionInfo.VsFixedFileInfo.dwProductVersionMS);
            Assert.Equal((uint)0x000B0000, vsVersionInfo.VsFixedFileInfo.dwProductVersionLS);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileFlagsMask);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileFlags);
            Assert.Equal((uint)0x00000004, vsVersionInfo.VsFixedFileInfo.dwFileOS);
            Assert.Equal((uint)0x00000001, vsVersionInfo.VsFixedFileInfo.dwFileType);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileSubType);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileDateMS);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.dwFileDateLS);

            Assert.Equal((ushort)0x02D4, vsVersionInfo.StringFileInfo.wLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.StringFileInfo.wValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.wType);
            Assert.Equal("StringFileInfo", vsVersionInfo.StringFileInfo.szKey);

            Assert.Equal((ushort)0x02B0, vsVersionInfo.StringFileInfo.StringTable[0].wLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.StringFileInfo.StringTable[0].wValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].wType);
            Assert.Equal("040904B0", vsVersionInfo.StringFileInfo.StringTable[0].szKey);

            Assert.Equal((ushort)0x005E, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wLength);
            Assert.Equal((ushort)0x001F, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[0].wType);
            Assert.Equal("CompanyName", vsVersionInfo.StringFileInfo.StringTable[0].String[0].szKey);
            Assert.Equal("The Pidgin developer community", vsVersionInfo.StringFileInfo.StringTable[0].String[0].Value);

            // ... more String entries ...

            Assert.Equal((ushort) 0x0034, vsVersionInfo.StringFileInfo.StringTable[0].String[7].wLength);
            Assert.Equal((ushort) 0x0008, vsVersionInfo.StringFileInfo.StringTable[0].String[7].wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[7].wType);
            Assert.Equal("ProductVersion", vsVersionInfo.StringFileInfo.StringTable[0].String[7].szKey);
            Assert.Equal("2.10.11", vsVersionInfo.StringFileInfo.StringTable[0].String[7].Value);

            Assert.Equal((ushort) 0x0044, vsVersionInfo.VarFileInfo.wLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.wValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.VarFileInfo.wType);
            Assert.Equal("VarFileInfo", vsVersionInfo.VarFileInfo.szKey);

            Assert.Equal((ushort) 0x0024, vsVersionInfo.VarFileInfo.Children[0].wLength);
            Assert.Equal((ushort) 0x0004, vsVersionInfo.VarFileInfo.Children[0].wValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.Children[0].wType);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].szKey);
            Assert.Equal((uint) 0x04b00409, vsVersionInfo.VarFileInfo.Children[0].Value[0]);
        }
    }
}