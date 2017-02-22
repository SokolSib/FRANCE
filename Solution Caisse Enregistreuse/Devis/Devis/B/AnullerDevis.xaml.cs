using System.Windows;

namespace Devis.B
{
    /// <summary>
    /// Логика взаимодействия для AnullerDevis.xaml
    /// </summary>
    public partial class AnullerDevis : Window
    {

        public object data { get; set; }

        public AnullerDevis()
        {
            InitializeComponent();
        }

        private void bok_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassProducts.clrElm( (System.Xml.Linq.XElement[] ) data );
            Close();
        }
    }
}
