using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_OPTIONAL_HEADER
    {
        public ushort Magic { get; set; }                      // decimal number 267 for 32 bit, and 523 for 64 bit.
        public byte MajorLinkerVersion { get; set; }                   // The version, in x.y format of the linker used to create the PE.
        public byte MinorLinkerVersion { get; set; }
        public UInt32 SizeOfCode { get; set; }               // Size of the .text (.code) section
        public UInt32 SizeOfInitializedData { get; set; }    // Size of .data section
        public UInt32 SizeOfUninitializedData { get; set; }
        public UInt32 AddressOfEntryPoint { get; set; }
        public UInt32 BaseOfCode { get; set; }               // RVA of the .text section
        public UInt32 BaseOfData { get; set; }               // RVA of .data section
        public UInt64 ImageBase { get; set; }   // Preferred location in memory for the module to be based at
        public UInt32 SectionAlignment { get; set; }
        public UInt32 FileAlignment { get; set; }
        public ushort MajorOSVersion { get; set; }
        public ushort MinorOSVersion { get; set; }
        public ushort MajorImageVersion { get; set; }
        public ushort MinorImageVersion { get; set; }
        public ushort MajorSubSystemVersion { get; set; }
        public ushort MinorSubSystemVersion { get; set; }
        public UInt32 Win32VersionValue { get; set; }
        public UInt32 SizeOfImage { get; set; }
        public UInt32 SizeOfHeaders { get; set; }
        public UInt32 Checksum { get; set; }        // Checksum of the file, only used to verify validity of modules being loaded into kernel space. The formula used to calculate PE file checksums is proprietary, although Microsoft provides API calls that can calculate the checksum for you.
        public ushort Subsystem { get; set; }       // The Windows subsystem that will be invoked to run the executable
        public ushort DllCharacteristics { get; set; }
        public UInt64 SizeOfStackReverse { get; set; }
        public UInt64 SizeOfStackCommit { get; set; }
        public UInt64 SizeOfHeapReverse { get; set; }
        public UInt64 SizeOfHeapCommit { get; set; }
        public UInt32 LoaderFlags { get; set; }
        public UInt32 NumberOfRVAandSizes { get; set; }

        public IMAGE_DATA_DIRECTORY ImageDataDirectory { private set; get; }
        

        public IMAGE_OPTIONAL_HEADER(byte [] buff, UInt32 offset, bool is32Bit)
        {
            Magic = Utility.BytesToUshort(buff, offset);
            MajorLinkerVersion = buff[offset + 2];
            MinorLinkerVersion = buff[offset + 3];
            SizeOfCode = Utility.BytesToUInt32(buff, offset + 4);
            SizeOfInitializedData = Utility.BytesToUInt32(buff, offset + 8);
            SizeOfUninitializedData = Utility.BytesToUInt32(buff, offset + 0xC);
            AddressOfEntryPoint = Utility.BytesToUInt32(buff, offset + 0x10);
            BaseOfCode = Utility.BytesToUInt32(buff, offset + 0x14);
            BaseOfData = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x18) : 0;
            ImageBase = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x1c) : Utility.BytesToUInt64(buff, offset + 0x18);
            SectionAlignment = Utility.BytesToUInt32(buff, offset + 0x20);
            FileAlignment = Utility.BytesToUInt32(buff, offset + 0x24);
            MajorOSVersion = Utility.BytesToUshort(buff, offset + 0x28);
            MinorOSVersion = Utility.BytesToUshort(buff, offset + 0x2a);
            MajorImageVersion = Utility.BytesToUshort(buff, offset + 0x2c);
            MinorImageVersion = Utility.BytesToUshort(buff, offset + 0x2e);
            MajorSubSystemVersion = Utility.BytesToUshort(buff, offset + 0x30);
            MinorSubSystemVersion = Utility.BytesToUshort(buff, offset + 0x32);
            Win32VersionValue = Utility.BytesToUInt32(buff, offset + 0x34);
            SizeOfImage = Utility.BytesToUInt32(buff, offset + 0x38);
            SizeOfHeaders = Utility.BytesToUInt32(buff, offset + 0x3c);
            Checksum = Utility.BytesToUInt32(buff, offset + 0x40);
            Subsystem = Utility.BytesToUshort(buff, offset + 0x44);
            DllCharacteristics = Utility.BytesToUshort(buff, offset + 0x46);
            SizeOfStackReverse = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x48) : Utility.BytesToUInt64(buff, offset + 0x48);
            SizeOfStackCommit = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x4c) : Utility.BytesToUInt64(buff, offset + 0x50);
            SizeOfHeapReverse = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x50) : Utility.BytesToUInt64(buff, offset + 0x58);
            SizeOfHeapCommit = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x54) : Utility.BytesToUInt64(buff, offset + 0x60);
            LoaderFlags = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x58) : Utility.BytesToUInt32(buff, offset + 0x68);
            NumberOfRVAandSizes = (is32Bit) ? Utility.BytesToUInt32(buff, offset + 0x5c) : Utility.BytesToUInt32(buff, offset + 0x6c);
            ImageDataDirectory = (is32Bit) ? new IMAGE_DATA_DIRECTORY(buff, offset + 0x60, is32Bit) : new IMAGE_DATA_DIRECTORY(buff, offset + 0x70, is32Bit);
        }

        /// <summary>
        /// Resolve the subsystem attribute to a human readable string.
        /// </summary>
        /// <param name="subsystem">Subsystem attribute.</param>
        /// <returns>Subsystem as readable string.</returns>
        public static string ResolveSubsystem(ushort subsystem)
        {
            var ss = "unknown";
            switch (subsystem)
            {
                case 1:
                    ss = "native";
                    break;
                case 2:
                    ss = "Windows/GUI";
                    break;
                case 3:
                    ss = "Windows non-GUI";
                    break;
                case 5:
                    ss = "OS/2";
                    break;
                case 7:
                    ss = "POSIX";
                    break;
                case 8:
                    ss = "Native Windows 9x Driver";
                    break;
                case 9:
                    ss = "Windows CE";
                    break;
                case 0xA:
                    ss = "EFI Application";
                    break;
                case 0xB:
                    ss = "EFI boot service device";
                    break;
                case 0xC:
                    ss = "EFI runtime driver";
                    break;
                case 0xD:
                    ss = "EFI ROM";
                    break;
                case 0xE:
                    ss = "XBox";
                    break;
            }
            return ss;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_OPTIONAL_HEADER\n");
            sb.Append(Utility.ToStringReflection(this));
            sb.AppendFormat(Utility._tableFormat, "Resolved Subsystem", ResolveSubsystem(Subsystem));
            return sb.ToString();
        }
    }
}
