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

namespace PeNet.Structures
{
    public class IMAGE_TLS_DIRECTORY : AbstractStructure
    {
        private bool _is64Bit;

        public IMAGE_TLS_DIRECTORY(byte[] buff, uint offset, bool is64Bit) 
            : base(buff, offset)
        {
            _is64Bit = is64Bit;
        }

        public ulong StartAddressOfRawData
        {
            get { return Buff.BytesToUInt64(0); }
            set { Buff.SetUInt64(value, 0); } 
        }
    }
}