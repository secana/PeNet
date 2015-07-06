using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PeNet
{
    public static class Utility
    {
        public static readonly string _tableFormat = "\t{0,-35}:\t{1,30}\n";
        public static string ToStringReflection(object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach (var p in properties)
            {
                if (p.PropertyType.IsArray)
                    continue;
                sb.AppendFormat(_tableFormat, p.Name, p.GetValue(obj, null));
            }

            return sb.ToString();
        }

        private static ushort BytesToUshort(byte b1, byte b2)
        {
            return BitConverter.ToUInt16(new byte[2] { b1, b2 }, 0);
        }

        public static ushort BytesToUshort(byte[] buff, UInt32 i)
        {
            return BytesToUshort(buff[i], buff[i + 1]);
        }

        private static UInt32 BytesToUInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return BitConverter.ToUInt32(new byte[4] { b1, b2, b3, b4 }, 0);
        }

        public static UInt32 BytesToUInt32(byte[] buff, UInt32 i)
        {
            return BytesToUInt32(buff[i], buff[i + 1], buff[i + 2], buff[i + 3]);
        }

        private static UInt64 BytesToUInt64(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8)
        {
            return BitConverter.ToUInt64(new byte[8] { b1, b2, b3, b4, b5, b6, b7, b8 }, 0);
        }

        public static UInt64 BytesToUInt64(byte[] buff, UInt32 i)
        {
            return BytesToUInt64(buff[i], buff[i + 1], buff[i + 2], buff[i + 3], buff[i + 4], buff[i + 5], buff[i + 6], buff[i + 7]);
        }

        public static UInt32 RVAtoFileMapping(UInt32 RVA, PeNet.IMAGE_SECTION_HEADER[] st)
        {
            var sortedSt = st.OrderBy(sh => sh.VirutalAddress).ToList();
            UInt32 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirutalAddress <= RVA && sortedSt[i + 1].VirutalAddress > RVA)
                {
                    vOffset = sortedSt[i].VirutalAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirutalAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirutalAddress)
                {
                    vOffset = sortedSt.Last().VirutalAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        public static UInt64 RVAtoFileMapping(UInt64 RVA, PeNet.IMAGE_SECTION_HEADER[] st)
        {
            var sortedSt = st.OrderBy(sh => sh.VirutalAddress).ToList();
            UInt64 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirutalAddress <= RVA && sortedSt[i + 1].VirutalAddress > RVA)
                {
                    vOffset = sortedSt[i].VirutalAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirutalAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirutalAddress)
                {
                    vOffset = sortedSt.Last().VirutalAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        public static string GetName(UInt32 name, byte[] buff)
        {
            var length = GetNameLength(name, buff);
            var tmp = new char[length];
            for (int i = 0; i < length; i++)
            {
                tmp[i] = (char)buff[name + i];
            }

            return new string(tmp);
        }

        public static int GetNameLength(UInt32 name, byte[] buff)
        {
            var offset = name;
            int length = 0;
            while (buff[offset] != 0x00)
            {
                length++;
                offset++;
            }
            return length;
        }
    }
}
