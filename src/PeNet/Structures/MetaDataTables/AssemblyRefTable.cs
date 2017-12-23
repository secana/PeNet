using System.Collections.Generic;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// Assembly Reference Table from the Meta Data Table Header of the
    /// .Net header. Contains information about Assembly references.
    /// </summary>
    public class AssemblyRefTable : AbstractMetaDataTable<AssemblyRefTableRow>
    {
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly IMETADATASTREAM_BLOB _metaDataStreamBlob;

        /// <summary>
        /// Create a new AssemblyRefTable instance.
        /// </summary>
        /// <param name="buff">Buffer containing the .Net header</param>
        /// <param name="offset">Offset to the AssemblyRef Table in the buffer.</param>
        /// <param name="numberOfRows">Number of rows of the table.</param>
        /// <param name="heapIndexSize">The HeapOffsetSizes flag of the Meta Data Tables Header.</param>
        /// <param name="metaDataStreamString">Meta Data stream "String" to resolve strings in
        /// the rows.</param>
        /// <param name="metaDataStreamBlob">Meta Data stream "Blob" to resolve strings and hashes in
        /// the rows.</param>
        public AssemblyRefTable(
            byte[] buff, 
            uint offset, 
            uint numberOfRows,
            IHeapOffsetBasedIndexSizes heapIndexSize,
            IMETADATASTREAM_STRING metaDataStreamString,
            IMETADATASTREAM_BLOB metaDataStreamBlob
            ) : base(buff, offset, numberOfRows, heapIndexSize)
        {
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamBlob = metaDataStreamBlob;
        }

        protected override List<AssemblyRefTableRow> ParseRows()
        {
            var currentOffset = Offset;
            var rows = new List<AssemblyRefTableRow>((int) NumberOfRows);
            for (var i = 0; i < NumberOfRows; i++)
            {
                var row = new AssemblyRefTableRow(Buff, currentOffset, HeapIndexSizes, _metaDataStreamString,
                    _metaDataStreamBlob);
                rows.Add(row);
                currentOffset += row.Length;
            }

            return rows;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("AssemblyRefTable\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));
            foreach (var assemblyRefRow in Rows)
                sb.Append(assemblyRefRow);
            return sb.ToString();
        }
    }
}