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
using ticketwindow.Class;

namespace ticketwindow.Winows.Pro
{
    /// <summary>
    /// Логика взаимодействия для W_ProList.xaml
    /// </summary>
    public partial class W_ProList : Window
    {
        public W_ProList()
        {
            InitializeComponent();
        }

       

        public void dataPro_Loaded(object sender, RoutedEventArgs e)
        {
           
            dataPro.ItemsSource =  ClassSync.classPro.get();

            unset.IsEnabled = ClassProMode.modePro;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            new ClassFunctuon().Click(sender);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }
        }
    }
}
