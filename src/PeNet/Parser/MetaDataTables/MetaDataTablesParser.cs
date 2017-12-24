using System.Linq;
using PeNet.Structures;
using PeNet.Structures.MetaDataTables.Indices;
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
        private readonly IMETADATASTREAM_STRING _metaDataStreamString;
        private readonly IMETADATASTREAM_GUID _metaDataStreamGuid;
        private readonly IMETADATASTREAM_BLOB _metaDataStreamBlob;
        private readonly HeapOffsetSizes _heapOffsetSizes;
        private ModuleTableParser _moduleTableParser;
        private AssemblyRefTableParser _assemblyRefTableParser;


        /// <summary>
        /// Create a new MetaDataTablesParser instance.
        /// </summary>
        /// <param name="buff">Buffer containing all Meta Data Tables.</param>
        /// <param name="metaDataTablesHdr">The Meta Data Tables Header structure of the .Net header.</param>
        /// <param name="metaDataStreamString">Meta Data stream "String".</param>
        /// <param name="metaDataStreamGuid">Meta Data stream "GUID".</param>
        /// <param name="metaDataStreamBlob">Meta Data stream "Blob".</param>
        public MetaDataTablesParser(
            byte[] buff, 
            METADATATABLESHDR metaDataTablesHdr, 
            IMETADATASTREAM_STRING metaDataStreamString, 
            IMETADATASTREAM_GUID metaDataStreamGuid,
            IMETADATASTREAM_BLOB metaDataStreamBlob
            )
            : base(buff, 0)
        {
            _metaDataTablesHdr = metaDataTablesHdr;
            _metaDataStreamString = metaDataStreamString;
            _metaDataStreamGuid = metaDataStreamGuid;
            _metaDataStreamBlob = metaDataStreamBlob;
            _heapOffsetSizes = new HeapOffsetSizes(metaDataTablesHdr.HeapOffsetSizes);
            InitParsers();
        }

        private void InitParsers()
        {
            var currentTableOffset = (uint) (_metaDataTablesHdr.Offset + 0x18 + _metaDataTablesHdr.TableDefinitions.Count*0x4);
            _moduleTableParser = InitModuleTableParser(currentTableOffset);
            _assemblyRefTableParser = InitAssemblyRefTableParser(currentTableOffset);
        }

        private ModuleTableParser InitModuleTableParser(uint offset)
        {
            var tableDef =
                _metaDataTablesHdr.TableDefinitions.FirstOrDefault(
                    x => x.Name == DotNetConstants.MaskValidFlags.Module.ToString());

            return tableDef == null
                ? null
                : new ModuleTableParser(
                    _buff,
                    offset,
                    tableDef.NumOfRows,
                    _metaDataStreamString,
                    _metaDataStreamGuid,
                    _heapOffsetSizes
                );
        }

        private AssemblyRefTableParser InitAssemblyRefTableParser(uint offset)
        {
            var tableDef = _metaDataTablesHdr.TableDefinitions.FirstOrDefault(x =>
                x.Name == DotNetConstants.MaskValidFlags.AssemblyRef.ToString());

            return tableDef == null
                ? null
                : new AssemblyRefTableParser(
                    _buff,
                    _offset,
                    tableDef.NumOfRows,
                    _heapOffsetSizes,
                    _metaDataStreamString,
                    _metaDataStreamBlob
                );
        }

        protected override Structures.MetaDataTables.MetaDataTables ParseTarget()
        {
            var metaDataTables = new Structures.MetaDataTables.MetaDataTables();

            metaDataTables.ModuleTable = _moduleTableParser?.GetParserTarget();
            metaDataTables.AssemblyRefTable = _assemblyRefTableParser?.GetParserTarget();

            return metaDataTables;
        }
    }
}