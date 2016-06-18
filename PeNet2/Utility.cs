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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PeNet.Structures;

namespace PeNet
{
    /// <summary>
    ///     This class provides useful functions to work with PE files.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        ///     Converts the section name (UTF-8 byte array) to a string.
        /// </summary>
        /// <param name="name">Section name byte array.</param>
        /// <returns>String representation of the section name.</returns>
        public static string ResolveSectionName(byte[] name)
        {
            return Encoding.UTF8.GetString(name).TrimEnd((char) 0);
        }

        /// <summary>
        ///     Resolves the target machine number to a string containing
        ///     the name of the target machine.
        /// </summary>
        /// <param name="targetMachine">Target machine value from the COFF header.</param>
        /// <returns>Name of the target machine as string.</returns>
        public static string ResolveTargetMachine(ushort targetMachine)
        {
            var tm = "unknown";
            switch (targetMachine)
            {
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386:
                    tm = "Intel 386";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I860:
                    tm = "Intel i860";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R3000:
                    tm = "MIPS R3000";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R4000:
                    tm = "MIPS little endian (R4000)";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R10000:
                    tm = "MIPS R10000";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_WCEMIPSV2:
                    tm = "MIPS little endian WCI v2";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_OLDALPHA:
                    tm = "old Alpha AXP";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA:
                    tm = "Alpha AXP";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3:
                    tm = "Hitachi SH3";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3DSP:
                    tm = "Hitachi SH3 DSP";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3E:
                    tm = "Hitachi SH3E";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH4:
                    tm = "Hitachi SH4";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH5:
                    tm = "Hitachi SH5";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARM:
                    tm = "ARM little endian";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_THUMB:
                    tm = "Thumb";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AM33:
                    tm = "Matsushita AM33";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPC:
                    tm = "PowerPC little endian";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPCFP:
                    tm = "PowerPC with floating point support";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_IA64:
                    tm = "Intel IA64";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPS16:
                    tm = "MIPS16";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M68K:
                    tm = "Motorola 68000 series";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA64:
                    tm = "Alpha AXP 64-bit";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU:
                    tm = "MIPS with FPU";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_TRICORE:
                    tm = "Tricore";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEF:
                    tm = "CEF";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU16:
                    tm = "MIPS16 with FPU";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_EBC:
                    tm = "EFI Byte Code";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64:
                    tm = "AMD AMD64";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M32R:
                    tm = "Mitsubishi M32R little endian";
                    break;
                case (ushort) Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEE:
                    tm = "clr pure MSIL";
                    break;
            }

            return tm;
        }

        /// <summary>
        ///     Resolves the characteristics attribute from the COFF header to an
        ///     object which holds all the characteristics a boolean properties.
        /// </summary>
        /// <param name="characteristics">File header characteristics.</param>
        /// <returns>Object with all characteristics as boolean properties.</returns>
        public static FileCharacteristics ResolveFileCharacteristics(ushort characteristics)
        {
            return new FileCharacteristics(characteristics);
        }

        /// <summary>
        ///     Resolve the resource identifier of resource entries
        ///     to a human readable string with a meaning.
        /// </summary>
        /// <param name="id">Resource identifier.</param>
        /// <returns>String representation of the ID.</returns>
        public static string ResolveResourceId(uint id)
        {
            switch (id)
            {
                case (uint) Constants.ResourceGroupIDs.Cursor:
                    return "Cursor";
                case (uint) Constants.ResourceGroupIDs.Bitmap:
                    return "Bitmap";
                case (uint) Constants.ResourceGroupIDs.Icon:
                    return "Icon";
                case (uint) Constants.ResourceGroupIDs.Menu:
                    return "Menu";
                case (uint) Constants.ResourceGroupIDs.Dialog:
                    return "Dialog";
                case (uint) Constants.ResourceGroupIDs.String:
                    return "String";
                case (uint) Constants.ResourceGroupIDs.FontDirectory:
                    return "FontDirectory";
                case (uint) Constants.ResourceGroupIDs.Fonst:
                    return "Fonst";
                case (uint) Constants.ResourceGroupIDs.Accelerator:
                    return "Accelerator";
                case (uint) Constants.ResourceGroupIDs.RcData:
                    return "RcData";
                case (uint) Constants.ResourceGroupIDs.MessageTable:
                    return "MessageTable";
                case (uint) Constants.ResourceGroupIDs.GroupIcon:
                    return "GroupIcon";
                case (uint) Constants.ResourceGroupIDs.Version:
                    return "Version";
                case (uint) Constants.ResourceGroupIDs.DlgInclude:
                    return "DlgInclude";
                case (uint) Constants.ResourceGroupIDs.PlugAndPlay:
                    return "PlugAndPlay";
                case (uint) Constants.ResourceGroupIDs.VXD:
                    return "VXD";
                case (uint) Constants.ResourceGroupIDs.AnimatedCurser:
                    return "AnimatedCurser";
                case (uint) Constants.ResourceGroupIDs.AnimatedIcon:
                    return "AnimatedIcon";
                case (uint) Constants.ResourceGroupIDs.HTML:
                    return "HTML";
                case (uint) Constants.ResourceGroupIDs.Manifest:
                    return "Manifest";
                default:
                    return "unknown";
            }
        }

        /// <summary>
        ///     Resolve the subsystem attribute to a human readable string.
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

        /// <summary>
        ///     Resolves the section flags to human readable strings.
        /// </summary>
        /// <param name="sectionFlags">Sections flags from the SectionHeader object.</param>
        /// <returns>List with flag names for the section.</returns>
        public static List<string> ResolveSectionFlags(uint sectionFlags)
        {
            var st = new List<string>();
            foreach (var flag in (Constants.SectionFlags[]) Enum.GetValues(typeof(Constants.SectionFlags)))
            {
                if ((sectionFlags & (uint) flag) == (uint) flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }

        /// <summary>
        ///     Convert to bytes to an 16 bit unsigned integer.
        /// </summary>
        /// <param name="b1">High byte.</param>
        /// <param name="b2">Low byte.</param>
        /// <returns>UInt16 of the input bytes.</returns>
        private static ushort BytesToUInt16(byte b1, byte b2)
        {
            return BitConverter.ToUInt16(new[] {b1, b2}, 0);
        }

        /// <summary>
        ///     Convert a two bytes in a byte array to an 16 bit unsigned integer.
        /// </summary>
        /// <param name="buff">Byte buffer.</param>
        /// <param name="i">Position of the high byte. Low byte is i+1.</param>
        /// <returns>UInt16 of the bytes in the buffer at position i and i+1.</returns>
        public static ushort BytesToUInt16(this byte[] buff, ulong i)
        {
            return BytesToUInt16(buff[i], buff[i + 1]);
        }

        /// <summary>
        ///     Convert 4 bytes to an 32 bit unsigned integer.
        /// </summary>
        /// <param name="b1">Highest byte.</param>
        /// <param name="b2">Second highest byte.</param>
        /// <param name="b3">Second lowest byte.</param>
        /// <param name="b4">Lowest byte.</param>
        /// <returns>UInt32 representation of the input bytes.</returns>
        private static uint BytesToUInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return BitConverter.ToUInt32(new[] {b1, b2, b3, b4}, 0);
        }

        /// <summary>
        ///     Convert 4 consecutive bytes out of a buffer to an 32 bit unsigned integer.
        /// </summary>
        /// <param name="buff">Byte buffer.</param>
        /// <param name="i">Offset of the highest byte.</param>
        /// <returns>UInt32 of 4 bytes.</returns>
        public static uint BytesToUInt32(this byte[] buff, uint i)
        {
            return BytesToUInt32(buff[i], buff[i + 1], buff[i + 2], buff[i + 3]);
        }

        /// <summary>
        ///     Converts 8 bytes to an 64 bit unsigned integer.
        /// </summary>
        /// <param name="b1">Highest byte.</param>
        /// <param name="b2">Second byte.</param>
        /// <param name="b3">Third byte.</param>
        /// <param name="b4">Fourth byte.</param>
        /// <param name="b5">Fifth byte.</param>
        /// <param name="b6">Sixth byte.</param>
        /// <param name="b7">Seventh byte.</param>
        /// <param name="b8">Lowest byte.</param>
        /// <returns>UInt64 of the input bytes.</returns>
        private static ulong BytesToUInt64(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8)
        {
            return BitConverter.ToUInt64(new[] {b1, b2, b3, b4, b5, b6, b7, b8}, 0);
        }

        /// <summary>
        ///     Convert 8 consecutive byte in a buffer to an
        ///     64 bit unsigned integer.
        /// </summary>
        /// <param name="buff">Byte buffer.</param>
        /// <param name="i">Offset of the highest byte.</param>
        /// <returns>UInt64 of the byte sequence at offset i.</returns>
        public static ulong BytesToUInt64(this byte[] buff, ulong i)
        {
            return BytesToUInt64(buff[i], buff[i + 1], buff[i + 2], buff[i + 3], buff[i + 4], buff[i + 5], buff[i + 6],
                buff[i + 7]);
        }

        /// <summary>
        ///     Convert an UIn16 to an byte array.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Two byte array of the input value.</returns>
        private static byte[] UInt16ToBytes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        ///     Set an UInt16 value at an offset in an byte array.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <param name="offset">Offset where the value is set.</param>
        /// <param name="buff">Buffer in which the value is set.</param>
        public static void SetUInt16(this ushort value, ulong offset, byte[] buff)
        {
            var x = UInt16ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
        }

        /// <summary>
        ///     Convert an UInt32 value into an byte array.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>4 byte array of the value.</returns>
        private static byte[] UInt32ToBytes(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        ///     Convert an UIn64 value into an byte array.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>8 byte array of the value.</returns>
        private static byte[] UInt64ToBytes(ulong value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        ///     Sets an UInt32 value at an offset in a buffer.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <param name="offset">Offset in the array for the value.</param>
        /// <param name="buff">Buffer to set the value in.</param>
        public static void SetUInt32(this uint value, uint offset, byte[] buff)
        {
            var x = UInt32ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
            buff[offset + 2] = x[2];
            buff[offset + 3] = x[3];
        }

        /// <summary>
        ///     Sets an UInt64 value at an offset in a buffer.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <param name="offset">Offset in the array for the value.</param>
        /// <param name="buff">Buffer to set the value in.</param>
        public static void SetUInt64(this ulong value, ulong offset, byte[] buff)
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

        internal static string PropertiesToString(object obj, string format)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach (var p in properties)
            {
                if (p.PropertyType.IsArray)
                    continue;

                sb.AppendFormat(format, p.Name, p.GetValue(obj));
            }

            return sb.ToString();
        }


        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="RVA">Relative Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static ulong RVAtoFileMapping(this ulong RVA, ICollection<IMAGE_SECTION_HEADER> sh)
        {
            var sortedSt = sh.OrderBy(x => x.VirtualAddress).ToList();
            uint vOffset = 0, rOffset = 0;
            var secFound = false;
            for (var i = 0; i < sortedSt.Count - 1; i++)
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
                if (RVA >= sortedSt.Last().VirtualAddress &&
                    RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirtualAddress)
                {
                    vOffset = sortedSt.Last().VirtualAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return RVA - vOffset + rOffset;
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="RVA">Relative Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static uint RVAtoFileMapping(this uint RVA, ICollection<IMAGE_SECTION_HEADER> sh)
        {
            return (uint) RVAtoFileMapping((ulong) RVA, sh);
        }

        internal static ushort GetOrdinal(uint ordinal, byte[] buff)
        {
            return BitConverter.ToUInt16(new[] {buff[ordinal], buff[ordinal + 1]}, 0);
        }

        /// <summary>
        ///     Get a name (C string) at a specific position in a buffer.
        /// </summary>
        /// <param name="name">Offset of the string.</param>
        /// <param name="buff">Containing buffer.</param>
        /// <returns>The parsed C string.</returns>
        public static string GetName(ulong name, byte[] buff)
        {
            var length = GetNameLength(name, buff);
            var tmp = new char[length];
            for (ulong i = 0; i < length; i++)
            {
                tmp[i] = (char) buff[name + i];
            }

            return new string(tmp);
        }

        /// <summary>
        ///     For a given offset in an byte array, find the next
        ///     null value which terminates a C string.
        /// </summary>
        /// <param name="name">Offset of the string.</param>
        /// <param name="buff">Buffer which contains the string.</param>
        /// <returns>Length of the string in bytes.</returns>
        public static ulong GetNameLength(ulong name, byte[] buff)
        {
            var offset = name;
            ulong length = 0;
            while (buff[offset] != 0x00)
            {
                length++;
                offset++;
            }
            return length;
        }

        /// <summary>
        ///     Compute the SHA-256 from a file.
        /// </summary>
        /// <param name="file">Path to the file</param>
        /// <returns>SHA-256 as 64 characters long hex-string</returns>
        public static string Sha256(string file)
        {
            byte[] hash;
            var sBuilder = new StringBuilder();

            using (var sr = new StreamReader(file))
            {
                var sha = new SHA256Managed();
                hash = sha.ComputeHash(sr.BaseStream);
            }

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Compute the SHA-256 from a byte array.
        /// </summary>
        /// <param name="buff">Binary as a byte buffer.</param>
        /// <returns>SHA-256 as 64 characters long hex-string</returns>
        public static string Sha256(byte[] buff)
        {
            var sBuilder = new StringBuilder();

            var sha = new SHA256Managed();
            var hash = sha.ComputeHash(buff);

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Compute the SHA-1 from a file.
        /// </summary>
        /// <param name="file">Path to the file</param>
        /// <returns>SHA-1 as 40 characters long hex-string</returns>
        public static string Sha1(string file)
        {
            byte[] hash;
            var sBuilder = new StringBuilder();

            using (var sr = new StreamReader(file))
            {
                var sha = new SHA1Managed();
                hash = sha.ComputeHash(sr.BaseStream);
            }

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Compute the SHA-1 from a byte array.
        /// </summary>
        /// <param name="buff">Binary as a byte buffer.</param>
        /// <returns>SHA-1 as 40 characters long hex-string</returns>
        public static string Sha1(byte[] buff)
        {
            var sBuilder = new StringBuilder();

            var sha = new SHA1Managed();
            var hash = sha.ComputeHash(buff);

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Compute the MD5 from a file.
        /// </summary>
        /// <param name="file">Path to the file</param>
        /// <returns>MD5 as 32 characters long hex-string</returns>
        public static string MD5(string file)
        {
            byte[] hash;
            var sBuilder = new StringBuilder();

            using (var sr = new StreamReader(file))
            {
                var sha = new MD5Cng();
                hash = sha.ComputeHash(sr.BaseStream);
            }

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Compute the MD5 from a byte array.
        /// </summary>
        /// <param name="buff">Binary as a byte buffer.</param>
        /// <returns>MD5 as 32 characters long hex-string</returns>
        public static string MD5(byte[] buff)
        {
            var sBuilder = new StringBuilder();

            var sha = new MD5Cng();
            var hash = sha.ComputeHash(buff);

            foreach (var t in hash)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }

        /// <summary>
        ///     Convert a sequence of bytes into a hexadecimal string.
        /// </summary>
        /// <param name="bytes">Byte sequence.</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ICollection<byte> bytes)
        {
            if (bytes == null) return null;

            var hex = new StringBuilder(bytes.Count*2);
            foreach (var b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return $"0x{hex}";
        }

        /// <summary>
        ///     Convert a sequence of ushorts into a hexadecimal string.
        /// </summary>
        /// <param name="values">Value sequence.</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ICollection<ushort> values)
        {
            if (values == null) return null;

            var hex = new StringBuilder(values.Count*2);
            foreach (var b in values)
                hex.AppendFormat("{0:X4}", b);
            return $"0x{hex}";
        }

        /// <summary>
        ///     Convert ushort into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ushort value)
        {
            return $"0x{value.ToString("X4")}";
        }

        /// <summary>
        ///     Convert uint into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this uint value)
        {
            return $"0x{value.ToString("X8")}";
        }

        /// <summary>
        ///     Convert ulong into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ulong value)
        {
            return $"0x{value.ToString("X16")}";
        }

        /// <summary>
        ///     Convert a sub array of an byte array to an hex string where
        ///     every byte is separated by an whitespace.
        /// </summary>
        /// <param name="input">Byte array.</param>
        /// <param name="from">Index in the byte array where the hex string starts.</param>
        /// <param name="length">Length of the hex string in the byte array.</param>
        /// <returns></returns>
        public static List<string> ToHexString(this byte[] input, ulong from, ulong length)
        {
            if (input == null) return null;

            var hexList = new List<string>();
            for (var i = from; i < from + length; i++)
            {
                hexList.Add(input[i].ToString("X2"));
            }
            return hexList;
        }

        /// <summary>
        ///     Converts a hex string of the form 0x435A4DE3 to a long value.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns>The hex string value as a long.</returns>
        public static long ToIntFromHexString(this string hexString)
        {
            return (long) new Int64Converter().ConvertFromString(hexString);
        }

        /// <summary>
        ///     Checks is a PE file is digitally signed. It does not
        ///     verify the signature!
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <returns>True if signed, false if not. </returns>
        public static bool IsSigned(string filePath)
        {
            try
            {
                var signer = X509Certificate.CreateFromSignedFile(filePath);
                var cert = new X509Certificate2(signer);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <param name="online">Check certificate chain online or offline.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(string filePath, bool online)
        {
            X509Certificate2 cert;

            try
            {
                var signer = X509Certificate.CreateFromSignedFile(filePath);
                cert = new X509Certificate2(signer);
            }
            catch (Exception)
            {
                return false;
            }

            return IsValidCertChain(cert, online);
        }

        /// <summary>
        ///     Checks if the digital signature of a PE file is valid.
        ///     Since .Net has not function for it, PInvoke is used to query
        ///     the native API like here http://geekswithblogs.net/robp/archive/2007/05/04/112250.aspx
        /// </summary>
        /// <param name="filePath">Path to a PE file.</param>
        /// <returns>True if the signature is valid, else false.</returns>
        public static bool IsSignatureValid(string filePath)
        {
            return NativeMethods.IsTrusted(filePath);
        }

        /// <summary>
        ///     Checks if cert is from a trusted CA with a valid certificate chain.
        /// </summary>
        /// <param name="cert">X509 Certificate</param>
        /// <param name="online">Check certificate chain online or offline.</param>
        /// <returns>True of cert chain is valid and from a trusted CA.</returns>
        public static bool IsValidCertChain(X509Certificate2 cert, bool online)
        {
            var chain = new X509Chain
            {
                ChainPolicy =
                {
                    RevocationFlag = X509RevocationFlag.ExcludeRoot,
                    RevocationMode = online ? X509RevocationMode.Online : X509RevocationMode.Offline,
                    UrlRetrievalTimeout = new TimeSpan(0, 1, 0),
                    VerificationFlags = X509VerificationFlags.NoFlag
                }
            };
            return chain.Build(cert);
        }

        /// <summary>
        ///     Describes which file characteristics based on the
        ///     file header are set.
        ///     The ToString Method creates a readable string containing
        ///     all the information.
        /// </summary>
        public class FileCharacteristics
        {
            /// <summary>
            ///     Create an object that contains all possible file characteristics
            ///     flags resolve to boolean properties.
            /// </summary>
            /// <param name="characteristics">Characteristics from the file header.</param>
            public FileCharacteristics(ushort characteristics)
            {
                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_RELOCS_STRIPPED) > 0)
                    RelocStripped = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_EXECUTABLE_IMAGE) > 0)
                    ExecutableImage = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_LINE_NUMS_STRIPPED) > 0)
                    LineNumbersStripped = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_LOCAL_SYMS_STRIPPED) > 0)
                    LocalSymbolsStripped = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_AGGRESIVE_WS_TRIM) > 0)
                    AggressiveWsTrim = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_LARGE_ADDRESS_AWARE) > 0)
                    LargeAddressAware = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_BYTES_REVERSED_LO) > 0)
                    BytesReversedLo = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_32BIT_MACHINE) > 0)
                    Machine32Bit = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_DEBUG_STRIPPED) > 0)
                    DebugStripped = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP) >
                    0)
                    RemovableRunFromSwap = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_NET_RUN_FROM_SWAP) > 0)
                    NetRunFroMSwap = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_SYSTEM) > 0)
                    System = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_DLL) > 0)
                    DLL = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_UP_SYSTEM_ONLY) > 0)
                    UpSystemOnly = true;

                if ((characteristics & (ushort) Constants.FileHeaderCharacteristics.IMAGE_FILE_BYTES_REVERSED_HI) > 0)
                    BytesReversedHi = true;
            }

            /// <summary>
            ///     Relocation stripped,
            /// </summary>
            public bool RelocStripped { get; private set; }

            /// <summary>
            ///     Is an executable image.
            /// </summary>
            public bool ExecutableImage { get; private set; }

            /// <summary>
            ///     Line numbers stripped.
            /// </summary>
            public bool LineNumbersStripped { get; private set; }

            /// <summary>
            ///     Local symbols stripped.
            /// </summary>
            public bool LocalSymbolsStripped { get; private set; }

            /// <summary>
            ///     (OBSOLTETE) Aggressively trim the working set.
            /// </summary>
            public bool AggressiveWsTrim { get; private set; }

            /// <summary>
            ///     Application can handle addresses larger than 2 GB.
            /// </summary>
            public bool LargeAddressAware { get; private set; }

            /// <summary>
            ///     (OBSOLTETE) Bytes of word are reversed.
            /// </summary>
            public bool BytesReversedLo { get; private set; }

            /// <summary>
            ///     Supports 32 Bit words.
            /// </summary>
            public bool Machine32Bit { get; private set; }

            /// <summary>
            ///     Debug stripped and stored in a separate file.
            /// </summary>
            public bool DebugStripped { get; private set; }

            /// <summary>
            ///     If the image is on a removable media, copy and run it from the swap file.
            /// </summary>
            public bool RemovableRunFromSwap { get; private set; }

            /// <summary>
            ///     If the image is on the network, copy and run it from the swap file.
            /// </summary>
            public bool NetRunFroMSwap { get; private set; }

            /// <summary>
            ///     The image is a system file.
            /// </summary>
            public bool System { get; private set; }

            /// <summary>
            ///     Is a dynamic loaded library and executable but cannot
            ///     be run on its own.
            /// </summary>
            public bool DLL { get; private set; }

            /// <summary>
            ///     Image should be run only on uniprocessor.
            /// </summary>
            public bool UpSystemOnly { get; private set; }

            /// <summary>
            ///     (OBSOLETE) Reserved.
            /// </summary>
            public bool BytesReversedHi { get; private set; }

            /// <summary>
            ///     Return string representation of all characteristics.
            /// </summary>
            /// <returns>Return string representation of all characteristics.</returns>
            public override string ToString()
            {
                var sb = new StringBuilder("File Characteristics\n");
                sb.Append(PropertiesToString(this, "{0,-30}:{1,10:X}\n"));
                return sb.ToString();
            }
        }
    }
}