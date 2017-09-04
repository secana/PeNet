using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for SectionHeaders.xaml
    /// </summary>
    public partial class SectionHeaders : UserControl
    {
        public SectionHeaders()
        {
            InitializeComponent();
        }

        private void CleanSections()
        {
            dgSections.Items.Clear();
        }

        public void SetSections(PeFile peFile)
        {
            CleanSections();

            var num = 1;
            foreach (var sec in peFile.ImageSectionHeaders)
            {
                var flags = string.Join(", ", PeNet.Utilities.FlagResolver.ResolveSectionFlags(sec.Characteristics));
                dgSections.Items.Add(new
                {
                    Number = num,
                    Name = PeNet.Utilities.FlagResolver.ResolveSectionName(sec.Name),
                    VSize = sec.VirtualSize.ToHexString(),
                    VAddress = sec.VirtualAddress.ToHexString(),
                    PSize = sec.SizeOfRawData.ToHexString(),
                    PAddress = sec.PhysicalAddress.ToHexString(),
                    Flags = sec.Characteristics.ToHexString(),
                    RFlags = flags
                });
                num++;
            }
        }
    }
}
