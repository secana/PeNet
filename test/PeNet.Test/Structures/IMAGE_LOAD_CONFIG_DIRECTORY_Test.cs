using PeNet.Structures;
using Xunit;

namespace PeNet.Test.Structures
{
    
    public class IMAGE_LOAD_CONFIG_DIRECTORY_Test
    {
        [Fact]
        public void ImageLoadConfigDirectory32ConstructorWorks_Test()
        {
            var imageLoadConfigDirectory = new IMAGE_LOAD_CONFIG_DIRECTORY(
                RawStructures.RawImageLoadConfigDirectory32,
                2, 
                false
                );

            Assert.Equal((uint) 0x33221100, imageLoadConfigDirectory.Size);
            Assert.Equal((uint) 0x77665544, imageLoadConfigDirectory.TimeDateStamp);
            Assert.Equal((ushort) 0x9988, imageLoadConfigDirectory.MajorVesion);
            Assert.Equal((ushort) 0xbbaa, imageLoadConfigDirectory.MinorVersion);
            Assert.Equal((uint) 0xffeeddcc, imageLoadConfigDirectory.GlobalFlagsClear);
            Assert.Equal((uint) 0x44332211, imageLoadConfigDirectory.GlobalFlagsSet);
            Assert.Equal((uint) 0x88776655, imageLoadConfigDirectory.CriticalSectionDefaultTimeout);
            Assert.Equal((uint) 0xccbbaa99, imageLoadConfigDirectory.DeCommitFreeBlockThreshold);
            Assert.Equal((uint) 0x00ffeedd, imageLoadConfigDirectory.DeCommitTotalFreeThreshold);
            Assert.Equal((uint) 0x55443322, imageLoadConfigDirectory.LockPrefixTable);
            Assert.Equal((uint) 0x99887766, imageLoadConfigDirectory.MaximumAllocationSize);
            Assert.Equal((uint) 0xddccbbaa, imageLoadConfigDirectory.VirtualMemoryThershold);
            Assert.Equal((uint) 0x221100ff, imageLoadConfigDirectory.ProcessHeapFlags);
            Assert.Equal((uint) 0x66554433, imageLoadConfigDirectory.ProcessAffinityMask);
            Assert.Equal((ushort) 0x8877, imageLoadConfigDirectory.CSDVersion);
            Assert.Equal((ushort) 0xaa99, imageLoadConfigDirectory.Reserved1);
            Assert.Equal((uint) 0xffddccbb, imageLoadConfigDirectory.EditList);
            Assert.Equal((uint) 0x77665544, imageLoadConfigDirectory.SecurityCoockie);
            Assert.Equal((uint) 0xbbaa9988, imageLoadConfigDirectory.SEHandlerTable);
            Assert.Equal((uint) 0xffeeddcc, imageLoadConfigDirectory.SEHandlerCount);
            Assert.Equal((uint) 0x88776655, imageLoadConfigDirectory.GuardCFCheckFunctionPointer);
            Assert.Equal((uint) 0xccbbaa99, imageLoadConfigDirectory.Reserved2);
            Assert.Equal((uint) 0x00ffeedd, imageLoadConfigDirectory.GuardCFFunctionTable);
            Assert.Equal((uint) 0x99887766, imageLoadConfigDirectory.GuardCFFunctionCount);
            Assert.Equal((uint) 0xddccbbaa, imageLoadConfigDirectory.GuardFlags);
        }

        [Fact]
        public void ImageLoadConfigDirectory64ConstructorWorks_Test()
        {
            var imageLoadConfigDirectory = new IMAGE_LOAD_CONFIG_DIRECTORY(
                RawStructures.RawImageLoadConfigDirectory64,
                2,
                true
                );

            Assert.Equal((uint)0x33221100, imageLoadConfigDirectory.Size);
            Assert.Equal((uint)0x77665544, imageLoadConfigDirectory.TimeDateStamp);
            Assert.Equal((ushort)0x9988, imageLoadConfigDirectory.MajorVesion);
            Assert.Equal((ushort)0xbbaa, imageLoadConfigDirectory.MinorVersion);
            Assert.Equal((uint)0xffeeddcc, imageLoadConfigDirectory.GlobalFlagsClear);
            Assert.Equal((uint)0x44332211, imageLoadConfigDirectory.GlobalFlagsSet);
            Assert.Equal((uint)0x88776655, imageLoadConfigDirectory.CriticalSectionDefaultTimeout);
            Assert.Equal((ulong)0xddccbbaaccbbaa99, imageLoadConfigDirectory.DeCommitFreeBlockThreshold);
            Assert.Equal((ulong)0xddccbbaa00ffeedd, imageLoadConfigDirectory.DeCommitTotalFreeThreshold);
            Assert.Equal((ulong)0xddccbbaa55443322, imageLoadConfigDirectory.LockPrefixTable);
            Assert.Equal((ulong)0xddccbbaa99887766, imageLoadConfigDirectory.MaximumAllocationSize);
            Assert.Equal((ulong)0xddccbbaaddccbbaa, imageLoadConfigDirectory.VirtualMemoryThershold);
            Assert.Equal((ulong)0xddccbbaa66554433, imageLoadConfigDirectory.ProcessAffinityMask);
            Assert.Equal((uint)0x221100ff, imageLoadConfigDirectory.ProcessHeapFlags);
            Assert.Equal((ushort)0x8877, imageLoadConfigDirectory.CSDVersion);
            Assert.Equal((ushort)0xaa99, imageLoadConfigDirectory.Reserved1);
            Assert.Equal((ulong)0xddccbbaaffddccbb, imageLoadConfigDirectory.EditList);
            Assert.Equal((ulong)0xddccbbaa77665544, imageLoadConfigDirectory.SecurityCoockie);
            Assert.Equal((ulong)0xddccbbaabbaa9988, imageLoadConfigDirectory.SEHandlerTable);
            Assert.Equal((ulong)0xddccbbaaffeeddcc, imageLoadConfigDirectory.SEHandlerCount);
            Assert.Equal((ulong)0xddccbbaa88776655, imageLoadConfigDirectory.GuardCFCheckFunctionPointer);
            Assert.Equal((ulong)0xddccbbaaccbbaa99, imageLoadConfigDirectory.Reserved2);
            Assert.Equal((ulong)0xddccbbaa00ffeedd, imageLoadConfigDirectory.GuardCFFunctionTable);
            Assert.Equal((ulong)0xddccbbaa99887766, imageLoadConfigDirectory.GuardCFFunctionCount);
            Assert.Equal((uint)0xddccbbaa, imageLoadConfigDirectory.GuardFlags);
        }
    }
}