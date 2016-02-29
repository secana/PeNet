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

namespace PeNet.Structures
{
    /// <summary>
    /// </summary>
    public class IMAGE_OPTIONAL_HEADER
    {
        private readonly byte[] _buff;
        private readonly bool _is64Bit;
        private readonly uint _offset;

        /// <summary>
        ///     The Data Directories.
        /// </summary>
        public readonly IMAGE_DATA_DIRECTORY[] DataDirectory;

        public IMAGE_OPTIONAL_HEADER(byte[] buff, uint offset, bool is64Bit)
        {
            _buff = buff;
            _offset = offset;
            _is64Bit = is64Bit;

            DataDirectory = new IMAGE_DATA_DIRECTORY[16];

            for (uint i = 0; i < 16; i++)
            {
                if (!_is64Bit)
                    DataDirectory[i] = new IMAGE_DATA_DIRECTORY(buff, offset + 0x60 + i*0x8);
                else
                    DataDirectory[i] = new IMAGE_DATA_DIRECTORY(buff, offset + 0x70 + i*0x8);
            }
        }

        /// <summary>
        ///     Flag if the file is x32, x64 or a ROM image.
        /// </summary>
        public ushort Magic
        {
            get { return Utility.BytesToUInt16(_buff, _offset); }
            set { Utility.SetUInt16(value, _offset, _buff); }
        }

        /// <summary>
        ///     Major linker version.
        /// </summary>
        public byte MajorLinkerVersion
        {
            get { return _buff[_offset + 0x2]; }
            set { _buff[_offset + 0x2] = value; }
        }

        /// <summary>
        ///     Minor linker version.
        /// </summary>
        public byte MinorLinkerVersion
        {
            get { return _buff[_offset + 0x3]; }
            set { _buff[_offset + 03] = value; }
        }

        /// <summary>
        ///     Size of all code sections together.
        /// </summary>
        public uint SizeOfCode
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x4); }
            set { Utility.SetUInt32(value, _offset + 0x4, _buff); }
        }

        /// <summary>
        ///     Size of all initialized data sections together.
        /// </summary>
        public uint SizeOfInitializedData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x8); }
            set { Utility.SetUInt32(value, _offset + 0x8, _buff); }
        }

        /// <summary>
        ///     Size of all unitialized data sections together.
        /// </summary>
        public uint SizeOfUninitializedData
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0xC); }
            set { Utility.SetUInt32(value, _offset + 0xC, _buff); }
        }

        /// <summary>
        ///     RVA of the entry point function.
        /// </summary>
        public uint AddressOfEntryPoint
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x10); }
            set { Utility.SetUInt32(value, _offset + 0x10, _buff); }
        }

        /// <summary>
        ///     RVA to the beginning of the code section.
        /// </summary>
        public uint BaseOfCode
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x14); }
            set { Utility.SetUInt32(value, _offset + 0x14, _buff); }
        }

        /// <summary>
        ///     RVA to the beginning of the data section.
        /// </summary>
        public uint BaseOfData
        {
            get { return _is64Bit ? 0 : Utility.BytesToUInt32(_buff, _offset + 0x18); }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x18, _buff);
                else
                    throw new Exception("IMAGE_OPTIONAL_HEADER->BaseOfCode does not exist in 64 bit applications.");
            }
        }

        /// <summary>
        ///     Preferred address of the image when it's loaded to memory.
        /// </summary>
        public ulong ImageBase
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(_buff, _offset + 0x18)
                    : Utility.BytesToUInt32(_buff, _offset + 0x1C);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, _offset + 0x1C, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x18, _buff);
            }
        }

        /// <summary>
        ///     Section aligment in memory in bytes. Must be greater or equal to the file alignment.
        /// </summary>
        public uint SectionAlignment
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x20); }
            set { Utility.SetUInt32(value, _offset + 0x20, _buff); }
        }

        /// <summary>
        ///     File alignment of the raw data of the sections in bytes.
        /// </summary>
        public uint FileAlignment
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x24); }
            set { Utility.SetUInt32(value, _offset + 0x24, _buff); }
        }

        /// <summary>
        ///     Major operation system version to run the file.
        /// </summary>
        public ushort MajorOperatingSystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x28); }
            set { Utility.SetUInt16(value, _offset + 0x28, _buff); }
        }

        /// <summary>
        ///     Minor operation system version to run the file.
        /// </summary>
        public ushort MinorOperatingSystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2A); }
            set { Utility.SetUInt16(value, _offset + 0x2A, _buff); }
        }

        /// <summary>
        ///     Major image version.
        /// </summary>
        public ushort MajorImageVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2C); }
            set { Utility.SetUInt16(value, _offset + 0x2C, _buff); }
        }

        /// <summary>
        ///     Minor image version.
        /// </summary>
        public ushort MinorImageVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x2E); }
            set { Utility.SetUInt16(value, _offset + 0x2E, _buff); }
        }

        /// <summary>
        ///     Major version of the sybsystem.
        /// </summary>
        public ushort MajorSubsystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x30); }
            set { Utility.SetUInt16(value, _offset + 0x30, _buff); }
        }

        /// <summary>
        ///     Minor version of the subsystem.
        /// </summary>
        public ushort MinorSubsystemVersion
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x32); }
            set { Utility.SetUInt16(value, _offset + 0x32, _buff); }
        }

        /// <summary>
        ///     Reserved and must be 0.
        /// </summary>
        public uint Win32VersionValue
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x34); }
            set { Utility.SetUInt32(value, _offset + 0x34, _buff); }
        }

        /// <summary>
        ///     Size of the image including all headers in bytes. Muste be a multiple of
        ///     the section alignment.
        /// </summary>
        public uint SizeOfImage
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x38); }
            set { Utility.SetUInt32(value, _offset + 0x38, _buff); }
        }

        /// <summary>
        ///     Sum of the e_lfanwe from the DOS header, the 4 byte signature, size of
        ///     the file header, size of the optional header and size of all section.
        ///     Rounded to the next file alignment.
        /// </summary>
        public uint SizeOfHeaders
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x3C); }
            set { Utility.SetUInt32(value, _offset + 0x3C, _buff); }
        }

        /// <summary>
        ///     Image checksum validated at runtime for drivers, DLLs loaded at boot time and
        ///     DLLs loaded into a critical system.
        /// </summary>
        public uint CheckSum
        {
            get { return Utility.BytesToUInt32(_buff, _offset + 0x40); }
            set { Utility.SetUInt32(value, _offset + 0x40, _buff); }
        }

        /// <summary>
        ///     The subsystem required to run the image e.g., Windows GUI, XBOX etc.
        ///     Can be resoved to a string with Utility.ResolveSubsystem(subsystem=
        /// </summary>
        public ushort Subsystem
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x44); }
            set { Utility.SetUInt16(value, _offset + 0x44, _buff); }
        }

        /// <summary>
        ///     Dll characteristics of the image.
        /// </summary>
        public ushort DllCharacteristics
        {
            get { return Utility.BytesToUInt16(_buff, _offset + 0x46); }
            set { Utility.SetUInt16(value, _offset + 0x46, _buff); }
        }

        /// <summary>
        ///     Size of stack reserve in bytes.
        /// </summary>
        public ulong SizeOfStackReserve
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(_buff, _offset + 0x48)
                    : Utility.BytesToUInt32(_buff, _offset + 0x48);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, _offset + 0x48, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x48, _buff);
            }
        }

        /// <summary>
        ///     Size of bytes committed for the stack in bytes.
        /// </summary>
        public ulong SizeOfStackCommit
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(_buff, _offset + 0x50)
                    : Utility.BytesToUInt32(_buff, _offset + 0x4C);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, _offset + 0x4C, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x50, _buff);
            }
        }

        /// <summary>
        ///     Size of the heap to reserve in bytes.
        /// </summary>
        public ulong SizeOfHeapReserve
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(_buff, _offset + 0x58)
                    : Utility.BytesToUInt32(_buff, _offset + 0x50);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, _offset + 0x50, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x58, _buff);
            }
        }

        /// <summary>
        ///     Size of the heap commit in bytes.
        /// </summary>
        public ulong SizeOfHeapCommit
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(_buff, _offset + 0x60)
                    : Utility.BytesToUInt32(_buff, _offset + 0x54);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, _offset + 0x54, _buff);
                else
                    Utility.SetUInt64(value, _offset + 0x60, _buff);
            }
        }

        /// <summary>
        ///     Obsolete
        /// </summary>
        public uint LoaderFlags
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt32(_buff, _offset + 0x68)
                    : Utility.BytesToUInt32(_buff, _offset + 0x58);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x58, _buff);
                else
                    Utility.SetUInt32(value, _offset + 0x68, _buff);
            }
        }

        /// <summary>
        ///     Number of directory entries in the remainder of the optional header.
        /// </summary>
        public uint NumberOfRvaAndSizes
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt32(_buff, _offset + 0x6C)
                    : Utility.BytesToUInt32(_buff, _offset + 0x5C);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, _offset + 0x5C, _buff);
                else
                    Utility.SetUInt32(value, _offset + 0x6C, _buff);
            }
        }

        /// <summary>
        ///     Creates a string representation of the objects
        ///     properties.
        /// </summary>
        /// <returns>Optional header propteries as a string.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_OPTIONAL_HEADER\n");
            sb.Append(Utility.PropertiesToString(this, "{0,-15}:\t{1,10:X}\n"));
            foreach (var dd in DataDirectory)
                sb.Append(dd);
            return sb.ToString();
        }
    }
}