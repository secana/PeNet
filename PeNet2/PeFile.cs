using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    public class PeFile
    {
        public class ExportFunction
        {
            public string Name { get; private set; }
            public UInt32 Address { get; private set; }
            public UInt16 Ordinal { get; private set; }

            public ExportFunction(string name, UInt32 address, UInt16 ordinal)
            {
                Name = name;
                Address = address;
                Ordinal = ordinal;
            }
        }

        public class ImportFunction
        {
            public string Name { get; private set; }
            public string DLL { get; private set; }
            public UInt16 Hint { get; private set; }

            public ImportFunction(string name, string dll, UInt16 hint)
            {
                Name = name;
                DLL = dll;
                Hint = hint;
            }
        }

        public IMAGE_DOS_HEADER ImageDosHeader { get; private set; }
        public IMAGE_NT_HEADERS ImageNtHeaders { get; private set; }
        public IMAGE_SECTION_HEADER[] ImageSectionHeaders { get; private set; }
        public IMAGE_EXPORT_DIRECTORY ImageExportDirectory { get; private set; }
        public IMAGE_IMPORT_DESCRIPTOR[] ImageImportDescriptors { get; private set; }
        public ExportFunction[] ExportedFunctions { get; private set; }
        public ImportFunction[] ImportedFunctions { get; private set; }

        public PeFile(string peFile)
        {
            var buff = File.ReadAllBytes(peFile);

            ImageDosHeader = new IMAGE_DOS_HEADER(buff);
            ImageNtHeaders = new IMAGE_NT_HEADERS(buff, ImageDosHeader.e_lfanew);

            ImageSectionHeaders = ParseImageSectionHeaders(
                buff, 
                ImageNtHeaders.FileHeader.NumberOfSections, 
                ImageDosHeader.e_lfanew + 0xF8
                );
            
            if (ImageNtHeaders.OptionalHeader.DataDirectory[0].VirtualAddress != 0)
            {
                try
                {
                    ImageExportDirectory = new IMAGE_EXPORT_DIRECTORY(
                        buff, 
                        Utility.RVAtoFileMapping(ImageNtHeaders.OptionalHeader.DataDirectory[0].VirtualAddress, 
                        ImageSectionHeaders)
                        );

                    ExportedFunctions = ParseExportedFunctions(
                        buff, 
                        ImageExportDirectory, 
                        ImageSectionHeaders
                        );
                }
                catch
                {
                    // No or invalid export directory.
                }
            }

            if(ImageNtHeaders.OptionalHeader.DataDirectory[1].VirtualAddress != 0)
            {
                ImageImportDescriptors = ParseImportDescriptors(
                    buff, 
                    Utility.RVAtoFileMapping(ImageNtHeaders.OptionalHeader.DataDirectory[1].VirtualAddress, ImageSectionHeaders), 
                    ImageSectionHeaders
                    );

                ImportedFunctions = ParseImportedFunctions(buff, ImageImportDescriptors, ImageSectionHeaders);
            }
        }

        
        private ImportFunction[] ParseImportedFunctions(byte[] buff, IMAGE_IMPORT_DESCRIPTOR[] idescs, IMAGE_SECTION_HEADER[] sh)
        {
            var impFuncs = new List<ImportFunction>();
            UInt32 sizeOfThunk = 0x4; // Size of IMAGE_THUNK_DATA

            foreach(var idesc in idescs)
            {
                var dllAdr = Utility.RVAtoFileMapping(idesc.Name, sh);
                var dll = Utility.GetName(dllAdr, buff);
                var thunkAdr = Utility.RVAtoFileMapping(idesc.FirstThunk, sh);
                UInt32 round = 0;
                while(true)
                {
                    var t = new IMAGE_THUNK_DATA(buff, thunkAdr + round * sizeOfThunk);
                    
                    if (t.AddressOfData == 0)
                        break;

                    var ibn = new IMAGE_IMPORT_BY_NAME(buff, Utility.RVAtoFileMapping(t.AddressOfData, sh));

                    impFuncs.Add(new ImportFunction(ibn.Name, dll, ibn.Hint));
                    round++;
                }
            }

            return impFuncs.ToArray();
        }
        

        private IMAGE_IMPORT_DESCRIPTOR[] ParseImportDescriptors(byte[] buff, UInt32 offset, IMAGE_SECTION_HEADER[] sh)
        {
            var idescs = new List<IMAGE_IMPORT_DESCRIPTOR>();
            UInt32 idescSize = 20; // Size of IMAGE_IMPORT_DESCRIPTOR (5 * 4 Byte)
            UInt32 round = 0;

            while(true)
            {
                var idesc = new IMAGE_IMPORT_DESCRIPTOR(buff, offset + idescSize * round);
                
                // Found the last IMAGE_IMPORT_DESCRIPTOR which is completely null.
                if(idesc.OriginalFirstThunk == 0 
                    && idesc.TimeDateStamp == 0
                    && idesc.ForwarderChain == 0
                    && idesc.Name == 0
                    && idesc.FirstThunk == 0)
                {
                    break;
                }

                idescs.Add(idesc);
                round++;
            }

            return idescs.ToArray();
        }

        private ExportFunction[] ParseExportedFunctions(byte[] buff, IMAGE_EXPORT_DIRECTORY ed, IMAGE_SECTION_HEADER[] sh)
        {
            var expFuncs = new ExportFunction[ed.NumberOfNames];
            var funcOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfFunctions, sh);
            var ordOffset = Utility.RVAtoFileMapping(ed.AddressOfNameOrdinals, sh);
            var nameOffsetPointer = Utility.RVAtoFileMapping(ed.AddressOfNames, sh);

            var funcOffset = Utility.BytesToUInt32(buff, funcOffsetPointer);

            for(UInt32 i = 0; i < expFuncs.Length; i++)
            {
                var namePtr = Utility.BytesToUInt32(buff, nameOffsetPointer + sizeof(UInt32) * i);
                var nameAdr = Utility.RVAtoFileMapping(namePtr, sh);
                var name = Utility.GetName(nameAdr, buff);
                var ordinalIndex = (UInt32)Utility.GetOrdinal(ordOffset + sizeof(UInt16) * i, buff);
                var ordinal = ordinalIndex + ed.Base;
                var address = Utility.BytesToUInt32(buff, funcOffsetPointer + sizeof(UInt32) * ordinalIndex);

                expFuncs[i] = new ExportFunction(name, address, (UInt16) ordinal);
            }

            return expFuncs;
        }

        private IMAGE_SECTION_HEADER[] ParseImageSectionHeaders(byte[] buff, UInt16 numOfSections, UInt32 offset)
        {
            var sh = new IMAGE_SECTION_HEADER[numOfSections];
            UInt32 secSize = 0x28; // Every section header is 40 bytes in size.
            for(UInt32 i = 0; i < numOfSections; i++)
            {
                sh[i] = new IMAGE_SECTION_HEADER(buff, offset + i * secSize);
            }

            return sh;
        }
    }
}
