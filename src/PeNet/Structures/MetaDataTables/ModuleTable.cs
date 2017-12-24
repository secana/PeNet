using System.Collections.Generic;
using System.Text;
using PeNet.Structures.MetaDataTables.Indices;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// Module Table from the Meta Data Table Header of the 
    /// .Net header. Contains information about the current 
    /// assembly. Has only one row.
    /// </summary>
    /// <inheritdoc />
    public class ModuleTable : AbstractMetaDataTable<ModuleTableRow>
    {
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly IMETADATASTREAM_GUID _metaDataStreamGuid;

        /// <summary>
        /// Create a new instance of the ModuleTable.
        /// </summary>
        /// <param name="buff">Buffer containing the .Net header.</param>
        /// <param name="offset">Offset to the Module Table in the buffer.</param>
        /// <param name="numberOfRows">Number of rows of the table.</param>
        /// <param name="metaDataStreamString">Meta Data stream "String" to resolve strings in
        /// the rows.</param>
        /// <param name="metaDataStreamGuid">Meta Data stream "GUID" to resolve GUIDs in
        /// the rows.</param>
        /// <param name="heapOffsetSizes">The HeapOffsetSizes flag of the Meta Data Tables Header.</param>
        public ModuleTable(
            byte[] buff, 
            uint offset, 
            uint numberOfRows,
            IMETADATASTREAM_STRING metaDataStreamString,
            IMETADATASTREAM_GUID metaDataStreamGuid,
            IHeapOffsetSizes heapOffsetSizes) 
            : base(buff, offset, numberOfRows, heapOffsetSizes)
        {
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamGuid = metaDataStreamGuid;
        }

        protected override List<ModuleTableRow> ParseRows()
        {
            var currentOffset = Offset;
            var rows = new List<ModuleTableRow>((int) NumberOfRows);
            for (var i = 0; i < NumberOfRows; i++)
            {
                var row = new ModuleTableRow(Buff, currentOffset, _metaDataStreamString, _metaDataStreamGuid, HeapIndexSizes);
                rows.Add(row);
                currentOffset += row.Length;
            }
            return rows;
        }

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