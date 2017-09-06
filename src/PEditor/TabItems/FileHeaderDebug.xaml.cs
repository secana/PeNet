using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for FileHeaderDebug.xaml
    /// </summary>
    public partial class FileHeader : UserControl
    {
        public FileHeader()
        {
            InitializeComponent();
        }

        public void SetFileHeader(PeFile peFile)
        {
            var fileHeader = peFile.ImageNtHeaders.FileHeader;
            var machine = fileHeader.Machine;
            var characteristics = fileHeader.Characteristics;

            tbMachine.Text = $"{machine.ToHexString()} <-> {PeNet.Utilities.FlagResolver.ResolveTargetMachine(machine)}";
            tbNumberOfSections.Text = fileHeader.NumberOfSections.ToHexString();
            tbTimeDateStamp.Text = fileHeader.TimeDateStamp.ToHexString();
            tbPointerToSymbolTable.Text = fileHeader.PointerToSymbolTable.ToHexString();
            tbNumberOfSymbols.Text = fileHeader.NumberOfSymbols.ToHexString();
            tbSizeOfOptionalHeader.Text = fileHeader.SizeOfOptionalHeader.ToHexString();
            tbCharacteristics.Text =
                $"{characteristics.ToHexString()}\n\n{PeNet.Utilities.FlagResolver.ResolveFileCharacteristics(characteristics)}";
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

            if(peFile.ImageDebugDirectory == null)
                return;

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
