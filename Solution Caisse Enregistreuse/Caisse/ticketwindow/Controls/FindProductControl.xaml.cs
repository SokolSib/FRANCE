using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.Services;

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
                DataGrid.ItemsSource = TextService.FindProductsByText(FilterBox.Text, true);
        }
        
        public ProductType Product
        {
            get
            {
                return (ProductType)DataGrid.SelectedItem;
            }
            set
            {
                if (value != null)
                {
                    FilterBox.Text = value.Name;
                    BtnFind_OnClick(null, null);
                    var selected =
                        ((IEnumerable<ProductType>) DataGrid.ItemsSource).FirstOrDefault(
                            p => p.CustomerId == value.CustomerId);
                    DataGrid.SelectedItem = selected;
                }
            }
        }
    }
}
