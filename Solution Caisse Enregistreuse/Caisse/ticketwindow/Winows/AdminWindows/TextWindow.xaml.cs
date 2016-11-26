using System.Windows;

namespace TicketWindow.Winows.AdminWindows
{
    /// <summary>
    /// Interaction logic for TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public TextWindow(bool isRedact = false, string info = null)
        {
            InitializeComponent();
            BtnAdd.Content = isRedact ? Properties.Resources.BtnRedact : Properties.Resources.BtnAdd;
            if (info != null) InfoBox.Text = info;
        }

        public string NameText
        {
            get { return NameBox.Text; }
            set { NameBox.Text = value ?? string.Empty; }
        }

        public string BtnText
        {
            get { return BtnAdd.Content.ToString(); }
            set { BtnAdd.Content = value ?? string.Empty; }
        }

        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            NameBox.Focus();
        }
    }
}
