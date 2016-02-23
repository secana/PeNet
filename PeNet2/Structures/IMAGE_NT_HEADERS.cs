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

using System.Text;

namespace PeNet
{
    public class IMAGE_NT_HEADERS
    {
        public readonly IMAGE_FILE_HEADER FileHeader;
        public readonly IMAGE_OPTIONAL_HEADER OptionalHeader;
        private readonly byte[] _buff;
        private readonly uint _offset;

        public IMAGE_NT_HEADERS(byte[] buff, uint offset, bool is64Bit)
        {
            _offset = offset;
            _buff = buff;
            FileHeader = new IMAGE_FILE_HEADER(buff, offset + 0x4);
            OptionalHeader = new IMAGE_OPTIONAL_HEADER(buff, offset + 0x18, is64Bit);
        }

        public uint Signature
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_NT_HEADERS\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            sb.Append(FileHeader);
            sb.Append(OptionalHeader);

            return sb.ToString();
        }
    }
}