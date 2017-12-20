using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Asn1 {
    public class Asn1UtcTime : Asn1Node {

        public override Asn1UniversalNodeType NodeType => Asn1UniversalNodeType.UtcTime;

        public override Asn1TagForm TagForm => Asn1TagForm.Primitive;

        public DateTimeOffset Value { get; set; }

        protected override XElement ToXElementCore() {
            return new XElement("UTCTime", Value.ToString(CultureInfo.InvariantCulture));
        }

        protected override byte[] GetBytesCore() {
            var format = Value.Offset != TimeSpan.Zero ? "yyMMddHHmmsszzz" : "yyMMddHHmmssZ";
            var str = Value.ToString(format, CultureInfo.InvariantCulture);
            var data = Encoding.UTF8.GetBytes(str);
            return data;
        }

        public static Asn1UtcTime ReadFrom(Stream stream) {
            var val = new StreamReader(stream).ReadToEnd();
            return new Asn1UtcTime {
                Value = DateTimeOffset.ParseExact(val, new[] { "yyMMddHHmmssZ", "yyMMddHHmmsszzz" }, CultureInfo.InvariantCulture, DateTimeStyles.None)
            };
        }
    }
}
