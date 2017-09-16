/***********************************************************************
Copyright 2016 Stefan Hausotte

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*************************************************************************/

using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

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
                Callbacks.Items.Add(new {Callback = cb.Callback.ToHexString()});
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
