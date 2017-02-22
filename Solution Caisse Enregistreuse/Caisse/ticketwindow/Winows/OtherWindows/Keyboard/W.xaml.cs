using System.Windows;
using System.Windows.Input;

namespace TicketWindow.Winows.OtherWindows.Keyboard
{
    /// <summary>
    /// Логика взаимодействия для W.xaml
    /// </summary>
    public partial class W : Window
    {
        public W()
        {
            InitializeComponent();
        }
      
        private void WKeyboardMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();

            Focusable = false;
        }
    }
}
