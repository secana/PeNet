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
    ///     Represents the optional header in
    ///     the NT header.
    /// </summary>
    public class IMAGE_OPTIONAL_HEADER_x64 : IMAGE_OPTIONAL_HEADER_x32
    {
        private readonly bool _is64Bit;

        /// <summary>
        ///     The Data Directories.
        /// </summary>
        public readonly IMAGE_DATA_DIRECTORY[] DataDirectory;

        /// <summary>
        ///     Create a new IMAGE_OPTIONAL_HEADER object.
        /// </summary>
        /// <param name="buff">A PE file as a byte array.</param>
        /// <param name="offset">Raw offset to the optional header.</param>
        public IMAGE_OPTIONAL_HEADER_x64(byte[] buff, uint offset)
            : base(buff, offset)
        {
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
        ///     Preferred address of the image when it's loaded to memory.
        /// </summary>
        public override ulong ImageBase
        {
            get { return Utility.BytesToUInt64(Buff, Offset + 0x18); }
            set { Utility.SetUInt64(value, Offset + 0x18, Buff); }
        }

        /// <summary>
        ///     Size of stack reserve in bytes.
        /// </summary>
        public ulong SizeOfStackReserve
        {
            get
            {
                return _is64Bit
                    ? Utility.BytesToUInt64(Buff, Offset + 0x48)
                    : Utility.BytesToUInt32(Buff, Offset + 0x48);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, Offset + 0x48, Buff);
                else
                    Utility.SetUInt64(value, Offset + 0x48, Buff);
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
                    ? Utility.BytesToUInt64(Buff, Offset + 0x50)
                    : Utility.BytesToUInt32(Buff, Offset + 0x4C);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, Offset + 0x4C, Buff);
                else
                    Utility.SetUInt64(value, Offset + 0x50, Buff);
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
                    ? Utility.BytesToUInt64(Buff, Offset + 0x58)
                    : Utility.BytesToUInt32(Buff, Offset + 0x50);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, Offset + 0x50, Buff);
                else
                    Utility.SetUInt64(value, Offset + 0x58, Buff);
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
                    ? Utility.BytesToUInt64(Buff, Offset + 0x60)
                    : Utility.BytesToUInt32(Buff, Offset + 0x54);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32((uint) value, Offset + 0x54, Buff);
                else
                    Utility.SetUInt64(value, Offset + 0x60, Buff);
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
                    ? Utility.BytesToUInt32(Buff, Offset + 0x68)
                    : Utility.BytesToUInt32(Buff, Offset + 0x58);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, Offset + 0x58, Buff);
                else
                    Utility.SetUInt32(value, Offset + 0x68, Buff);
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
                    ? Utility.BytesToUInt32(Buff, Offset + 0x6C)
                    : Utility.BytesToUInt32(Buff, Offset + 0x5C);
            }
            set
            {
                if (!_is64Bit)
                    Utility.SetUInt32(value, Offset + 0x5C, Buff);
                else
                    Utility.SetUInt32(value, Offset + 0x6C, Buff);
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