using System.Collections.Generic;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    public class AssemblyRefTable : AbstractMetaDataTable<AssemblyRefTableRow>
    {
        public AssemblyRefTable(
            byte[] buff, 
            uint offset, 
            uint numberOfRows,
            IHeapOffsetBasedIndexSizes heapIndexSizex
            ) : base(buff, offset, numberOfRows, heapIndexSizex)
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
        /// Assembly flags.
        /// </summary>
        public uint Flags => Buff.BytesToUInt32(Offset + 0x8);

        /// <summary>
        /// Index into the Blob heap with PublicKey or Token information.
        /// </summary>
        public uint PublicKeyOrToken => Buff.BytesToUInt32(Offset + 0xC, HeapIndexSizes.BlobSize);

        protected override List<AssemblyRefTableRow> ParseRows()
        {
            throw new System.NotImplementedException();
        }
    }
}