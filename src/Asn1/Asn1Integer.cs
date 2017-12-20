using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Asn1.Utils;

namespace Asn1 {
    public class Asn1Integer : Asn1Node {

        public const string NODE_NAME = "Integer";

        public byte[] Value { get; set; }

        public Asn1Integer(long value) {
            var zero = true;
            var vals = new List<byte>();
            for (var i = 7; i >= 0; i--) {
                var b = value >> (i * 8);
                if (!zero || b != 0) {
                    zero = false;
                    vals.Add((byte)(b & 0xff));
                }
            }
            Value = vals.Count > 0 ? vals.ToArray() : new byte[1];
        }

        public Asn1Integer(byte[] data) {
            Value = data.ToArray();
        }

        public ulong ToUInt64() {
            ulong val = 0;
            for (var i = 0; i < Value.Length; i++) {
                val = val << 8;
                val |= Value[i];
            }
            return val;
        }

        public static Asn1Integer ReadFrom(Stream stream) {
            var vals = new List<byte>();
            while (stream.Position < stream.Length) {
                vals.Add((byte)stream.ReadByte());
            }
            return new Asn1Integer(vals.ToArray());
        }

        public static Asn1Integer FromHexString(string val) {
            var data = StringUtils.GetBytesFromHexString(val);
            return new Asn1Integer(data);
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Integer;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, string.Join("", Value.Select(v => v.ToString("x2"))));
        }

        protected override byte[] GetBytesCore() {
            return Value.ToArray();
        }

        public new static Asn1Integer Parse(XElement xNode) {
            var vals = new List<byte>();
            var val = xNode.Value.Trim();
            for (var i = 0; i < val.Length; i += 2) {
                var b = val.Substring(i, 2);
                var bi = byte.Parse(b, NumberStyles.HexNumber);
                vals.Add(bi);
            }
            var res = new Asn1Integer(vals.ToArray());
            return res;
        }

    }
}
