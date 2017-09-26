using PeNet.Structures.MetaDataTables;

namespace PeNet.Parser.MetaDataTables
{
    internal class AssemblyRefTableParser : SafeParser<AssemblyRefTable>
    {
        public AssemblyRefTableParser(byte[] buff, uint offset) : base(buff, offset)
        {
        }

        protected override AssemblyRefTable ParseTarget()
        {
            throw new System.NotImplementedException();
        }
    }
}