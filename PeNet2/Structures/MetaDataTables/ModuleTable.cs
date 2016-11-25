using System.Collections.Generic;

namespace PeNet.Structures.MetaDataTables
{
    public class ModuleTable : AbstractMetaDataTable<ModuleTableRow>
    {
        public ModuleTable(byte[] buff, uint offset, int numberOfRows) 
            : base(buff, offset, numberOfRows)
        {
        }

        protected override List<ModuleTableRow> ParseRows()
        {
            throw new System.NotImplementedException();
        }
    }
}