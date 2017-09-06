using System.Linq;
using System.Windows.Controls;
using PeNet;
using PeNet.Utilities;

namespace PEditor.TabItems
{
    /// <summary>
    /// Interaction logic for Exceptions.xaml
    /// </summary>
    public partial class Exceptions : UserControl
    {
        private PeFile _peFile;

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
            var funcStart = selected.FunctionStart.ToIntFromHexString();
            var funcEnd = selected.FunctionEnd.ToIntFromHexString();
            var uw = selected.UnwindInfo.ToIntFromHexString();

            // Find the RUNTIME_FUNCTION which was selected.
            var rt = _peFile.RuntimeFunctions.First(x => x.FunctionStart == funcStart
                                                         && x.FunctionEnd == funcEnd
                                                         && x.UnwindInfo == uw
                );

            // Set the UNWIND_INFO properties.
            tbUIVersion.Text = rt.ResolvedUnwindInfo.Version.ToHexString();
            tbUIFlags.Text = rt.ResolvedUnwindInfo.Flags.ToHexString();
            tbUISizeOfProlog.Text = rt.ResolvedUnwindInfo.SizeOfProlog.ToHexString();
            tbUICountOfCodes.Text = rt.ResolvedUnwindInfo.CountOfCodes.ToHexString();
            tbUIFrameRegister.Text = rt.ResolvedUnwindInfo.FrameRegister.ToHexString();
            tbUIFrameOffset.Text = rt.ResolvedUnwindInfo.FrameOffset.ToHexString();
            tbUIExHandlerFuncEntry.Text = rt.ResolvedUnwindInfo.ExceptionHandler.ToHexString();
            // TODO: display exception data as a hex array.
            //tbUIExData.Text = string.Format("", rt.ResolvedUnwindInfo.ExceptionData);

            // Set the UNWIND_CODE structures for the UNWIND_INFO
            lbUnwindCode.Items.Clear();
            foreach (var uc in rt.ResolvedUnwindInfo.UnwindCode)
            {
                lbUnwindCode.Items.Add(new
                {
                    CodeOffset = uc.CodeOffset.ToHexString(),
                    UnwindOp = uc.UnwindOp.ToHexString(),
                    FrameOffset = uc.FrameOffset.ToHexString()
                });
            }
        }


        public void SetException(PeFile peFile)
        {
            _peFile = peFile;
            lbRuntimeFunctions.Items.Clear();

            if (peFile.Is32Bit || peFile.RuntimeFunctions == null)
                return;

            foreach (var rt in peFile.RuntimeFunctions)
            {
                lbRuntimeFunctions.Items.Add(new
                {
                    FunctionStart = rt.FunctionStart.ToHexString(),
                    FunctionEnd = rt.FunctionEnd.ToHexString(),
                    UnwindInfo = rt.UnwindInfo.ToHexString()
                });
            }
        }

    }
}
