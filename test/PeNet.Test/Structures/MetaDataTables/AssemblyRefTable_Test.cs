using Xunit;

namespace PeNet.Test.Structures.MetaDataTables
{
    public class AssemblyRefTable_Test
    {
        [Fact(Skip = "Cannot be implemented atm")]
        public void AssemblyRefTable_NetFrameworkConsoleExe_CorrectValues()
        {
            var peFile = new PeFile(@"./Binaries/NetFrameworkConsole.exe");

            var assemblyRefTable = peFile.MetaDataTables.AssemblyRefTable;

            Assert.Equal(0x0001u, assemblyRefTable.NumberOfRows);
            Assert.Equal(0x0004u, assemblyRefTable.Rows[0].MajorVersion);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].MinorVersion);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].BuildNumber);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].RevisionNumber);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].Flags);
            Assert.Equal(0x0053u, assemblyRefTable.Rows[0].PublicKeyOrToken);
            Assert.Equal(0x0030u, assemblyRefTable.Rows[0].Name);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].Culture);
            Assert.Equal(0x0000u, assemblyRefTable.Rows[0].HashValue);
        }
    }
}