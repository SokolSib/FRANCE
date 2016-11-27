using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;

namespace TicketWindow.Controls
{
    /// <summary>
    /// Логика взаимодействия для FindProductControl.xaml
    /// </summary>
    public partial class FindProductControl : UserControl
    {
        public FindProductControl()
        {
            InitializeComponent();
        }

        private void BtnFind_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilterBox.Text.Length > 0)
            {
                var text1 = FilterBox.Text;
                var text2 = FilterBox.Text;
                for (var i = 0; i < Config.SymbolsForReplace.Length; i++)
                {
                    text2 = text2.Replace(Config.SymbolsForReplace[i], Config.SymbolsToReplace[i]);
                }

                DataGrid.ItemsSource =
                    RepositoryProduct.Products.Where(
                        p => p.Name.IndexOf(text1, StringComparison.OrdinalIgnoreCase) != -1 ||
                             p.Desc.IndexOf(text1, StringComparison.OrdinalIgnoreCase) != -1 ||
                             p.Name.IndexOf(text2, StringComparison.OrdinalIgnoreCase) != -1 ||
                             p.Desc.IndexOf(text2, StringComparison.OrdinalIgnoreCase) != -1 ||
                             p.CodeBare.IndexOf(text1, StringComparison.OrdinalIgnoreCase) != -1);
            }
        }

        public ProductType Product
        {
            get
            {
                return (ProductType)DataGrid.SelectedItem;
            }
            set
            {
                FilterBox.Text = value.Name;
                BtnFind_OnClick(null, null);
                var selected = ((IEnumerable<ProductType>)DataGrid.ItemsSource).FirstOrDefault(p => p.CustomerId == value.CustomerId);
                DataGrid.SelectedItem = selected;
            }
        }
    }
}
