using System;
using System.Collections.Generic;
using System.Linq;

namespace PeNet.Structures.MetaDataTables
{
    public class MetaDataTableIndexComputation
    {
        private readonly IMETADATATABLESHDR _metaDataTablesHeader;

        public MetaDataTableIndexComputation(IMETADATATABLESHDR metaDataTablesHeader)
        {
            _metaDataTablesHeader = metaDataTablesHeader;
        }

        public Tuple<string, uint> GetTableNameAndIndex(uint index)
        {
            // TODO: return the name of the table to which the index points and the index
            return null;
        }

        public uint GetTableIndexSize(Type indexEnumType)
        {
            if(!indexEnumType.IsEnum)
                throw new ArgumentException("Generic parameter must be of type enum.");

            var names = Enum.GetNames(indexEnumType);
            var maxRows = GetMaxRows(names);
            return GetIndexSize(names.Length, maxRows);
        }

        private uint GetIndexSize(int numOfChoices, uint maxRows)
        {
            var numOfTagBits = (int) Math.Ceiling(Math.Log(numOfChoices, 2));
            var numOfIndexBits = sizeof(ushort) * 8 - numOfTagBits;
            var numOfIndexableRows = (uint) Math.Pow(numOfIndexBits, 2);

            return (uint) (maxRows > numOfIndexableRows ? 4 : 2);
        }

        private uint GetMaxRows(IEnumerable<string> names)
        {
            return names
                .Select(name => _metaDataTablesHeader.TableDefinitions.FirstOrDefault(x => x.Name == name))
                .Where(tableDef => tableDef != null)
                .Max(x => x.NumOfRows);
        }
    }
}