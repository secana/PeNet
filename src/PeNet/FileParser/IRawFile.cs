using System;
using System.IO;

namespace PeNet.FileParser
{
    public interface IRawFile : IDisposable
    {
        /// <summary>
        /// Read a byte at the given offset.
        /// </summary>
        /// <param name="offset">Offset to read from.</param>
        /// <returns>Byte at given offset.</returns>
        byte ReadByte(long offset);

        /// <summary>
        /// Read an unsigned short (two bytes) at the given offset.
        /// </summary>
        /// <param name="offset">Offset to read from.</param>
        /// <returns>UShort at given offset.</returns>
        ushort ReadUShort(long offset);

        /// <summary>
        /// Read an unsigned integer (four bytes) at the given offset.
        /// </summary>
        /// <param name="offset">Offset to read from.</param>
        /// <returns>UInt at given offset.</returns>
        uint ReadUInt(long offset);

        /// <summary>
        /// Read an unsigned long (eight bytes) at the given offset.
        /// </summary>
        /// <param name="offset">Offset to read from.</param>
        /// <returns>ULong at given offset.</returns>
        ulong ReadULong(long offset);

        /// <summary>
        /// Write a byte at a given offset.
        /// </summary>
        /// <param name="offset">Offset to write to.</param>
        /// <param name="value">Byte value to write.</param>
        void WriteByte(long offset, byte value);

        /// <summary>
        /// Write a ushort at a given offset.
        /// </summary>
        /// <param name="offset">Offset to write to.</param>
        /// <param name="value">UShort value to write.</param>
        void WriteUShort(long offset, ushort value);

        /// <summary>
        /// Write a uint at a given offset.
        /// </summary>
        /// <param name="offset">Offset to write to.</param>
        /// <param name="value">UInt value to write.</param>
        void WriteUInt(long offset, uint value);

        /// <summary>
        /// Write a ulong at a given offset.
        /// </summary>
        /// <param name="offset">Offset to write to.</param>
        /// <param name="value">ULong value to write.</param>
        void WriteULong(long offset, ulong value);

        /// <summary>
        /// Read an unicode (two byte per char) string at a given offset.
        /// </summary>
        /// <param name="offset">Start offset of the string.</param>
        /// <returns>Parsed unicode string.</returns>
        string ReadUnicodeString(long offset);

        /// <summary>
        /// Read a ASCII (zero-terminated, one byte per character) string at a given offset.
        /// </summary>
        /// <param name="offset">Start offset of the string.</param>
        /// <returns>Parsed C string.</returns>
        string ReadAsciiString(long offset);

        /// <summary>
        /// Get a Span from the underlying file.
        /// </summary>
        /// <param name="offset">Start offset of the span.</param>
        /// <param name="length">Length of the span in byte.</param>
        /// <returns></returns>
        Span<byte> AsSpan(long offset, long length);

        /// <summary>
        /// Get the underlying file as a stream.
        /// </summary>
        /// <returns>Stream representation of the file.</returns>
        Stream ToStream();

        /// <summary>
        /// Get the underlying file as a byte array.
        /// </summary>
        /// <returns>Byte array representation fo the file.</returns>
        byte[] ToArray();

        /// <summary>
        /// Write a byte sequence to a given offset.
        /// </summary>
        /// <param name="offset">Start offset to write to.</param>
        /// <param name="bytes">Byte sequence to write.</param>
        void WriteBytes(long offset, Span<byte> bytes);

        /// <summary>
        /// Get the length of the unerlying PE file 
        /// in bytes.
        /// </summary>
        long Length { get; }
    }
}
