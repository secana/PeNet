using System;
using System.Collections.Generic;
using System.Linq;
/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using PeNet.Parser;
using PeNet.Structures;

namespace PeNet
{
    internal class DataDirectories
    {
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectories => _imageExportDirectoriesParser?.GetParserTarget();
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors => _imageImportDescriptorsParser?.GetParserTarget();
        public IMAGE_RESOURCE_DIRECTORY ImageResourceDirectory { get; private set; }
        public IMAGE_BASE_RELOCATION[] ImageBaseRelocations => _imageBaseRelocationsParser?.GetParserTarget();
        public WIN_CERTIFICATE WinCertificate { get; private set; }
        public IMAGE_DEBUG_DIRECTORY ImageDebugDirectory { get; private set; }
        public RUNTIME_FUNCTION[] RuntimeFunctions => _runtimeFunctionsParser?.GetParserTarget();

        private ImageExportDirectoriesParser _imageExportDirectoriesParser;
        private RuntimeFunctionsParser _runtimeFunctionsParser;
        private ImageImportDescriptorsParser _imageImportDescriptorsParser;
        private ImageBaseRelocationsParser _imageBaseRelocationsParser;

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
            _imageImportDescriptorsParser = InitImageImportDescriptorsParser();
            _imageBaseRelocationsParser = InitImageBaseRelocationsParser();
        }

        private ImageBaseRelocationsParser InitImageBaseRelocationsParser()
        {
            var rawAddress =
                SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].VirtualAddress);

            if (rawAddress == null)
                return null;

            return new ImageBaseRelocationsParser(
                _buff,
                rawAddress.Value,
                _dataDirectories[(int) Constants.DataDirectoryIndex.BaseReloc].Size,
                _sectionHeaders
                );
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

        private ImageImportDescriptorsParser InitImageImportDescriptorsParser()
        {
            var rawAddress =
                SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.Import].VirtualAddress);

            if (rawAddress == null)
                return null;

            return new ImageImportDescriptorsParser(_buff, rawAddress.Value);
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