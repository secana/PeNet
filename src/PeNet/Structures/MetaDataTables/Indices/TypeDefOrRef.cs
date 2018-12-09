using System;

namespace PeNet.Structures.MetaDataTables.Indices
{
    public class TypeDefOrRef : AbstractIndex
    {
        public TypeDefOrRef(
            byte[] buff,
            uint offset,
            int numRowsTypeDefTable, 
            int numRowsTypeRefTable, 
            int numRowsTypeSpecTable)
        : base(
            buff, 
            offset, 
            3, 
            numRowsTypeDefTable, 
            numRowsTypeRefTable, 
            numRowsTypeSpecTable)
        {
            switch (Tag)
            {
                    case 0:
                        IsTypeDefIndex = true;
                        break;
                    case 1:
                        IsTypeRefIndex = true;
                        break;
                    case 2:
                        IsTypeSpecIndex = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("The index has an invalid value.");
            }

        }

        public bool IsTypeDefIndex { get; }
        public bool IsTypeRefIndex { get; }
        public bool IsTypeSpecIndex { get; }
    }
}