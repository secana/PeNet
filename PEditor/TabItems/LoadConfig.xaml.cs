using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for LoadConfig.xaml
    /// </summary>
    public partial class LoadConfig : UserControl
    {
        public LoadConfig()
        {
            InitializeComponent();
        }

        private void ClearLoadConfig()
        {
            Size.Text = string.Empty;
            TimeDateStamp.Text = string.Empty;
            MajorVersion.Text = string.Empty;
            MinorVersion.Text = string.Empty;
            GlobalFlagsClear.Text = string.Empty;
            GlobalFlagsSet.Text = string.Empty;
            CriticalSectionDefaultTimeout.Text = string.Empty;
            DeCommitTotalFreeThreshold.Text = string.Empty;
            DeCommitFreeBlockThreshold.Text = string.Empty;
            LockPrefixTable.Text = string.Empty;
            MaximumAllocationSize.Text = string.Empty;
            VirtualMemoryThreshold.Text = string.Empty;
            ProcessHeapFlags.Text = string.Empty;
            ProcessAffinityMask.Text = string.Empty;
            CSDVersion.Text = string.Empty;
            Reserved1.Text = string.Empty;
            EditList.Text = string.Empty;
            SecurityCookie.Text = string.Empty;
            SEHandlerTable.Text = string.Empty;
            SEHandlerCount.Text = string.Empty;
            GuardCFCheckFunctionPointer.Text = string.Empty;
            Reserved2.Text = string.Empty;
            GuardCFFunctionTable.Text = string.Empty;
            GuardCFFunctionCount.Text = string.Empty;
            GuardFlags.Text = string.Empty;
        }

        public void SetLoadConfig(PeFile peFile)
        {
            ClearLoadConfig();

            if(peFile.ImageLoadConfigDirectory == null)
                return;

            Size.Text = peFile.ImageLoadConfigDirectory.Size.ToHexString();
            TimeDateStamp.Text = peFile.ImageLoadConfigDirectory.TimeDateStamp.ToHexString();
            MajorVersion.Text = peFile.ImageLoadConfigDirectory.MajorVesion.ToHexString();
            MinorVersion.Text = peFile.ImageLoadConfigDirectory.MinorVersion.ToHexString();
            GlobalFlagsClear.Text = peFile.ImageLoadConfigDirectory.GlobalFlagsClear.ToHexString();
            GlobalFlagsSet.Text = peFile.ImageLoadConfigDirectory.GlobalFlagsSet.ToHexString();
            CriticalSectionDefaultTimeout.Text =
                peFile.ImageLoadConfigDirectory.CriticalSectionDefaultTimeout.ToHexString();
            DeCommitTotalFreeThreshold.Text = peFile.ImageLoadConfigDirectory.DeCommitTotalFreeThreshold.ToHexString();
            DeCommitFreeBlockThreshold.Text = peFile.ImageLoadConfigDirectory.DeCommitFreeBlockThreshold.ToHexString();
            LockPrefixTable.Text = peFile.ImageLoadConfigDirectory.LockPrefixTable.ToHexString();
            MaximumAllocationSize.Text = peFile.ImageLoadConfigDirectory.MaximumAllocationSize.ToHexString();
            VirtualMemoryThreshold.Text = peFile.ImageLoadConfigDirectory.VirtualMemoryThershold.ToHexString();
            ProcessHeapFlags.Text = peFile.ImageLoadConfigDirectory.ProcessHeapFlags.ToHexString();
            ProcessAffinityMask.Text = peFile.ImageLoadConfigDirectory.ProcessAffinityMask.ToHexString();
            CSDVersion.Text = peFile.ImageLoadConfigDirectory.CSDVersion.ToHexString();
            Reserved1.Text = peFile.ImageLoadConfigDirectory.Reserved1.ToHexString();
            EditList.Text = peFile.ImageLoadConfigDirectory.EditList.ToHexString();
            SecurityCookie.Text = peFile.ImageLoadConfigDirectory.SecurityCoockie.ToHexString();
            SEHandlerTable.Text = peFile.ImageLoadConfigDirectory.SEHandlerTable.ToHexString();
            SEHandlerCount.Text = peFile.ImageLoadConfigDirectory.SEHandlerCount.ToHexString();
            GuardCFCheckFunctionPointer.Text = peFile.ImageLoadConfigDirectory.GuardCFCheckFunctionPointer.ToHexString();
            Reserved2.Text = peFile.ImageLoadConfigDirectory.Reserved2.ToHexString();
            GuardCFFunctionTable.Text = peFile.ImageLoadConfigDirectory.GuardCFFunctionTable.ToHexString();
            GuardCFFunctionCount.Text = peFile.ImageLoadConfigDirectory.GuardCFFunctionCount.ToHexString();
            GuardFlags.Text = peFile.ImageLoadConfigDirectory.GuardFlags.ToHexString();
        }
    }
}
