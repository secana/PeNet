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
    internal class DotNetStructureParsers
    {
        private readonly byte[] _buff;
        private readonly IMAGE_SECTION_HEADER[] _sectionHeaders;
        private readonly IMAGE_COR20_HEADER _imageCor20Header;
        private MetaDataHdrParser _metaDataHdrParser;

        public METADATAHDR MetaDataHdr => _metaDataHdrParser?.GetParserTarget();

        public DotNetStructureParsers(
            byte[] buff,
            IMAGE_COR20_HEADER imageCor20Header,
            IMAGE_SECTION_HEADER[] sectionHeaders)
        {
            _buff = buff;
            _sectionHeaders = sectionHeaders;
            _imageCor20Header = imageCor20Header;

            InitAllParsers();
        }

        private void InitAllParsers()
        {
            _metaDataHdrParser = InitMetaDataParser();
        }

        private MetaDataHdrParser InitMetaDataParser()
        {
            if (_imageCor20Header == null)
                return null;

            var rawAddress = Utility.SafeRVAtoFileMapping(_imageCor20Header.MetaData.VirtualAddress, _sectionHeaders);

            if (rawAddress == null)
                return null;

            return new MetaDataHdrParser(_buff, rawAddress.Value);
        }
    }
}