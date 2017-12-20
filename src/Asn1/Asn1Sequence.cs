using System.IO;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1Sequence : Asn1CompositeNode {

        public const string NODE_NAME = "Sequence";

        public static Asn1Sequence ReadFrom(Stream stream) {
            var res = new Asn1Sequence();
            res.ReadChildren(stream);
            return res;
        }

        public static Asn1Sequence ReadFrom(byte[] data) {
            return ReadFrom(new MemoryStream(data));
        }

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.Sequence;

        protected override XElement ToXElementCore() {
            return new XElement(NODE_NAME);
        }

        public new static Asn1Sequence Parse(XElement xNode) {
            var res = new Asn1Sequence();
            res.ParseChildren(xNode);
            return res;
        }

    }
}
