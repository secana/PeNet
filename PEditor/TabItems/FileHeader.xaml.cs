using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for FileHeader.xaml
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

            tbMachine.Text = $"{machine.ToHexString()} <-> {Utility.ResolveTargetMachine(machine)}";
            tbNumberOfSections.Text = fileHeader.NumberOfSections.ToHexString();
            tbTimeDateStamp.Text = fileHeader.TimeDateStamp.ToHexString();
            tbPointerToSymbolTable.Text = fileHeader.PointerToSymbolTable.ToHexString();
            tbNumberOfSymbols.Text = fileHeader.NumberOfSymbols.ToHexString();
            tbSizeOfOptionalHeader.Text = fileHeader.SizeOfOptionalHeader.ToHexString();
            tbCharacteristics.Text =
                $"{characteristics.ToHexString()}\n\n{Utility.ResolveFileCharacteristics(characteristics)}";
        }
    }
}
