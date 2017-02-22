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
    /// Логика взаимодействия для W_Pro_add.xaml
    /// </summary>
    public partial class W_Pro_add : Window
    {
        public W_Pro_add()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "Save")
            {
                string error = "";

                foreach (TextBox bs in ClassETC_fun.FindVisualChildren<TextBox>(this))
                {
                    if (!Valid(bs)) error += (bs.Name + " incorrect") + Environment.NewLine;
                }

                if (error.Length == 0)

                    new ClassFunctuon().Click(sender);

                else new ClassFunctuon().showMessageTimeList(error);
            }
            else
            {
                new ClassFunctuon().Click(sender);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }
        }


        private bool Valid (object sender)
        {

            switch (((TextBox)sender).Name)
            {
                case "xNameCompany": return ((TextBox)sender).Text.Length > 3;
                case "xMail": return ((TextBox)sender).Text.Length > 5;
                case "xTel": return ((TextBox)sender).Text.Length >= 0;
                case "xCodePostal": return ((TextBox)sender).Text.Length >= 0;
                case "xVille":return ((TextBox)sender).Text.Length >= 0;
                case "xAdress": return ((TextBox)sender).Text.Length >= 0;

            }

            return false;
        }

        private void _LostFocus(object sender, RoutedEventArgs e)
        {
            //(sender as TextBox).Background =  Valid(sender);
        }
    }
}
