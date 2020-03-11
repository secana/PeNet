using System;

namespace PeNet
{
    /// <summary>
    ///     Describes which file characteristics based on the
    ///     file header are set.
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
            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_RELOCS_STRIPPED) > 0)
                RelocStripped = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_EXECUTABLE_IMAGE) > 0)
                ExecutableImage = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_LINE_NUMS_STRIPPED) > 0)
                LineNumbersStripped = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_LOCAL_SYMS_STRIPPED) > 0)
                LocalSymbolsStripped = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_AGGRESIVE_WS_TRIM) > 0)
                AggressiveWsTrim = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_LARGE_ADDRESS_AWARE) > 0)
                LargeAddressAware = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_BYTES_REVERSED_LO) > 0)
                BytesReversedLo = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_32BIT_MACHINE) > 0)
                Machine32Bit = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_DEBUG_STRIPPED) > 0)
                DebugStripped = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP) >
                0)
                RemovableRunFromSwap = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_NET_RUN_FROM_SWAP) > 0)
                NetRunFromSwap = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_SYSTEM) > 0)
                System = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_DLL) > 0)
                DLL = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_UP_SYSTEM_ONLY) > 0)
                UpSystemOnly = true;

            if ((characteristics & (ushort) CharacteristicsType.IMAGE_FILE_BYTES_REVERSED_HI) > 0)
                BytesReversedHi = true;
        }

        /// <summary>
        ///     Relocation stripped,
        /// </summary>
        public bool RelocStripped { get; }

        /// <summary>
        ///     Is an executable image.
        /// </summary>
        public bool ExecutableImage { get; }

        /// <summary>
        ///     Line numbers stripped.
        /// </summary>
        public bool LineNumbersStripped { get; }

        /// <summary>
        ///     Local symbols stripped.
        /// </summary>
        public bool LocalSymbolsStripped { get; }

        /// <summary>
        ///     (OBSOLTETE) Aggressively trim the working set.
        /// </summary>
        public bool AggressiveWsTrim { get; }

        /// <summary>
        ///     Application can handle addresses larger than 2 GB.
        /// </summary>
        public bool LargeAddressAware { get; }

        /// <summary>
        ///     (OBSOLTETE) Bytes of word are reversed.
        /// </summary>
        public bool BytesReversedLo { get; }

        /// <summary>
        ///     Supports 32 Bit words.
        /// </summary>
        public bool Machine32Bit { get; }

        /// <summary>
        ///     Debug stripped and stored in a separate file.
        /// </summary>
        public bool DebugStripped { get; }

        /// <summary>
        ///     If the image is on a removable media, copy and run it from the swap file.
        /// </summary>
        public bool RemovableRunFromSwap { get; }

        /// <summary>
        ///     If the image is on the network, copy and run it from the swap file.
        /// </summary>
        public bool NetRunFromSwap { get; }

        /// <summary>
        ///     The image is a system file.
        /// </summary>
        public bool System { get; }

        /// <summary>
        ///     Is a dynamic loaded library and executable but cannot
        ///     be run on its own.
        /// </summary>
        public bool DLL { get; }

        /// <summary>
        ///     Image should be run only on uniprocessor.
        /// </summary>
        public bool UpSystemOnly { get; }

        /// <summary>
        ///     (OBSOLETE) Reserved.
        /// </summary>
        public bool BytesReversedHi { get; }
    }

    /// <summary>
    ///     File characteristics from the file header.
    /// </summary>
    [Flags]
    public enum CharacteristicsType : ushort
    {
        /// <summary>
        ///     Relocation stripped.
        /// </summary>
        IMAGE_FILE_RELOCS_STRIPPED = 0x01,

        /// <summary>
        ///     Executable image.
        /// </summary>
        IMAGE_FILE_EXECUTABLE_IMAGE = 0x02,

        /// <summary>
        ///     Line numbers stripped.
        /// </summary>
        IMAGE_FILE_LINE_NUMS_STRIPPED = 0x04,

        /// <summary>
        ///     Local symbols stripped.
        /// </summary>
        IMAGE_FILE_LOCAL_SYMS_STRIPPED = 0x08,

        /// <summary>
        ///     (OBSOLTETE) Aggressively trim the working set.
        /// </summary>
        IMAGE_FILE_AGGRESIVE_WS_TRIM = 0x10,

        /// <summary>
        ///     Application can handle addresses larger than 2 GB.
        /// </summary>
        IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x20,

        /// <summary>
        ///     (OBSOLTETE) Bytes of word are reversed.
        /// </summary>
        IMAGE_FILE_BYTES_REVERSED_LO = 0x80,

        /// <summary>
        ///     Supports 32 Bit words.
        /// </summary>
        IMAGE_FILE_32BIT_MACHINE = 0x100,

        /// <summary>
        ///     Debug stripped and stored in a separate file.
        /// </summary>
        IMAGE_FILE_DEBUG_STRIPPED = 0x200,

        /// <summary>
        ///     If the image is on a removable media, copy and run it from the swap file.
        /// </summary>
        IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = 0x400,

        /// <summary>
        ///     If the image is on the network, copy and run it from the swap file.
        /// </summary>
        IMAGE_FILE_NET_RUN_FROM_SWAP = 0x800,

        /// <summary>
        ///     The image is a system file.
        /// </summary>
        IMAGE_FILE_SYSTEM = 0x1000,

        /// <summary>
        ///     Is a dynamic loaded library and executable but cannot
        ///     be run on its own.
        /// </summary>
        IMAGE_FILE_DLL = 0x2000,

        /// <summary>
        ///     Image should be run only on uniprocessor.
        /// </summary>
        IMAGE_FILE_UP_SYSTEM_ONLY = 0x4000,

        /// <summary>
        ///     (OBSOLETE) Reserved.
        /// </summary>
        IMAGE_FILE_BYTES_REVERSED_HI = 0x8000
    }
}