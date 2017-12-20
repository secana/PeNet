using System.IO;
using Xunit;

namespace Asn1.Tests {
    public class Asn1PrintableStringTests : BaseTest {

        private static readonly byte[] _etalon = { 0x13, 0x07, 0x54, 0x65, 0x73, 0x74, 0x4f, 0x72, 0x67 };

        [Fact]
        public void ReadTest() {
            var node = Asn1Node.ReadNode(new MemoryStream(_etalon));
            var typed = node as Asn1PrintableString;
            Assert.NotNull(typed);
            Assert.Equal("TestOrg", typed.Value);
        }

        [Fact]
        public void WriteTest() {
            var node = new Asn1PrintableString { Value = "TestOrg" };
            var data = node.GetBytes();
            AreEqual(_etalon, data);
        }
    }
}
