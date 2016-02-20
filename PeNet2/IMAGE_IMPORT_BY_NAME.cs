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
    public class IMAGE_IMPORT_BY_NAME
    {
        byte[] _buff;
        UInt64 _offset;

        public UInt16 Hint
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
        }

        public string Name
        {
            get { return Utility.GetName(_offset + 0x2, _buff); }
        }

        public IMAGE_IMPORT_BY_NAME(byte[] buff, UInt64 offset)
        {
            _offset = offset;
            _buff = buff;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_IMPORT_BY_NAME\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
