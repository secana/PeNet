using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using PeNet.FileParser;
using PeNet.Header.Pe;

namespace PeNet
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
        /// <param name="va">Virtual Address</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static ulong VaToOffset(this ulong va, ICollection<ImageSectionHeader> sectionHeaders)
        {
            var rva= va - sectionHeaders.First().ImageBaseAddress;
            return RvaToOffset(rva, sectionHeaders);
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="rva">Relative Virtual Address</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static ulong RvaToOffset(this ulong rva, ICollection<ImageSectionHeader> sectionHeaders)
        {
            static ImageSectionHeader? GetSectionForRva(ICollection<ImageSectionHeader> sh, ulong relVirAdr)
            {
                ImageSectionHeader? sec = null;
                for (var i = 0; i < sh.Count; i++)
                {
                    if (relVirAdr >= sh.ElementAt(i).VirtualAddress
                        && relVirAdr < sh.ElementAt(i).VirtualAddress + sh.ElementAt(i).VirtualSize)
                    {
                        sec = sh.ElementAt(i);
                    }
                }

                if (sec != null)
                    return sec;

                for (var i = sh.Count - 1; i >= 0; i--)
                {
                    if (relVirAdr >= sh.ElementAt(i).VirtualAddress &&
                        relVirAdr <= sh.ElementAt(i).VirtualAddress + sh.ElementAt(i).VirtualSize)
                    {
                        sec = sh.ElementAt(i);
                    }
                }

                return sec;
            }

            var section = GetSectionForRva(sectionHeaders, rva);

            if (section is null)
            {
                throw new Exception("Cannot find corresponding section.");
            }

            return rva - section.VirtualAddress + section.PointerToRawData;
        }

        /// <summary>
        ///     Map an relative virtual address to the raw file address.
        /// </summary>
        /// <param name="rva">Relative Virtual Address</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <returns>Raw file address.</returns>
        public static uint RvaToOffset(this uint rva, ICollection<ImageSectionHeader> sectionHeaders)
        {
            return (uint) RvaToOffset((ulong) rva, sectionHeaders);
        }

        /// <summary>
        ///     Map a raw offset to a relative virtual address.
        /// </summary>
        /// <param name="offset">Raw file offset.</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <returns>Relative Virtual Address</returns>
        public static ulong OffsetToRva(this ulong offset, ICollection<ImageSectionHeader> sectionHeaders)
        {
            static ImageSectionHeader? GetSectionForOffset(ICollection<ImageSectionHeader> sh, ulong rawOffset)
            {
                ImageSectionHeader? sec = null;
                for (var i = 0; i < sh.Count; i++)
                {
                    if (rawOffset >= sh.ElementAt(i).PointerToRawData
                        && rawOffset < sh.ElementAt(i).PointerToRawData + sh.ElementAt(i).SizeOfRawData)
                    {
                        sec = sh.ElementAt(i);
                    }
                }

                if (sec != null)
                    return sec;

                for (var i = sh.Count - 1; i >= 0; i--)
                {
                    if (rawOffset >= sh.ElementAt(i).PointerToRawData &&
                        rawOffset <= sh.ElementAt(i).PointerToRawData + sh.ElementAt(i).SizeOfRawData)
                    {
                        sec = sh.ElementAt(i);
                    }
                }

                return sec;
            }

            var section = GetSectionForOffset(sectionHeaders, offset);

            if (section is null)
            {
                throw new Exception("Cannot find corresponding section.");
            }

            return offset + section.VirtualAddress - section.PointerToRawData;
        }

        /// <summary>
        ///     Map a raw offset to a relative virtual address.
        /// </summary>
        /// <param name="offset">Raw file offset.</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <returns>Relative Virtual Address</returns>
        public static uint OffsetToRva(this uint offset, ICollection<ImageSectionHeader> sectionHeaders)
        {
            return (uint) OffsetToRva((ulong) offset, sectionHeaders);
        }

        /// <summary>
        /// Try to map a relative virtual address to a file offset.
        /// </summary>
        /// <param name="rva">Relative Virtual Address</param>
        /// <param name="sectionHeaders">Section Headers</param>
        /// <param name="fileOffset">File offset if mapping was successful.</param>
        /// <returns>True if mapping was successful, false if not.</returns>
        public static bool TryRvaToOffset(this uint rva, ICollection<ImageSectionHeader>? sectionHeaders, out uint fileOffset)
        {
            fileOffset = 0;

            if (sectionHeaders is null)
                return false;

            try
            {
                fileOffset = rva.RvaToOffset(sectionHeaders);
                return true;
            }
            catch (Exception)
            {
                return false;
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
        ///     Convert a sequence of ushort into a hexadecimal string.
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
            return (long) (new Int64Converter().ConvertFromString(hexString) ?? throw new InvalidOperationException());
        }

        /// <summary>
        /// Get the length of a unicode string in bytes.
        /// </summary>
        /// <param name="s">A unicode string.</param>
        /// <returns>Length in bytes.</returns>
        public static int UStringByteLength(this string s) => s.Length * 2 + 2;

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

        /// <summary>
        /// Check if a given file if 64 Bit
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <returns>True, if 64 bit.</returns>
        public static bool Is64Bit(this IRawFile peFile)
            => peFile.ReadUShort(peFile.ReadUInt(0x3c) + 0x18) == (ushort) MagicType.Bit64;

        /// <summary>
        /// Check if a given file if 32 Bit
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <returns>True, if 32 bit.</returns>
        public static bool Is32Bit(this IRawFile peFile)
            => peFile.ReadUShort(peFile.ReadUInt(0x3c) + 0x18) == (ushort) MagicType.Bit32;

        /// <summary>
        ///     Makes an IEnumerable safe to enumerate by returning an empty enumerable in case it is null.
        /// </summary>
        /// <param name="enumerable">An IEnumerable.</param>
        /// <returns>The enumerable, it it exists. An empty Enumerable if enumerable is null.</returns>
        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        /// <summary>
        ///     Selects the Value component of the tuple if the Success component of the tuple is true.
        /// </summary>
        /// <param name="source">An IEnumerable.</param>
        /// <param name="tryFunc">A function which maps elements of source to tuples of success and a mapped value.</param>
        /// <returns>An IEnumerable of all successful values.</returns>
        public static IEnumerable<TOut> TrySelect<T, TOut>(this IEnumerable<T> source, Func<T, (bool Success, TOut Value)> tryFunc)
        {
            return source.Select(tryFunc)
                .Where(o => o.Success)
                .Select(o => o.Value);
        }
    }
}
