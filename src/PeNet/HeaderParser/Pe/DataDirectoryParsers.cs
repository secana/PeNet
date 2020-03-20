using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;
using PeNet.Header.Pe;
using PeNet.Header.Resource;
using PeNet.HeaderParser.Resource;

namespace PeNet.HeaderParser.Pe
{
    internal class DataDirectoryParsers
    {
        private readonly IRawFile _peFile;
        private readonly ImageDataDirectory[] _dataDirectories;

        private readonly bool _is32Bit;
        private readonly ImageSectionHeader[] _sectionHeaders;
        private readonly ExportedFunctionsParser _exportedFunctionsParser;
        private readonly ImageBaseRelocationsParser? _imageBaseRelocationsParser;
        private readonly ImageDebugDirectoryParser? _imageDebugDirectoryParser;
        private readonly ImageBoundImportDescriptorParser? _imageBoundImportDescriptorParser;
        private readonly ImageExportDirectoriesParser? _imageExportDirectoriesParser;
        private readonly ImageImportDescriptorsParser? _imageImportDescriptorsParser;
        private readonly ImageResourceDirectoryParser? _imageResourceDirectoryParser;
        private readonly ImportedFunctionsParser _importedFunctionsParser;
        private readonly RuntimeFunctionsParser? _runtimeFunctionsParser;
        private readonly WinCertificateParser? _winCertificateParser;
        private readonly ImageTlsDirectoryParser? _imageTlsDirectoryParser;
        private readonly ImageDelayImportDescriptorParser? _imageDelayImportDescriptorParser;
        private readonly ImageLoadConfigDirectoryParser? _imageLoadConfigDirectoryParser;
        private readonly ImageCor20HeaderParser? _imageCor20HeaderParser;
        private readonly ResourcesParser? _resourcesParser;

        public DataDirectoryParsers(
            IRawFile peFile,
            IEnumerable<ImageDataDirectory>? dataDirectories,
            IEnumerable<ImageSectionHeader>? sectionHeaders,
            bool is32Bit
            )
        {
            _peFile = peFile;
            _dataDirectories = dataDirectories.ToArray();
            _sectionHeaders = sectionHeaders.ToArray();
            _is32Bit = is32Bit;

            // Init all parsers
            _imageExportDirectoriesParser = InitImageExportDirectoryParser();
            _runtimeFunctionsParser = InitRuntimeFunctionsParser();
            _imageImportDescriptorsParser = InitImageImportDescriptorsParser();
            _imageBaseRelocationsParser = InitImageBaseRelocationsParser();
            _imageResourceDirectoryParser = InitImageResourceDirectoryParser();
            _imageDebugDirectoryParser = InitImageDebugDirectoryParser();
            _winCertificateParser = InitWinCertificateParser();
            _exportedFunctionsParser = InitExportFunctionParser();
            _importedFunctionsParser = InitImportedFunctionsParser();
            _imageBoundImportDescriptorParser = InitBoundImportDescriptorParser();
            _imageTlsDirectoryParser = InitImageTlsDirectoryParser();
            _imageDelayImportDescriptorParser = InitImageDelayImportDescriptorParser();
            _imageLoadConfigDirectoryParser = InitImageLoadConfigDirectoryParser();
            _imageCor20HeaderParser = InitImageComDescriptorParser();
            _resourcesParser = InitResourcesParser();
        }

        public ImageExportDirectory? ImageExportDirectories => _imageExportDirectoriesParser?.GetParserTarget();
        public ImageImportDescriptor[]? ImageImportDescriptors => _imageImportDescriptorsParser?.GetParserTarget();
        public ImageResourceDirectory? ImageResourceDirectory => _imageResourceDirectoryParser?.GetParserTarget();
        public ImageBaseRelocation[]? ImageBaseRelocations => _imageBaseRelocationsParser?.GetParserTarget();
        public WinCertificate? WinCertificate => _winCertificateParser?.GetParserTarget();
        public ImageDebugDirectory[]? ImageDebugDirectory => _imageDebugDirectoryParser?.GetParserTarget();
        public RuntimeFunction[]? RuntimeFunctions => _runtimeFunctionsParser?.GetParserTarget();
        public ExportFunction[]? ExportFunctions => _exportedFunctionsParser.GetParserTarget();
        public ImportFunction[]? ImportFunctions => _importedFunctionsParser.GetParserTarget();
        public ImageBoundImportDescriptor? ImageBoundImportDescriptor => _imageBoundImportDescriptorParser?.GetParserTarget();
        public ImageTlsDirectory? ImageTlsDirectory => _imageTlsDirectoryParser?.GetParserTarget();
        public ImageDelayImportDescriptor? ImageDelayImportDescriptor => _imageDelayImportDescriptorParser?.GetParserTarget();
        public ImageLoadConfigDirectory? ImageLoadConfigDirectory => _imageLoadConfigDirectoryParser?.GetParserTarget();
        public ImageCor20Header? ImageComDescriptor => _imageCor20HeaderParser?.GetParserTarget();
        public Resources? Resources => _resourcesParser?.GetParserTarget();

        private ResourcesParser? InitResourcesParser()
        {
            var vsVersionOffset = ImageResourceDirectory
                ?.DirectoryEntries?.FirstOrDefault(e => e?.ID == (int)ResourceGroupIdType.Version) // Root
                ?.ResourceDirectory?.DirectoryEntries?.FirstOrDefault() // Type
                ?.ResourceDirectory?.DirectoryEntries?.FirstOrDefault() // Name
                ?.ResourceDataEntry?.OffsetToData; // Language

            if (vsVersionOffset is null)
                return null;

            return vsVersionOffset.Value.TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ResourcesParser(_peFile, 0, offset)
                : null;
        }

        private ImageCor20HeaderParser? InitImageComDescriptorParser()
            => _dataDirectories[(int)DataDirectoryType.ComDescriptor].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageCor20HeaderParser(_peFile, offset)
                : null;

        private ImageLoadConfigDirectoryParser? InitImageLoadConfigDirectoryParser()
            => _dataDirectories[(int)DataDirectoryType.LoadConfig].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageLoadConfigDirectoryParser(_peFile, offset, !_is32Bit)
                : null;

        private ImageDelayImportDescriptorParser? InitImageDelayImportDescriptorParser()
            => _dataDirectories[(int)DataDirectoryType.DelayImport].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageDelayImportDescriptorParser(_peFile, offset)
                : null;

        private ImageTlsDirectoryParser? InitImageTlsDirectoryParser()
            => _dataDirectories[(int)DataDirectoryType.TLS].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageTlsDirectoryParser(_peFile, offset, !_is32Bit, _sectionHeaders)
                : null;

        private ImageBoundImportDescriptorParser? InitBoundImportDescriptorParser()
            => _dataDirectories[(int)DataDirectoryType.BoundImport].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageBoundImportDescriptorParser(_peFile, offset)
                : null;

        private ImportedFunctionsParser InitImportedFunctionsParser()
            => new ImportedFunctionsParser(
                _peFile,
                ImageImportDescriptors,
                _sectionHeaders,
                _dataDirectories,
                !_is32Bit
                );

        private ExportedFunctionsParser InitExportFunctionParser()
            => new ExportedFunctionsParser(
                _peFile,
                ImageExportDirectories,
                _sectionHeaders,
                _dataDirectories[(int)DataDirectoryType.Export]);

        private WinCertificateParser? InitWinCertificateParser()
        {
            // The security directory is the only one where the DATA_DIRECTORY VirtualAddress
            // is not an RVA but an raw offset.
            var rawAddress = _dataDirectories[(int)DataDirectoryType.Security].VirtualAddress;
            return rawAddress == 0 ? null : new WinCertificateParser(_peFile, rawAddress);
        }

        private ImageDebugDirectoryParser? InitImageDebugDirectoryParser()
            => _dataDirectories[(int)DataDirectoryType.Debug].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageDebugDirectoryParser(_peFile, offset,
                    _dataDirectories[(int)DataDirectoryType.Debug].Size)
                : null;

        private ImageResourceDirectoryParser? InitImageResourceDirectoryParser()
            => _dataDirectories[(int)DataDirectoryType.Resource].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageResourceDirectoryParser(_peFile, offset)
                : null;

        private ImageBaseRelocationsParser? InitImageBaseRelocationsParser()
            => _dataDirectories[(int)DataDirectoryType.BaseReloc].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageBaseRelocationsParser(_peFile, offset,
                    _dataDirectories[(int)DataDirectoryType.BaseReloc].Size)
                : null;

        private ImageExportDirectoriesParser? InitImageExportDirectoryParser()
            => _dataDirectories[(int)DataDirectoryType.Export].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageExportDirectoriesParser(_peFile, offset)
                : null;


        private RuntimeFunctionsParser? InitRuntimeFunctionsParser()
            => _dataDirectories[(int)DataDirectoryType.Exception].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new RuntimeFunctionsParser(_peFile, offset, _is32Bit,
                    _dataDirectories[(int)DataDirectoryType.Exception].Size, _sectionHeaders)
                : null;

        private ImageImportDescriptorsParser? InitImageImportDescriptorsParser()
            => _dataDirectories[(int)DataDirectoryType.Import].VirtualAddress
                .TryRvaToOffset(_sectionHeaders, out var offset)
                ? new ImageImportDescriptorsParser(_peFile, offset)
                : null;
    }
}