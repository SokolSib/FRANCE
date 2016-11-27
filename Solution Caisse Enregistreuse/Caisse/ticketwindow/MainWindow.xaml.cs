using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using TicketWindow.Class;
using TicketWindow.Classes;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.PortClasses;
using TicketWindow.Properties;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;
using TicketWindow.Winows.AdminWindows.GroupProductWindow;
using TicketWindow.Winows.LoginWindow;
using TicketWindow.Winows.AdminWindows.RoleWindow;
using TicketWindow.Winows.AdminWindows.TvaWindow;
using TicketWindow.Winows.AdminWindows.UsersWindow;
using TicketWindow.Winows.OtherWindows.DetailsProducts;
using TicketWindow.Winows.OtherWindows.Product.AddProduct;
using TicketWindow.Winows.SyncSettingWindow;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Controls.MenuItem;
using TextBox = System.Windows.Controls.TextBox;

namespace TicketWindow
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int Sec = 5;
        private string _bUpdText = string.Empty;
        private int _countTick;
        private DispatcherTimer _dispatcherTimer;
        public int I, J;

        public MainWindow()
        {
            DataContext = new GridSize();
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                //if (!IsRun.GetIsRun())
                {
                    InitializeComponent();
                    
                    BtnRoles.Visibility =
                       !RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactRole) ?
                       Visibility.Collapsed :
                       Visibility.Visible;

                    BtnUsers.Visibility =
                       !RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactUser) ?
                       Visibility.Collapsed :
                       Visibility.Visible;

                    BtnTva.Visibility =
                       !RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactTva) ?
                       Visibility.Collapsed :
                       Visibility.Visible;

                    BtnGroupsProduct.Visibility =
                       !RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactGroupsProduct) ?
                       Visibility.Collapsed :
                       Visibility.Visible;

                    BtnSyncSettings.Visibility =
                       !RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.RedactSyncSettings) ?
                       Visibility.Collapsed :
                       Visibility.Visible;

                    SyncService.SyncAll(Dispatcher.CurrentDispatcher);
                    GridLoad("b", ClassGridGroup.Grid);
                    GridLoad("a", ClassGridGroup.GridLeft);
                    GridLoad("e", ClassGridGroup.GridRigthBottom);
                    Loaded += MainWindowLoaded;
                    InkInputHelper.DisableWpfTabletSupport();

                    var langMenu = MenuLanguage.ContextMenu.Items.Cast<MenuItem>().FirstOrDefault(m => m.Tag.ToString() == Config.Language);
                    if (langMenu != null) langMenu.IsChecked = true;
                    else DefoultLangMenu.IsChecked = true;
                }
            }
            else Close();
        }

        public XDocument CheckOriginalDocument
        {
            get { return GridProducts.Tag as XDocument; }
            set { GridProducts.Tag = value; }
        }

        private void GridLoad(string s, ClassGridGroup.Elm[,] grid)
        {
            for (var i = 0; i < grid.GetLength(0); i++)
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        var b = (Button) FindName(s + "_" + i + "x" + j);

                        b.Content = grid[i, j].Caption;

                        if (b.Background != null) 
                            b.Background = grid[i, j].Background;
                        if (grid[i, j].Foreground != null)
                            b.Foreground = grid[i, j].Foreground;

                        b.Tag = grid[i, j];

                        ProductType product;
                        b.ToolTip = FunctionsTranslateService.GetTranslatedFunctionWithProd(grid[i, j].Func, out product);
                        if (!string.IsNullOrEmpty(grid[i, j].ProductId))
                        {
                            product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == new Guid(grid[i, j].ProductId));
                            if (product != null)
                            {
                                b.ToolTip = product.Name;
                                if (string.IsNullOrEmpty(b.Content?.ToString()))
                                    b.Content = product.Name;
                            }
                        }
                        if (string.IsNullOrEmpty(b.ToolTip?.ToString())) b.ToolTip = grid[i, j].Caption;
                    }
                }
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            var dt = DateTime.Now;
            _countTick++;

            ldt.Content = string.Format("{0} ({1}) ", dt, Sec - _countTick);

            if (!Config.FromLoadSyncAll)
            {
                if (_countTick == Sec)
                {
                    _countTick = 0;

                    foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this).Where(bs => (string) bs.ToolTip == "UpdateDB"))
                    {
                        if (_bUpdText == string.Empty) _bUpdText = bs.Content.ToString();
                        bs.Content = string.Format("{0} ({1})", _bUpdText, RepositoryProduct.GetAbCountFromDb());
                    }

                    ldtc.Content = "BD a été MàJ " + dt.ToLongTimeString();
                }
            }
            else ldtc.Content = "Происходит фоновая синхронизация... работайте ....";

            CommandManager.InvalidateRequerySuggested();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            lnm.Content = Config.Name;
            lnt.Content = Config.NameTicket;
            lnu.Content = Config.User;

            if (Config.IsUseServer)
            {
                _dispatcherTimer = new DispatcherTimer();
                _dispatcherTimer.Tick += DispatcherTimerTick;
                _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
                _dispatcherTimer.Start();
            }

            ClassBallance.Opn();
            //Windows 8 API to enable touch keyboard to monitor for focus tracking in this WPF application
            try
            {
                var cp = new InputPanelConfiguration();
                var icp = cp as IInputPanelConfiguration;
                if (icp != null) icp.EnableFocusTracking();
            }
            catch
            {
                // ignored
            }

            foreach (var bs in ClassEtcFun.FindVisualChildren<Button>(this))
                bs.Click += ButtonClick;

            xProduct.Focus();
            ClassCustomerDisplay.Hi();

            status_message.Text += Environment.NewLine + "--------------------------------" + Environment.NewLine +
                                   "Caisse : " + Config.NameTicket + Environment.NewLine +
                                   "Post : " + Config.NumberTicket + Environment.NewLine +
                                   "Nom d'usager : " + Config.User + Environment.NewLine + Environment.NewLine +
                                   "--------------------------------" + Environment.NewLine +
                                   "La clé d'ouverture générale : " + GlobalVar.TicketWindowG + Environment.NewLine +
                                   "La clé d'ouverture local : " + GlobalVar.TicketWindow + Environment.NewLine;

            foreach (var gs in ClassEtcFun.FindVisualChildren<GridSplitter>(this))
                gs.IsEnabled = Config.GridModif;
        }

        private void GridSplitterDragDelta(object sender, DragDeltaEventArgs e)
        {
            try
            {
                switch (((GridSplitter) sender).Name)
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
            {
                // ignored
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            _countTick = 0;
            FunctionsService.Click(sender);
        }

        private void ButtonMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _countTick = 0;

            if (RepositoryAccountUser.PermiseByPrivelege(Privelege.RedactButton))
                FunctionsService.ButtonMouseRightButtonDown(sender, e);
        }

        public void KeyReturn()
        {
            var barcode = xProduct.Text.Trim();

            if (barcode.Length > 0)
            {
                var count = barcode.Split('-').Length;
                lrendu.Content = "Rendu: " + RepositoryCurrencyRelations.Residue().ToString("0.00") + " €";

                if (count != 4)
                {
                    XElement xP = null;
                    decimal qty = 1;

                    try
                    {
                        xP = RepositoryProduct.GetXElementByBarcode(barcode);

                        if (xP != null)
                        {
                            var cbm = xP.GetXElementValue("CodeBare").Split('[');

                            foreach (var s in cbm)
                            {
                                var indx = s.IndexOf(barcode + "]", StringComparison.Ordinal);

                                if (indx != -1)
                                {
                                    var sqty = s.Replace(barcode + "]", "").Replace("^", "");

                                    if (sqty.Length > 0)
                                        qty = decimal.Parse(sqty);
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogService.Log(TraceLevel.Error, 2, "Barcode :" + barcode + " " + ex.Message + ".");
                    }

                    if (xP != null)
                    {
                        try
                        {
                            CheckService.AddProductCheck(xP, qty != 1 ? qty : FunctionsService.GetQty(qty_label));
                            xProduct.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            xProduct.Text = "";
                        }
                        catch (System.Exception ex)
                        {
                            LogService.Log(TraceLevel.Error, 3, "Barcode :" + barcode + " " + ex.Message + ".");
                        }
                    }
                    else
                    {
                        if (count == 10)
                        {
                            var cent = int.Parse(barcode.Substring(barcode.Length - 2, 2));
                            var sd = barcode.Substring(barcode.Length - 11, 8).Replace("-", "");
                            var euro = int.Parse(sd);
                            var m = ((decimal) (euro*100 + cent))/100;
                            qty_label.Text = m.ToString();
                        }
                        else
                        {
                            if (barcode.Length == 13)
                            {
                                xP = RepositoryProduct.GetXElementByBarcode(barcode.Substring(0, 7));

                                if (xP != null)
                                {
                                    var qtyCurrent = decimal.Parse(barcode.Substring(7, 5))/1000;
                                    CheckService.AddProductCheck(xP, qtyCurrent);
                                    xProduct.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                    xProduct.Text = "";
                                }
                            }

                            if (xP == null && barcode.Length == 13)
                            {
                                xP = RepositoryProduct.GetXElementByBarcode(barcode.Substring(0, 8));

                                if (xP != null)
                                {
                                    var qtyCurrent = decimal.Parse(barcode.Substring(8, 4))/1000;
                                    CheckService.AddProductCheck(xP, qtyCurrent);
                                    xProduct.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                    xProduct.Text = "";
                                }
                            }

                            if (xP == null)
                            {
                                try
                                {
                                    xProduct.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    xProduct.Text = "";
                                    ClassEtcFun.WmSound(@"Data\Computer_Error.wav");
                                    var result = FunctionsService.ShowMessage(Properties.Resources.BtnAdd, "Cet article n'existe pas! Ajouter un article?", Properties.Resources.BtnAdd);
                                    if (result)
                                    {
                                        var windowAddProduct = new WAddProduct {xCodeBar = {Text = barcode}};
                                        windowAddProduct.ShowDialog();
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    LogService.Log(TraceLevel.Error, 4, "Barcode :" + barcode + " " + ex.Message + ".");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (RepositoryDiscount.Client.Barcode == barcode)
                    {
                        RepositoryDiscount.RestoreDiscount();
                        CheckService.DiscountCalc();
                        FunctionsService.WriteTotal();
                    }
                    else
                    {
                        var discountCard = RepositoryDiscount.GetDiscount(barcode);
                        if (discountCard == null)
                            FunctionsService.ShowMessageSb("La carte n'existe pas ou il y a peut-être des problèmes avec l'Internet");
                        else if (discountCard.IsActive)
                            FunctionsService.WriteTotal();
                        else
                            FunctionsService.ShowMessageSb(" La carte est bloquée ");

                        RepositoryDiscount.GetInfClt(RepositoryDiscount.Client.InfoClientsCustomerId);
                    }

                    xProduct.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    xProduct.Text = "";
                }
            }
        }

        private void XProductKeyUp(object sender, KeyEventArgs e)
        {
            _countTick = 0;
            if (e.Key == Key.F11)
            {
                foreach (var gs in ClassEtcFun.FindVisualChildren<GridSplitter>(this))
                    gs.IsEnabled = !gs.IsEnabled;
            }

            if (e.Key == Key.F12)
                Settings.Default.Reset();

            if (e.Key == Key.Return)
                KeyReturn();
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            xProduct.Focus();
            _countTick = 0;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            ClassBallance.Close();
            Application.Current.Shutdown();
        }

        private void GridProductsMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _countTick = 0;
            var dp = new WDetailsProducts();
            dp.ShowDialog();
        }

        #region Nested type: FocusableAutoCompleteBox

        public class FocusableAutoCompleteBox : AutoCompleteBox
        {
            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                var textbox = Template.FindName("Text", this) as TextBox;
                if (textbox != null) textbox.Focus();
            }
        }

        #endregion

        private void ButtonUsersClick(object sender, RoutedEventArgs e)
        {
            var wnd = new UsersWindow();
            wnd.ShowDialog();
        }

        private void ButtonRolesClick(object sender, RoutedEventArgs e)
        {
            var wnd = new RolesWindow();
            wnd.ShowDialog();
        }

        private void ButtonTvaClick(object sender, RoutedEventArgs e)
        {
            var wnd = new TvasesWindow();
            wnd.ShowDialog();
        }

        private void ButtonGroupsProductClick(object sender, RoutedEventArgs e)
        {
            var wnd = new GroupsProductWindow();
            wnd.ShowDialog();
        }

        private void ButtonSyncSettingsClick(object sender, RoutedEventArgs e)
        {
            var wnd = new SyncSettingWindow();
            wnd.ShowDialog();
        }

        private void MenuLanguage_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as FrameworkElement;
            if (button != null)
            {
                button.ContextMenu.PlacementTarget = (sender as Button);
                button.ContextMenu.Placement = PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }
        
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var menu = (MenuItem) sender;
            foreach (var m in MenuLanguage.ContextMenu.Items.Cast<MenuItem>().Where(m => m != menu))
                m.IsChecked = false;

            var language = menu.Tag.ToString();
            SetLanguage(new CultureInfo(language));
            Config.Language = language;
        }

        private void SetLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Properties.Resources.Culture = culture;

            BtnUsers.Content = Properties.Resources.MenuUsers;
            BtnRoles.Content = Properties.Resources.MenuRoles;
            BtnTva.Content = Properties.Resources.MenuVats;
            BtnGroupsProduct.Content = Properties.Resources.MenuGroupsProduct;
            MenuLanguage.Content = Properties.Resources.MenuLanguage;
            BtnSyncSettings.Content = Properties.Resources.MenuSyncSetting;

            ColumnCount.Header = Properties.Resources.LabelQty;
            ColumnDescription.Header = Properties.Resources.LabelDescription;
            ColumnDiscount.Header = Properties.Resources.LabelDiscount;
            ColumnTotal.Header = Properties.Resources.LabelTotal;

            var mTotal = new EuroConverter(CheckService.GetTotalPrice());
            ltotal.Content = Properties.Resources.LabelTotal + " : " + mTotal.Euro + "," + mTotal.Cent.ToString("00") + " €";

            SyncService.SetDefoultTypesPays();
        }
    }
}