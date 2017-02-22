using Devis.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;


namespace Devis
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
        private void calcTotal ()
        {


            _k.Foreground = comma ? Brushes.Red : Brushes.White;
    
            _k.BorderThickness = comma ? new Thickness(5) : new Thickness(0);

            _k.BorderBrush = comma ? Brushes.Red : Brushes.White;

            try
            {

                lTotal.Content = "Total:" + shelfProducts.Sum(l => decimal.Parse(l.Element("TOTAL").Value.Replace('.', ','))).ToString("C3", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));
            }
            catch
            {

            }
        }

        private List<XElement> x;

        private List<ClassSync.ProductDB.ProductsBC_> productsBC_;

        private List<XElement> elm ;

        public List<XElement> shelfProducts;

        private List<XElement> BC;


        public void restart ()
        {
               x = ClassProducts.x.Element("Product").Elements("rec").ToList();

        productsBC_ = new List<ClassSync.ProductDB.ProductsBC_>();

        elm = new List<XElement>();

        shelfProducts = ClassProducts.shelfProducts;
            BC = x.FindAll(l => l.Element("BC").Value != "");

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public MainWindow()
        {
            new B.Sync().ShowDialog();

            InitializeComponent();

            foreach (Button bs in FindVisualChildren<Button>(this.butons))
            {
                bs.Click += bClick_;
            }
            foreach (Button bs in FindVisualChildren<Button>(this.numpad))
            {
                bs.Click += bClick;
            }

            

          
            rowbf.Height = new GridLength(0);
            butons.Height = 0;

            text.Focus();

         


            calcTotal();

            b1.Visibility = Visibility.Hidden;
            b2.Visibility = Visibility.Hidden;
            b3.Visibility = Visibility.Hidden;
            b4.Visibility = Visibility.Hidden;
            b5.Visibility = Visibility.Hidden;
            b6.Visibility = Visibility.Hidden;
            b7.Visibility = Visibility.Hidden;
            tbcb.Visibility = Visibility.Hidden;
            tgbFind.Visibility = Visibility.Hidden;

            restart();

            _ProductsGrid.DataContext = shelfProducts;
        }

        private void setQTY(int c)
        {
            if (_ProductsGrid.SelectedItem != null)
            {
                var elm = (XElement)_ProductsGrid.SelectedItem;

             //   ClassProducts.updQTYProduct(c,elm, tbcb.IsChecked?? false);
                 
                int indx = shelfProducts.FindIndex(l => l == elm);

                if (indx == -1)
                {
                    shelfProducts.Insert(0,elm);
                    
                    ClassProducts.updQTYProduct(c, elm, tbcb.IsChecked ?? false, typefind);

                }
                else
                {

                    ClassProducts.updQTYProduct(c, elm, tbcb.IsChecked ?? false, typefind);

                    shelfProducts.Remove( elm);

                    shelfProducts.Insert(0, elm);

                }


            }
            calcTotal();
        }

        private void bClick_(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            int qty = 0;

            bool f = int.TryParse(b.Content.ToString().Replace("+", "") , out qty);

            setQTY(qty);

            ClassF.wm_sound(@"Data\Beep.wav");
        }

        private void sendDevis()
        {
            if (shelfProducts.Count > 0)
            {
                var mes = new B.Message();

                mes.message.AppendText( "load please wite ...");

                mes.ShowDialog();

                mes.Owner = this;
            }
        }

        private bool modif = false;

        private decimal bufqty = 0.0m;

        private void modStart()
        {
            XElement elm = (XElement)_ProductsGrid.SelectedItem;

            if (elm != null)
            {
                decimal.TryParse(elm.Element("QTY").Value.Replace('.', ','), out bufqty);

                elm.Element("QTY").SetValue("");

                elm.Element("QTY_box").SetValue("");

                elm.Element("TOTAL").SetValue("");

                _ProductsGrid.Items.Refresh();

                modif = true;
            }
        }

        private void modEnd()
        {
            XElement elm = (XElement)_ProductsGrid.SelectedItem;

            if (elm != null)
            {
                modif = false;
                calcTotal();
                
            }

            comma = false;

          //  tbcb.IsChecked = true;

            calcTotal();
        }

        private void modCancel()
        {
            XElement elm = (XElement)_ProductsGrid.SelectedItem;

            if (elm != null)
            {
                string pr = elm.Element("price").Value.Replace('.', ',');

                decimal cb = decimal.Parse(elm.Element("ContenanceBox").Value.Replace('.', ','));

                cb = cb == 0.0m ? 1 : cb;

                elm.Element("QTY").SetValue(bufqty);

                elm.Element("QTY_box").SetValue(bufqty / cb);

                elm.Element("TOTAL").SetValue(bufqty * decimal.Parse(pr));

                _ProductsGrid.Items.Refresh();

                modif = false;

                comma = false;
                 
                calcTotal();
            }
        }


        private static bool comma = false;

        private void mod(string number)
        {
            XElement elm = (XElement)_ProductsGrid.SelectedItem;

            if (elm != null)
            {
                string s = (tbcb.IsChecked ?? false ? elm.Element("QTY_box").Value : elm.Element("QTY").Value).Replace('.',',');

                decimal a = 0.0m;

                decimal.TryParse(s, out a);

                decimal c = 0.0m;

                if (!comma)
                {

                    if (s.IndexOf(',') != -1)
                    {
                        string[] comma_ = s.Split(',');

                        comma_[0] += number;

                        decimal.TryParse(comma_[0] + "," + comma_[1], out c);
                    }

                    else
                    {
                        decimal.TryParse(s + number, out c) ;
                    }

                 //   ClassProducts.updQTYProduct(c - a, elm, tbcb.IsChecked ?? false);
                }

                else
                {
                    
                    if (s.IndexOf(',') == -1)
                        s += ",";

                    string[] comma_ = s.Split(',');

                    comma_[1] += number;

                    decimal.TryParse(comma_[0] + "," + comma_[1] , out c) ;
                    
                }

                ClassProducts.updQTYProduct(c - a, elm, tbcb.IsChecked ?? false,  typefind);

                _ProductsGrid.Items.Refresh();

                calcTotal();

            }
        }

        private void setLabelQTY(string number)
        {
            switch (number.TrimEnd())
            {
                case "cl":
                    if (!modif)
                        qty_label.Text = "__";
                    else modCancel();
                    break;

                case "Enter":
                    if (modif)
                    {
                        modEnd();
                    }



                    break;

                case ",":
                    if (!modif)
                    {
                        string fe = (tbcb.IsChecked ?? false) ? " box " : " pièces ";

                        qty_label.Text = (qty_label.Text.Replace(" box ", "").Replace(" pièces ", "").Replace("_", "")) + "," + fe;
                    }

                    else
                    {
                        comma = comma ? false : true;

                        _k.Foreground = comma ? Brushes.Red : Brushes.White;

                        _k.BorderThickness = comma ? new Thickness(5) : new Thickness(0);

                        _k.BorderBrush = comma ? Brushes.Red : Brushes.White;

                    }
                    break;



                case "Annuler":

                    if (shelfProducts.Count > 0)
                    {

                        var w = new B.AnullerDevis();

                        w.Owner = this;

                        w.data = shelfProducts.ToArray();

                        w.ShowDialog();

                        _ProductsGrid.Items.Refresh();
                    }
                    break;
                case "CE":

                    XElement selectElm = (XElement)_ProductsGrid.SelectedItem;

                    if (selectElm != null)
                    {
                        ClassProducts.clrElm(new XElement[] { selectElm });

                        _ProductsGrid.Items.Refresh();
                    }
                    break;
                case "Valider":
                    sendDevis();
                    break;
                case "BOX": break;
                case "Modif": if (modif) modCancel(); else modStart(); break;
                case "Find": break;
                case "sync":
                    var mes = new B.Sync();

                    mes.Owner = this;
                    mes.message.Content = "load please wite ...";

                    mes.ShowDialog();
                    break;
                case "Close":
                    Window_Closed(this, null);
                    break;
                case "Réduire":
                    WindowState = WindowState.Minimized;
                    break;
                default:
                    if (!modif)
                    {
                        string fe = (tbcb.IsChecked ?? false) ? " box " : " pièces ";

                        qty_label.Text = (qty_label.Text.Replace(" box ", "").Replace(" pièces ", "").Replace("_", "")) + int.Parse(number) + fe;
                    }

                    else

                        mod(number);

                    break;
            }
        }

        private void bClick(object sender, RoutedEventArgs e)
        {
            ClassF.wm_sound(@"Data\Beep.wav");
            setLabelQTY(((Button)sender).Content.ToString());
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        private static List<XElement> findAll(string search, IEnumerable<XElement> products)
        {
            string[] m = search.Split(' ');

            List<XElement> res = new List<XElement>();

            List<string> stroki = new List<string>();

            foreach (string s in m)
            {
                String str = RemoveDiacritics(s.Trim().TrimEnd().TrimStart());
                if (str.Length > 0)
                    stroki.Add(str);
            }

            switch (stroki.Count)
            {

                case 1:

                    res.AddRange(
                            products.Where(l => RemoveDiacritics( l.Element("Name").Value ?? "").ToUpper().Contains(stroki[0]))
                            );
                    break;
                case 2:
                    res.AddRange(
                         products.Where(l => 
                         RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[0]) 
                      && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[1]))
                               );
                    break;
                case 3:
                    res.AddRange(
                          products.Where(l =>
                          RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[0])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[1])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[2]))
                                );
                    break;
                case 4:
                    res.AddRange(
                          products.Where(l =>
                          RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[0])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[1])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[2])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[3]))
                                );
                    break;
                case 5:
                    res.AddRange(
                          products.Where(l =>
                          RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[0])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[1])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[2])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[3])
                       && RemoveDiacritics(l.Element("Name").Value ?? "").ToUpper().Contains(stroki[4]))
                                );
                    break;

                default: break;
            }
            return res;
        }

        private void modeFind(KeyEventArgs e)
        {
           
            if (e.Key == Key.Enter)
            {
                
                    elm.Clear();

                string item = text.Text.TrimEnd().TrimStart().ToUpper();

                long digital;

                bool f = long.TryParse(item, out digital);

                if (f)
                {
                    IEnumerable<XElement> elms = x.Where(l => l.Element("CodeBare").Value.Contains(item));

         
                    elm.AddRange(elms);
                }

                else
                    elm.AddRange( findAll(item, x));

                if (elm.Count() == 0)
                {
                    var ek = find(item);
                    if (ek!=null)
                   elm.Add( ek ) ;
                }

                if (elm.Count() == 0)
                {
                    var w = new B.messageDlgTime();

                    w.message.Text = "not found";



                    w.Show();

                }

               
                    _ProductsGrid.SelectedItem = elm.FirstOrDefault();


                    //  _ProductsGrid.DataContext = shelfProducts;

                    _ProductsGrid.DataContext = elm;
              

              
                _ProductsGrid.Items.Refresh();

            //    tbcb.IsChecked = true;

                text.Text = "";
            }
        }

        private int typefind = -1;

        private XElement find(string item)
        {
            XElement elm = x.FirstOrDefault(l => l.Element("CodeBare").Value == item);
            typefind = 0;
            try {
                if (elm == null && item.Length > 5)
                {
                    elm = BC.FirstOrDefault(l => l.Element("BC").Elements("CodeBar").FirstOrDefault(j => j.Value == item) != null);

                    XNode node = elm.Element("BC").Elements("CodeBar").FirstOrDefault(l => l.Value == item).NextNode;

                    /// string s =  (node as XElement).Value == "1.000" ? node.Parent.Element("ContenanceBox").Value : (node as XElement).Value;

                    string s = node.Parent.Element("ContenanceBox").Value ;

                    //  tbcb.IsChecked = false;

                    typefind = 1;

                    qty_label.Text = s + " box ";
                }
            }
            catch
            {
                new ClassLog(" " + item);
            }

            if (elm == null && item.Length == 13)
            {

                elm = x.FirstOrDefault(l => l.Element("CodeBare").Value == item.Substring(0, 8));

                typefind = 2;

                if (elm != null)
                {
                    decimal qty_ = decimal.Parse(item.Substring(8, 4)) / 1000;

                    qty_label.Text = qty_ + " pièces ";

                  //  tbcb.IsChecked = false;
                   
                   
                }
            }

            if (elm == null && item.Length == 13)
            {

                elm = x.FirstOrDefault(l => l.Element("CodeBare").Value == item.Substring(0, 7));


                typefind = 2;

                if (elm != null)
                {
                    decimal qty_ = decimal.Parse(item.Substring(7, 5)) / 1000;

                    qty_label.Text = qty_ + " pièces ";

                 //   tbcb.IsChecked = false;


                }
            }
            return elm;
        }

        private void modeStandart(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string item = text.Text.TrimEnd().TrimStart().ToUpper();

                long digital;

                bool f = long.TryParse(item, out digital);

                text.Text = "";
                if (f)
                {
                    XElement elm = find(item);

                    if (elm == null)
                    {
                        var w =   new B.messageDlgTime();

                        w.message.Text = "not found";

                       

                        w.Show();

                        _ProductsGrid.Items.Refresh();
                    }
                    else
                    {

                        int indx = shelfProducts.FindIndex(l => l == elm);

                        string qtys = qty_label.Text.Replace(" box ", "").Replace(" pièces ", "").Replace('_', ' ').Replace('.', ',').Trim();

                        qty_label.Text = "__";

                        decimal qty_ = 0.0m;

                       decimal.TryParse(qtys == "0,0" || qtys == string.Empty ? "1,00" : qtys, out qty_);

                        if (indx == -1)
                        {
                            shelfProducts.Insert(0,elm);
                            
                            ClassProducts.updQTYProduct(qty_, elm, tbcb.IsChecked ?? false, typefind);

                            

                        }
                        else
                        {

                            ClassProducts.updQTYProduct(qty_, elm, tbcb.IsChecked ?? false,  typefind);

                            shelfProducts.Remove(elm);

                            shelfProducts.Insert(0,elm);

                        }

                    
                        _ProductsGrid.SelectedItem = elm;



                        _ProductsGrid.Items.Refresh();


                        _ProductsGrid.ScrollIntoView(_ProductsGrid.SelectedItem);

                        ClassF.wm_sound(@"Data\Beep.wav");

                     //   tbcb.IsChecked = true;
                         
                        calcTotal();
                    }

                }
            }
        }

        private void text_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (tgbFind.IsChecked == false)
                {
                    modeStandart(e);
                    
                }
                else
                {
                    modeFind(e);
                }
            }
            catch
            {

            }
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            text.Focus();
        }

        private void _ProductsGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (modif)
                modCancel();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (tgbFind.IsChecked == true)
            {
                _ProductsGrid.DataContext = x;

                rowbf.Height = new GridLength(60);
                butons.Height = 60;
            }
            else
            {
                _ProductsGrid.DataContext = shelfProducts;
                rowbf.Height = new GridLength(0);
                butons.Height = 0;
                shelfProducts.Clear();
                shelfProducts.AddRange(  x.Where(l => decimal.Parse(l.Element("QTY").Value.ToString().Trim() == "" ? "0" : l.Element("QTY").Value.ToString().Trim().Replace(".",",")) > 0));
            }

            _ProductsGrid.Items.Refresh();

            calcTotal();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
       
         
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ClassProducts.saveLocal();

            Close();
        }

        private void _ProductsGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {

        }

        private void cntrEnable_Click(object sender, RoutedEventArgs e)
        {
            if (!cntrEnable.IsChecked??false)
            {
                b1.Visibility = Visibility.Hidden;
                b2.Visibility = Visibility.Hidden;
                b3.Visibility = Visibility.Hidden;
                b4.Visibility = Visibility.Hidden;
                b5.Visibility = Visibility.Hidden;
                b6.Visibility = Visibility.Hidden;
                b7.Visibility = Visibility.Hidden;
                tbcb.Visibility = Visibility.Hidden;
                tgbFind.Visibility = Visibility.Hidden;
            }
            else
            {
                b1.Visibility = Visibility.Visible;
                b2.Visibility = Visibility.Visible;
                b3.Visibility = Visibility.Visible;
                b4.Visibility = Visibility.Visible;
                b5.Visibility = Visibility.Visible;
                b6.Visibility = Visibility.Visible;
                b7.Visibility = Visibility.Visible;
                tbcb.Visibility = Visibility.Visible;
                tgbFind.Visibility = Visibility.Visible;
            }
        }
    }
}
