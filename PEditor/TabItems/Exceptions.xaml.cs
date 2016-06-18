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
    /// Interaction logic for Exceptions.xaml
    /// </summary>
    public partial class Exceptions : UserControl
    {
        public Exceptions()
        {
            InitializeComponent();
        }

        private void lbRuntimeFunctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;
            dynamic selected = listBox.SelectedItem;
            if (selected == null) return;

            // Convert string of format 0x... to an integer.
            var funcStart = Utility.ToIntFromHexString(selected.FunctionStart);
            var funcEnd = Utility.ToIntFromHexString(selected.FunctionEnd);
            var uw = Utility.ToIntFromHexString(selected.UnwindInfo);

            // Find the RUNTIME_FUNCTION which was selected.
            var rt = MainWindow.PeFile.RuntimeFunctions.First(x => x.FunctionStart == funcStart
                                                         && x.FunctionEnd == funcEnd
                                                         && x.UnwindInfo == uw
                );

            // Set the UNWIND_INFO properties.
            tbUIVersion.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.Version);
            tbUIFlags.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.Flags);
            tbUISizeOfProlog.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.SizeOfProlog);
            tbUICountOfCodes.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.CountOfCodes);
            tbUIFrameRegister.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.FrameRegister);
            tbUIFrameOffset.Text = Utility.ToHexString(rt.ResolvedUnwindInfo.FrameOffset);
            tbUIExHandlerFuncEntry.Text = rt.ResolvedUnwindInfo.ExceptionHandler.ToHexString();
            // TODO: display excetption data as a hex array.
            //tbUIExData.Text = string.Format("", rt.ResolvedUnwindInfo.ExceptionData);

            // Set the UNWIND_CODE structures for the UNWIND_INFO
            lbUnwindCode.Items.Clear();
            foreach (var uc in rt.ResolvedUnwindInfo.UnwindCode)
            {
                lbUnwindCode.Items.Add(new
                {
                    CodeOffset = Utility.ToHexString(uc.CodeOffset),
                    UnwindOp = Utility.ToHexString(uc.UnwindOp),
                    FrameOffset = uc.FrameOffset.ToHexString()
                });
            }
        }
    }
}
