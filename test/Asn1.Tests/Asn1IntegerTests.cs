using System.IO;
using Xunit;

namespace Asn1.Tests {
    public class Asn1IntegerTests : BaseTest {

        private static readonly byte[] _etalon = { 0x02, 0x01, 0x00 };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1Integer;
            Assert.NotNull(typed);
            Assert.Equal(new byte[] { 0 }, typed.Value);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1Integer(0);
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
    }
}
