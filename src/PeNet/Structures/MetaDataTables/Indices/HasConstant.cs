using System;

namespace PeNet.Structures.MetaDataTables.Indices
{
    public class HasConstant : AbstractIndex
    {
        protected HasConstant(
            byte[] buff, 
            uint offset, 
            params int[] numRowsTable) 
            : base(
                  buff, 
                  offset, 
                  3, 
                  numRowsTable)
        {
            switch(Tag)
            {
                case 0:
                    IsFieldDef = true;
                    break;
                case 1:
                    IsParamDef = true;
                    break;
                case 2:
                    IsProperty = true;
                    break;
                default:
                        throw new ArgumentOutOfRangeException("The index has an invalid value.");
            }
        }

        public bool IsFieldDef { get; }
        public bool IsParamDef { get; }
        public bool IsProperty { get; }
    }
}
