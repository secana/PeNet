using System;
using PeNet.FileParser;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The resource directory entry represents one entry (e.g. icon)
    ///     in a resource directory.
    /// </summary>
    public class ImageResourceDirectoryEntry : AbstractStructure
    {
        /// <summary>
        ///     Create a new instance of the IMAGE_RESOURCE_DIRECTORY_ENTRY.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the entry.</param>
        /// <param name="resourceDirOffset">Raw offset to the resource directory.</param>
        public ImageResourceDirectoryEntry(IRawFile peFile, long offset, long resourceDirOffset)
            : base(peFile, offset)
        {
            // Resolve the Name
            try
            {
                if (IsIdEntry)
                {
                    NameResolved = ResolveResourceId(ID);
                }
                else if (IsNamedEntry)
                {
                    var nameAddress = resourceDirOffset + (Name & 0x7FFFFFFF);
                    var unicodeName = new ImageResourceDirStringU(PeFile, nameAddress);
                    NameResolved = unicodeName.NameString;
                }
            }
            catch (Exception)
            {
                NameResolved = null;
            }
        }

        /// <summary>
        ///     Get the Resource Directory which the Directory Entry points
        ///     to if the Directory Entry has DataIsDirectory set.
        /// </summary>
        public ImageResourceDirectory? ResourceDirectory { get; internal set; }

        /// <summary>
        ///     Get the Resource Data Entry if the entry is no directory.
        /// </summary>
        public ImageResourceDataEntry? ResourceDataEntry { get; internal set; }

        /// <summary>
        ///     Address of the name if its a named resource.
        /// </summary>
        public uint Name
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     The resolved name as a string if it's a named resource
        ///     or a known resource ID.
        /// </summary>
        public string? NameResolved { get; }

        /// <summary>
        ///     The ID if its a ID resource.
        ///     You can resolve the ID to a string with Utility.ResolveResourceId(id)
        /// </summary>
        public uint ID
        {
            get => Name & 0xFFFF;
            set => Name = value & 0xFFFF;
        }

        /// <summary>
        ///     Offset to the data.
        /// </summary>
        public uint OffsetToData
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Offset to the next directory.
        /// </summary>
        public uint OffsetToDirectory => OffsetToData & 0x7FFFFFFF;

        /// <summary>
        ///     True if the entry data is a directory
        /// </summary>
        public bool DataIsDirectory
        {
            get
            {
                if ((OffsetToData & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with a name.
        /// </summary>
        public bool IsNamedEntry
        {
            get
            {
                if ((Name & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with an ID instead of a name.
        /// </summary>
        public bool IsIdEntry => !IsNamedEntry;

        /// <summary>
        ///     Resolve the resource identifier of resource entries
        ///     to a human readable string with a meaning.
        /// </summary>
        /// <param name="id">Resource identifier.</param>
        /// <returns>String representation of the ID.</returns>
        public static string ResolveResourceId(uint id)
            => id switch
            {
                (uint)ResourceGroupIDType.Cursor => "Cursor",
                (uint)ResourceGroupIDType.Bitmap => "Bitmap",
                (uint)ResourceGroupIDType.Icon => "Icon",
                (uint)ResourceGroupIDType.Menu => "Menu",
                (uint)ResourceGroupIDType.Dialog => "Dialog",
                (uint)ResourceGroupIDType.String => "String",
                (uint)ResourceGroupIDType.FontDirectory => "FontDirectory",
                (uint)ResourceGroupIDType.Fonst => "Fonst",
                (uint)ResourceGroupIDType.Accelerator => "Accelerator",
                (uint)ResourceGroupIDType.RcData => "RcData",
                (uint)ResourceGroupIDType.MessageTable => "MessageTable",
                (uint)ResourceGroupIDType.GroupIcon => "GroupIcon",
                (uint)ResourceGroupIDType.Version => "Version",
                (uint)ResourceGroupIDType.DlgInclude => "DlgInclude",
                (uint)ResourceGroupIDType.PlugAndPlay => "PlugAndPlay",
                (uint)ResourceGroupIDType.VXD => "VXD",
                (uint)ResourceGroupIDType.AnimatedCurser => "AnimatedCurser",
                (uint)ResourceGroupIDType.AnimatedIcon => "AnimatedIcon",
                (uint)ResourceGroupIDType.HTML => "HTML",
                (uint)ResourceGroupIDType.Manifest => "Manifest",
                _ => "unknown"
            };
    }


    /// <summary>
    ///     Mapping from Resources Group ID to a meaningful
    ///     string. Used for ID resources (opposite to named resource).
    /// </summary>
    public enum ResourceGroupIDType : uint
    {
        /// <summary>
        ///     Cursor resource.
        /// </summary>
        Cursor = 1,

        /// <summary>
        ///     Bitmap resource.
        /// </summary>
        Bitmap = 2,

        /// <summary>
        ///     Icon resource.
        /// </summary>
        Icon = 3,

        /// <summary>
        ///     Menu resource.
        /// </summary>
        Menu = 4,

        /// <summary>
        ///     Dialog resource.
        /// </summary>
        Dialog = 5,

        /// <summary>
        ///     String resource.
        /// </summary>
        String = 6,

        /// <summary>
        ///     Font Directory resource.
        /// </summary>
        FontDirectory = 7,

        /// <summary>
        ///     Fonst resource.
        /// </summary>
        Fonst = 8,

        /// <summary>
        ///     Accelerator resource.
        /// </summary>
        Accelerator = 9,

        /// <summary>
        ///     RC Data resource.
        /// </summary>
        RcData = 10,

        /// <summary>
        ///     Message Table resource.
        /// </summary>
        MessageTable = 11,

        /// <summary>
        ///     Group Icon resource.
        /// </summary>
        GroupIcon = 14,

        /// <summary>
        ///     Version resource.
        /// </summary>
        Version = 16,

        /// <summary>
        ///     Dlg Include resource.
        /// </summary>
        DlgInclude = 17,

        /// <summary>
        ///     Plug and Play resource.
        /// </summary>
        PlugAndPlay = 19,

        /// <summary>
        ///     VXD resource.
        /// </summary>
        VXD = 20,

        /// <summary>
        ///     Animated Cursor resource.
        /// </summary>
        AnimatedCurser = 21,

        /// <summary>
        ///     Animated Icon resource.
        /// </summary>
        AnimatedIcon = 22,

        /// <summary>
        ///     HTML resource.
        /// </summary>
        HTML = 23,

        /// <summary>
        ///     Manifest resource.
        /// </summary>
        Manifest = 24
    }

}