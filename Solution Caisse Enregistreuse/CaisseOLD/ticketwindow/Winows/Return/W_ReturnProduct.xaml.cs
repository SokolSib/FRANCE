using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using System.Xml.Linq;
using ticketwindow.Class;

namespace ticketwindow.Winows.Return
{
    /// <summary>
    /// Логика взаимодействия для W_ReturnProduct.xaml
    /// </summary>
    public partial class W_ReturnProduct : Window
    {
        private Class.ClassSync.ClassCloseTicketTmp.ChecksTicket check;
        private List<Class.ClassSync.ClassCloseTicketTmp.PayProducts> resetPayProducts = new List<Class.ClassSync.ClassCloseTicketTmp.PayProducts>();
        private List<Class.ClassSync.ClassCloseTicketTmp.PayProducts> productsGet;
        public W_ReturnProduct(object arg)
        {
            InitializeComponent();

            check = (Class.ClassSync.ClassCloseTicketTmp.ChecksTicket)arg;

            foreach (var e in check.PayProducts)
            {
                Class.ClassSync.ClassCloseTicketTmp.PayProducts n = new ClassSync.ClassCloseTicketTmp.PayProducts();

                n.Barcode = e.Barcode;
                n.ChecksTicketCloseTicketCustumerId = e.ChecksTicketCloseTicketCustumerId;
                n.ChecksTicketCustumerId = e.ChecksTicketCustumerId;
                n.IdCheckTicket = e.IdCheckTicket;
                n.Name = e.Name;
                n.PriceHT = e.PriceHT;
                n.ProductId = e.ProductId;
                n.QTY = e.QTY;
                n.Total = e.Total;
                n.TVA = e.TVA;
                resetPayProducts.Add(n);
            }
            listDetails.ItemsSource = check.PayProducts;

            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();

            productsGet = new List<Class.ClassSync.ClassCloseTicketTmp.PayProducts>();

            listDetailsget.ItemsSource = productsGet;

            CollectionViewSource.GetDefaultView(listDetailsget.ItemsSource).Refresh();

            codebare.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string bc = codebare.Text.Trim().TrimEnd().TrimStart();

            Class.ClassSync.ClassCloseTicketTmp.PayProducts p = check.PayProducts.ToList().Find(l => l.Barcode == bc);

            if (p != null)
                listDetails.SelectedItem = p;
              
            recalc(1, p);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string bc = ((TextBox)sender).Text.Trim().TrimEnd().TrimStart();

            if ((e.Key == Key.Return) && (bc != ""))
            {
                Class.ClassSync.ClassCloseTicketTmp.PayProducts p = check.PayProducts.ToList().Find(l => l.Barcode == bc);
               
                if (p != null)
                {
                    listDetails.SelectedItem = p;
              
                    if (p.QTY == 1)
                        recalc(1, p);
                    else
                    {
                        W_How h = new W_How(p.QTY, ClassProducts.listProducts.Find(l => l.CustumerId == p.ProductId).balance);
                        h.Owner = this;
                    
                        h.ShowDialog();
                    }
                }

                ((TextBox)sender).Text = "";
               
            }
        }

        public void recalc(decimal qty, ClassSync.ClassCloseTicketTmp.PayProducts p)
        {
            bool q = qty > 0;

            p = q ? listDetails.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.PayProducts : listDetailsget.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.PayProducts;

            if (p != null)
            {
                qty = Math.Abs(qty);

                p.QTY = p.QTY - qty;

                if (p.QTY < 1)
                {
                    if (q)
                        check.PayProducts.Remove(p);
                    else
                        productsGet.Remove(p);
                }

                Class.ClassSync.ClassCloseTicketTmp.PayProducts g = (q) ? productsGet.Find(l => l.ProductId == p.ProductId) : check.PayProducts.ToList().Find(l => l.ProductId == p.ProductId);

                if (g != null)
                {
                    g.QTY = g.QTY + qty;
                    g.Total = (g.PriceHT * g.QTY);
                    p.Total = (p.PriceHT * p.QTY);

                }
                else
                {
                    Class.ClassSync.ClassCloseTicketTmp.PayProducts n = new ClassSync.ClassCloseTicketTmp.PayProducts();

                    n.Barcode = p.Barcode;
                    n.ChecksTicketCloseTicketCustumerId = p.ChecksTicketCloseTicketCustumerId;
                    n.ChecksTicketCustumerId = p.ChecksTicketCustumerId;
                    n.IdCheckTicket = p.IdCheckTicket;
                    n.Name = p.Name;
                    n.PriceHT = p.PriceHT;
                    n.ProductId = p.ProductId;
                    n.QTY += qty;
                    n.Total = (n.PriceHT * n.QTY);
                    p.Total = (p.PriceHT * p.QTY);
                    n.TVA = p.TVA;
                    if (q)
                        productsGet.Add(n);
                    else
                        check.PayProducts.Add(n);
                }
                CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
                CollectionViewSource.GetDefaultView(listDetailsget.ItemsSource).Refresh();

            }
            else
                new ClassFunctuon().showMessageTime("pas trouvé");

        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicketTmp.PayProducts p = this.listDetails.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.PayProducts;
            if (p != null)
            {
                if (p.QTY == 1)
                    recalc(1, p);
                else
                {
                    testProduct(p, false);
                }
            }

        }

        private void testProduct (Class.ClassSync.ClassCloseTicketTmp.PayProducts p, bool minus)
        {
            ClassProducts.product pr = ClassProducts.listProducts.Find(l => l.CustumerId == p.ProductId);

            if (pr != null)
            {
                W_How h = new W_How(p.QTY, pr.balance);
                h.Owner = this;
                h.ShowDialog();
            }
            else
            {
                new ClassFunctuon().showMessageSB("Такой продукт удален");
                W_How h = new W_How(p.QTY *( minus ? -1 : 1)  , p.QTY - (int)p.QTY == 0 ? false : true);
                h.Owner = this;
                h.ShowDialog();
            }
        }

        private void mins_Click(object sender, RoutedEventArgs e)
        {
            Class.ClassSync.ClassCloseTicketTmp.PayProducts p = this.listDetailsget.SelectedItem as Class.ClassSync.ClassCloseTicketTmp.PayProducts;
            if (p != null)
            {
                if (p.QTY == 1)
                    recalc(-1, p);
                else
                {
                    testProduct(p, true);
                }
            }

        }

        private void clr_Click(object sender, RoutedEventArgs e)
        {
            check.PayProducts.Clear();
            foreach (var elm in resetPayProducts)
            {
                Class.ClassSync.ClassCloseTicketTmp.PayProducts n = new ClassSync.ClassCloseTicketTmp.PayProducts();

                n.Barcode = elm.Barcode;
                n.ChecksTicketCloseTicketCustumerId = elm.ChecksTicketCloseTicketCustumerId;
                n.ChecksTicketCustumerId = elm.ChecksTicketCustumerId;
                n.IdCheckTicket = elm.IdCheckTicket;
                n.Name = elm.Name;
                n.PriceHT = elm.PriceHT;
                n.ProductId = elm.ProductId;
                n.QTY = elm.QTY;
                n.Total = elm.Total;
                n.TVA = elm.TVA;
             
                if (n.QTY > 0)
                    check.PayProducts.Add(n);
            }
            productsGet.Clear();
            CollectionViewSource.GetDefaultView(listDetails.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(listDetailsget.ItemsSource).Refresh();
        }

        private string getBarcodeMoney(decimal money)
        {
            DateTime dt = DateTime.Now;
            ClassMoney m = new ClassMoney(money);
            //01-12-08-09-30-4500-22-16-00
            return ClassGlobalVar.numberTicket.ToString("00")+"-" + dt.ToString("MM-dd-HH-mm-ss-") + m.Euro.ToString("00-00-00") + m.Cent.ToString("-00");
        }

        
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            decimal d = productsGet.Sum(l => l.PriceHT * l.QTY);
            new ClassPrintReturnProducts(getBarcodeMoney(d), null, d);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            codebare.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
