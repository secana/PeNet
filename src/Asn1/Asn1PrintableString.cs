using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1PrintableString : Asn1Node {

        public const string NODE_NAME = "PrintableString";

        public string Value { get; set; }

        public static Asn1PrintableString ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1PrintableString { Value = Encoding.ASCII.GetString(data) };
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.PrintableString;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.ASCII.GetBytes(Value);
        }

        public new static Asn1PrintableString Parse(XElement xNode) {
            var res = new Asn1PrintableString();
            res.Value = xNode.Value.Trim(); //todo should it be trimmed?
            return res;
        }
    }
}
