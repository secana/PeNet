using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using PeNet.Structures;

namespace PeNet.Utilities
{
    /// <summary>
    /// Extensions method to work make the work with buffers 
    /// and addresses easier.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Map an virtual address to the raw file address.
        /// </summary>
        /// <param name="virtualAddress">Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static ulong VAtoFileMapping(this ulong virtualAddress, ICollection<IMAGE_SECTION_HEADER> sh)
        {
            var rva= virtualAddress - sh.FirstOrDefault().ImageBaseAddress;
            return RVAtoFileMapping(rva, sh);
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="relativeVirtualAddress">Relative Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static ulong RVAtoFileMapping(this ulong relativeVirtualAddress, ICollection<IMAGE_SECTION_HEADER> sh)
        {
            IMAGE_SECTION_HEADER GetSectionForRva(ulong rva)
            {
                var sectionsByRva = sh.OrderBy(s => s.VirtualAddress).ToList();
                var notLastSection = sectionsByRva.FirstOrDefault(s =>
                    rva >= s.VirtualAddress && rva < s.VirtualAddress + s.VirtualSize);

                if (notLastSection != null)
                    return notLastSection;

                var lastSection = sectionsByRva.LastOrDefault(s => 
                        rva >= s.VirtualAddress && rva <= s.VirtualAddress + s.VirtualSize);

                return lastSection;
            }

            var section = GetSectionForRva(relativeVirtualAddress);

            if (section is null)
            {
                throw new Exception("Cannot find corresponding section.");
            }

            return relativeVirtualAddress - section.VirtualAddress + section.PointerToRawData;
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="relativeVirtualAddress">Relative Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static uint RVAtoFileMapping(this uint relativeVirtualAddress, ICollection<IMAGE_SECTION_HEADER> sh)
        {
            return (uint) RVAtoFileMapping((ulong) relativeVirtualAddress, sh);
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="RelativeVirtualAddress">Relative Virtual Address</param>
        /// <param name="sh">Section Headers</param>
        /// <returns>Raw address of null if error occurred.</returns>
        public static uint? SafeRVAtoFileMapping(this uint RelativeVirtualAddress, ICollection<IMAGE_SECTION_HEADER>? sh)
        {
            if (sh is null)
                return null;

            try
            {
                return RelativeVirtualAddress.RVAtoFileMapping(sh);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Convert a sequence of bytes into a hexadecimal string.
        /// </summary>
        /// <param name="bytes">Byte sequence.</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ICollection<byte> bytes)
        {
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
            var hex = new StringBuilder(values.Count*2);
            foreach (var b in values)
                hex.AppendFormat("{0:X4}", b);
            return $"0x{hex}";
        }


        /// <summary>
        ///     Convert byte into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this byte value)
        {
            return $"0x{value:X2}";
        }

        /// <summary>
        ///     Convert ushort into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ushort value)
        {
            return $"0x{value:X4}";
        }

        /// <summary>
        ///     Convert uint into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this uint value)
        {
            return $"0x{value:X8}";
        }

        /// <summary>
        ///     Convert ulong into a hexadecimal string.
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Hex-String</returns>
        public static string ToHexString(this ulong value)
        {
            return $"0x{value:X16}";
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
            var hexList = new List<string>();
            for (var i = @from; i < @from + length; i++)
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
        /// Get the length of a unicode string in bytes.
        /// </summary>
        /// <param name="s">A unicode string.</param>
        /// <returns>Length in bytes.</returns>
        public static int LengthInByte(this string s) => s.Length * 2 + 2;

        /// <summary>
        /// Compute the padding to align a
        /// data structure.
        /// </summary>
        /// <param name="offset">Offset to start the alignment of
        /// the next member.</param>
        /// <param name="alignment">Bitness of the alignment, e.g. "32".</param>
        /// <returns>Number of bytes needed to align the next structure.</returns>
        public static uint PaddingBytes(this long offset, int alignment) 
            => PaddingBytes((uint)offset, alignment);

        /// <summary>
        /// Compute the padding to align a
        /// data structure.
        /// </summary>
        /// <param name="offset">Offset to start the alignment of
        /// the next member.</param>
        /// <param name="alignment">Bitness of the alignment, e.g. "32".</param>
        /// <returns>Number of bytes needed to align the next structure.</returns>
        public static uint PaddingBytes(this int offset, int alignment)
            => PaddingBytes((uint)offset, alignment);

        /// <summary>
        /// Compute the padding to align a
        /// data structure.
        /// </summary>
        /// <param name="offset">Offset to start the alignment of
        /// the next member.</param>
        /// <param name="alignment">Bitness of the alignment, e.g. "32".</param>
        /// <returns>Number of bytes needed to align the next structure.</returns>
        public static uint PaddingBytes(this uint offset, int alignment) 
            => offset % (uint)(alignment / 8);
    }
}