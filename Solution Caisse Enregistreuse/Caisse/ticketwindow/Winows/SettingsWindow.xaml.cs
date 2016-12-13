using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TicketWindow.Class;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;

namespace TicketWindow.Winows
{
    /// <summary>
    ///     Логика взаимодействия для W_edit.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly List<RadioButton> _buttons;

        public SettingsWindow(int x, int y)
        {
            InitializeComponent();

            xName.Content = X = x;
            yName.Content = Y = y;

            cb.ItemsSource = RepositoryTypePay.TypePays;
            //DataGrid.ItemsSource = RepositoryProduct.Products;

            _buttons = new List<RadioButton>();
            _buttons.AddRange(PanelA.Children.Cast<RadioButton>());
            _buttons.AddRange(PanelB.Children.Cast<RadioButton>());

            HideOnlyServerButtons();
            HideRolesButtons();
        }

        public string Sub { get; set; }
        public int X { get; }
        public int Y { get; }

        private void HideOnlyServerButtons()
        {
            if (!SyncData.IsConnect)
            {
                var hideTags = new List<string> {"Show Pro", "toDevis", "UpdateDb"};

                foreach (var btn in _buttons.Where(b => hideTags.Contains(b.Tag.ToString())))
                    btn.Visibility = Visibility.Collapsed;
            }
        }

        private void HideRolesButtons()
        {
            if (!RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.WriteOff))
            {
                var hideTags = new List<string> { "WriteOff" };

                foreach (var btn in _buttons.Where(b => hideTags.Contains(b.Tag.ToString())))
                    btn.Visibility = Visibility.Collapsed;
            }
        }

        public string GetSelected(GroupBox grpBox)
        {
            var res = "None - Vide";

            foreach (var btn in _buttons.Where(btn => btn.IsChecked == true))
            {
                cb.SelectedItem = null;
                res = btn.Tag.ToString();
            }

            if (cb.SelectedItem != null)
                res = "_TypesPayDynamic" + ((TypePay) cb.SelectedItem).Id;

            if (FindProduct.Product != null)
                res = "Products id=[" + FindProduct.Product.CustomerId + "]";

            return res;
        }

        public void SetSelected(GroupBox grpBox, string textSelector)
        {
            var flag = false;

            foreach (var btn in _buttons)
            {
                btn.IsChecked = false;
                if (btn.Tag.ToString() == textSelector)
                {
                    btn.IsChecked = true;
                    flag = true;
                }
            }

            if (!flag && (textSelector.Substring(0, textSelector.Length > 15 ? 15 : 0) == "_TypesPayDynamic"))
            {
                int indx;
                if (int.TryParse(textSelector.Replace("_TypesPayDynamic", string.Empty), out indx))
                    cb.SelectedItem = RepositoryTypePay.GetById(indx);
            }

            if (!flag && (textSelector.Substring(0, textSelector.Length > 13 ? 13 : 0) == "Products id=["))
            {
                var sd = textSelector.Substring(
                    textSelector.IndexOf("[", StringComparison.Ordinal) + 1,
                    textSelector.IndexOf("]", StringComparison.Ordinal) -
                    textSelector.IndexOf("[", StringComparison.Ordinal) - 1);

                var guid = Guid.Parse(sd);
                var products = RepositoryProduct.Products.FindAll(p => p.CustomerId == guid);

                switch (products.Count)
                {
                    case 0:
                        StatusMes.Content = Properties.Resources.LabelProductNotFind;
                        xtbc.SelectedIndex = 3;
                        break;
                    case 1:
                        StatusMes.Content = "";
                        FindProduct.Product = products[0];
                        break;
                    default:
                        xtbc.SelectedIndex = 3;
                        FindProduct.Product = products[0];
                        break;
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var main = Owner as MainWindow;
            var name = Sub + "_" + xName.Content + "x" + yName.Content;
            var button = (Button) main.FindName(name);

            if (button != null)
            {
                button.Background = new SolidColorBrush(xColor.SelectedColor);
                button.Foreground = new SolidColorBrush(xColorFont.SelectedColor);
                button.Content = xCaption.Text;

                var elm = button.Tag as ClassGridGroup.Elm ?? new ClassGridGroup.Elm(
                    RepositoryXmlFile.GetPathByType(XmlDocEnum.B),
                    (byte) X,
                    (byte) Y,
                    button.Content.ToString(),
                    GetSelected(xGBFunction));

                elm.Func = GetSelected(xGBFunction);
                elm.Background = button.Background;
                elm.Foreground = button.Foreground;
                elm.Caption = button.Content.ToString();

                button.Tag = elm;

                var bReset = (Button) sender;
                if (bReset.Tag.ToString() == "Réinitialiser la configuration")
                    button.Tag = "None - Vide";

                if (button.Tag != null && button.Tag.ToString() == "None - Vide")
                {
                    button.ClearValue(BackgroundProperty);
                    button.Content = string.Empty;
                    button.ToolTip = Properties.Resources.LabelNone;
                }
                ClassGridGroup.Save(button);
                Close();
            }
        }

        private void CbSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSelected(null, string.Empty);
        }

        private void RadioButtonClick(object sender, RoutedEventArgs e)
        {
            var rb = (RadioButton) sender;

            foreach (var bs in _buttons)
                bs.IsChecked = false;

            rb.IsChecked = true;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var bs in _buttons)
                bs.Click += RadioButtonClick;
        }
        
        private void ButtonClick1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}