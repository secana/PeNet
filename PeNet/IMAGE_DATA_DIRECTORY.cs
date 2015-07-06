using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_DATA_DIRECTORY
    {
        public UInt32 edataOffset { get; set; }
        public UInt32 edataSize { get; set; }
        public UInt32 idataOffset { get; set; }
        public UInt32 idataSize { get; set; }
        public UInt32 rsrcOffset { get; set; }
        public UInt32 rsrcSize { get; set; }
        public UInt32 pdataOffset { get; set; }
        public UInt32 pdataSize { get; set; }
        public UInt32 AttributeCertificateOffsetImage { get; set; }
        public UInt32 AttributeCertificateSizeImage { get; set; }
        public UInt32 relocOffsetImage { get; set; }
        public UInt32 relocSizeImage { get; set; }
        public UInt32 debugOffset { get; set; }
        public UInt32 debugSize { get; set; }
        public UInt32 Architecture1 { get; set; }
        public UInt32 Architecture2 { get; set; }
        public UInt32 GlobalPtrOffset { get; set; }
        public UInt32 Zero { get; set; }
        public UInt32 tlsOffset { get; set; }
        public UInt32 tlsSize { get; set; }
        public UInt32 LoadConfigTableOffsetImage { get; set; }
        public UInt32 LoadConfigTableSizeImage { get; set; }
        public UInt32 BoundImportTableOffset { get; set; }
        public UInt32 BoundImportTableSize { get; set; }
        public UInt32 ImportAddressTableOffset { get; set; }
        public UInt32 ImportAddressTableSize { get; set; }
        public UInt32 DelayImportDescriptorOffsetImage { get; set; }
        public UInt32 DelayImportDescriptorSizeImage { get; set; }
        public UInt32 CLRRuntimeHeaderOffsetObject { get; set; }
        public UInt32 CLRRuntimeHeaderSizeObject { get; set; }
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectory { get; set; }
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors { get; set; }


        public IMAGE_DATA_DIRECTORY(byte[] buff, UInt32 offset, bool is32Bit)
        {
            edataOffset = Utility.BytesToUInt32(buff, offset);      // Offset to the ExportTable
            edataSize = Utility.BytesToUInt32(buff, offset + 4);  // Size of the ExportTable
            idataOffset = Utility.BytesToUInt32(buff, offset + 8);  // Offset to the ImportTable
            idataSize = Utility.BytesToUInt32(buff, offset + 12); // Size of the ImportTable
            rsrcOffset = Utility.BytesToUInt32(buff, offset + 16);
            rsrcSize = Utility.BytesToUInt32(buff, offset + 20);
            pdataOffset = Utility.BytesToUInt32(buff, offset + 24);
            pdataSize = Utility.BytesToUInt32(buff, offset + 28);
            AttributeCertificateOffsetImage = Utility.BytesToUInt32(buff, offset + 32);
            AttributeCertificateSizeImage = Utility.BytesToUInt32(buff, offset + 36);
            relocOffsetImage = Utility.BytesToUInt32(buff, offset + 40);
            relocSizeImage = Utility.BytesToUInt32(buff, offset + 44);
            debugOffset = Utility.BytesToUInt32(buff, offset + 48);
            debugSize = Utility.BytesToUInt32(buff, offset + 52);
            Architecture1 = Utility.BytesToUInt32(buff, offset + 56);
            Architecture2 = Utility.BytesToUInt32(buff, offset + 60);
            GlobalPtrOffset = Utility.BytesToUInt32(buff, offset + 64);
            Zero = Utility.BytesToUInt32(buff, offset + 68); // Always 0x00
            tlsOffset = Utility.BytesToUInt32(buff, offset + 72);
            tlsSize = Utility.BytesToUInt32(buff, offset + 76);
            LoadConfigTableOffsetImage = Utility.BytesToUInt32(buff, offset + 80);
            LoadConfigTableSizeImage = Utility.BytesToUInt32(buff, offset + 84);
            BoundImportTableOffset = Utility.BytesToUInt32(buff, offset + 88);
            BoundImportTableSize = Utility.BytesToUInt32(buff, offset + 92);
            ImportAddressTableOffset = Utility.BytesToUInt32(buff, offset + 96);
            ImportAddressTableSize = Utility.BytesToUInt32(buff, offset + 100);
            DelayImportDescriptorOffsetImage = Utility.BytesToUInt32(buff, offset + 104);
            DelayImportDescriptorSizeImage = Utility.BytesToUInt32(buff, offset + 108);
            CLRRuntimeHeaderOffsetObject = Utility.BytesToUInt32(buff, offset + 112);
            CLRRuntimeHeaderSizeObject = Utility.BytesToUInt32(buff, offset + 116);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DATA_DIRECTORY\n");
            sb.Append(Utility.ToStringReflection(this));

            if (ImageExportDirectory.ExportFunctions != null)
            {
                sb.Append("Export Functions\n");
                foreach (var e in ImageExportDirectory.ExportFunctions)
                    sb.Append(e.ToString());
            }

            if (ImageImportDescriptors != null)
            {
                sb.Append("Image Import Descriptors");
                foreach (var i in ImageImportDescriptors)
                    sb.Append(i.ToString());
            }

            return sb.ToString();
        }

        /*
         * 2 * UInt32 reserved
         */

        public class IMAGE_EXPORT_DIRECTORY
        {
            public UInt32 Characteristics { get; set; }
            public UInt32 TimeDateStamp { get; set; }
            public ushort MajorVersion { get; set; }
            public ushort MinorVersion { get; set; }
            public UInt32 Name { get; set; }
            public UInt32 Base { get; set; }
            public UInt32 NumberOfFuncions { get; set; }
            public UInt32 NumberOfNames { get; set; }
            public UInt32 AddressOfFunctions { get; set; }
            public UInt32 AddressOfNames { get; set; }
            public UInt32 AddressOfNameOrdinals { get; set; }
            public ExportFunction[] ExportFunctions { get; set; }

            public IMAGE_EXPORT_DIRECTORY(byte[] buff, UInt32 offset)
            {
                Characteristics = Utility.BytesToUInt32(buff, offset);
                TimeDateStamp = Utility.BytesToUInt32(buff, offset + 4);
                MajorVersion = Utility.BytesToUshort(buff, offset + 8);
                MinorVersion = Utility.BytesToUshort(buff, offset + 0x0A);
                Name = Utility.BytesToUInt32(buff, offset + 0x0C);
                Base = Utility.BytesToUInt32(buff, offset + 0x10);
                NumberOfFuncions = Utility.BytesToUInt32(buff, offset + 0x14);
                NumberOfNames = Utility.BytesToUInt32(buff, offset + 0x18);
                AddressOfFunctions = Utility.BytesToUInt32(buff, offset + 0x1C);
                AddressOfNames = Utility.BytesToUInt32(buff, offset + 0x20);
                AddressOfNameOrdinals = Utility.BytesToUInt32(buff, offset + 0x24);
            }

            public override string ToString()
            {
                var sb = new StringBuilder("IMAGE_EXPORT_DIRECTORY\n");
                sb.Append(Utility.ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class ExportFunction
        {
            public UInt32 Address { get; private set; }
            public string Name { get; private set; }
            public ushort Ordinal { get; private set; }

            public ExportFunction(UInt32 address, string name, ushort ordinal)
            {
                Address = address;
                Name = name;
                Ordinal = ordinal;
            }

            public override string ToString()
            {
                var sb = new StringBuilder("Export Function\n");
                sb.Append(Utility.ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class IMAGE_IMPORT_DESCRIPTOR
        {
            public UInt32 OriginalFirstThunk { get; set; }
            public UInt32 TimeDataStamp { get; set; }
            public UInt32 ForwarderChain { get; set; }
            public UInt32 Name { get; set; }
            public UInt32 FirstThunk { get; set; }

            public IMAGE_THUNK_DATA ImageThunkData32 { get; set; }
            public String NameResolved { get; set; }

            public IMAGE_IMPORT_DESCRIPTOR(byte[] buff, UInt32 offset, IMAGE_SECTION_HEADER[] sh)
            {
                OriginalFirstThunk = Utility.BytesToUInt32(buff, offset);
                TimeDataStamp = Utility.BytesToUInt32(buff, offset + sizeof(UInt32));
                ForwarderChain = Utility.BytesToUInt32(buff, offset + 2 * sizeof(UInt32));
                Name = Utility.BytesToUInt32(buff, offset + 3 * sizeof(UInt32));
                FirstThunk = Utility.BytesToUInt32(buff, offset + 4 * sizeof(UInt32));

                try
                {
                    // If no name can be resolved, break
                    NameResolved = Utility.GetName(Utility.RVAtoFileMapping(Name, sh), buff);
                }
                catch
                {
                    NameResolved = null;
                }
            }

            public override string ToString()
            {
                var sb = new StringBuilder("IMAGE_IMPORT_DESCRIPTOR\n");
                sb.Append(Utility.ToStringReflection(this));
                return sb.ToString();
            }
        }

        public class IMAGE_THUNK_DATA
        {
            private UInt64 _value;

            public UInt64 ForwarderString  // PBYTE
            {
                get { return _value; }
                set { _value = value; }
            }

            public UInt64 Function  // PDWORD
            {
                get { return _value; }
                set { _value = value; }
            }

            public UInt64 Ordinal
            {
                get { return _value; }
                set { _value = value; }
            }

            public UInt64 AddressOfData  // PIMAGE_IMPORT_BY_NAME
            {
                get { return _value; }
                set { _value = value; }
            }

            public IMAGE_IMPORT_BY_NAME[] ImageImportByName;

            public IMAGE_THUNK_DATA(byte[] buff, UInt32 offset, IMAGE_SECTION_HEADER[] sh, bool is32Bit)
            {
                if (is32Bit)
                {
                    Ordinal = Utility.BytesToUInt32(buff, Utility.RVAtoFileMapping(offset, sh));

                    // Check if import by name or by ordinal.
                    // If it is an import by ordinal, the most significant bit of "Ordinal" is "1" and the ordinal can
                    // be extracted from the least significant bits.
                    // Else it is an import by name and the link to the IMAGE_IMPORT_BY_NAME has to be followed
                    if ((Ordinal & 0x80000000) == 0x80000000)
                    {
                        Ordinal = (Ordinal & 0x7FFFFFFF);
                    }
                    else
                    {
                        var ordinal = Utility.RVAtoFileMapping((UInt32) Ordinal, sh);
                        ImageImportByName = ParseImageImportByName(nextItdAddress, mode2, buff, st);
                    }
                }
                else
                {
                    Ordinal = Utility.BytesToUInt64(buff, Utility.RVAtoFileMapping(offset, sh));
                    if ((Ordinal & 0x8000000000000000) == 0x8000000000000000)
                    {
                        Ordinal = (Ordinal & 0x7FFFFFFFFFFFFFFF);
                    }
                    else
                    {
                        var ordinal = Utility.RVAtoFileMapping(Ordinal, sh);
                        ImageImportByName = ParseImageImportByName(nextItdAddress, mode2, buff, st);
                    }
                }
                    
                private IMAGE_IMPORT_BY_NAME[] ParseImageImportByName(UInt32 nextItdAddress, bool mode2, byte[] buff, IMAGE_SECTION_HEADER[] sh)
                {
                    var iibns = new List<IMAGE_IMPORT_BY_NAME>();
                    UInt64 address = Utility.RVAtoFileMapping(AddressOfData, sh);

                    if (nextItdAddress != 0)
                    {
                        nextItdAddress = Utility.RVAtoFileMapping(nextItdAddress, sh);
                        // Runs until the address of the next IMAGE_IMPORT_BY_NAME structure is reached.
                        while (address + 1 < nextItdAddress)
                        {
                            var name = Utility.GetName(address + 2, buff);
                            if (name == "" || name[0] < 0x41 || name[0] > 0x7a)
                            {
                                address += 1;
                                continue;
                            }
                            var length = GetNameLength(address + 2, buff);
                            ushort hint = BytesToUshort(buff, address);

                            if (!mode2)
                            {
                                address += (UInt32)(2 + length + 1); // 2 byte hint, length of name, one terminating null byte
                            }
                            else
                            {
                                address += (UInt32)length + 2;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (buff[address] != 0)
                                    {
                                        address -= 2;
                                        break;
                                    }
                                    address++;
                                    if (i == 3)
                                        break;
                                }
                            }
                            iibns.Add(new DATADIRECTORIES.IMAGE_IMPORT_BY_NAME() { Hint = hint, Name = name });
                        }
                    }
                    else
                    {
                        int counter = 0;
                        while (counter <= 3)
                        {
                            var name = GetName(address + 2, buff);
                            if (name == "" || name[0] < 0x41 || name[0] > 0x7a)
                            {
                                address += 1;
                                counter++;
                                continue;
                            }
                            counter = 0;
                            var length = GetNameLength(address + 2, buff);
                            ushort hint = BytesToUshort(buff, address);

                            address += (UInt32)(2 + length + 1); // 2 byte hint, length of name, one terminating null byte
                            iibns.Add(new DATADIRECTORIES.IMAGE_IMPORT_BY_NAME() { Hint = hint, Name = name });
                        }
                    }

                    return iibns.ToArray();
                }
                
            }

            public override string ToString()
            {
                var sb = new StringBuilder("IMAGE_THUNK_DATA\n");
                sb.Append(Utility.ToStringReflection(this));

                if (ImageImportByName != null)
                {
                    sb.Append("Image Imports By Name\n");
                    foreach (var i in ImageImportByName)
                        sb.Append(i.ToString());
                }

                return sb.ToString();
            }
        }

        public class IMAGE_IMPORT_BY_NAME
        {
            public UInt16 Hint { get; set; }
            public String Name { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder("IMAGE_IMPORT_BY_NAME\n");
                sb.Append(Utility.ToStringReflection(this));
                return sb.ToString();
            }
        }
    }
}
