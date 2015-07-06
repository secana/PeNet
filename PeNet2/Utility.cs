using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeNet2
{
    internal static class Utility
    {
        private static ushort BytesToUInt16(byte b1, byte b2)
        {
            return BitConverter.ToUInt16(new byte[2] { b1, b2 }, 0);
        }

        public static ushort BytesToUInt16(byte[] buff, UInt32 i)
        {
            return BytesToUInt16(buff[i], buff[i + 1]);
        }

        private static UInt32 BytesToUInt32(byte b1, byte b2, byte b3, byte b4)
        {
            return BitConverter.ToUInt32(new byte[4] { b1, b2, b3, b4 }, 0);
        }

        public static UInt32 BytesToUInt32(byte[] buff, UInt32 i)
        {
            return BytesToUInt32(buff[i], buff[i + 1], buff[i + 2], buff[i + 3]);
        } 

        private static byte[] UInt16ToBytes(UInt16 value)
        {
            return BitConverter.GetBytes(value);
        }

        public static void SetUInt16(UInt16 value, UInt32 offset, byte[] buff)
        {
            var x = UInt16ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
        }

        private static byte[] UInt32ToBytes(UInt32 value)
        {
            return BitConverter.GetBytes(value);
        }

        public static void SetUInt32(UInt32 value, UInt32 offset, byte[] buff)
        {
            var x = UInt32ToBytes(value);
            buff[offset] = x[0];
            buff[offset + 1] = x[1];
            buff[offset + 2] = x[2];
            buff[offset + 3] = x[3];
        }

        public static string PropertiesToString(object obj, string format)
        {
            var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach(var p in properties)
            {
                if (p.PropertyType.IsArray)
                    continue;

                sb.AppendFormat(format, p.Name, p.GetValue(obj));
            }

            return sb.ToString();
        }

        public static UInt32 RVAtoFileMapping(UInt32 RVA, IMAGE_SECTION_HEADER[] sh)
        {
            var sortedSt = sh.OrderBy(x => x.VirtualAddress).ToList();
            UInt32 vOffset = 0, rOffset = 0;
            bool secFound = false;
            for (int i = 0; i < sortedSt.Count - 1; i++)
            {
                if (sortedSt[i].VirtualAddress <= RVA && sortedSt[i + 1].VirtualAddress > RVA)
                {
                    vOffset = sortedSt[i].VirtualAddress;
                    rOffset = sortedSt[i].PointerToRawData;
                    secFound = true;
                    break;
                }
            }

            // try last section
            if (secFound == false)
            {
                if (RVA >= sortedSt.Last().VirtualAddress && RVA <= sortedSt.Last().VirtualSize + sortedSt.Last().VirtualAddress)
                {
                    vOffset = sortedSt.Last().VirtualAddress;
                    rOffset = sortedSt.Last().PointerToRawData;
                }
                else
                {
                    throw new Exception("Cannot find corresponding section.");
                }
            }

            return (RVA - vOffset) + rOffset;
        }

        public static UInt16 GetOrdinal(UInt32 ordinal, byte[] buff)
        {
            return BitConverter.ToUInt16(new byte[2] { buff[ordinal], buff[ordinal + 1] }, 0);
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
