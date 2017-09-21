using System.Collections.Generic;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// Module Table from the Meta Data Table Header of the 
    /// .Net header. Contains information about the current 
    /// assembly. Has only one row.
    /// </summary>
    public class ModuleTable : AbstractMetaDataTable<ModuleTableRow>
    {
        private readonly HeapOffsetBasedIndexSizes _heapOffsetIndexSizes;

        /// <summary>
        /// Create a new instance of the ModuleTable.
        /// </summary>
        /// <param name="buff">Buffer containing the ModuleTable.</param>
        /// <param name="offset">Offset to the ModuleTable in the buffer.</param>
        /// <param name="numberOfRows">Number of rows of the table.</param>
        /// <param name="heapOffsetSizes">The HeapOffsetSizes flag of the Meta Data Tables Header.</param>
        public ModuleTable(byte[] buff, uint offset, uint numberOfRows, byte heapOffsetSizes) 
            : base(buff, offset, numberOfRows)
        {
            _heapOffsetIndexSizes = new HeapOffsetBasedIndexSizes(heapOffsetSizes);
        }

        /// <summary>
        /// Parse the rows of the table.
        /// </summary>
        /// <returns>List with rows.</returns>
        protected override List<ModuleTableRow> ParseRows()
        {
            var currentOffset = Offset;
            var rows = new List<ModuleTableRow>((int) NumberOfRows);
            for (var i = 0; i < NumberOfRows; i++)
            {
                var row = new ModuleTableRow(Buff, currentOffset, _heapOffsetIndexSizes);
                rows.Add(row);
                currentOffset += row.Length;
            }
            return rows;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder("ModuleTable\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));
            foreach (var moduleTableRow in Rows)
                sb.Append(moduleTableRow);
            return sb.ToString();
        }
    }
}