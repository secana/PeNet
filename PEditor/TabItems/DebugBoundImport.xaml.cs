using System.Windows.Controls;
using PeNet;

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

        public void SetDebug(PeFile peFile)
        {
            // Clean
            tbDebugCharacteristics.Text = string.Empty;
            tbDebugTimeDateStamp.Text = string.Empty;
            tbDebugMajorVersion.Text = string.Empty;
            tbDebugMinorVersion.Text = string.Empty;
            tbDebugType.Text = string.Empty;
            tbDebugSizeOfData.Text = string.Empty;
            tbDebugAddressOfRawData.Text = string.Empty;
            tbDebugPointerToRawData.Text = string.Empty;

            // Set
            tbDebugCharacteristics.Text = peFile.ImageDebugDirectory.Characteristics.ToHexString();
            tbDebugTimeDateStamp.Text = peFile.ImageDebugDirectory.TimeDateStamp.ToHexString();
            tbDebugMajorVersion.Text = peFile.ImageDebugDirectory.MajorVersion.ToHexString();
            tbDebugMinorVersion.Text = peFile.ImageDebugDirectory.MinorVersion.ToHexString();
            tbDebugType.Text = peFile.ImageDebugDirectory.Type.ToHexString();
            tbDebugSizeOfData.Text = peFile.ImageDebugDirectory.SizeOfData.ToHexString();
            tbDebugAddressOfRawData.Text = peFile.ImageDebugDirectory.AddressOfRawData.ToHexString();
            tbDebugPointerToRawData.Text = peFile.ImageDebugDirectory.PointerToRawData.ToHexString();
        }
    }
}
