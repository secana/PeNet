using System.IO;
using System.Linq;
using Xunit;

namespace PeNet.Test.Header.Resource
{
    public class ResourcesTest
    {
        [Fact]
        public void Resources_GivenAPEFile1_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/firefox_invalid_x64.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal((ushort) 0x03E8, vsVersionInfo.WLength);
            Assert.Equal((ushort) 0x0034, vsVersionInfo.WValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.WType);
            Assert.Equal("VS_VERSION_INFO", vsVersionInfo.SzKey);

            Assert.Equal(0xFEEF04BD, vsVersionInfo.VsFixedFileInfo.DwSignature);
            Assert.Equal((uint) 0x00010000, vsVersionInfo.VsFixedFileInfo.DwStrucVersion);
            Assert.Equal((uint) 0x00390000, vsVersionInfo.VsFixedFileInfo.DwFileVersionMS);
            Assert.Equal((uint) 0x00021995, vsVersionInfo.VsFixedFileInfo.DwFileVersionLS);
            Assert.Equal((uint) 0x00390000, vsVersionInfo.VsFixedFileInfo.DwProductVersionMS);
            Assert.Equal((uint) 0x00020000, vsVersionInfo.VsFixedFileInfo.DwProductVersionLS);
            Assert.Equal((uint) 0x0000003F, vsVersionInfo.VsFixedFileInfo.DwFileFlagsMask);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileFlags);
            Assert.Equal((uint) 0x00000004, vsVersionInfo.VsFixedFileInfo.DwFileOS);
            Assert.Equal((uint) 0x00000002, vsVersionInfo.VsFixedFileInfo.DwFileType);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileSubType);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileDateMS);
            Assert.Equal((uint) 0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileDateLS);

            Assert.Equal((ushort) 0x0346, vsVersionInfo.StringFileInfo.WLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.WType);
            Assert.Equal("StringFileInfo", vsVersionInfo.StringFileInfo.SzKey);

            Assert.Equal((ushort) 0x0322, vsVersionInfo.StringFileInfo.StringTable[0].WLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.StringTable[0].WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].WType);
            Assert.Equal("000004b0", vsVersionInfo.StringFileInfo.StringTable[0].SzKey);

            Assert.Equal((ushort) 0x0018, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WType);
            Assert.Equal("Comments", vsVersionInfo.StringFileInfo.StringTable[0].String[0].SzKey);


            Assert.Equal((ushort)0x00AC, vsVersionInfo.StringFileInfo.StringTable[0].String[1].WLength);
            Assert.Equal((ushort)0x0044, vsVersionInfo.StringFileInfo.StringTable[0].String[1].WValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[1].WType);
            Assert.Equal("LegalCopyright", vsVersionInfo.StringFileInfo.StringTable[0].String[1].SzKey);
            Assert.Equal("\u00a9Firefox and Mozilla Developers; available under the MPL 2 license.", vsVersionInfo.StringFileInfo.StringTable[0].String[1].Value);

            // ... more String entries ...

            Assert.Equal((ushort)0x0036, vsVersionInfo.StringFileInfo.StringTable[0].String[10].WLength);
            Assert.Equal((ushort)0x000F, vsVersionInfo.StringFileInfo.StringTable[0].String[10].WValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[10].WType);
            Assert.Equal("BuildID", vsVersionInfo.StringFileInfo.StringTable[0].String[10].SzKey);
            Assert.Equal("20171206182557", vsVersionInfo.StringFileInfo.StringTable[0].String[10].Value);

            Assert.Equal("\u00a9Firefox and Mozilla Developers; available under the MPL 2 license.",
                vsVersionInfo.StringFileInfo.StringTable[0].LegalCopyright);
            Assert.Equal("Mozilla Corporation", vsVersionInfo.StringFileInfo.StringTable[0].CompanyName);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].FileDescription);
            Assert.Equal("57.0.2", vsVersionInfo.StringFileInfo.StringTable[0].FileVersion);
            Assert.Equal("57.0.2", vsVersionInfo.StringFileInfo.StringTable[0].ProductVersion);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].InternalName);
            Assert.Equal("Firefox is a Trademark of The Mozilla Foundation.",
                vsVersionInfo.StringFileInfo.StringTable[0].LegalTrademarks);
            Assert.Equal("firefox.exe", vsVersionInfo.StringFileInfo.StringTable[0].OriginalFilename);
            Assert.Equal("Firefox", vsVersionInfo.StringFileInfo.StringTable[0].ProductName);

            Assert.Equal((ushort) 0x0044, vsVersionInfo.VarFileInfo.WLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.VarFileInfo.WType);
            Assert.Equal("VarFileInfo", vsVersionInfo.VarFileInfo.SzKey);

            Assert.Equal((ushort) 0x0024, vsVersionInfo.VarFileInfo.Children[0].WLength);
            Assert.Equal((ushort) 0x0004, vsVersionInfo.VarFileInfo.Children[0].WValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.Children[0].WType);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].SzKey);
            Assert.Equal((uint) 0x04b00000, vsVersionInfo.VarFileInfo.Children[0].Value[0]);
        }

        [Fact]
        public void Resources_GivenAPEFile2_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/pidgin.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal((ushort)0x0374, vsVersionInfo.WLength);
            Assert.Equal((ushort)0x0034, vsVersionInfo.WValueLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.WType);
            Assert.Equal("VS_VERSION_INFO", vsVersionInfo.SzKey);

            Assert.Equal(0xFEEF04BD, vsVersionInfo.VsFixedFileInfo.DwSignature);
            Assert.Equal((uint)0x00010000, vsVersionInfo.VsFixedFileInfo.DwStrucVersion);
            Assert.Equal((uint)0x0002000A, vsVersionInfo.VsFixedFileInfo.DwFileVersionMS);
            Assert.Equal((uint)0x000B0000, vsVersionInfo.VsFixedFileInfo.DwFileVersionLS);
            Assert.Equal((uint)0x0002000A, vsVersionInfo.VsFixedFileInfo.DwProductVersionMS);
            Assert.Equal((uint)0x000B0000, vsVersionInfo.VsFixedFileInfo.DwProductVersionLS);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileFlagsMask);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileFlags);
            Assert.Equal((uint)0x00000004, vsVersionInfo.VsFixedFileInfo.DwFileOS);
            Assert.Equal((uint)0x00000001, vsVersionInfo.VsFixedFileInfo.DwFileType);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileSubType);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileDateMS);
            Assert.Equal((uint)0x00000000, vsVersionInfo.VsFixedFileInfo.DwFileDateLS);

            Assert.Equal((ushort)0x02D4, vsVersionInfo.StringFileInfo.WLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.StringFileInfo.WValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.WType);
            Assert.Equal("StringFileInfo", vsVersionInfo.StringFileInfo.SzKey);

            Assert.Equal((ushort)0x02B0, vsVersionInfo.StringFileInfo.StringTable[0].WLength);
            Assert.Equal((ushort)0x0000, vsVersionInfo.StringFileInfo.StringTable[0].WValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].WType);
            Assert.Equal("040904B0", vsVersionInfo.StringFileInfo.StringTable[0].SzKey);

            Assert.Equal((ushort)0x005E, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WLength);
            Assert.Equal((ushort)0x001F, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WValueLength);
            Assert.Equal((ushort)0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[0].WType);
            Assert.Equal("CompanyName", vsVersionInfo.StringFileInfo.StringTable[0].String[0].SzKey);
            Assert.Equal("The Pidgin developer community", vsVersionInfo.StringFileInfo.StringTable[0].String[0].Value);

            // ... more String entries ...

            Assert.Equal((ushort) 0x0034, vsVersionInfo.StringFileInfo.StringTable[0].String[7].WLength);
            Assert.Equal((ushort) 0x0008, vsVersionInfo.StringFileInfo.StringTable[0].String[7].WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.StringFileInfo.StringTable[0].String[7].WType);
            Assert.Equal("ProductVersion", vsVersionInfo.StringFileInfo.StringTable[0].String[7].SzKey);
            Assert.Equal("2.10.11", vsVersionInfo.StringFileInfo.StringTable[0].String[7].Value);

            Assert.Equal((ushort) 0x0044, vsVersionInfo.VarFileInfo.WLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.WValueLength);
            Assert.Equal((ushort) 0x0001, vsVersionInfo.VarFileInfo.WType);
            Assert.Equal("VarFileInfo", vsVersionInfo.VarFileInfo.SzKey);

            Assert.Equal((ushort) 0x0024, vsVersionInfo.VarFileInfo.Children[0].WLength);
            Assert.Equal((ushort) 0x0004, vsVersionInfo.VarFileInfo.Children[0].WValueLength);
            Assert.Equal((ushort) 0x0000, vsVersionInfo.VarFileInfo.Children[0].WType);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].SzKey);
            Assert.Equal((uint) 0x04b00409, vsVersionInfo.VarFileInfo.Children[0].Value[0]);
        }

        [Fact]
        public void Resources_GivenAPEFile3_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/NetFrameworkConsole.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal("NetFrameworkConsole.exe", vsVersionInfo.StringFileInfo.StringTable[0].OriginalFilename);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].SzKey);
        }

        [Fact]
        public void Resources_GivenAPEFile4_VsVersionInfoSet()
        {
            var peFile = new PeFile("./Binaries/firefox_x86.exe");
            var vsVersionInfo = peFile.Resources.VsVersionInfo;

            Assert.Equal("firefox.exe", vsVersionInfo.StringFileInfo.StringTable[0].OriginalFilename);
            Assert.Equal("Translation", vsVersionInfo.VarFileInfo.Children[0].SzKey);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", 18)]
        [InlineData(@"Binaries/firefox_x86.exe", 18)]
        [InlineData(@"Binaries/HelloWorld.exe", 0)]
        [InlineData(@"Binaries/chrome_elf.dll", 0)]
        public void Icons_GivenPeFiles_IconsParsed(string file, int expectedIcons)
        {
            var peFile = new PeFile(file);
            Assert.Equal(expectedIcons, peFile.Icons().ToArray().Length);
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", 4, 4, 2, 2, 2, 4)]
        [InlineData(@"Binaries/HelloWorld.exe")]
        public void Icons_GivenPeFiles_IconsInGroupsParsed(string file, params int[] expectedIconsInGroups)
        {
            var peFile = new PeFile(file);
            var groupIcons = peFile.GroupIcons().ToArray();
            Assert.Equal(expectedIconsInGroups.Length, groupIcons.Length);
            for (var i = 0; i < expectedIconsInGroups.Length; ++i)
            {
                Assert.Equal(expectedIconsInGroups[i], groupIcons[i].ToArray().Length);
            }
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", @"Icons/firefox_x64.exe")]
        [InlineData(@"Binaries/pidgin.exe", @"Icons/pidgin.exe")]
        public void Icons_GivenPeFilesAndIconIncludedInPeFile_FoundIconContainsGivenIcon(string filePeFile, string iconDirectory)
        {
            var peFile = new PeFile(filePeFile);

            foreach (var iconFile in Directory.EnumerateFiles(iconDirectory))
            {
                var icon = File.ReadAllBytes(iconFile);
                Assert.Contains(icon, peFile.Icons());
            }
        }

        [Theory]
        [InlineData(@"Binaries/firefox_x64.exe", @"Icons/pidgin.exe/Icon5.raw")]
        [InlineData(@"Binaries/pidgin.exe", @"Icons/firefox_x64.exe/Icon1.raw")]
        public void Icons_GivenPeFilesAndIconNotIncludedInPeFile_FoundIconDoNotContainsGivenIcon(string filePeFile, string fileIcon)
        {
            var peFile = new PeFile(filePeFile);
            var icon = File.ReadAllBytes(fileIcon);

            Assert.DoesNotContain(icon, peFile.Icons());
        }
    }
}
