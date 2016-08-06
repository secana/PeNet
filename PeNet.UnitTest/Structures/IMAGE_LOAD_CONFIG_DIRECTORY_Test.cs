using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeNet.Structures;

namespace PeNet.UnitTest.Structures
{
    [TestClass]
    public class IMAGE_LOAD_CONFIG_DIRECTORY_Test
    {
        [TestMethod]
        public void ImageLoadConfigDirectory32ConstructorWorks_Test()
        {
            var imageLoadConfigDirectory = new IMAGE_LOAD_CONFIG_DIRECTORY(
                RawStructures.RawImageLoadConfigDirectory32,
                2, 
                false
                );

            Assert.AreEqual((uint) 0x33221100, imageLoadConfigDirectory.Size);
            Assert.AreEqual((uint) 0x77665544, imageLoadConfigDirectory.TimeDateStamp);
            Assert.AreEqual((ushort) 0x9988, imageLoadConfigDirectory.MajorVesion);
            Assert.AreEqual((ushort) 0xbbaa, imageLoadConfigDirectory.MinorVersion);
            Assert.AreEqual((uint) 0xffeeddcc, imageLoadConfigDirectory.GlobalFlagsClear);
            Assert.AreEqual((uint) 0x44332211, imageLoadConfigDirectory.GlobalFlagsSet);
            Assert.AreEqual((uint) 0x88776655, imageLoadConfigDirectory.CriticalSectionDefaultTimeout);
            Assert.AreEqual((uint) 0xccbbaa99, imageLoadConfigDirectory.DeCommitFreeBlockThreshold);
            Assert.AreEqual((uint) 0x00ffeedd, imageLoadConfigDirectory.DeCommitTotalFreeThreshold);
            Assert.AreEqual((uint) 0x55443322, imageLoadConfigDirectory.LockPrefixTable);
            Assert.AreEqual((uint) 0x99887766, imageLoadConfigDirectory.MaximumAllocationSize);
            Assert.AreEqual((uint) 0xddccbbaa, imageLoadConfigDirectory.VirtualMemoryThershold);
            Assert.AreEqual((uint) 0x221100ff, imageLoadConfigDirectory.ProcessHeapFlags);
            Assert.AreEqual((uint) 0x66554433, imageLoadConfigDirectory.ProcessAffinityMask);
            Assert.AreEqual((ushort) 0x8877, imageLoadConfigDirectory.CSDVersion);
            Assert.AreEqual((ushort) 0xaa99, imageLoadConfigDirectory.Reserved1);
            Assert.AreEqual((uint) 0xffddccbb, imageLoadConfigDirectory.EditList);
            Assert.AreEqual((uint) 0x77665544, imageLoadConfigDirectory.SecurityCoockie);
            Assert.AreEqual((uint) 0xbbaa9988, imageLoadConfigDirectory.SEHandlerTable);
            Assert.AreEqual((uint) 0xffeeddcc, imageLoadConfigDirectory.SEHandlerCount);
            Assert.AreEqual((uint) 0x88776655, imageLoadConfigDirectory.GuardCFCheckFunctionPointer);
            Assert.AreEqual((uint) 0xccbbaa99, imageLoadConfigDirectory.Reserved2);
            Assert.AreEqual((uint) 0x00ffeedd, imageLoadConfigDirectory.GuardCFFunctionTable);
            Assert.AreEqual((uint) 0x99887766, imageLoadConfigDirectory.GuardCFFunctionCount);
            Assert.AreEqual((uint) 0xddccbbaa, imageLoadConfigDirectory.GuardFlags);
        }

        [TestMethod]
        public void ImageLoadConfigDirectory64ConstructorWorks_Test()
        {
            var imageLoadConfigDirectory = new IMAGE_LOAD_CONFIG_DIRECTORY(
                RawStructures.RawImageLoadConfigDirectory64,
                2,
                true
                );

            Assert.AreEqual((uint)0x33221100, imageLoadConfigDirectory.Size);
            Assert.AreEqual((uint)0x77665544, imageLoadConfigDirectory.TimeDateStamp);
            Assert.AreEqual((ushort)0x9988, imageLoadConfigDirectory.MajorVesion);
            Assert.AreEqual((ushort)0xbbaa, imageLoadConfigDirectory.MinorVersion);
            Assert.AreEqual((uint)0xffeeddcc, imageLoadConfigDirectory.GlobalFlagsClear);
            Assert.AreEqual((uint)0x44332211, imageLoadConfigDirectory.GlobalFlagsSet);
            Assert.AreEqual((uint)0x88776655, imageLoadConfigDirectory.CriticalSectionDefaultTimeout);
            Assert.AreEqual((ulong)0xddccbbaaccbbaa99, imageLoadConfigDirectory.DeCommitFreeBlockThreshold);
            Assert.AreEqual((ulong)0xddccbbaa00ffeedd, imageLoadConfigDirectory.DeCommitTotalFreeThreshold);
            Assert.AreEqual((ulong)0xddccbbaa55443322, imageLoadConfigDirectory.LockPrefixTable);
            Assert.AreEqual((ulong)0xddccbbaa99887766, imageLoadConfigDirectory.MaximumAllocationSize);
            Assert.AreEqual((ulong)0xddccbbaaddccbbaa, imageLoadConfigDirectory.VirtualMemoryThershold);
            Assert.AreEqual((ulong)0xddccbbaa66554433, imageLoadConfigDirectory.ProcessAffinityMask);
            Assert.AreEqual((uint)0x221100ff, imageLoadConfigDirectory.ProcessHeapFlags);
            Assert.AreEqual((ushort)0x8877, imageLoadConfigDirectory.CSDVersion);
            Assert.AreEqual((ushort)0xaa99, imageLoadConfigDirectory.Reserved1);
            Assert.AreEqual((ulong)0xddccbbaaffddccbb, imageLoadConfigDirectory.EditList);
            Assert.AreEqual((ulong)0xddccbbaa77665544, imageLoadConfigDirectory.SecurityCoockie);
            Assert.AreEqual((ulong)0xddccbbaabbaa9988, imageLoadConfigDirectory.SEHandlerTable);
            Assert.AreEqual((ulong)0xddccbbaaffeeddcc, imageLoadConfigDirectory.SEHandlerCount);
            Assert.AreEqual((ulong)0xddccbbaa88776655, imageLoadConfigDirectory.GuardCFCheckFunctionPointer);
            Assert.AreEqual((ulong)0xddccbbaaccbbaa99, imageLoadConfigDirectory.Reserved2);
            Assert.AreEqual((ulong)0xddccbbaa00ffeedd, imageLoadConfigDirectory.GuardCFFunctionTable);
            Assert.AreEqual((ulong)0xddccbbaa99887766, imageLoadConfigDirectory.GuardCFFunctionCount);
            Assert.AreEqual((uint)0xddccbbaa, imageLoadConfigDirectory.GuardFlags);
        }
    }
}