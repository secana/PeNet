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
    public class IMAGE_DOS_HEADER
    {
        byte[] _buff;

        public UInt16 e_magic    
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x00);
            }
            set
            {
                Utility.SetUInt16(value, 0x00, _buff);
            }
        }

        public UInt16 e_cblp
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x02);
            }
            set
            {
                Utility.SetUInt16(value, 0x02, _buff);
            }
        }

        public UInt16 e_cp
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x04);
            }
            set
            {
                Utility.SetUInt16(value, 0x04, _buff);
            }
        }

        public UInt16 e_crlc
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x06);
            }
            set
            {
                Utility.SetUInt16(value, 0x06, _buff);
            }
        }

        public UInt16 e_cparhdr
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x08);
            }
            set
            {
                Utility.SetUInt16(value, 0x08, _buff);
            }
        }

        public UInt16 e_minalloc
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x0A);
            }
            set
            {
                Utility.SetUInt16(value, 0x0A, _buff);
            }
        }

        public UInt16 e_maxalloc
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x0C);
            }
            set
            {
                Utility.SetUInt16(value, 0x0C, _buff);
            }
        }

        public UInt16 e_ss
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x0E);
            }
            set
            {
                Utility.SetUInt16(value, 0x0E, _buff);
            }
        }

        public UInt16 e_sp
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x10);
            }
            set
            {
                Utility.SetUInt16(value, 0x10, _buff);
            }
        }

        public UInt16 e_csum
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x12);
            }
            set
            {
                Utility.SetUInt16(value, 0x12, _buff);
            }
        }

        public UInt16 e_ip
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x14);
            }
            set
            {
                Utility.SetUInt16(value, 0x14, _buff);
            }
        }

        public UInt16 e_cs
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x16);
            }
            set
            {
                Utility.SetUInt16(value, 0x16, _buff);
            }
        }

        public UInt16 e_lfarlc
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x18);
            }
            set
            {
                Utility.SetUInt16(value, 0x18, _buff);
            }
        }

        public UInt16 e_ovno
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x1A);
            }
            set
            {
                Utility.SetUInt16(value, 0x1A, _buff);
            }
        }

        public UInt16[] e_res // 4 * UInt16
        {
            get
            {
                return new UInt16[] 
                {
                    Utility.BytesToUInt16(_buff, 0x1C),
                    Utility.BytesToUInt16(_buff, 0x1E),
                    Utility.BytesToUInt16(_buff, 0x20),
                    Utility.BytesToUInt16(_buff, 0x22)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], 0x1C, _buff);
                Utility.SetUInt16(value[1], 0x1E, _buff);
                Utility.SetUInt16(value[2], 0x20, _buff);
                Utility.SetUInt16(value[3], 0x22, _buff);
            }
        }

        public UInt16 e_oemid
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x24);
            }
            set
            {
                Utility.SetUInt16(value, 0x24, _buff);
            }
        }

        public UInt16 e_oeminfo
        {
            get
            {
                return Utility.BytesToUInt16(_buff, 0x26);
            }
            set
            {
                Utility.SetUInt16(value, 0x26, _buff);
            }
        }

        public UInt16[] e_res2 // 10 * UInt16
        {
            get
            {
                return new UInt16[10] 
                {
                    Utility.BytesToUInt16(_buff, 0x28),
                    Utility.BytesToUInt16(_buff, 0x2A),
                    Utility.BytesToUInt16(_buff, 0x2C),
                    Utility.BytesToUInt16(_buff, 0x2E),
                    Utility.BytesToUInt16(_buff, 0x30),
                    Utility.BytesToUInt16(_buff, 0x32),
                    Utility.BytesToUInt16(_buff, 0x34),
                    Utility.BytesToUInt16(_buff, 0x36),
                    Utility.BytesToUInt16(_buff, 0x38),
                    Utility.BytesToUInt16(_buff, 0x3A)
                };
            }
            set
            {
                Utility.SetUInt16(value[0], 0x28, _buff);
                Utility.SetUInt16(value[1], 0x2A, _buff);
                Utility.SetUInt16(value[2], 0x2C, _buff);
                Utility.SetUInt16(value[3], 0x2E, _buff);
                Utility.SetUInt16(value[4], 0x30, _buff);
                Utility.SetUInt16(value[5], 0x32, _buff);
                Utility.SetUInt16(value[6], 0x34, _buff);
                Utility.SetUInt16(value[7], 0x36, _buff);
                Utility.SetUInt16(value[8], 0x38, _buff);
                Utility.SetUInt16(value[9], 0x3A, _buff);
            }
        }

        public UInt32 e_lfanew
        {
            get
            {
                return Utility.BytesToUInt32(_buff, 0x3C);
            }
            set
            {
                Utility.SetUInt32(value, 0x3C, _buff);
            }
        }

        public IMAGE_DOS_HEADER(byte[] buff)
        {
            _buff = buff;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DOS_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-10}:\t{1,10:X}\n"));
            return sb.ToString();
        }
    }
}
