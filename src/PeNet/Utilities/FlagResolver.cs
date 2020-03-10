using System;
using System.Collections.Generic;
using System.Text;

namespace PeNet.Utilities
{
    /// <summary>
    /// Parser functions for different flags in the PE header.
    /// </summary>
    public static class FlagResolver
    {
        /// <summary>
        ///     Resolve the resource identifier of resource entries
        ///     to a human readable string with a meaning.
        /// </summary>
        /// <param name="id">Resource identifier.</param>
        /// <returns>String representation of the ID.</returns>
        public static string ResolveResourceId(uint id)
            => id switch
            {
                (uint)Constants.ResourceGroupIDs.Cursor => "Cursor",
                (uint)Constants.ResourceGroupIDs.Bitmap => "Bitmap",
                (uint)Constants.ResourceGroupIDs.Icon => "Icon",
                (uint)Constants.ResourceGroupIDs.Menu => "Menu",
                (uint)Constants.ResourceGroupIDs.Dialog => "Dialog",
                (uint)Constants.ResourceGroupIDs.String => "String",
                (uint)Constants.ResourceGroupIDs.FontDirectory => "FontDirectory",
                (uint)Constants.ResourceGroupIDs.Fonst => "Fonst",
                (uint)Constants.ResourceGroupIDs.Accelerator => "Accelerator",
                (uint)Constants.ResourceGroupIDs.RcData => "RcData",
                (uint)Constants.ResourceGroupIDs.MessageTable => "MessageTable",
                (uint)Constants.ResourceGroupIDs.GroupIcon => "GroupIcon",
                (uint)Constants.ResourceGroupIDs.Version => "Version",
                (uint)Constants.ResourceGroupIDs.DlgInclude => "DlgInclude",
                (uint)Constants.ResourceGroupIDs.PlugAndPlay => "PlugAndPlay",
                (uint)Constants.ResourceGroupIDs.VXD => "VXD",
                (uint)Constants.ResourceGroupIDs.AnimatedCurser => "AnimatedCurser",
                (uint)Constants.ResourceGroupIDs.AnimatedIcon => "AnimatedIcon",
                (uint)Constants.ResourceGroupIDs.HTML => "HTML",
                (uint)Constants.ResourceGroupIDs.Manifest => "Manifest",
                _ => "unknown"
            };

        /// <summary>
        ///     Resolve the subsystem attribute to a human readable string.
        /// </summary>
        /// <param name="subsystem">Subsystem attribute.</param>
        /// <returns>Subsystem as readable string.</returns>
        public static string ResolveSubsystem(ushort subsystem)
            => subsystem switch
            {
                1 => "native",
                2 => "Windows/GUI",
                3 => "Windows non-GUI",
                5 => "OS/2",
                7 => "POSIX",
                8 => "Native Windows 9x Driver",
                9 => "Windows CE",
                0xA => "EFI Application",
                0xB => "EFI boot service device",
                0xC => "EFI runtime driver",
                0xD => "EFI ROM",
                0xE => "XBox",
                _ => "unknown"
            };

        /// <summary>
        ///     Resolves the section flags to human readable strings.
        /// </summary>
        /// <param name="sectionFlags">Sections flags from the SectionHeader object.</param>
        /// <returns>List with flag names for the section.</returns>
        public static List<string> ResolveSectionFlags(uint sectionFlags)
        {
            var st = new List<string>();
            foreach (var flag in (Constants.SectionFlags[])Enum.GetValues(typeof(Constants.SectionFlags)))
            {
                if ((sectionFlags & (uint)flag) == (uint)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }

        /// <summary>
        ///     Resolve flags from the IMAGE_COR20_HEADER COM+ 2 (CLI) header to
        ///     their string representation.
        /// </summary>
        /// <param name="comImageFlags">Flags from IMAGE_COR20_HEADER.</param>
        /// <returns>List with resolved flag names.</returns>
        public static List<string> ResolveComImageFlags(uint comImageFlags)
        {
            var st = new List<string>();
            foreach (var flag in (DotNetConstants.COMImageFlag[])Enum.GetValues(typeof(DotNetConstants.COMImageFlag)))
            {
                if ((comImageFlags & (uint)flag) == (uint)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }

        /// <summary>
        ///     Resolve which tables are present in the .Net header based
        ///     on the MaskValid flags from the METADATATABLESHDR.
        /// </summary>
        /// <param name="maskValid">MaskValid value from the METADATATABLESHDR</param>
        /// <returns>List with present table names.</returns>
        public static List<string> ResolveMaskValidFlags(ulong maskValid)
        {
            var st = new List<string>();
            foreach (var flag in (DotNetConstants.MaskValidFlags[])Enum.GetValues(typeof(DotNetConstants.MaskValidFlags)))
            {
                if ((maskValid & (ulong)flag) == (ulong)flag)
                {
                    st.Add(flag.ToString());
                }
            }
            return st;
        }
    }
}