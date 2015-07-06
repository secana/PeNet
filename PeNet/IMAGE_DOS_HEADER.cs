using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_DOS_HEADER
    {
        public ushort e_magic { get; set; }
        public ushort e_cblp { get; set; }
        public ushort e_cp { get; set; }
        public ushort e_crlc { get; set; }
        public ushort e_cparhdr { get; set; }
        public ushort e_minalloc { get; set; }
        public ushort e_maxalloc { get; set; }
        public ushort e_ss { get; set; }
        public ushort e_sp { get; set; }
        public ushort e_csum { get; set; }
        public ushort e_ip { get; set; }
        public ushort e_cs { get; set; }
        public ushort e_lfaric { get; set; }
        public ushort e_ovno { get; set; }
        /*
         * 4 * ushort reserved
         */
        public ushort e_oemid { get; set; }
        public ushort e_oeminfo { get; set; }
        /*
         * 10 * ushort reserved
         */
        public UInt32 e_lfanew { get; set; } // Offset to the PE signature (IMAGE_NT_HEADERS)

        public IMAGE_DOS_HEADER(byte[] buff)
        {
            e_magic = Utility.BytesToUshort(buff, 0);
            e_cblp = Utility.BytesToUshort(buff, 2);
            e_cp = Utility.BytesToUshort(buff, 4);
            e_crlc = Utility.BytesToUshort(buff, 6);
            e_cparhdr = Utility.BytesToUshort(buff, 8);
            e_minalloc = Utility.BytesToUshort(buff, 0xA);
            e_maxalloc = Utility.BytesToUshort(buff, 0xC);
            e_ss = Utility.BytesToUshort(buff, 0xE);
            e_sp = Utility.BytesToUshort(buff, 0x10);
            e_csum = Utility.BytesToUshort(buff, 0x12);
            e_ip = Utility.BytesToUshort(buff, 0x14);
            e_cs = Utility.BytesToUshort(buff, 0x16);
            e_lfaric = Utility.BytesToUshort(buff, 0x18);
            e_ovno = Utility.BytesToUshort(buff, 0x1A);
            e_oemid = Utility.BytesToUshort(buff, 0x24);
            e_oeminfo = Utility.BytesToUshort(buff, 0x26);
            e_lfanew = Utility.BytesToUInt32(buff, 0x3C);
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_DOS_HEADER\n");
            sb.Append(Utility.ToStringReflection(this));
            return sb.ToString();
        }

    }
}
