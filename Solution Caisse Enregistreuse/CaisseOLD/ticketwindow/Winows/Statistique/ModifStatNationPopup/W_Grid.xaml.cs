using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ticketwindow.Winows.Statistique.ModifStatNationPopup
{
    /// <summary>
    /// Логика взаимодействия для W_Grid.xaml
    /// </summary>
    public partial class W_Grid : Window
    {
        public W_Grid()
        {
            InitializeComponent();

           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = (this.Owner as W_Stat).A;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Class.ClassFunctuon().Click(sender, (this.Owner as W_Stat).A);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Class.ClassFunctuon().Click(sender, (this.Owner as W_Stat).A, this.dataGrid.SelectedItem);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
