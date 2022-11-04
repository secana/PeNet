using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PeNet.FileParser;

namespace PeNet.Header.Resource
{
    /// <summary>
    ///     Information about Icons.
    /// </summary>
    public class Icon : AbstractStructure
    {
        public uint Size { get; }
        public uint Id { get; }
        public Resources Parent { get; }
        private GroupIconDirectoryEntry? AssociatedGroupIconDirectoryEntry { get; }

        private const uint ICOHeaderSize = 22;
        private static byte[] PNGHeader = { 137, 80, 78, 71, 13, 10, 26, 10 }; //TODO:Hex

        /// <summary>
        ///     Creates a new Icon instance and sets Size and ID.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Offset of the Icon image in the PE file.</param>
        /// <param name="size">Size of the Icon image in the PE file.</param>
        /// <param name="id">ID of the Icon.</param>
        /// <param name="parent">Resources parent of the Icon</param>
        public Icon(IRawFile peFile, long offset, uint size, uint id, Resources parent)
            : base(peFile, offset)
        {
            Size = size;
            Id = id;
            Parent = parent;
            AssociatedGroupIconDirectoryEntry = GetAssociatedGroupIconDirectoryEntry();
        }

        /// <summary>
        ///     Byte span of the icon image.
        /// </summary>
        public Span<byte> AsSpan()
        {
            return PeFile.AsSpan(Offset, Size);
        }

        /// <summary>
        ///     Adding .ICO-Header to the bytes of the icon image.
        /// </summary>
        /// <returns>Bytes of the icon image as .ICO file.</returns>
        public byte[]? AsICO()
        {
            if (AssociatedGroupIconDirectoryEntry == null) return null;
            var iconBytes = AsSpan().ToArray();
            if (iconBytes == null) return null;

            if (iconBytes.Take(8).SequenceEqual(PNGHeader)) return iconBytes; // TODO: CFF does not use an additional header for .PNG. But this would not break anything.

            var iconICOBytes = new byte[ICOHeaderSize + iconBytes.Length];
            SetICOHeader(iconICOBytes);
            
            iconBytes.CopyTo(iconICOBytes, ICOHeaderSize);
            return iconICOBytes;
        }

        /// <summary>
        ///     Setting .ICO-Header with data from the associated GroupIconDirectoryEntry.
        ///     According to the structure described in :https://en.wikipedia.org/wiki/ICO_(file_format)
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetICOHeader(IList<byte> iconICOBytes)
        {
            SetIconDirectoryStructure(iconICOBytes);
            SetIconDirectoryEntryStructure(iconICOBytes);
        }

        /// <summary>
        ///     Setting bytes in IconDirectoryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private static void SetIconDirectoryStructure(IList<byte> iconICOBytes)
        {
            iconICOBytes[0] = 0x00; //Res
            iconICOBytes[1] = 0x00;

            iconICOBytes[2] = 0x01; //Type .ICO
            iconICOBytes[3] = 0x00;

            iconICOBytes[4] = 0x01; //Num. of Icons in File (=1)
            iconICOBytes[5] = 0x00;
        }

        /// <summary>
        ///     Setting bytes in IconDirectoryEntryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetIconDirectoryEntryStructure(IList<byte> iconICOBytes)
        {
            SetIconDirectoryEntryStructureDimensions(iconICOBytes);
            SetIconDirectoryEntryStructureColorInfos(iconICOBytes);
            SetIconDirectoryEntryStructureSize(iconICOBytes);
            SetIconDirectoryEntryStructureOffsetToImage(iconICOBytes);
        }

        /// <summary>
        ///     Setting bytes associated with the Width and Height of the image in IconDirectoryEntryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetIconDirectoryEntryStructureDimensions(IList<byte> iconICOBytes)
        {
            iconICOBytes[6] = AssociatedGroupIconDirectoryEntry!.BWidth; //Width
            iconICOBytes[7] = AssociatedGroupIconDirectoryEntry!.BHeight; //Height 
        }

        /// <summary>
        ///     Setting bytes associated with color information of the image in IconDirectoryEntryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetIconDirectoryEntryStructureColorInfos(IList<byte> iconICOBytes)
        {
            //Information not included in the GroupIconDirectoryEntry, only in the image byte array for .BMP. By default 0x00.
            iconICOBytes[8] = AsSpan()[32];                                             //Number of Colors in color palette

            iconICOBytes[9] = 0x00;                                                     //Res

            iconICOBytes[10] = (byte)AssociatedGroupIconDirectoryEntry!.WPlanes;        //Color planes (=1 for .BMP) 
            iconICOBytes[11] = (byte)(AssociatedGroupIconDirectoryEntry!.WPlanes >> 8);

            iconICOBytes[12] = (byte)AssociatedGroupIconDirectoryEntry!.WBitCount;      //Bit per Pixel  
            iconICOBytes[13] = (byte)(AssociatedGroupIconDirectoryEntry!.WBitCount >> 8);
        }

        /// <summary>
        ///     Setting bytes associated with the Size of the image in IconDirectoryEntryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetIconDirectoryEntryStructureSize(IList<byte> iconICOBytes)
        {
            iconICOBytes[14] = (byte)AssociatedGroupIconDirectoryEntry!.DwBytesInRes;           //Size    
            iconICOBytes[15] = (byte)(AssociatedGroupIconDirectoryEntry!.DwBytesInRes >> 8);
            iconICOBytes[16] = (byte)(AssociatedGroupIconDirectoryEntry!.DwBytesInRes >> 16);
            iconICOBytes[17] = (byte)(AssociatedGroupIconDirectoryEntry!.DwBytesInRes >> 24);
        }

        /// <summary>
        ///     Setting bytes associated with the Offset to the image in IconDirectoryEntryStructure.
        /// </summary>
        /// <param name="iconICOBytes">Byte array to process.</param>
        private void SetIconDirectoryEntryStructureOffsetToImage(IList<byte> iconICOBytes)
        {
            iconICOBytes[18] = (byte)(ICOHeaderSize >> 0); // Offset to icon image bytes
            iconICOBytes[19] = (byte)(ICOHeaderSize >> 8);
            iconICOBytes[20] = (byte)(ICOHeaderSize >> 16);
            iconICOBytes[21] = (byte)(ICOHeaderSize >> 24);
        }

        /// <summary>
        ///     Searching for the associated GroupIconDirectoryEntry.
        /// </summary>
        /// <returns>GroupIconDirectoryEntry with same nId as the Id of the icon.</returns>
        public GroupIconDirectoryEntry? GetAssociatedGroupIconDirectoryEntry()
        {
            return Parent.GroupIconDirectories?
                .Where(groupIconsDirectory => groupIconsDirectory.DirectoryEntries?
                    .Where(groupIconsDirectoryEntry => groupIconsDirectoryEntry.NId == Id).Count() != 0)
                .First().DirectoryEntries?
                .First(groupIconsDirectoryEntry => groupIconsDirectoryEntry?.NId == Id);
        }
    }
}
