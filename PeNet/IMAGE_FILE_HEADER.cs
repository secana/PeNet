using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public class IMAGE_FILE_HEADER
    {
        public ushort Machine { get; set; }        // This field determines what machine the file was compiled for. 
        public ushort NumberOfSections { get; set; }     // The number of sections that are described at the end of the PE headers.
        public UInt32 TimeDateStamp { get; set; }        // 32 bit time at which this header was generated: is used in the process of "Binding".
        public UInt32 PointerToSymbolTable { get; set; }
        public UInt32 NumberOfSymbols { get; set; }
        public ushort SizeOfOptionalHeaders { get; set; } // This field shows how long the "PE Optional Header" is that follows the COFF header.
        public ushort Characteristics { get; set; }

        public IMAGE_FILE_HEADER(byte [] buff, UInt32 offset)
        {
            Machine = Utility.BytesToUshort(buff, offset);
            NumberOfSections = Utility.BytesToUshort(buff, offset + 2);
            TimeDateStamp = Utility.BytesToUInt32(buff, offset + 4);
            PointerToSymbolTable = Utility.BytesToUInt32(buff, offset + 8);
            NumberOfSymbols = Utility.BytesToUInt32(buff, offset + 12);
            SizeOfOptionalHeaders = Utility.BytesToUshort(buff, offset + 16);
            Characteristics = Utility.BytesToUshort(buff, offset + 18);
        }

        /// <summary>
        /// Resolves the target machine number to a string containing
        /// the name of the target machine.
        /// </summary>
        /// <param name="targetMachine">Target machine value from the COFF header.</param>
        /// <returns>Name of the target machine as string.</returns>
        public static string ResolveTargetMachine(ushort targetMachine)
        {
            string tm = "unknown";
            switch (targetMachine)
            {
                case 0x14c:
                    tm = "Intel 386";
                    break;
                case 0x14d:
                    tm = "Intel i860";
                    break;
                case 0x162:
                    tm = "MIPS R3000";
                    break;
                case 0x166:
                    tm = "MIPS little endian (R4000)";
                    break;
                case 0x168:
                    tm = "MIPS R10000";
                    break;
                case 0x169:
                    tm = "MIPS little endian WCI v2";
                    break;
                case 0x183:
                    tm = "old Alpha AXP";
                    break;
                case 0x184:
                    tm = "Alpha AXP";
                    break;
                case 0x1a2:
                    tm = "Hitachi SH3";
                    break;
                case 0x1a3:
                    tm = "Hitachi SH3 DSP";
                    break;
                case 0x1a6:
                    tm = "Hitachi SH4";
                    break;
                case 0x1a8:
                    tm = "Hitachi SH5";
                    break;
                case 0x1c0:
                    tm = "ARM little endian";
                    break;
                case 0x1c2:
                    tm = "Thumb";
                    break;
                case 0x1d3:
                    tm = "Matsushita AM33";
                    break;
                case 0x1f0:
                    tm = "PowerPC little endian";
                    break;
                case 0x1f1:
                    tm = "PowerPC with floating point support";
                    break;
                case 0x200:
                    tm = "Intel IA64";
                    break;
                case 0x266:
                    tm = "MIPS16";
                    break;
                case 0x268:
                    tm = "Motorola 68000 series";
                    break;
                case 0x284:
                    tm = "Alpha AXP 64-bit";
                    break;
                case 0x366:
                    tm = "MIPS with FPU";
                    break;
                case 0x466:
                    tm = "MIPS16 with FPU";
                    break;
                case 0xebc:
                    tm = "EFI Byte Code";
                    break;
                case 0x8664:
                    tm = "AMD AMD64";
                    break;
                case 0x9041:
                    tm = "Mitsubishi M32R little endian";
                    break;
                case 0xc0ee:
                    tm = "clr pure MSIL";
                    break;
            }

            return tm;
        }

        /// <summary>
        /// Resolves the characteristics attribute from the COFF header to a human
        /// readable string.
        /// </summary>
        /// <param name="characteristics">COFF header characteristics.</param>
        /// <returns>Human readable characteristics string.</returns>
        public static string ResolveCharacteristics(ushort characteristics)
        {
            var c = "unknown";
            if ((characteristics & 0x02) == 0x02)
                c = "EXE";
            else if ((characteristics & 0x200) == 0x200)
                c = "File is non-relocatable (addresses are absolute, not RVA).";
            else if ((characteristics & 0x2000) == 0x2000)
                c = "DLL";
            return c;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("IMAGE_FILE_HEADER\n");
            sb.Append(Utility.ToStringReflection(this));
            sb.AppendFormat(Utility._tableFormat, "Resolved TargetMachine", ResolveTargetMachine(Machine));
            sb.AppendFormat(Utility._tableFormat, "Resolved Characteristic", ResolveCharacteristics(Characteristics));
            return sb.ToString();
        }
    }
}
