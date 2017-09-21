using PeNet.Utilities;

namespace PeNet.Structures.MetaDataTables
{
    /// <summary>
    /// TypeDef Table row in the Meta Data Tables Header of
    /// the .Net header.
    /// </summary>
    public class TypeRefTableRow : AbstractMetaDataTableRow
    {
        private readonly uint _resolutionScopeSize;
        private readonly uint _stringSize;
        private uint _internalOffset;

        /// <summary>
        /// Create a new TypeRef Table Row instance.
        /// </summary>
        /// <param name="buff">Buffer containing the row.</param>
        /// <param name="offset">Offset in the buffer where the row starts.</param>
        /// <param name="resolutionScopeSize">Size of the resolution scope field in bytes (2 or 4)</param>
        /// <param name="stringSize">Size of the string fields in bytes (2 or 4)</param>
        public TypeRefTableRow(byte[] buff, uint offset, uint resolutionScopeSize, uint stringSize) 
            : base(buff, offset)
        {
            _resolutionScopeSize = resolutionScopeSize;
            _stringSize = stringSize;
        }

        public uint ResolutionScope => Buff.BytesToUInt32(Offset, _resolutionScopeSize, ref _internalOffset);

        public uint TypeName => Buff.BytesToUInt32(Offset + _internalOffset, _stringSize, ref _internalOffset);

        public uint TypeNamespace => Buff.BytesToUInt32(Offset + _internalOffset, _stringSize, ref _internalOffset);

        /// <summary>
        /// Length of the row in bytes.
        /// </summary>
        public override uint Length => _internalOffset;
    }
}