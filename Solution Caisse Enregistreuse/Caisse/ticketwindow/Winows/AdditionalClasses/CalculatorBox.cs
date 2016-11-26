using System.Windows.Controls;
using System.Windows.Input;

namespace TicketWindow.Winows.AdditionalClasses
{
    internal class CalculatorBox : TextBox
    {
        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            e.Handled = true;
            base.OnPreviewGotKeyboardFocus(e);
        }
    }
}