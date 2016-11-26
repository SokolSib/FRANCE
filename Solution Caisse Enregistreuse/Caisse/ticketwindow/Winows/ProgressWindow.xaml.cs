using System.Windows;

namespace TicketWindow.Winows
{
    /// <summary>
    ///     Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private int _current;

        public ProgressWindow(int count, string name = null)
        {
            InitializeComponent();
            Count = count;
            Current = 0;
            BoxProgress.Maximum = 100;
            TitleText = name;
            Description = TitleText;
        }

        private string TitleText { get; set; }

        public int Count { get; set; }

        public int Current
        {
            get { return _current; }
            set
            {
                _current = value;
                BoxProgress.Value = 100*_current/Count;
                BoxText.Text = string.Format("{0} {1} {2} ({3} {4} {5} %)", 
                    _current, 
                    Properties.Resources.LabelFrom.ToLower(),
                    Count,
                    BoxProgress.Value,
                    Properties.Resources.LabelFrom.ToLower(),
                    BoxProgress.Maximum);
            }
        }

        public string Description
        {
            get { return BoxName.Text; }
            set { BoxName.Text = value == null ? TitleText : string.Format("{0} - {1}", TitleText, value); }
        }
    }
}