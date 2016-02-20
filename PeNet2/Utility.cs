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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PeNet
{
    public static class Utility
    {
        /// <summary>
        /// Resolves the target machine number to a string containing
        /// the name of the target machine.
        /// </summary>
        /// <param name="targetMachine">Target machine value from the COFF header.</param>
        /// <returns>Name of the target machine as string.</returns>
        public static string ResolveTargetMachine(ushort targetMachine)
        {
            string tm = "unknown";
            switch (targetMachine)
            {
                case 0x14c:
                    tm = "Intel 386";
                    break;
                case 0x14d:
                    tm = "Intel i860";
                    break;
                case 0x162:
                    tm = "MIPS R3000";
                    break;
                case 0x166:
                    tm = "MIPS little endian (R4000)";
                    break;
                case 0x168:
                    tm = "MIPS R10000";
                    break;
                case 0x169:
                    tm = "MIPS little endian WCI v2";
                    break;
                case 0x183:
                    tm = "old Alpha AXP";
                    break;
                case 0x184:
                    tm = "Alpha AXP";
                    break;
                case 0x1a2:
                    tm = "Hitachi SH3";
                    break;
                case 0x1a3:
                    tm = "Hitachi SH3 DSP";
                    break;
                case 0x1a6:
                    tm = "Hitachi SH4";
                    break;
                case 0x1a8:
                    tm = "Hitachi SH5";
                    break;
                case 0x1c0:
                    tm = "ARM little endian";
                    break;
                case 0x1c2:
                    tm = "Thumb";
                    break;
                case 0x1d3:
                    tm = "Matsushita AM33";
                    break;
                case 0x1f0:
                    tm = "PowerPC little endian";
                    break;
                case 0x1f1:
                    tm = "PowerPC with floating point support";
                    break;
                case 0x200:
                    tm = "Intel IA64";
                    break;
                case 0x266:
                    tm = "MIPS16";
                    break;
                case 0x268:
                    tm = "Motorola 68000 series";
                    break;
                case 0x284:
                    tm = "Alpha AXP 64-bit";
                    break;
                case 0x366:
                    tm = "MIPS with FPU";
                    break;
                case 0x466:
                    tm = "MIPS16 with FPU";
                    break;
                case 0xebc:
                    tm = "EFI Byte Code";
                    break;
                case 0x8664:
                    tm = "AMD AMD64";
                    break;
                case 0x9041:
                    tm = "Mitsubishi M32R little endian";
                    break;
                case 0xc0ee:
                    tm = "clr pure MSIL";
                    break;
            }

            return tm;
        }

        /// <summary>
        /// Resolves the characteristics attribute from the COFF header to a human
        /// readable string.
        /// </summary>
        /// <param name="characteristics">COFF header characteristics.</param>
        /// <returns>Human readable characteristics string.</returns>
        public static string ResolveCharacteristics(ushort characteristics)
        {
            string c = "";
            if ((characteristics & 0x02) == 0x02)
                c += "EXE";

            if ((characteristics & 0x200) == 0x200)
                c += "File is non-relocatable (addresses are absolute, not RVA).";

            if ((characteristics & 0x2000) == 0x2000)
                c += "DLL";
            return c;
        }

        /// <summary>
        /// Resolve the subsystem attribute to a human readable string.
        /// </summary>
        /// <param name="subsystem">Subsystem attribute.</param>
        /// <returns>Subsystem as readable string.</returns>
        public static string ResolveSubsystem(ushort subsystem)
        {
            var ss = "unknown";
            switch (subsystem)
            {
                case 1:
                    ss = "native";
                    break;
                case 2:
                    ss = "Windows/GUI";
                    break;
                case 3:
                    ss = "Windows non-GUI";
                    break;
                case 5:
                    ss = "OS/2";
                    break;
                case 7:
                    ss = "POSIX";
                    break;
                case 8:
                    ss = "Native Windows 9x Driver";
                    break;
                case 9:
                    ss = "Windows CE";
                    break;
                case 0xA:
                    ss = "EFI Application";
                    break;
                case 0xB:
                    ss = "EFI boot service device";
                    break;
                case 0xC:
                    ss = "EFI runtime driver";
                    break;
                case 0xD:
                    ss = "EFI ROM";
                    break;
                case 0xE:
                    ss = "XBox";
                    break;
            }
            return ss;
        }

        [Flags]
        public enum SectionFlags : uint
        {
            IMAGE_SCN_TYPE_NO_PAD = 0x00000008,  // Reserved.
            IMAGE_SCN_CNT_CODE = 0x00000020,  // Section contains code.
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,  // Section contains initialized data.
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,  // Section contains uninitialized data.
            IMAGE_SCN_LNK_OTHER = 0x00000100,  // Reserved.
            IMAGE_SCN_LNK_INFO = 0x00000200,  // Section contains comments or some  other type of information.
            IMAGE_SCN_LNK_REMOVE = 0x00000800,  // Section contents will not become part of image.
            IMAGE_SCN_LNK_COMDAT = 0x00001000,  // Section contents comdat.
            IMAGE_SCN_NO_DEFER_SPEC_EXC = 0x00004000,  // Reset speculative exceptions handling bits in the TLB entries for this section.
            IMAGE_SCN_GPREL = 0x00008000,  // Section content can be accessed relative to GP
            IMAGE_SCN_MEM_FARDATA = 0x00008000,
            IMAGE_SCN_MEM_PURGEABLE = 0x00020000,
            IMAGE_SCN_MEM_16BIT = 0x00020000,
            IMAGE_SCN_MEM_LOCKED = 0x00040000,
            IMAGE_SCN_MEM_PRELOAD = 0x00080000,
            IMAGE_SCN_ALIGN_1BYTES = 0x00100000,  //
            IMAGE_SCN_ALIGN_2BYTES = 0x00200000,  //
            IMAGE_SCN_ALIGN_4BYTES = 0x00300000,  //
            IMAGE_SCN_ALIGN_8BYTES = 0x00400000,  //
            IMAGE_SCN_ALIGN_16BYTES = 0x00500000,  // Default alignment if no others are specified.
            IMAGE_SCN_ALIGN_32BYTES = 0x00600000,  //
            IMAGE_SCN_ALIGN_64BYTES = 0x00700000,  //
            IMAGE_SCN_ALIGN_128BYTES = 0x00800000,  //
            IMAGE_SCN_ALIGN_256BYTES = 0x00900000,  //
            IMAGE_SCN_ALIGN_512BYTES = 0x00A00000,  //
            IMAGE_SCN_ALIGN_1024BYTES = 0x00B00000,  //
            IMAGE_SCN_ALIGN_2048BYTES = 0x00C00000,  //
            IMAGE_SCN_ALIGN_4096BYTES = 0x00D00000,  //
            IMAGE_SCN_ALIGN_8192BYTES = 0x00E00000,  //
            IMAGE_SCN_ALIGN_MASK = 0x00F00000,
            IMAGE_SCN_LNK_NRELOC_OVFL = 0x01000000,  // Section contains extended relocations.
            IMAGE_SCN_MEM_DISCARDABLE = 0x02000000,  // Section can be discarded.
            IMAGE_SCN_MEM_NOT_CACHED = 0x04000000,  // Section is not cache-able.
            IMAGE_SCN_MEM_NOT_PAGED = 0x08000000,  // Section is not page-able.
            IMAGE_SCN_MEM_SHARED = 0x10000000,  // Section is shareable.
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,  // Section is executable.
            IMAGE_SCN_MEM_READ = 0x40000000,  // Section is readable.
            IMAGE_SCN_MEM_WRITE = 0x80000000   // Section is write-able.
        }

        /// <summary>
        /// Resolves the section flags to human readable strings. 
        /// </summary>
        /// <param name="sectionFlags">Sections flags from the SectionHeader object.</param>
        /// <returns>List with flag names for the section.</returns>
        public static List<string> ResolveSectionFlags(UInt32 sectionFlags)
        {
            var st = new List<string>();
            foreach (var flag in (SectionFlags[])Enum.GetValues(typeof(SectionFlags)))
            {
                if ((sectionFlags & (uint)flag) == (uint)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }

        static ushort BytesToUInt16(byte b1, byte b2)
        {
            return BitConverter.ToUInt16(new byte[2] { b1, b2 }, 0);
        }

        public static ushort BytesToUInt16(byte[] buff, UInt64 i)
        {
            return BytesToUInt16(buff[i], buff[i + 1]);
        }

        static UInt32 BytesToUInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return BitConverter.ToUInt32(new byte[4] { b1, b2, b3, b4 }, 0);
        }

        public static UInt32 BytesToUInt32(byte[] buff, UInt32 i)
        {
            return BytesToUInt32(buff[i], buff[i + 1], buff[i + 2], buff[i + 3]);
        }

        static UInt64 BytesToUInt64(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8)
        {
            return BitConverter.ToUInt64(new byte[8] { b1, b2, b3, b4, b5, b6, b7, b8 }, 0);
        }

        public static UInt64 BytesToUInt64(byte[] buff, UInt64 i)
        {
            return BytesToUInt64(buff[i], buff[i + 1], buff[i + 2], buff[i + 3], buff[i + 4], buff[i + 5], buff[i + 6], buff[i + 7]);
        }

        static byte[] UInt16ToBytes(UInt16 value)
        {
            return BitConverter.GetBytes(value);
        }

        public static void SetUInt16(UInt16 value, UInt64 offset, byte[] buff)
        {
            var x = UInt16ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
        }

        static byte[] UInt32ToBytes(UInt32 value)
        {
            return BitConverter.GetBytes(value);
        }

        static byte[] UInt64ToBytes(UInt64 value)
        {
            return BitConverter.GetBytes(value);
        }

        public static void SetUInt32(UInt32 value, UInt32 offset, byte[] buff)
        {
            var x = UInt32ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
            buff[offset + 2] = x[2];
            buff[offset + 3] = x[3];
        }

        public static void SetUInt64(UInt64 value, UInt64 offset, byte[] buff)
        {
            var x = UInt64ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
            buff[offset + 2] = x[2];
            buff[offset + 3] = x[3];
            buff[offset + 4] = x[4];
            buff[offset + 5] = x[5];
            buff[offset + 6] = x[6];
            buff[offset + 7] = x[7];
        }

        public static string PropertiesToString(object obj, string format)
        {
            var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach(var p in properties)
            {
                if (p.PropertyType.IsArray)
                    continue;

                sb.AppendFormat(format, p.Name, p.GetValue(obj));
            }

            return sb.ToString();
        }

        public static UInt32 RVAtoFileMapping(UInt32 RVA, IMAGE_SECTION_HEADER[] sh)
        {
            var sortedSt = sh.OrderBy(x => x.VirtualAddress).ToList();
            UInt32 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirtualAddress <= RVA && sortedSt[i + 1].VirtualAddress > RVA)
                {
                    vOffset = sortedSt[i].VirtualAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirtualAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirtualAddress)
                {
                    vOffset = sortedSt.Last().VirtualAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        public static UInt64 RVAtoFileMapping(UInt64 RVA, IMAGE_SECTION_HEADER[] sh)
        {
            var sortedSt = sh.OrderBy(x => x.VirtualAddress).ToList();
            UInt32 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirtualAddress <= RVA && sortedSt[i + 1].VirtualAddress > RVA)
                {
                    vOffset = sortedSt[i].VirtualAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirtualAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirtualAddress)
                {
                    vOffset = sortedSt.Last().VirtualAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        public static UInt16 GetOrdinal(UInt32 ordinal, byte[] buff)
        {
            return BitConverter.ToUInt16(new byte[2] { buff[ordinal], buff[ordinal + 1] }, 0);
        }

        public static string GetName(UInt64 name, byte[] buff)
        {
            var length = GetNameLength(name, buff);
            var tmp = new char[length];
            for (UInt64 i = 0; i < length; i++)
            {
                tmp[i] = (char)buff[name + i];
            }

            return new string(tmp);
        }

        public static UInt64 GetNameLength(UInt64 name, byte[] buff)
        {
            var offset = name;
            UInt64 length = 0;
            while (buff[offset] != 0x00)
            {
                length++;
                offset++;
            }
            return length;
        }
    }
}
