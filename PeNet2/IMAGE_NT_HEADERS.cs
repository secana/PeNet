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
using System.Text;

namespace PeNet
{
    public class IMAGE_NT_HEADERS
    {
        UInt32 _offset;
        byte[] _buff;
        bool _is64Bit;

        public UInt32 Signature
        {
            get
            {
                return Utility.BytesToUInt32(_buff, _offset);
            }
            set
            {
                Utility.SetUInt32(value, _offset, _buff);
            }
        }

        public readonly IMAGE_FILE_HEADER FileHeader;
        public readonly IMAGE_OPTIONAL_HEADER OptionalHeader;
        
        public IMAGE_NT_HEADERS(byte[] buff, UInt32 offset, bool is64Bit)
        {
            _offset = offset;
            _buff = buff;
            _is64Bit = is64Bit;
            FileHeader = new IMAGE_FILE_HEADER(buff, offset + 0x4);
            OptionalHeader = new IMAGE_OPTIONAL_HEADER(buff, offset + 0x18, _is64Bit);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_NT_HEADERS\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            sb.Append(FileHeader.ToString());
            sb.Append(OptionalHeader.ToString());

            return sb.ToString();
        }
    }
}
