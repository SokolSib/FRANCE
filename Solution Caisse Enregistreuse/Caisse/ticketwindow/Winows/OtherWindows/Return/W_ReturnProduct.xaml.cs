using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TicketWindow.Classes;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Models.Base;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.Services;

namespace TicketWindow.Winows.OtherWindows.Return
{
    /// <summary>
    ///     Логика взаимодействия для W_ReturnProduct.xaml
    /// </summary>
    public partial class WReturnProduct : Window
    {
        private readonly CheckTicket _check;
        private readonly List<PayProduct> _returnedProducts=new List<PayProduct>();
        private readonly List<PayProduct> _products = new List<PayProduct>();

        public WReturnProduct(object arg)
        {
            InitializeComponent();
            _check = (CheckTicket) arg;
            foreach (var n in _check.PayProducts.Select(e => new PayProduct(
                e.IdCheckTicket,
                e.ProductId,
                e.Name,
                e.Barcode,
                e.Qty,
                e.Tva,
                e.PriceHt,
                e.Total,
                e.ChecksTicketCustomerId,
                e.ChecksTicketCloseTicketCustomerId,
                0,
                0)))
                //(e.IdCheckTicket, e.ProductId, e.Name, e.Barcode, e.Qty, e.Tva, e.PriceHt, e.Total, e.ChecksTicketCustomerId, 0, 0);
                _products.Add(n);

            GridProducts.ItemsSource = _check.PayProducts;
            CollectionViewSource.GetDefaultView(GridProducts.ItemsSource).Refresh();

            GridReturnedProducts.ItemsSource = _returnedProducts;
            CollectionViewSource.GetDefaultView(GridReturnedProducts.ItemsSource).Refresh();

            BarCodeBox.Focus();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var bc = BarCodeBox.Text.Trim();
            var p = _check.PayProducts.ToList().Find(l => l.Barcode == bc);

            if (p != null)
                GridProducts.SelectedItem = p;

            Recalc(1, p);
        }

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            var bc = ((TextBox) sender).Text.Trim().TrimEnd().TrimStart();

            if ((e.Key == Key.Return) && (bc != ""))
            {
                var p = _check.PayProducts.ToList().Find(l => l.Barcode == bc);

                if (p != null)
                {
                    GridProducts.SelectedItem = p;

                    if (p.Qty == 1)
                        Recalc(1, p);
                    else
                    {
                        var h = new WHow(p.Qty, RepositoryProduct.Products.Find(l => l.CustomerId == p.ProductId).Balance) {Owner = this};
                        h.ShowDialog();
                    }
                }
                ((TextBox) sender).Text = "";
            }
        }

        public void Recalc(decimal qty, PayProduct p)
        {
            var q = qty > 0;
            p = q ? GridProducts.SelectedItem as PayProduct : GridReturnedProducts.SelectedItem as PayProduct;

            if (p != null)
            {
                qty = Math.Abs(qty);
                p.Qty = p.Qty - qty;

                if (p.Qty < 1)
                {
                    if (q)
                        _check.PayProducts.Remove(p);
                    else
                        _returnedProducts.Remove(p);
                }

                var g = (q) ? _returnedProducts.Find(l => l.ProductId == p.ProductId) : _check.PayProducts.ToList().Find(l => l.ProductId == p.ProductId);

                if (g != null)
                {
                    g.Qty = g.Qty + qty;
                    g.Total = (g.PriceHt*g.Qty);
                    p.Total = (p.PriceHt*p.Qty);
                }
                else
                {
                    var n = new PayProduct(
                        p.IdCheckTicket,
                        p.ProductId,
                        p.Name,
                        p.Barcode,
                        qty,
                        p.Tva,
                        p.PriceHt,
                        (p.PriceHt*qty),
                        p.ChecksTicketCustomerId,
                        p.ChecksTicketCloseTicketCustomerId,
                        0,
                        0);

                    p.Total = (p.PriceHt*p.Qty);
                    if (q)
                        _returnedProducts.Add(n);
                    else
                        _check.PayProducts.Add(n);
                }
                CollectionViewSource.GetDefaultView(GridProducts.ItemsSource).Refresh();
                CollectionViewSource.GetDefaultView(GridReturnedProducts.ItemsSource).Refresh();
            }
            else
                FunctionsService.ShowMessageTime(Properties.Resources.LabelNofFound);
        }

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            var p = GridProducts.SelectedItem as PayProduct;
            if (p != null)
            {
                if (p.Qty == 1)
                    Recalc(1, p);
                else
                    TestProduct(p, false);
            }
        }

        private void TestProduct(PayProductBase p, bool minus)
        {
            var pr = RepositoryProduct.Products.Find(l => l.CustomerId == p.ProductId);

            if (pr != null)
            {
                var h = new WHow(p.Qty, pr.Balance) {Owner = this};
                h.ShowDialog();
            }
            else
            {
                FunctionsService.ShowMessageSb(Properties.Resources.LabelProductRemoved);
                var h = new WHow(p.Qty*(minus ? -1 : 1), p.Qty - (int) p.Qty != 0) {Owner = this};
                h.ShowDialog();
            }
        }

        private void MinsClick(object sender, RoutedEventArgs e)
        {
            var p = GridReturnedProducts.SelectedItem as PayProduct;
            if (p != null)
            {
                if (p.Qty == 1)
                    Recalc(-1, p);
                else
                    TestProduct(p, true);
            }
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            _check.PayProducts.Clear();

            foreach (var n in _products.Select(elm => new PayProduct(
                elm.IdCheckTicket,
                elm.ProductId,
                elm.Name,
                elm.Barcode,
                elm.Qty,
                elm.Tva,
                elm.PriceHt,
                elm.Total,
                elm.ChecksTicketCustomerId,
                elm.ChecksTicketCloseTicketCustomerId,
                0,
                0)).Where(n => n.Qty > 0))
                _check.PayProducts.Add(n);

            _returnedProducts.Clear();
            CollectionViewSource.GetDefaultView(GridProducts.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(GridReturnedProducts.ItemsSource).Refresh();
        }

        private static string GetBarcodeMoney(decimal money)
        {
            var dt = DateTime.Now;
            var m = new EuroConverter(money);
            //01-12-08-09-30-4500-22-16-00
            return Config.NumberTicket.ToString("00") + "-" + dt.ToString("MM-dd-HH-mm-ss-") + m.Euro.ToString("00-00-00") + m.Cent.ToString("-00");
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            var d = _returnedProducts.Sum(l => l.PriceHt*l.Qty);
            DotLiquidService.Print(GetBarcodeMoney(d), d);

            RepositoryCheck.ReturnCheck(_check, _returnedProducts);
            Close();
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            BarCodeBox.Focus();
        }

        private void BarCodeOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}