using System.Linq;
using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet.Parser.MetaDataTables
{
    /// <summary>
    /// Parser for all Meta Data Tables in the Meta Data Tables Header 
    /// of the .Net header.
    /// </summary>
    internal class MetaDataTablesParser : SafeParser<Structures.MetaDataTables.MetaDataTables>
    {
        private readonly METADATATABLESHDR _metaDataTablesHdr;
        private readonly HeapOffsetBasedIndexSizes _heapOffsetBasedIndexSizes;
        private ModuleTableParser _moduleTableParser;
        private TypeRefTableParser _typeRefTableParser;

        /// <summary>
        /// Create a new MetaDataTablesParser instance.
        /// </summary>
        /// <param name="buff">Buffer containing all Meta Data Tables.</param>
        /// <param name="metaDataTablesHdr">The Meta Data Tables Header structure of the .Net header.</param>
        public MetaDataTablesParser(byte[] buff, METADATATABLESHDR metaDataTablesHdr)
            : base(buff, 0)
        {
            _metaDataTablesHdr = metaDataTablesHdr;
            _heapOffsetBasedIndexSizes = new HeapOffsetBasedIndexSizes(metaDataTablesHdr.HeapOffsetSizes);
            InitParsers();
        }

        private void InitParsers()
        {
            var currentTableOffset = (uint) (_metaDataTablesHdr.Offset + 0x18 + _metaDataTablesHdr.TableDefinitions.Count*0x4);
            _moduleTableParser = InitModuleTableParser(currentTableOffset);
        }

        private ModuleTableParser InitModuleTableParser(uint offset)
        {
            var tableDef =
                _metaDataTablesHdr.TableDefinitions.FirstOrDefault(
                    x => x.Name == DotNetConstants.MaskValidFlags.Module.ToString());

            return tableDef == null ? null : new ModuleTableParser(_buff, offset, tableDef.NumOfRows, _heapOffsetBasedIndexSizes);
        }

        protected override Structures.MetaDataTables.MetaDataTables ParseTarget()
        {
            var metaDataTables = new Structures.MetaDataTables.MetaDataTables();

            metaDataTables.ModuleTable = _moduleTableParser?.GetParserTarget();

            return metaDataTables;
        }
    }
}