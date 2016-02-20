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
    public class IMAGE_THUNK_DATA
    {
        byte[] _buff;
        UInt32 _offset;
        bool _is64Bit;

        public UInt64 AddressOfData
        {
            get 
            { 
                return _is64Bit ? Utility.BytesToUInt64(_buff, _offset) : Utility.BytesToUInt32(_buff, _offset); 
            }
            set 
            {
                if (!_is64Bit)
                    Utility.SetUInt32((UInt32)value, _offset, _buff);
                else
                    Utility.SetUInt64(value, _offset, _buff);
            }
        }

        public UInt64 Ordinal
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt64 ForwarderString
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public UInt64 Function
        {
            get { return AddressOfData; }
            set { AddressOfData = value; }
        }

        public IMAGE_THUNK_DATA(byte[] buff, UInt32 offset, bool is64Bit)
        {
            _buff = buff;
            _offset = offset;
            _is64Bit = is64Bit;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_THUNK_DATA\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
