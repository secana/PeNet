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
    public class IMAGE_DATA_DIRECTORY
    {
        byte[] _buff;
        UInt32 _offset;

        public UInt32 VirtualAddress
        {
            get { return Utility.BytesToUInt32(_buff, _offset); }
            set { Utility.SetUInt32(value, _offset, _buff); }
        }

        public UInt32 Size
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        public IMAGE_DATA_DIRECTORY(byte[] buff, UInt32 offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DATA_DIRECTORY\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));

            return sb.ToString();
        }
    }
}
