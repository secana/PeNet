using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    /// The Meta Data Header is part of the .Net/CLI (COM+ 2) header and is reachable
    /// from the .Net/CLI (COM+2) header IMAGE_COR20_HEADER. It contains information
    /// about embedded streams (sections) in the .Net assembly.
    /// </summary>
    public class METADATAHDR : AbstractStructure
    {
        private METADATASTREAMHDR[]? _metaDataStreamsHdrs;
        private bool _metaDataStreamsHdrsParsed;
        private string? _versionString;
        private bool _versionStringParsed;

        /// <summary>
        /// Create a new Meta Data Header from a byte array.
        /// </summary>
        /// <param name="stream">Stream which contains a Meta Data Header.</param>
        /// <param name="offset">Offset of the header start in the byte buffer.</param>
        public METADATAHDR(Stream stream, int offset) 
            : base(stream, offset)
        {
        }

        /// <summary>
        /// Signature. Always 0x424A5342 in a valid .Net assembly.
        /// </summary>
        public uint Signature
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        /// Major version.
        /// </summary>
        public ushort MajorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x4);
            set => PeFile.WriteUShort(Offset + 0x4, value);
        }

        /// <summary>
        /// Minor version.
        /// </summary>
        public ushort MinorVersion
        {
            get => PeFile.ReadUShort(Offset + 0x6);
            set => PeFile.WriteUShort(Offset + 0x6, value);
        }

        /// <summary>
        /// Reserved. Always 0.
        /// </summary>
        public uint Reserved
        {
            get => PeFile.ReadUInt(Offset + 0x8);
            set => PeFile.WriteUInt(Offset + 0x8, value);
        }

        /// <summary>
        /// Length of the UTF-8 version string rounded up to a multiple of 4.
        /// For e.g., v1.3.4323
        /// </summary>
        public uint VersionLength
        {
            get => PeFile.ReadUInt(Offset + 0xC);
            set => PeFile.WriteUInt(Offset + 0xC, value);
        }

        /// <summary>
        /// Version number as an UTF-8 string.
        /// </summary>
        public string? Version {
            get
            {
                if (!_versionStringParsed)
                {
                    _versionStringParsed = true;
                    try
                    {
                        _versionString = ParseVersionString(Offset + 0x10, VersionLength);
                    }
                    catch (Exception)
                    {
                        _versionString = null;
                    }
                    
                }

                return _versionString;
            }
        }
            

        /// <summary>
        /// Reserved flags field. Always 0.
        /// </summary>
        public ushort Flags
        {
            get => PeFile.ReadUShort(VersionLength + Offset + 0x10);
            set => PeFile.WriteUShort(VersionLength + Offset + 0x10, value);
        }

        /// <summary>
        /// Number of streams (sections) to follow. 
        /// </summary>
        public ushort Streams
        {
            get => PeFile.ReadUShort(VersionLength + Offset + 0x12);
            set => PeFile.WriteUShort(VersionLength + Offset + 0x12, value);
        }

        /// <summary>
        /// Array with all Meta Data Stream Headers.
        /// </summary>
        public METADATASTREAMHDR[]? MetaDataStreamsHdrs
        {
            get
            {
                if (!_metaDataStreamsHdrsParsed)
                {
                    _metaDataStreamsHdrsParsed = true;
                    try
                    {
                        _metaDataStreamsHdrs = ParseMetaDataStreamHdrs(VersionLength + Offset + 0x14);
                    }
                    catch (Exception)
                    {
                        _metaDataStreamsHdrs = null;
                    }
                }
                return _metaDataStreamsHdrs;
            }
        }

        private METADATASTREAMHDR[] ParseMetaDataStreamHdrs(long offset)
        {
            var metaDataStreamHdrs = new List<METADATASTREAMHDR>();
            var tmpOffset = offset;

            for (var i = 0; i < Streams; i++)
            {
                var metaDataStreamHdr = new METADATASTREAMHDR(PeFile, tmpOffset);
                metaDataStreamHdrs.Add(metaDataStreamHdr);
                tmpOffset += metaDataStreamHdr.HeaderLength;
            }

            return metaDataStreamHdrs.ToArray();
        }

        private string ParseVersionString(int offset, uint versionLength)
        {
            var bytes = new byte[versionLength];
            Array.Copy(PeFile, offset, bytes, 0, versionLength);
            var paddedString = Encoding.UTF8.GetString(bytes);

            // Remove padding and return.
            return paddedString.Replace("\0", string.Empty);
        }
    }
}