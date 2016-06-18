using System.Windows.Controls;
using PeNet;

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

        public void SetSections(PeFile peFile)
        {
            var num = 1;
            foreach (var sec in peFile.ImageSectionHeaders)
            {
                var flags = string.Join(", ", Utility.ResolveSectionFlags(sec.Characteristics));
                dgSections.Items.Add(new
                {
                    Number = num,
                    Name = Utility.ResolveSectionName(sec.Name),
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
