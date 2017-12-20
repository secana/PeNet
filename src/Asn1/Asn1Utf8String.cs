using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Utf8String : Asn1Node {

        public const string NODE_NAME = "UTF8";

        public string Value { get; set; }

        public Asn1Utf8String() {
        }

        public Asn1Utf8String(string value) {
            Value = value;
        }

        public static Asn1Utf8String ReadFrom(Stream stream) {
            var data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            return new Asn1Utf8String { Value = Encoding.UTF8.GetString(data) };
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Utf8String;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME, Value);
        }

        protected override byte[] GetBytesCore() {
            return Encoding.UTF8.GetBytes(Value);
        }

        public new static Asn1Utf8String Parse(XElement xNode) {
            var res = new Asn1Utf8String();
            res.Value = xNode.Value.Trim(); //todo should it be trimmed?
            return res;
        }
    }
}
