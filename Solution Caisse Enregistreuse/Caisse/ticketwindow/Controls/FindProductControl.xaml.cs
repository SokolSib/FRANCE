﻿using System;
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
                DataGrid.ItemsSource = FindByText(FilterBox.Text);
        }

        private static IEnumerable<ProductType> FindByText(string text)
        {
            var textOriginal = text.Trim();
            var textTranslated = text.Trim();
            for (var i = 0; i < Config.SymbolsForReplace.Length; i++)
            {
                textTranslated = textTranslated.Replace(Config.SymbolsForReplace[i], Config.SymbolsToReplace[i]);
            }

            var dic = new Dictionary<Guid, ProductType>();

            foreach (var product in RepositoryProduct.Products.Where(
                p => p.Name.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1 ||
                     p.Name.IndexOf(textTranslated, StringComparison.OrdinalIgnoreCase) != -1 ||
                     p.CodeBare.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1).ToList())
            {
                dic[product.CustomerId] = product;
            }

            foreach (
                var word in
                textOriginal.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length > 1))
            {
                foreach (var product in RepositoryProduct.Products.Where(
                    p => p.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) != -1 ||
                         p.CodeBare.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1))
                {
                    dic[product.CustomerId] = product;
                }
            }
            foreach (
                var word in
                textTranslated.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Where(w => w.Length > 1))
            {
                foreach (var product in RepositoryProduct.Products.Where(
                    p => p.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) != -1 ||
                         p.CodeBare.IndexOf(textOriginal, StringComparison.OrdinalIgnoreCase) != -1))
                {
                    dic[product.CustomerId] = product;
                }
            }
            return dic.Values.ToList();
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
