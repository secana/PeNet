using System.Linq;
using PeNet.FileParser;
using PeNet.Parser;
using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet
{
    internal class DotNetStructureParsers
    {
        private readonly IRawFile _peFile;
        private readonly ImageSectionHeader[]? _sectionHeaders;
        private readonly ImageCor20Header? _imageCor20Header;
        private readonly MetaDataHdrParser? _metaDataHdrParser;
        private readonly MetaDataStreamStringParser? _metaDataStreamStringParser;
        private readonly MetaDataStreamUsParser? _metaDataStreamUSParser;
        private readonly MetaDataStreamTablesHeaderParser? _metaDataStreamTablesHeaderParser;
        private readonly MetaDataStreamGuidParser? _metaDataStreamGuidParser;
        private readonly MetaDataStreamBlobParser? _metaDataStreamBlobParser;

        public MetaDataHdr? MetaDataHdr => _metaDataHdrParser?.GetParserTarget();
        public MetaDataStreamString? MetaDataStreamString => _metaDataStreamStringParser?.GetParserTarget();
        public MetaDataStreamUs? MetaDataStreamUS => _metaDataStreamUSParser?.GetParserTarget();
        public MetaDataStreamGuid? MetaDataStreamGUID => _metaDataStreamGuidParser?.GetParserTarget();
        public byte[]? MetaDataStreamBlob => _metaDataStreamBlobParser?.GetParserTarget();
        public MetaDataTablesHdr? MetaDataStreamTablesHeader => _metaDataStreamTablesHeaderParser?.GetParserTarget();

        public DotNetStructureParsers(
            IRawFile peFile,
            ImageCor20Header? imageCor20Header,
            ImageSectionHeader[]? sectionHeaders)
        {
            _peFile = peFile;
            _sectionHeaders = sectionHeaders;
            _imageCor20Header = imageCor20Header;

            // Init all parsers
            _metaDataHdrParser = InitMetaDataParser();
            _metaDataStreamStringParser = InitMetaDataStreamStringParser();
            _metaDataStreamUSParser = InitMetaDataStreamUSParser();
            _metaDataStreamTablesHeaderParser = InitMetaDataStreamTablesHeaderParser();
            _metaDataStreamGuidParser = InitMetaDataStreamGUIDParser();
            _metaDataStreamBlobParser = InitMetaDataStreamBlobParser();
        }

        private MetaDataHdrParser? InitMetaDataParser()
        {
            var rawAddress = _imageCor20Header?.MetaData?.VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);
            return rawAddress == null ? null : new MetaDataHdrParser(_peFile, rawAddress.Value);
        }

        private MetaDataStreamStringParser? InitMetaDataStreamStringParser()
        {
            var metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.StreamName == "#Strings");

            return metaDataStream == null 
                ? null 
                : new MetaDataStreamStringParser(_peFile, MetaDataHdr!.Offset + metaDataStream.RelOffset, metaDataStream.Size);
        }

        private MetaDataStreamUsParser? InitMetaDataStreamUSParser()
        {
            var metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.StreamName == "#US");

            return metaDataStream == null 
                ? null 
                : new MetaDataStreamUsParser(_peFile, MetaDataHdr!.Offset + metaDataStream.RelOffset, metaDataStream.Size);
        }

        private MetaDataStreamTablesHeaderParser? InitMetaDataStreamTablesHeaderParser()
        {
            var metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.StreamName == "#~");

            return metaDataStream == null 
                ? null 
                : new MetaDataStreamTablesHeaderParser(_peFile, MetaDataHdr!.Offset + metaDataStream.RelOffset);
        }

        private MetaDataStreamGuidParser? InitMetaDataStreamGUIDParser()
        {
            var metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.StreamName == "#GUID");

            return metaDataStream == null 
                ? null 
                : new MetaDataStreamGuidParser(_peFile, MetaDataHdr!.Offset +  metaDataStream.RelOffset, metaDataStream.Size);
        }

        private MetaDataStreamBlobParser? InitMetaDataStreamBlobParser()
        {
            var metaDataStream = MetaDataHdr?.MetaDataStreamsHdrs?.FirstOrDefault(x => x.StreamName == "#Blob");

            return metaDataStream == null 
                ? null 
                : new MetaDataStreamBlobParser(_peFile, MetaDataHdr!.Offset + metaDataStream.RelOffset, metaDataStream.Size);
        }
    }
}