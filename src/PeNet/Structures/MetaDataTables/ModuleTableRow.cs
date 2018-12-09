using System;
using System.Text;
using PeNet.Structures.MetaDataTables.Indices;
using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// A row in the Module Table of the Meta Data Tables Header
    /// in the .Net header.
    /// </summary>
    /// <inheritdoc />
    public class ModuleTableRow : AbstractMetaDataTableRow
    {
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly IMETADATASTREAM_GUID _metaDataStreamGuid;

        /// <summary>
        /// Create a new ModuleTableRow instance.
        /// </summary>
        /// <param name="buff">Buffer which contains the row.</param>
        /// <param name="offset">Offset in the buff, where the header starts.</param>
        /// <param name="metaDataStreamString">Meta Data stream "String" object to resolve strings in 
        /// the ModuleTableRow.</param>
        /// <param name="metaDataStreamGuid">Meta Data stream "GUID" object to resolve GUIDs in 
        /// the ModuleTableRow.</param>
        /// <param name="heapOffsetSizes">Computes sizes of the heap bases indexes.</param>
        public ModuleTableRow(
            byte[] buff, 
            uint offset,
            IMETADATASTREAM_STRING metaDataStreamString, 
            IMETADATASTREAM_GUID metaDataStreamGuid,
            IHeapOffsetSizes heapOffsetSizes
            ) 
            : base(buff, offset, heapOffsetSizes)
        {
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamGuid = metaDataStreamGuid;
        }

        /// <summary>
        /// Reserved, should be 0.
        /// </summary>
        public ushort Generation
        {
            get { return Buff.BytesToUInt16(Offset); }
            set { Buff.SetUInt16(Offset, value); }
        }

        /// <summary>
        /// Index into #String heap which contains the assembly name.
        /// </summary>
        public uint Name => Buff.BytesToUInt32(Offset + 0x2, HeapIndexSizes.StringIndexSize);

        /// <summary>
        /// The resolved "Name" attribute of this row.
        /// </summary>
        public string NameResolved => _metaDataStreamString.GetStringAtIndex(Name);

        /// <summary>
        /// Index into #GUID heap which contains the module version ID.
        /// </summary>
        public uint Mvid
            => Buff.BytesToUInt32(Offset + 0x2 + HeapIndexSizes.StringIndexSize, HeapIndexSizes.GuidIndexSize);

        /// <summary>
        /// Resolved "Mvid" (GUID) attribute of this row.
        /// </summary>
        public Guid? MvidResolved => _metaDataStreamGuid.GetGuidAtIndex(Mvid);

        /// <summary>
        /// Index into GUID heap. Reserved, should be 0.
        /// </summary>
        public uint EncId
            =>
            Buff.BytesToUInt32(Offset + 0x2 + HeapIndexSizes.StringIndexSize + HeapIndexSizes.GuidIndexSize,
                HeapIndexSizes.GuidIndexSize);

        /// <summary>
        /// Index into GUID heap. Reserved, should be 0.
        /// </summary>
        public uint EncBaseId
            =>
            Buff.BytesToUInt32(Offset + 0x2 + HeapIndexSizes.StringIndexSize + HeapIndexSizes.GuidIndexSize*2,
                HeapIndexSizes.GuidIndexSize);

        /// <inheritdoc />
        public override uint Length => 0x2 + HeapIndexSizes.StringIndexSize + HeapIndexSizes.GuidIndexSize*3;

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder("ModuleTableRow\n");
            sb.Append(this.PropertiesToString("{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}