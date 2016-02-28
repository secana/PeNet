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

namespace PeNet.Structures
{
    public class UNWIND_CODE
    {
        private readonly byte[] _buff;
        private readonly uint _offset;

        public UNWIND_CODE(byte[] buff, uint offset)
        {
            _buff = buff;
            _offset = offset;
        }

        public byte CodeOffset
        {
            get { return _buff[_offset]; }
            set { _buff[_offset] = value; }
        }

        public byte UnwindOp
        {
            get { return (byte) (_buff[_offset + 0x1] & 0xF); }
        }

        public byte Opinfo
        {
            get { return (byte) (_buff[_offset + 0x1] >> 0x4); }
        }

        public ushort FrameOffset
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2); }
            set { Utility.SetUInt16(value, _offset + 0x2, _buff); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder("UNWIND_CODE\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-20}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}