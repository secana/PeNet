using System;
using System.Text;

namespace Asn1.Utils {
    public static class StringUtils {

        public static byte[] GetBytesFromHexString(string val) {
            var data = new byte[(val.Length + 1) / 2];
            for (var i = 0; i < val.Length; i++) {
                var ch = val[i];
                var bt = GetHexDigit(ch);
                var item = data[i / 2];
                if ((i & 1) == 0) {
                    item = (byte)((item & 0x0f) | (bt << 4));
                } else {
                    item = (byte)((item & 0xf0) | bt);
                }
                data[i / 2] = item;
            }
            return data;
        }

        public static string GetHexString(this byte[] data) {
            var sb = new StringBuilder(data.Length * 2);
            foreach (var bt in data) {
                sb.Append(GetHexDigit((byte)(bt >> 4)));
                sb.Append(GetHexDigit((byte)(bt & 0x0f)));
            }
            return sb.ToString();
        }

        private static char GetHexDigit(byte bt) {
            if (bt <= 9) return (char)('0' + bt);
            if (bt <= 15) return (char)('a' + bt - 10);
            throw new ArgumentException();
        }

        private static byte GetHexDigit(char ch) {
            if (ch >= '0' && ch <= '9') return (byte)(ch - '0');
            if (ch >= 'a' && ch <= 'f') return (byte)(ch - 'a' + 10);
            if (ch >= 'A' && ch <= 'F') return (byte)(ch - 'A' + 10);
            throw new FormatException();
        }
    }
}
