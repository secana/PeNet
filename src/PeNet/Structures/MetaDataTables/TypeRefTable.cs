using System.Collections.Generic;

namespace PeNet.Structures.MetaDataTables
{
    public class TypeRefTable : AbstractStructure
    {
        private readonly uint _numOfRows;
        private List<TypeRefTableRow> _rows;

        public TypeRefTable(byte[] buff, uint offset, uint numOfRows) 
            : base(buff, offset)
        {
            _numOfRows = numOfRows;
        }

        public List<TypeRefTableRow> Rows => _rows ?? (_rows = ParseRows(_numOfRows));

        private List<TypeRefTableRow> ParseRows(uint numOfRows)
        {
            var rows = new List<TypeRefTableRow>((int) numOfRows);
            uint rowLength = 0; // TODO: Compute row length
            uint resolutionScopeSize = 0; // TODO: Compute size (2 or 4 bytes) based on the number of elements where the index points to
            uint stringSize = 4;


            for (var i = 0; i < numOfRows; i++)
            {
                rows.Add(new TypeRefTableRow(Buff, Offset + rowLength, resolutionScopeSize, stringSize));
            }

            return rows;
        }
     
    }
}