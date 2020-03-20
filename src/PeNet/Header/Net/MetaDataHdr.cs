using System;
using System.Collections.Generic;
using PeNet.FileParser;

namespace PeNet.Header.Net
{
    /// <summary>
    /// The Meta Data Header is part of the .Net/CLI (COM+ 2) header and is reachable
    /// from the .Net/CLI (COM+2) header ImageCor20Header. It contains information
    /// about embedded streams (sections) in the .Net assembly.
    /// </summary>
    public class MetaDataHdr : AbstractStructure
    {
        private MetaDataStreamHdr[]? _metaDataStreamsHdrs;
        private bool _metaDataStreamsHdrsParsed;
        private string? _versionString;
        private bool _versionStringParsed;

        /// <summary>
        /// Create a new Meta Data Header from a PE file.
        /// </summary>
        /// <param name="peFile">PE file which contains a Meta Data Header.</param>
        /// <param name="offset">Offset of the header start in the PE file.</param>
        public MetaDataHdr(IRawFile peFile, long offset) 
            : base(peFile, offset)
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
                        _versionString = PeFile.ReadAsciiString(Offset + 0x10);
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
        public MetaDataStreamHdr[]? MetaDataStreamsHdrs
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

        private MetaDataStreamHdr[] ParseMetaDataStreamHdrs(long offset)
        {
            var metaDataStreamHdrs = new List<MetaDataStreamHdr>();
            var tmpOffset = offset;

            for (var i = 0; i < Streams; i++)
            {
                var metaDataStreamHdr = new MetaDataStreamHdr(PeFile, tmpOffset);
                metaDataStreamHdrs.Add(metaDataStreamHdr);
                tmpOffset += metaDataStreamHdr.HeaderLength;
            }

            return metaDataStreamHdrs.ToArray();
        }
    }
}