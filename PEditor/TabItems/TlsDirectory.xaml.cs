using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PeNet;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for TlsDirectory.xaml
    /// </summary>
    public partial class TlsDirectory : UserControl
    {
        public TlsDirectory()
        {
            InitializeComponent();
        }

        private void ClearTlsDirectory()
        {
            StartAddressOfRawData.Text = string.Empty;
            EndAddressOfRawData.Text = string.Empty;
            AddressOfCallBacks.Text = string.Empty;
            AddressOfCallBacks.Text = string.Empty;
            SizeOfZeroFill.Text = string.Empty;
            Characteristics.Text = string.Empty;
        }

        private void ClearTlsCallbacks()
        {
            Callbacks.Items.Clear();
        }

        private void SetCallbacks(PeFile peFile)
        {
            if(peFile.ImageTlsDirectory?.TlsCallbacks == null)
                return;

            foreach (var cb in peFile.ImageTlsDirectory.TlsCallbacks)
            {
                Callbacks.Items.Add(new {Callback = cb.Callback});
            }
        }

        public void SetTlsDirectory(PeFile peFile)
        {
            ClearTlsDirectory();
            ClearTlsCallbacks();

            if(peFile.ImageTlsDirectory == null)
                return;

            StartAddressOfRawData.Text = peFile.ImageTlsDirectory.StartAddressOfRawData.ToHexString();
            EndAddressOfRawData.Text = peFile.ImageTlsDirectory.EndAddressOfRawData.ToHexString();
            AddressOfIndex.Text = peFile.ImageTlsDirectory.AddressOfIndex.ToHexString();
            AddressOfCallBacks.Text = peFile.ImageTlsDirectory.AddressOfCallBacks.ToHexString();
            SizeOfZeroFill.Text = peFile.ImageTlsDirectory.SizeOfZeroFill.ToHexString();
            Characteristics.Text = peFile.ImageTlsDirectory.Characteristics.ToHexString();

            SetCallbacks(peFile);
        }
    }
}
