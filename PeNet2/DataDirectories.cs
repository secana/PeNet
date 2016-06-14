using System;
using System.Collections.Generic;
using System.Linq;
using PeNet.Parser;
using PeNet.Structures;

namespace PeNet
{
    public class DataDirectories
    {
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectories => _imageExportDirectoriesParser?.GetParserTarget(); 
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors { get; private set; }
        public IMAGE_RESOURCE_DIRECTORY ImageResourceDirectory { get; private set; }
        public IMAGE_BASE_RELOCATION[] ImageBaseRelocations { get; private set; }
        public WIN_CERTIFICATE WinCertificate { get; private set; }
        public IMAGE_DEBUG_DIRECTORY ImageDebugDirectory { get; private set; }
        public RUNTIME_FUNCTION[] RuntimeFunctions => _runtimeFunctionsParser?.GetParserTarget();

        private ImageExportDirectoriesParser _imageExportDirectoriesParser;
        private RuntimeFunctionsParser _runtimeFunctionsParser;
        private readonly byte[] _buff;
        private readonly IMAGE_DATA_DIRECTORY[] _dataDirectories;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;
        private List<Exception> _rvaToFileMappingExceptions = new List<Exception>();
        private readonly bool _is32Bit;

        public DataDirectories(
            byte[] buff, 
            ICollection<IMAGE_DATA_DIRECTORY> dataDirectories, 
            ICollection<IMAGE_SECTION_HEADER> sectionHeaders,
            bool is32Bit
            )
        {
            _buff = buff;
            _dataDirectories = dataDirectories.ToArray();
            _sectionHeaders = sectionHeaders.ToArray();
            _is32Bit = is32Bit;

            InitAllParsers();
        }

        private void InitAllParsers()
        {
            _imageExportDirectoriesParser = InitImageExportDirectoryParser();
            _runtimeFunctionsParser = InitRuntimeFunctionsParser();
        }

        private ImageExportDirectoriesParser InitImageExportDirectoryParser()
        {
            var rawAddress = SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.Export].VirtualAddress);
            if (rawAddress == null)
                return null;

            return new ImageExportDirectoriesParser(_buff, rawAddress.Value);
        }

        private RuntimeFunctionsParser InitRuntimeFunctionsParser()
        {
            var rawAddress =
                SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.Exception].VirtualAddress);

            if (rawAddress == null)
                return null;

            return  new RuntimeFunctionsParser(
                _buff,
                rawAddress.Value,
                _is32Bit,
                _dataDirectories[(int) Constants.DataDirectoryIndex.Exception].Size,
                _sectionHeaders
                );
        }

        private uint? SafeRVAtoFileMapping(uint rva)
        {
            uint? rawAddress = null;
            try
            {
                rawAddress = Utility.RVAtoFileMapping(rva, _sectionHeaders);
            }
            catch (Exception exception)
            {
                _rvaToFileMappingExceptions.Add(exception);
            }

            return rawAddress;
        }
    }
}