using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_NT_HEADERS
    {
        public UInt32 Signature { get; set; } // PE\0\0
        public IMAGE_FILE_HEADER ImageFileHeader { get; private set; }
        public IMAGE_OPTIONAL_HEADER ImageOptionalHeader { get; private set; }
        
        public IMAGE_NT_HEADERS(byte [] buff, UInt32 offset)
        {
            Signature = Utility.BytesToUInt32(buff, offset);
            ImageFileHeader = new IMAGE_FILE_HEADER(buff, offset + 0x4);

            // Determine if the PE file is a x32 or x64 file.
            bool is32Bit = (ImageFileHeader.Machine == 34404) ? false : true;

            ImageOptionalHeader = new IMAGE_OPTIONAL_HEADER(buff, offset + 0x18, is32Bit);
        }
    }
}
