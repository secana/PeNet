using System.Collections.Generic;
using System.Linq;
using PeNet.FileParser;
using PeNet.Parser;
using PeNet.Structures;
using PeNet.Utilities;

namespace PeNet
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
                ?.DirectoryEntries?.FirstOrDefault(e => e?.ID == (int) ResourceGroupIdType.Version) // Root
                ?.ResourceDirectory?.DirectoryEntries?.FirstOrDefault() // Type
                ?.ResourceDirectory?.DirectoryEntries?.FirstOrDefault() // Name
                ?.ResourceDataEntry?.OffsetToData; // Language

            if (vsVersionOffset is null)
                return null;

            var rawVsVersionOffset = vsVersionOffset.Value.SafeRVAtoFileMapping(_sectionHeaders);

            return rawVsVersionOffset is null ? null : new ResourcesParser(_peFile, 0, rawVsVersionOffset.Value);
        }

        private ImageCor20HeaderParser? InitImageComDescriptorParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.COM_Descriptor].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageCor20HeaderParser(_peFile, rawAddress.Value);
        }

        private ImageLoadConfigDirectoryParser? InitImageLoadConfigDirectoryParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.LoadConfig].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageLoadConfigDirectoryParser(_peFile, rawAddress.Value, !_is32Bit);
        }

        private ImageDelayImportDescriptorParser? InitImageDelayImportDescriptorParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.DelayImport].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageDelayImportDescriptorParser(_peFile, rawAddress.Value);
        }

        private ImageTlsDirectoryParser? InitImageTlsDirectoryParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.TLS].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageTlsDirectoryParser(_peFile, rawAddress.Value, !_is32Bit, _sectionHeaders);
        }

        private ImageBoundImportDescriptorParser? InitBoundImportDescriptorParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.BoundImport].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageBoundImportDescriptorParser(_peFile, rawAddress.Value);
        }

        private ImportedFunctionsParser InitImportedFunctionsParser() {
            return new ImportedFunctionsParser(
                _peFile,
                ImageImportDescriptors,
                _sectionHeaders,
                _dataDirectories,
                !_is32Bit
                );
        }

        private ExportedFunctionsParser InitExportFunctionParser()
        {
            return new ExportedFunctionsParser(
                _peFile, 
                ImageExportDirectories, 
                _sectionHeaders, 
                _dataDirectories[(int) Constants.DataDirectoryIndex.Export]);
        }

        private WinCertificateParser? InitWinCertificateParser()
        {
            // The security directory is the only one where the DATA_DIRECTORY VirtualAddress
            // is not an RVA but an raw offset.
            var rawAddress = _dataDirectories[(int) Constants.DataDirectoryIndex.Security].VirtualAddress;

            if (rawAddress == 0)
                return null;

            return new WinCertificateParser(_peFile, rawAddress);
        }

        private ImageDebugDirectoryParser? InitImageDebugDirectoryParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.Debug].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);
            var size = _dataDirectories[(int)Constants.DataDirectoryIndex.Debug].Size;

            return rawAddress == null ? null : new ImageDebugDirectoryParser(_peFile, rawAddress.Value, size);
        }

        private ImageResourceDirectoryParser? InitImageResourceDirectoryParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.Resource].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageResourceDirectoryParser(_peFile, rawAddress.Value);
        }

        private ImageBaseRelocationsParser? InitImageBaseRelocationsParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            if (rawAddress == null)
                return null;

            return new ImageBaseRelocationsParser(
                _peFile,
                rawAddress.Value,
                _dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].Size
                );
        }

        private ImageExportDirectoriesParser? InitImageExportDirectoryParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.Export].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);
            if (rawAddress == null)
                return null;

            return new ImageExportDirectoriesParser(_peFile, rawAddress.Value);
        }

        private RuntimeFunctionsParser? InitRuntimeFunctionsParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.Exception].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            if (rawAddress == null)
                return null;

            return new RuntimeFunctionsParser(
                _peFile,
                rawAddress.Value,
                _is32Bit,
                _dataDirectories[(int) Constants.DataDirectoryIndex.Exception].Size,
                _sectionHeaders
                );
        }

        private ImageImportDescriptorsParser? InitImageImportDescriptorsParser()
        {
            var rawAddress =
                _dataDirectories[(int) Constants.DataDirectoryIndex.Import].VirtualAddress.SafeRVAtoFileMapping(_sectionHeaders);

            return rawAddress == null ? null : new ImageImportDescriptorsParser(_peFile, rawAddress.Value);
        }

        
    }
}