using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// A row in the AssemblyRef Table of the Meta Data Tables Header
    /// in the .Net header.
    /// </summary>
    /// <inheritdoc />
    public class AssemblyRefTableRow : AbstractMetaDataTableRow
    {
        public AssemblyRefTableRow(
            byte[] buff, 
            uint offset,
            IHeapOffsetBasedIndexSizes heapOffsetSizes,
            IMETADATASTREAM_STRING metaDataStreamString,
            IMETADATASTREAM_BLOB metaDataStreamBlob
            ) 
            : base(buff, offset, heapOffsetSizes)
        {
        }

        /// <summary>
        /// Major assembly version.
        /// </summary>
        public ushort MajorVersion => Buff.BytesToUInt16(Offset);

        /// <summary>
        /// Minor assembly version.
        /// </summary>
        public ushort MinorVersion => Buff.BytesToUInt16(Offset + 0x2);

        /// <summary>
        /// Assembly build number.
        /// </summary>
        public ushort BuildNumber => Buff.BytesToUInt16(Offset + 0x4);

        /// <summary>
        /// Assembly revision number.
        /// </summary>
        public ushort RevisionNumber => Buff.BytesToUInt16(Offset + 0x6);

        /// <summary>
        /// Assembly flags. The same as used by the Assembly table.
        /// </summary>
        public uint Flags => Buff.BytesToUInt32(Offset + 0x8);

        /// <summary>
        /// Index into the Blob heap with PublicKey or Token information.
        /// </summary>
        public uint PublicKeyOrToken => Buff.BytesToUInt32(Offset + 0xC, HeapIndexSizes.BlobSize);

        /// <summary>
        /// Index into the String heap to get the Assembly name.
        /// </summary>
        public uint Name => Buff.BytesToUInt32(Offset + 0xC + HeapIndexSizes.BlobSize, HeapIndexSizes.StringIndexSize);

        /// <summary>
        /// Index into the Culture heap to get the Assembly culture.
        /// </summary>
        public uint Culture => Buff.BytesToUInt32(Offset + HeapIndexSizes.BlobSize + HeapIndexSizes.StringIndexSize,
            HeapIndexSizes.StringIndexSize);

        /// <summary>
        /// Index into the Blob heap to get the Assembly hash.
        /// </summary>
        public uint HashValue =>
            Buff.BytesToUInt32(Offset + HeapIndexSizes.BlobSize + HeapIndexSizes.StringIndexSize * 2,
                HeapIndexSizes.BlobSize);

        /// <inheritdoc />
        public override uint Length => 0xC + HeapIndexSizes.BlobSize * 2 + HeapIndexSizes.StringIndexSize * 2;

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder("AssemblyRefTableRow\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}