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
        ///     Converts the section name (UTF-8 byte array) to a string.
        /// </summary>
        /// <param name="name">Section name byte array.</param>
        /// <returns>String representation of the section name.</returns>
        public static string ResolveSectionName(byte[] name)
        {
            return Encoding.UTF8.GetString(name).TrimEnd((char)0);
        }

        /// <summary>
        ///     Resolves the target machine number to a string containing
        ///     the name of the target machine.
        /// </summary>
        /// <param name="targetMachine">Target machine value from the COFF header.</param>
        /// <returns>Name of the target machine as string.</returns>
        public static string ResolveTargetMachine(ushort targetMachine)
            => targetMachine switch
            {
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I386 => "Intel 386",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_I860 => "Intel i860",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R3000 => "MIPS R3000",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R4000 => "MIPS little endian (R4000)",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_R10000 => "MIPS R10000",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_WCEMIPSV2 => "MIPS little endian WCI v2",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_OLDALPHA => "old Alpha AXP",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA => "Alpha AXP",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3 => "Hitachi SH3",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3DSP => "Hitachi SH3 DSP",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH3E => "Hitachi SH3E",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH4 => "Hitachi SH4",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_SH5 => "Hitachi SH5",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARM => "ARM little endian",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_THUMB => "Thumb",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AM33 => "Matsushita AM33",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPC => "PowerPC little endian",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_POWERPCFP => "PowerPC with floating point support",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_IA64 => "Intel IA64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPS16 => "MIPS16",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M68K => "Motorola 68000 series",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ALPHA64 => "Alpha AXP 64-bit",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU => "MIPS with FPU",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_TRICORE => "Tricore",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEF => "CEF",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_MIPSFPU16 => "MIPS16 with FPU",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_EBC => "EFI Byte Code",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_AMD64 => "AMD AMD64",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_M32R => "Mitsubishi M32R little endian",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_CEE => "clr pure MSIL",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARM64 => "ARM64 Little-Endian",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_ARMNT => "ARM Thumb-2 Little-Endian",
                (ushort)Constants.FileHeaderMachine.IMAGE_FILE_MACHINE_TARGET_HOST => "Interacts with the host and not a WOW64 guest",
                _ => "unknown"
            };

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
        public static List<string> ResolveCOMImageFlags(uint comImageFlags)
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