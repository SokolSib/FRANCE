using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using ticketwindow.Class;
using System.IO;
using System.Windows.Media.Animation;
using ticketwindow.Winows.DetailsProducts;
using ticketwindow.Winows.Message;
using System.Threading;
using ticketwindow.Winows.Loading;
using ticketwindow.Winows.Setting;

namespace ticketwindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class FocusableAutoCompleteBox : AutoCompleteBox
        {
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                var textbox = Template.FindName("Text", this) as TextBox;
                if (textbox != null) textbox.Focus();
            }
        }
        public int I, J;
        private int countTick = 0;
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private void gridLoad(string s, Class.ClassGridGroup.elm[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        Button b = (Button)this.FindName(s + "_" + i + "x" + j);
                        b.Content = grid[i, j].caption;
                        if (b.Background != null)
                            b.Background = grid[i, j].background;
                        else
                            b.ClearValue(Button.BackgroundProperty);
                        b.ToolTip = grid[i, j].func;
                        if (grid[i, j].foreground != null)
                            b.Foreground = grid[i, j].foreground;
                    }
                }
            }
        }
        public MainWindow()
        {
            if (!new ClassSync.isRun().isRun_())
            {
                InitializeComponent();

                //    xProduct.ItemsSource = Class.ClassProducts.listProducts;
                //      xProduct.ItemFilter += (searchText, obj) =>
                //            (searchText.Length > 1) && (obj as Class.ClassProducts.product).Name.IndexOf(searchText.ToUpper()) != -1;

                new ClassSync().SyncAll();
                gridLoad("b", ClassGridGroup.grid);
                gridLoad("a", ClassGridGroup.gridLeft);
                gridLoad("e", ClassGridGroup.gridRigthBottom);
                Loaded += MainWindow_Loaded;
                InkInputHelper.DisableWPFTabletSupport();
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;

            countTick++;

            ldt.Content = dt.ToString() + " (" + (200 - countTick) + ") ";

            if (countTick == 200)
            {
                countTick = 0;
                ClassCustomerDisplay.hi();
                ClassSync.ProductDB.set();
                ldtc.Content = "BD a été MàJ " + dt.ToLongTimeString();
            }

            CommandManager.InvalidateRequerySuggested();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lnm.Content = ClassGlobalVar.Name;
            lnt.Content = ClassGlobalVar.nameTicket;
            lnu.Content = ClassGlobalVar.user;
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
           // dispatcherTimer.Start();
            ClassBallance.opn();
            //Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application
            try
            {
                InputPanelConfiguration cp = new InputPanelConfiguration();
                IInputPanelConfiguration icp = cp as IInputPanelConfiguration;
                if (icp != null)
                    icp.EnableFocusTracking();
            }
            catch
            { }

            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(this))
            {
                bs.Click += Button_Click;
            }
            xProduct.Focus();

            ClassCustomerDisplay.hi();

            status_message.Text += Environment.NewLine + "--------------------------------" + Environment.NewLine +
                "Caisse : " + ClassGlobalVar.nameTicket + Environment.NewLine +
                "Post : " + ClassGlobalVar.numberTicket + Environment.NewLine +
                "Nom d'usager : " + ClassGlobalVar.user + Environment.NewLine + Environment.NewLine +
                "--------------------------------" + Environment.NewLine +
                "La clé d'ouverture générale : " + ClassGlobalVar.TicketWindowG + Environment.NewLine +
                "La clé d'ouverture local : " + ClassGlobalVar.TicketWindow + Environment.NewLine;

            foreach (GridSplitter gs in ClassETC_fun.FindVisualChildren<GridSplitter>(this))
                gs.IsEnabled = ClassGlobalVar.gridModif;



        }
        private void GridSplitter_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

            try
            {
                switch (((GridSplitter)sender).Name)
                {
                    case "GS1":
                        row2.Height = new GridLength(row2.Height.Value - e.VerticalChange, GridUnitType.Pixel);
                        break;
                    case "GS2":
                        row3.Height = new GridLength(row3.Height.Value - e.VerticalChange, GridUnitType.Pixel);
                        break;

                }
            }

            catch
            { }


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            countTick = 0;
            new ClassFunctuon().Click(sender);
        }
        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            countTick = 0;

            new ClassFunctuon().Button_MouseRightButtonDown(sender, e);

        }

        public void keyReturn()
        {

            string barcode = xProduct.Text.TrimEnd().TrimStart().Trim();

            if (barcode.Length > 0)
            {
                //1234-1234-1234-1234
                //01-12-08-09-30-45-00-22-16-00
                int count__ = barcode.Split('-').Length;

                lrendu.Content = "Rendu: " + ClassBond.residue().ToString("0.00") + " €";

                if (count__ != 4)
                {
                    XElement xP = null;
                    decimal qty = 1;
                    try
                    {
                        xP = ClassProducts.findCodeBar(barcode);

                        string cb = xP.Element("CodeBare").Value;


                        string[] cbm = cb.Split('[');

                        foreach (var s in cbm)
                        {
                            int indx = s.IndexOf(barcode + "]");

                            if (indx != -1)
                            {
                                string sqty = s.Replace( barcode + "]", "").Replace("^", "");

                                if (sqty.Length > 0)
                                {
                                    qty = decimal.Parse(sqty);
                                }
                            }
                        }

                        



                        
                    }

                    catch (Exception ex)
                    {
                        new ClassLog("  code 0002 barcode :" + barcode + " " + ex.Message);
                    }
                    if (xP != null)
                    {
                        try
                        {
                            ClassCheck.addProductCheck(xP, qty != 1 ? qty : new ClassFunctuon().getQTY(qty_label));
                            xProduct.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                            xProduct.Text = "";
                        }
                        catch (Exception ex)
                        {
                            new ClassLog("  code 0003 barcode :" + barcode + " " + ex.Message);
                        }
                    }
                    else
                    {
                        if (count__ == 10)
                        {
                            int cent = int.Parse(barcode.Substring(barcode.Length - 2, 2));
                            string sd = barcode.Substring(barcode.Length - 11, 8).Replace("-", "");
                            int euro = int.Parse(sd);

                            decimal m = ((decimal)(euro * 100 + cent)) / 100;

                            //   new ClassFunctuon().showMessageTime(m.ToString());

                            qty_label.Text = m.ToString();


                        }
                        else
                        {
                            if (barcode.Length == 13)
                            {
                                xP = Class.ClassProducts.findCodeBar(barcode.Substring(0,7));

                                if (xP != null)
                                {
                                    decimal qty_ = decimal.Parse(barcode.Substring(7, 5)) / 1000;

                                    ClassCheck.addProductCheck(xP, qty_);
                                    xProduct.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                                    xProduct.Text = "";
                                }
                            }

                            if (xP == null && barcode.Length == 13)
                            {
                              //  2386999903842
                                xP = Class.ClassProducts.findCodeBar(barcode.Substring(0,8));

                                if (xP != null)
                                {
                                    decimal qty_ = decimal.Parse(barcode.Substring(8, 4)) / 1000;

                                    ClassCheck.addProductCheck(xP, qty_);
                                    xProduct.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                                    xProduct.Text = "";
                                }
                            }

                            if (xP == null)
                            {
                                try
                                {
                                    xProduct.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                                    xProduct.Text = "";
                                    ClassETC_fun.wm_sound(@"Data\Computer_Error.wav");
                                    //  new ClassFunctuon().showMessage("Add Product", "Cet article n'existe pas! Ajouter un article?");

                                    new ClassFunctuon().showMessageSB("Cet article n'existe pas! Ajouter un article?");
                                }
                                catch (Exception ex)
                                {
                                    new ClassLog("  code 0004 barcode :" + barcode + " " + ex.Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ClassDiscounts.client.barcode == barcode)
                        ClassDiscounts.restoreDiscount();
                    else
                    {
                        ClassDiscounts.getDiscount(barcode);

                        ClassDiscounts.getInfClt(ClassDiscounts.client.InfoClients_customerId);
                    }

                    xProduct.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));

                    xProduct.Text = "";




                }
            }
        }
        private void xProduct_KeyUp(object sender, KeyEventArgs e)
        {
            countTick = 0;
            if (e.Key == Key.F11)
            {
                foreach (GridSplitter gs in ClassETC_fun.FindVisualChildren<GridSplitter>(this))
                    gs.IsEnabled = !gs.IsEnabled;

            }
            if (e.Key == Key.F12)
            {
                Properties.Settings.Default.Reset();
            }
            if (e.Key == Key.Return)
            {
                keyReturn();
            }
        }
        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            xProduct.Focus();
            countTick = 0;

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Class.ClassBallance.close();
            Application.Current.Shutdown();
        }
        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            countTick = 0;

            W_DetailsProducts dp = new W_DetailsProducts(sender);

            dp.ShowDialog();
        }

    }
}
