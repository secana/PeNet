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

using System;
using System.Collections.Generic;
using System.Linq;
using PeNet.Parser;
using PeNet.Structures;

namespace PeNet
{
    internal class DataDirectories
    {
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectories => _imageExportDirectoriesParser?.GetParserTarget();
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors => _imageImportDescriptorsParser?.GetParserTarget();
        public IMAGE_RESOURCE_DIRECTORY ImageResourceDirectory => _imageResourceDirectoryParser?.GetParserTarget();
        public IMAGE_BASE_RELOCATION[] ImageBaseRelocations => _imageBaseRelocationsParser?.GetParserTarget();
        public WIN_CERTIFICATE WinCertificate => _winCertificateParser?.GetParserTarget();
        public IMAGE_DEBUG_DIRECTORY ImageDebugDirectory => _imageDebugDirectoryParser?.GetParserTarget();
        public RUNTIME_FUNCTION[] RuntimeFunctions => _runtimeFunctionsParser?.GetParserTarget();

        private ImageExportDirectoriesParser _imageExportDirectoriesParser;
        private RuntimeFunctionsParser _runtimeFunctionsParser;
        private ImageImportDescriptorsParser _imageImportDescriptorsParser;
        private ImageBaseRelocationsParser _imageBaseRelocationsParser;
        private ImageResourceDirectoryParser _imageResourceDirectoryParser;
        private ImageDebugDirectoryParser _imageDebugDirectoryParser;
        private WinCertificateParser _winCertificateParser;

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
            _imageResourceDirectoryParser = InitImageResourceDirectoryParser();
            _imageDebugDirectoryParser = InitImageDebugDirectoryParser();
            _winCertificateParser = InitWinCertificateParser();
        }

        private WinCertificateParser InitWinCertificateParser()
        {
            // The security directory is the only one where the DATA_DIRECTORY VirtualAddress
            // is not an RVA but an raw offset.
            var rawAddress = _dataDirectories[(int) Constants.DataDirectoryIndex.Security].VirtualAddress;

            if (rawAddress == 0)
                return null;

            return new WinCertificateParser(_buff, rawAddress);
        }

        private ImageDebugDirectoryParser InitImageDebugDirectoryParser()
        {
            var rawAddress =
                SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.Debug].VirtualAddress);

            if (rawAddress == null)
                return null;

            return new ImageDebugDirectoryParser(_buff, rawAddress.Value);
        }

        private ImageResourceDirectoryParser InitImageResourceDirectoryParser()
        {
            var rawAddress =
                SafeRVAtoFileMapping(_dataDirectories[(int) Constants.DataDirectoryIndex.Resource].VirtualAddress);

            if (rawAddress == null)
                return null;

            return new ImageResourceDirectoryParser(_buff, rawAddress.Value);
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