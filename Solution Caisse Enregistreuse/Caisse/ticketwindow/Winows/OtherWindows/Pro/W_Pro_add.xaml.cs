using System;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Winows.OtherWindows.Pro
{
    /// <summary>
    /// Логика взаимодействия для W_Pro_add.xaml
    /// </summary>
    public partial class WProAdd : Window
    {
        public WProAdd()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name == "Save")
            {
                string error = "";

                foreach (TextBox bs in ClassEtcFun.FindVisualChildren<TextBox>(this))
                    if (!Valid(bs)) error += (bs.Name + " incorrect") + Environment.NewLine;

                if (error.Length == 0)
                    FunctionsService.Click(sender);
                else FunctionsService.ShowMessageTimeList(error);
            }
            else
                FunctionsService.Click(sender);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;
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
    }
}
