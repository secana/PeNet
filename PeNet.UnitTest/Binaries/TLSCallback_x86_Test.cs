using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeNet.UnitTest.Binaries
{
    [TestClass]
    public class TLSCallback_x86_Test
    {
        [TestMethod]
        public void TLSCallback_x86_Works_Test()
        {
            // Given
            var peFile = new PeFile(@"../../Binaries/TLSCallback_x86.exe");

            // When
            var path = peFile.FileLocation;
            var callbacks = peFile.ImageTlsDirectory.TlsCallbacks;

            // Then
            Assert.AreEqual(1, callbacks.Length);
            Assert.AreEqual((ulong) 0x004111CC, callbacks.First().Callback);
        }
    }
}