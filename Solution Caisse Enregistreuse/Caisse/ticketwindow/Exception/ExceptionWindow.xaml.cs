using System.Windows;

namespace TicketWindow.Exception
{
    /// <summary>
    ///     Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        public ExceptionWindow(System.Exception exception)
        {
            InitializeComponent();

            Exception = exception;
            ExceptionBox.Text = exception.ToString();
        }

        public System.Exception Exception { get; private set; }
    }
}