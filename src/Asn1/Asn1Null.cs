using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Null : Asn1Node {

        public const string NODE_NAME = "Null";

        public static Asn1Null ReadFrom(Stream stream) {
            return new Asn1Null();
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Null;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME);
        }

        protected override byte[] GetBytesCore() {
            return new byte[0];
        }

        public new static Asn1Null Parse(XElement xNode) {
            var res = new Asn1Null();
            return res;
        }
    }
}
