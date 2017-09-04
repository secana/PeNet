using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for DebugBoundImport.xaml
    /// </summary>
    public partial class DebugBoundImport : UserControl
    {
        public DebugBoundImport()
        {
            InitializeComponent();
        }

        public void SetBoundImport(PeFile peFile)
        {
            // Clean
            tbBoundImportNumberOfModuleForwarderRefs.Text = string.Empty;
            tbBoundImportOffsetModuleName.Text = string.Empty;
            tbBoundImportTimeDateStamp.Text = string.Empty;

            if (peFile.ImageBoundImportDescriptor == null)
                return;

            // Set
            tbBoundImportNumberOfModuleForwarderRefs.Text =
                peFile.ImageBoundImportDescriptor.NumberOfModuleForwarderRefs.ToHexString();
            tbBoundImportOffsetModuleName.Text = peFile.ImageBoundImportDescriptor.OffsetModuleName.ToHexString();
            tbBoundImportTimeDateStamp.Text = peFile.ImageBoundImportDescriptor.TimeDateStamp.ToHexString();
        }

        public void SetDelayImport(PeFile peFile)
        {
            // Clean
            grAttr.Text = string.Empty;
            szName.Text = string.Empty;
            phmod.Text = string.Empty;
            pIAT.Text = string.Empty;
            pINT.Text = string.Empty;
            pBoundIAT.Text = string.Empty;
            pUnloadIAT.Text = string.Empty;
            dwTimeStamp.Text = string.Empty;

            if (peFile.ImageDelayImportDescriptor == null)
                return;

            // Set
            grAttr.Text = peFile.ImageDelayImportDescriptor.grAttrs.ToHexString();
            szName.Text = peFile.ImageDelayImportDescriptor.szName.ToHexString();
            phmod.Text = peFile.ImageDelayImportDescriptor.phmod.ToHexString();
            pIAT.Text = peFile.ImageDelayImportDescriptor.pIAT.ToHexString();
            pINT.Text = peFile.ImageDelayImportDescriptor.pINT.ToHexString();
            pBoundIAT.Text = peFile.ImageDelayImportDescriptor.pBoundIAT.ToHexString();
            pUnloadIAT.Text = peFile.ImageDelayImportDescriptor.pUnloadIAT.ToHexString();
            dwTimeStamp.Text = peFile.ImageDelayImportDescriptor.dwTimeStamp.ToHexString();

        }
    }
}
