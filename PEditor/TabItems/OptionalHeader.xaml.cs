using System.Windows.Controls;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for OptionalHeader.xaml
    /// </summary>
    public partial class OptionalHeader : UserControl
    {
        public OptionalHeader()
        {
            InitializeComponent();
        }

        public void SetOptionalHeader(PeFile peFile)
        {
            var oh = peFile.ImageNtHeaders.OptionalHeader;

            tbMagic.Text = oh.Magic.ToHexString();
            tbMajorLinkerVersion.Text = Utility.ToHexString(oh.MajorLinkerVersion);
            tbMinorLinkerVersion.Text = Utility.ToHexString(oh.MinorLinkerVersion);
            tbSizeOfCode.Text = oh.SizeOfCode.ToHexString();
            tbSizeOfInitializedData.Text = oh.SizeOfInitializedData.ToHexString();
            tbSizeOfUninitializedData.Text = oh.SizeOfUninitializedData.ToHexString();
            tbAddressOfEntryPoint.Text = oh.AddressOfEntryPoint.ToHexString();
            tbBaseOfCode.Text = oh.BaseOfCode.ToHexString();
            tbBaseOfData.Text = oh.BaseOfData.ToHexString();
            tbImageBase.Text = oh.ImageBase.ToHexString();
            tbSectionAlignment.Text = oh.SectionAlignment.ToHexString();
            tbFileAlignment.Text = oh.FileAlignment.ToHexString();
            tbMajorOperatingSystemVersion.Text = oh.MajorOperatingSystemVersion.ToHexString();
            tbMinorOperatingSystemVersion.Text = oh.MinorOperatingSystemVersion.ToHexString();
            tbMajorImageVersion.Text = oh.MajorImageVersion.ToHexString();
            tbMinorImageVersion.Text = oh.MinorImageVersion.ToHexString();
            tbMajorSubsystemVersion.Text = oh.MajorSubsystemVersion.ToHexString();
            tbMinorSubsystemVersion.Text = oh.MinorSubsystemVersion.ToHexString();
            tbWin32VersionValue.Text = oh.Win32VersionValue.ToHexString();
            tbSizeOfImage.Text = oh.SizeOfImage.ToHexString();
            tbSizeOfHeaders.Text = oh.SizeOfHeaders.ToHexString();
            tbCheckSum.Text = oh.CheckSum.ToHexString();
            tbSubsystem.Text = oh.Subsystem.ToHexString();
            tbDllCharacteristics.Text = oh.DllCharacteristics.ToHexString();
            tbSizeOfStackReserve.Text = oh.SizeOfStackReserve.ToHexString();
            tbSizeOfStackCommit.Text = oh.SizeOfStackCommit.ToHexString();
            tbSizeOfHeapReserve.Text = oh.SizeOfHeapReserve.ToHexString();
            tbSizeOfHeapCommit.Text = oh.SizeOfHeapCommit.ToHexString();
            tbLoaderFlags.Text = oh.LoaderFlags.ToHexString();
            tbNumberOfRvaAndSizes.Text = oh.NumberOfRvaAndSizes.ToHexString();
        }

    }
}
