using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
using TicketWindow.Winows;
using TicketWindow.Winows.AdditionalClasses;
using TicketWindow.Winows.AdminWindows;
using TicketWindow.Winows.OtherWindows.Ballance;
using TicketWindow.Winows.OtherWindows.Calculator;
using TicketWindow.Winows.OtherWindows.DetailsProducts;
using TicketWindow.Winows.OtherWindows.Discount;
using TicketWindow.Winows.OtherWindows.Divers;
using TicketWindow.Winows.OtherWindows.EnAttente;
using TicketWindow.Winows.OtherWindows.History;
using TicketWindow.Winows.OtherWindows.Message;
using TicketWindow.Winows.OtherWindows.Payment;
using TicketWindow.Winows.OtherWindows.Pro;
using TicketWindow.Winows.OtherWindows.Product;
using TicketWindow.Winows.OtherWindows.Product.AddProduct;
using TicketWindow.Winows.OtherWindows.Product.FindProduct;
using TicketWindow.Winows.OtherWindows.Product.FindProduct.RemoveProduct;
using TicketWindow.Winows.OtherWindows.Return;
using TicketWindow.Winows.OtherWindows.Setting;
using TicketWindow.Winows.OtherWindows.Statistique;
using TicketWindow.Winows.OtherWindows.Statistique.ModifStatNationPopup;
using TicketWindow.Winows.OtherWindows.UpdateDB;

namespace TicketWindow.Services
{
    internal static class FunctionsService
    {
        private static bool _setBc;
        private static MainWindow _mainAppWindow;
        private static IEnumerable _mainItems;

        public static MainWindow MainAppWindow
        {
            get
            {
                if (_mainAppWindow != null) return _mainAppWindow;
                if (Application.Current != null)
                    return _mainAppWindow = Application.Current.MainWindow as MainWindow;
                return null;
            }
        }

        public static void Click(object sender)
        {
            RunFun(sender, null, null);
        }

        public static void Click(object sender, object arg)
        {
            RunFun(sender, arg, null);
        }

        public static void Click(object sender, object arg, object arg1)
        {
            RunFun(sender, arg, arg1);
        }

        public static bool ShowMessage(string okToolTipText, string message, string okText = null)
        {
            var messageWindow = new WMessage(okToolTipText, message, okText);
            var result = messageWindow.ShowDialog();
            return result != null && result.Value;
        }

        public static void ShowMessageSb(string message)
        {
            var messageWindow = new WMessageSb(message);
            messageWindow.ShowDialog();
        }

        public static void ShowMessageTime(string message)
        {
            var messageWindow = new WMessageTime(message);
            messageWindow.Show();
        }

        public static void ShowMessageTimeList(string message)
        {
            var messageWindow = new WMessageTimeList(message);
            messageWindow.Show();
        }

        public static decimal GetQty(TextBlock tb)
        {
            decimal qty;
            if (!decimal.TryParse(tb.Text.Replace(" X ", ""), NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("fr-FR"), out qty)) qty = 1;
            tb.Text = "__";
            return qty;
        }

        public static decimal GetMoney(TextBlock tb)
        {
            decimal m;
            if (!decimal.TryParse(tb.Text.Replace(" X ", ""), out m)) m = 0.0m;

            return m;
        }

        public static void AddCurrency(Currency currency, int count, WGridPay ws)
        {
            var sumMoney = RepositoryCurrencyRelations.AddCurrency(currency, count);
            var odd = RepositoryCurrencyRelations.Residue() - sumMoney;

            if (odd < 0)
                if (!ws.TypesPay.RenduAvoir ?? false)
                {
                    var money = RepositoryCurrencyRelations.GetSumMoney();
                    var d = RepositoryTypePay.TypePays.Sum(tm => tm.RenduAvoir ?? false ? RepositoryCurrencyRelations.GetMoneyFromType(tm) : 0.0m);
                    var etc = RepositoryTypePay.TypePays.Sum(tm => !tm.RenduAvoir ?? false ? RepositoryCurrencyRelations.GetMoneyFromType(tm) : 0.0m);
                    odd = money - etc - sumMoney - d;

                    if (money - sumMoney - etc < 0)
                        odd = -d;
                }

            var m = new EuroConverter(RepositoryCurrencyRelations.Residue());
            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent + " centime(s)";

            foreach (var ls in ClassEtcFun.FindVisualChildren<Label>(ws))
            {
                if ((ls.Name != "") && (ls.Name.Substring(0, 4) == "mla_") && ((Button) ((StackPanel) ls.Parent).Parent).ToolTip != null)
                {
                    ls.Content = "";
                    ws.CountCurrency = "";
                }
            }

            ws.ListBoxlog.Items.Clear();
            ws.ListBoxlog.Items.Add("Montant : " + m.Euro + "," + m.Cent);

            if (odd < 0)
            {
                var ms = new EuroConverter(odd);
                ws.lblGet.Content = ms.Euro + " euro(s) et " + ms.Cent.ToString("00");
                ws.ListBoxlog.Items.Add("Rendu : " + ms.Euro + "," + ms.Cent.ToString("00"));
            }

            ws.ListBoxlog.Items.Add("Détails du paiement");

            foreach (var c in RepositoryCurrencyRelations.CountCurencys)
                ws.ListBoxlog.Items.Add(c.Count + " X " + c.Currency.Desc);

            ws.ListBoxlog.Items.Add("      ______________");
            ws.ListBoxlog.Items.Add(sumMoney.ToString("0.00"));
        }

        public static void WriteToatl(int idx)
        {
            if (MainAppWindow != null)
            {
                if (RepositoryCheck.DocumentProductCheck != null)
                {
                    var mTotal = new EuroConverter(CheckService.GetTotalPrice());
                    MainAppWindow.ltotal.Content = Resources.LabelTotal + " : " + mTotal.Euro + "," + mTotal.Cent.ToString("00") + " €";

                    if (MainAppWindow.GridProducts.Items.Count == 1)
                        MainAppWindow.GridProducts.SelectedIndex = 0;

                    var indx = MainAppWindow.GridProducts.SelectedIndex;

                    MainAppWindow.GridProducts.DataContext = RepositoryCheck.C == null
                        ? RepositoryCheck.DocumentProductCheck.Element("check")
                        : RepositoryCheck.C.Element("check");

                    MainAppWindow.GridProducts.SelectedIndex = indx != -1
                        ? MainAppWindow.GridProducts.Items.Count - 1
                        : (idx > MainAppWindow.GridProducts.Items.Count - 1 ? MainAppWindow.GridProducts.Items.Count - 1 : idx);

                    var x = MainAppWindow.GridProducts.SelectedItem as XElement;

                    if (x == null)
                    {
                        MainAppWindow.GridProducts.SelectedIndex = MainAppWindow.GridProducts.Items.Count - 1;
                        x = MainAppWindow.GridProducts.SelectedItem as XElement;
                    }

                    if (MainAppWindow.GridProducts.Items.Count > 0)
                        try
                        {
                            var border = VisualTreeHelper.GetChild(MainAppWindow.GridProducts, 0) as Decorator;

                            var scroll = border?.Child as ScrollViewer;
                            scroll?.ScrollToEnd();
                        }
                        catch
                        {
                            LogService.Log(TraceLevel.Error, 990);
                        }

                    if (x?.GetXElementValue("price").ToDecimal() == 0)
                    {
                        var productType = ProductType.FromXElement(x);
                        var wp = new WModifierPrix(productType);
                        Effect(wp);
                    }
                }
                if ((RepositoryDiscount.Client.Barcode != null) || RepositoryDiscount.Client.ProcentDefault > 0)
                {
                    if (RepositoryDiscount.Client.Points > RepositoryDiscount.Client.MaxPoints - 1)
                        if (!RepositoryDiscount.Client.DiscountSet && RepositoryDiscount.Client.ShowMessaget)
                        {
                            ShowMessage("DiscountSet",
                                "Vous avez " + RepositoryDiscount.Client.Points + " points." + Environment.NewLine + "Vous voulez vous servir de reduction de 20%?");
                            RepositoryDiscount.Client.ShowMessaget = false;
                        }

                    MainAppWindow.lProcentDiscount.Content = RepositoryDiscount.Client.Procent + RepositoryDiscount.Client.ProcentDefault + "%";
                    MainAppWindow.lNameClient.Content = RepositoryDiscount.Client.NameFirst ?? "Inconnue " + " " + RepositoryDiscount.Client.NameLast;
                    MainAppWindow.lPointsClient.Content = RepositoryDiscount.Client.Points;
                }
                else
                {
                    MainAppWindow.lProcentDiscount.Content = "";
                    MainAppWindow.lNameClient.Content = "";
                    MainAppWindow.lPointsClient.Content = "";
                }

                if (ClassProMode.ModePro)
                    MainAppWindow.lProcentDiscount.Content = ClassProMode.Pro.NameCompany;
            }
        }

        public static void WriteTotal()
        {
            WriteToatl(-1);
        }

        private static void MinimizeWindow(object sender)
        {
            var window = Window.GetWindow((Button) sender);
            if (window != null) window.WindowState = WindowState.Minimized;
        }

        private static void Effect(object sender)
        {
            var w = (Window) sender;
            w.WindowStyle = WindowStyle.None;
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (w.WindowState != WindowState.Maximized)
                w.ShowDialog();
            else w.Show();
        }

        private static void CloaseMainWindow()
        {
            try
            {
                var countBufCheck = RepositoryCheck.GetCountOfProductInCheckFromBuf() + RepositoryCheck.GetCountOfProductInCheckFromEnAttenete();
                if (countBufCheck == 0)
                    if (!SyncData.IsSync)
                    {
                        Application.Current.Shutdown();
                        ClassCustomerDisplay.Bye();
                        GlobalVar.IsBreak = true;
                    }
                    else ShowMessageSb(Resources.LabelSyncProcess);
                else
                    ShowMessageSb(Resources.LabelEditListProducts + Environment.NewLine + Resources.LabelQty.ToLower() +
                                  " " + countBufCheck);
            }
            catch
            {
                Application.Current.Shutdown();
            }
        }

        private static void OpenWProduct(object sender)
        {
            var name = ((Button) sender).Name.Split('x');
            var x = int.Parse(name[0].Substring(2, name[0].Length - 2));
            var y = int.Parse(name[1].Substring(0, name[1].Length));

            XGridLoaded(x, y);
        }

        private static void XGridLoaded(int i, int j)
        {
            MainAppWindow.I = i;
            MainAppWindow.J = j;
            MainAppWindow.gidLeft.Visibility = Visibility.Hidden;
            MainAppWindow.xGrid.Visibility = Visibility.Visible;
            for (var ii = 0; ii < ClassGridProduct.Grid.GetLength(2); ii++)
            {
                for (var jj = 0; jj < ClassGridProduct.Grid.GetLength(3); jj++)
                {
                    if (ClassGridProduct.Grid[i, j, ii, jj] != null)
                    {
                        var b = (Button) MainAppWindow.FindName("_b_" + ii + "x" + jj);

                        if (ClassGridProduct.Grid[i, j, ii, jj].CustomerId == Guid.Empty)
                            b.ToolTip = null;
                        else
                        {
                            var productData = ClassGridProduct.Grid[i, j, ii, jj];
                            var text = "Products id=[" + productData.CustomerId + "]";
                            ProductType product;
                            b.ToolTip =
                                FunctionsTranslateService.GetTranslatedFunctionWithProd(text, out product);
                            b.Tag = text;

                            ((TextBlock)b.Content).Text = productData.Description;
                            b.Background = productData.Background;
                            b.Foreground = productData.Font;
                        }
                    }
                }
            }
        }

        public static void ButtonMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Button) sender;

            var coordinatesText = button.Name;
            if (button.Name.IndexOf('_') == 0) coordinatesText = coordinatesText.Remove(0, 1);
            var array = coordinatesText.Split('x');
            var x = array[0].Substring(2, array[0].Length - 2).ToInt();
            var y = array[1].ToInt();

            if (button.Name.IndexOf('_') == 0)
            {
                var i = MainAppWindow.I;
                var j = MainAppWindow.J;
                var w = new WSetProduct
                        {
                            Owner = MainAppWindow,
                            X = x,
                            Y = y
                        };

                var gridElm = ClassGridProduct.Grid;

                if (gridElm[i, j, x, y] != null)
                {
                    var background = (SolidColorBrush) gridElm[i, j, x, y].Background;
                    var font = (SolidColorBrush)gridElm[i, j, x, y].Font;

                    if (background != null)
                        w.xColor.SelectedColor = background.Color;

                    if (font != null)
                        w.xFontColor.SelectedColor = font.Color;

                    w.FindProduct.Product =
                        RepositoryProduct.Products.FirstOrDefault(p => p.Name == gridElm[i, j, x, y].Description);
                }
                w.ShowDialog();
            }
            else
            {
                var we = new SettingsWindow(x,y)
                         {
                             Owner = MainAppWindow,
                             xCaption = {Text = button.Content?.ToString() ?? ""},
                             xColor = {Background = button.Background},
                             xColorFont = {Background = button.Foreground}
                         };

                var elm = button.Tag as ClassGridGroup.Elm;
                we.SetSelected(we.xGBFunction, elm != null ? elm.Func : "None");

                try
                {
                    we.xColor.SelectedColor = ((SolidColorBrush) button.Background).Color;
                    we.xColorFont.SelectedColor = ((SolidColorBrush) button.Foreground).Color;
                }
                catch
                {
                    // ignored
                }
                we.Sub = button.Name.Substring(0, 1);
                we.ShowDialog();
            }
        }

        private static void OpenWCountrys()
        {
            Effect(new WStat());
        }

        private static void ShowGridStatNationPopup(object sender)
        {
            var w = new WGrid
                    {
                        Owner = Window.GetWindow((Button) sender) as WStat
                    };

            Effect(w);
        }

        private static void ShowGridStatPlaceArrond(object sender)
        {
            var w = new Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WGrid
                    {
                        Owner = Window.GetWindow((Button) sender) as WStat
                    };

            Effect(w);
        }

        private static void ShowInsertNation(object sender, object arg)
        {
            var w = new WAdd
                    {
                        Snp = (List<StatNationPopup>) arg,
                        Owner = Window.GetWindow((Button) sender) as WGrid
                    };

            Effect(w);
        }

        private static void ShowModifNation(object sender, object arg, object arg1)
        {
            var w = new WMod
                    {
                        Snp = (List<StatNationPopup>) arg
                    };

            var elm = arg1 as StatNationPopup;
            w.xNameNation.Text = elm.NameNation;
            w.xQTY.Text = elm.Qty.ToString();
            w.CustomerId = elm.CustomerId;
            w.Owner = (Window.GetWindow((Button) sender) as WGrid);

            Effect(w);
        }

        private static void ShowDeleteNation(object sender, object arg, object arg1)
        {
            var w = new WDel
                    {
                        Snp = (List<StatNationPopup>) arg
                    };

            var elm = arg1 as StatNationPopup;

            w.xNameNation.Text = elm.NameNation;
            w.xQTY.Text = elm.Qty.ToString();
            w.CustomerId = elm.CustomerId;
            w.Owner = (Window.GetWindow((Button) sender) as WGrid);

            Effect(w);
        }

        private static void ShowInsertPlaceArrond(object sender, object arg)
        {
            var w = new Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WAdd
                    {
                        Snp = (List<StatPlaceArrond>) arg,
                        Owner = Window.GetWindow((Button) sender) as Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WGrid
                    };

            Effect(w);
        }

        private static void ShowModifPlaceArrond(object sender, object arg, object arg1)
        {
            var w = new Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WMod
                    {
                        Snp = (List<StatPlaceArrond>) arg
                    };

            var elm = arg1 as StatPlaceArrond;
            w.xNamePlaceArrond.Text = elm.NamePlaceArrond;
            w.xQTY.Text = elm.Qty.ToString();
            w.CustomerId = elm.CustomerId;
            w.Owner = (Window.GetWindow((Button) sender) as Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WGrid);

            Effect(w);
        }

        private static void ShowDeletePlaceArrond(object sender, object arg, object arg1)
        {
            var w = new Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WDel
                    {
                        Snp = (List<StatPlaceArrond>) arg
                    };

            var elm = arg1 as StatPlaceArrond;

            w.xNamePlaceArrond.Text = elm.NamePlaceArrond;
            w.xQTY.Text = elm.Qty.ToString();
            w.CustomerId = elm.CustomerId;
            w.Owner = (Window.GetWindow((Button) sender) as Winows.OtherWindows.Statistique.ModifStatPlaceArrond.WGrid);

            Effect(w);
        }

        private static void ShowGridProduct()
        {
            Effect(new WGridProduct());
        }

        private static void ShowReturn()
        {
            Effect(new WReturn());
        }

        private static void ShowReturnProduct(object sender, object arg)
        {
            Effect(new WReturnProduct(arg));
            var wReturn = (WReturn) Window.GetWindow((Button) sender);
            wReturn?.Close();
        }

        private static void AnnulationDeTicket()
        {
            if (!RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").Any())
                Effect(new WAnnulationDeTicket());

            else ShowMessageSb(Resources.LabelEditListProducts);
        }

        private static void ClickAnnulationDeTicket(object sender)
        {
            if (!SyncData.IsSync)
            {
                var w = Window.GetWindow((Button) sender) as WAnnulationDeTicket;

                var bc = w.codebare_.Text.Trim();

                if (CheckService.GetCheckFromChecksAndDelete(bc))
                {
                    w.Close();
                    w.codebare_.Text = "";
                }
                else
                {
                    ShowMessageTime("pas trouvé");
                    w.codebare_.Text = "";
                }
            }
            else ShowMessageSb("Synchronisation en cours, a quitté plus tard");
        }

        private static void ShowRemoveProduct(object arg)
        {
            Effect(new WRemoveProduct(arg));
        }

        private static void ShowFindProduct(object sender)
        {
            Effect(new WFindProduct {Owner = Window.GetWindow((Button) sender) as WGridProduct});
        }

        private static void SetProduct(object sender, object arg)
        {
            var windowProducts = ClassEtcFun.GetParents((Button) sender, 0) as WGridProduct;

            if (windowProducts != null && arg != null)
            {
                CheckService.AddProductCheck((ProductType)arg, GetQty(MainAppWindow.qty_label));
                windowProducts.Close();
            }
        }

        private static void ShowBallanceWindow(object arg)
        {
            Effect(new WBallance(arg));
        }

        private static void ShowAppCalculator()
        {
            Effect(new AppCalculator());
        }

        private static void ShowEnAttente()
        {
            Effect(new WEnAttente());
        }

        private static void ShowHistory()
        {
            Effect(new WAllHistory());
        }

        private static void ShowHistoryLocal()
        {
            Effect(new WHistory());
        }

        private static void ShowGeneralHistoryOfClosing()
        {
            Effect(new WgHistory());
        }

        private static void CloseAnyWindows(object sender)
        {
            var w = Window.GetWindow((Button) sender);

            var mw = w as MainWindow;
            if (mw?.gidLeft.Visibility == Visibility.Hidden)
            {
                mw.gidLeft.Visibility = Visibility.Visible;
                mw.xGrid.Visibility = Visibility.Hidden;
                mw.I = -1;
                mw.J = -1;
                for (var i = 0; i < ClassGridProduct.Grid.GetLength(2); i++)
                {
                    for (var j = 0; j < ClassGridProduct.Grid.GetLength(3); j++)
                    {
                        var b = (Button) mw.FindName("_b_" + i + "x" + j);

                        if (b != null)
                        {
                            b.ClearValue(Control.ForegroundProperty);
                            b.ClearValue(Control.BackgroundProperty);

                            b.ToolTip = null;
                            ((TextBlock) b.Content).Text = "";
                            //        b.Background = Class.ClassGridProduct.grid[I, J, i, j].background;
                            //      b.Foreground = Class.ClassGridProduct.grid[I, J, i, j].font;
                        }
                    }
                }
            }

            if (w is WProduct) w.Close();
            if (w is WGridProduct) w.Close();
        }

        private static void MoveToCheck(object sender)
        {
            var wp = ClassEtcFun.GetParents((Button) sender, 0) as WProduct;

            if (wp != null)
            {
                var productName = ((TextBlock) ((Button) sender).Content).Text;

                if (productName.Trim().Length > 0)
                {
                    var x = RepositoryProduct.GetXElementByElementName("Name", productName.Trim().ToUpper());

                    if (x != null)
                        CheckService.AddProductCheck(x, GetQty(MainAppWindow.qty_label));
                }
                wp.Close();
            }
        }

        private static void MoveToCheck(object sender, Guid guidProduct)
        {
            var wp = ClassEtcFun.GetParents((Button) sender, 0) as WProduct;
            var mw = Window.GetWindow((Button) sender) as MainWindow;

            if (mw != null)
            {
                var product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == guidProduct);

                if (product != null)
                {
                    var productElement = ProductType.ToXElement(product, null);
                    CheckService.AddProductCheck(productElement, GetQty(mw.qty_label));
                }
            }

            if (wp != null)
            {
                var product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == guidProduct);

                if (product != null)
                {
                    var productElement = ProductType.ToXElement(product, null);
                    CheckService.AddProductCheck(productElement, GetQty(MainAppWindow.qty_label));
                }
            }
        }

        private static void NumpadClick(object sender)
        {
            var b = (Button) sender;
            var number = b.Content.ToString();
            var w = Window.GetWindow(b);

            if (w is MainWindow)
            {
                var tb = MainAppWindow.qty_label;
                // changePayButton(mw, number);
                if (_setBc)
                {
                    var txb = MainAppWindow.xProduct;
                    switch (number.TrimEnd())
                    {
                        case "cl":
                            txb.Text = "";
                            break;
                        case ",enter":
                            MainAppWindow.KeyReturn();
                            SetBarCode(b);
                            break;
                        default:
                            txb.Text += int.Parse(number);
                            break;
                    }
                }
                else
                    switch (number.TrimEnd())
                    {
                        case "cl":
                            tb.Text = "__";
                            break;
                        case ",enter":
                            tb.Text = tb.Text.Replace(" X ", "").Replace("_", "") + "," + " X ";
                            break;
                        default:
                            tb.Text = tb.Text.Replace(" X ", "").Replace("_", "") + int.Parse(number) + " X ";
                            break;
                    }
            }

            if (w is WGridPay)
            {
                var mw = w as WGridPay;
                var c = b.Content.ToString();
                var cc = mw.CountCurrency;
                var s = cc + c;
                if (b.Content.ToString() == "cl") s = "";

                foreach (var ls in ClassEtcFun.FindVisualChildren<Label>(mw))
                {
                    if ((ls.Name != "") && (ls.Name.Substring(0, 4) == "mla_") && ((Button) ((StackPanel) ls.Parent).Parent).ToolTip != null)
                    {
                        ls.Content = s.Length > 0 ? s + " X " : "";
                        mw.CountCurrency = s;
                    }
                }
            }
        }

        private static void ClickClearCurrency(object sender)
        {
            var ws = Window.GetWindow((Button) sender) as WGridPay;
            RepositoryCurrencyRelations.ClearCurrency(ws.TypesPay);
            var sumMoney = RepositoryCurrencyRelations.Currencys.Sum(l => l.CurrencyMoney);
            var m = new EuroConverter(sumMoney);
            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent + " centime(s)";
            var ms = new EuroConverter(0.0m);
            ws.lblGet.Content = ms.Euro + " euro(s) et " + ms.Cent.ToString("00") + " centime(s)";
            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent.ToString("00") + " centime(s)";
            ws.ListBoxlog.Items.Clear();
        }

        private static void ClickCurrency(object sender)
        {
            if (((Button) sender).Tag != null)
            {
                Guid g;

                if (Guid.TryParse(((Button) sender).Tag.ToString(), out g))
                {
                    var ws = Window.GetWindow((Button) sender) as WGridPay;
                    var currency = RepositoryCurrency.Currencys.Find(l => l.CustomerId == g);
                    var count = int.Parse((ws.CountCurrency == "") || (ws.CountCurrency == null) ? "1" : ws.CountCurrency);
                    AddCurrency(currency, count, ws);
                }
            }
        }

        private static void ClickOkCurrency(object sender)
        {
            var ws = Window.GetWindow((Button) sender) as WGridPay;
            WPayEtc w = null;
            WTypePay wind = null;
            TypePay typePay = null;
            var f = true;

            if (ws == null)
            {
                w = Window.GetWindow((Button) sender) as WPayEtc;
                const decimal d = 0.0m;
                var resis = RepositoryCurrencyRelations.Residue();

                if ((((Button) sender).ToolTip.ToString() == "click_ok_Currency") && d > resis)
                {
                    ShowMessageSb("Montant ne doit pas dépasser de la somme total");
                    w.tbS.Text = RepositoryCurrencyRelations.Residue().ToString();
                    f = false;
                }
                else
                {
                    typePay = w.TypesPay;
                    wind = w.Owner as WTypePay;
                    RepositoryCurrencyRelations.Pay(typePay, decimal.Parse(w.tbS.Text));
                }
            }
            else
            {
                typePay = ws.TypesPay;

                if (RepositoryCurrencyRelations.Currencys.Count > 0)
                {
                    RepositoryCurrencyRelations.Pay(typePay, RepositoryCurrencyRelations.Currencys.Sum(l => l.CurrencyMoney));
                    RepositoryCurrencyRelations.Currencys.Clear();
                }

                wind = ws.Owner as WTypePay;
            }
            if (f)
            {
                var calc = ClassEtcFun.RenduCalc();

                if (calc == 0)
                    PrintCheck(sender);
                if (calc < 0)
                {
                    var m = new EuroConverter(calc);

                    ClassCustomerDisplay.OddMoney(calc, RepositoryCurrencyRelations.GetMoneyFromType(typePay));

                    if (MainAppWindow != null)
                        MainAppWindow.lrendu.Content = "Rendu: " + calc.ToString("0.00") + " €";

                    PrintCheck(sender);

                    ShowMessageTime("Rendu : " + m.Euro + " euro(s) et " + m.Cent + " centime(s) ");
                }
                if (calc > 0)
                {
                    ws?.Close();
                    w?.Close();

                    if (wind == null)
                    {
                        wind = new WTypePay();
                        Effect(wind);
                    }
                    else
                    {
                        CollectionViewSource.GetDefaultView(wind.dataGrid1.ItemsSource).Refresh();
                        var mReste = new EuroConverter(RepositoryCurrencyRelations.Calc());
                        wind.lblReste.Content = mReste.Euro + " euro(s) et " + mReste.Cent + " centime(s)";
                        ClassCustomerDisplay.Reste(RepositoryCurrencyRelations.Calc(), RepositoryCurrencyRelations.GetSumMoney());
                    }
                }
            }
        }

        private static void ClickOkPayType(object sender)
        {
            if (RepositoryCurrencyRelations.Calc() > 0)
                ShowMessageTime(Resources.LabelNotEnoughMoney);
            else
                PrintCheck(sender);
        }

        private static void CancelLine(object sender)
        {
            if (!RepositoryAccountUser.LoginedUser.Role.IsPermiss(Privelege.DeleteProductFromCheck))
            {
                MessageBox.Show(Resources.LabelNotPermission, Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);

                var isNext = false;
                var window = new TextWindow(false, Resources.LabelPin) {BtnText = Resources.BtnOk};
                if (window.ShowDialog() == true)
                    if (!string.IsNullOrEmpty(window.NameText) && RepositoryAccountUser.AccountUsers.FirstOrDefault(user => user.PinCode == window.NameText) != null)
                        isNext = true;
                if (!isNext) return;
            }

            var detailsWindow = Window.GetWindow((Button) sender) as WDetailsProducts;

            var productElement = detailsWindow != null ? (XElement) detailsWindow.ProductsGrid.SelectedItem : (XElement) MainAppWindow.GridProducts.SelectedItem;

            if (productElement != null)
            {
                if (MainAppWindow.CheckOriginalDocument == null || detailsWindow != null)
                {
                    CheckService.DelProductCheck(productElement.GetXElementValue("ii").ToInt());
                    RepositoryDiscount.Client.ProcentDefault = 0;
                    MainAppWindow.qty_label.Text = "__";
                    MainAppWindow.CheckOriginalDocument = null;
                    MainAppWindow.GridProducts.DataContext = RepositoryCheck.DocumentProductCheck.Root;

                    detailsWindow?.SetDataSet();
                }
                else
                {
                    var dp = new WDetailsProducts();
                    dp.ShowDialog();
                }

                if (RepositoryDiscount.Client.Procent > 0)
                    CheckService.DiscountCalc();
            }
        }

        private static void CancelTicket(object sender)
        {
            if (MainAppWindow.GridProducts.Items.Count > 0)
            {
                RepositoryDiscount.RestoreDiscount();
                CheckService.DiscountCalc();
                WriteTotal();
                CassieService.ClearProductsCheck();
                RepositoryDiscount.Client.ProcentDefault = 0;

                if (ClassProMode.ModePro)
                {
                    ClassProMode.ModePro = false;
                    ClassProMode.Pro = null;
                    WriteTotal();
                }

                if (RepositoryDiscount.Client.Procent > 0)
                    CheckService.DiscountCalc();

                MainAppWindow.qty_label.Text = "__";
                MainAppWindow.CheckOriginalDocument = null;
            }
        }

        private static void ShowPayment(object sender)
        {
            var tpom = false;
            ShowMessageCustomerDisplay(null);

            if (!GlobalVar.IsOpen)
                ShowMessageTime("La caisse est fermée");
            else
            {
                if (RepositoryCheck.DocumentProductCheck != null)
                {
                    if (RepositoryCheck.DocumentProductCheck.Element("check").Elements("product").ToList().Count > 0)
                    {
                        var d = CheckService.GetTotalPrice();

                        if (Window.GetWindow((Button) sender) is MainWindow)
                        {
                            RepositoryCurrencyRelations.SetMoneySum(d);
                            int idTp;
                            var b = int.TryParse(((Button) sender).ToolTip.ToString().Replace("_TypesPayDynamic", ""), out idTp);

                            if (b)
                            {
                                var money = GetMoney(MainAppWindow.qty_label);
                                var tp = RepositoryTypePay.GetById(idTp);
                                tpom = PayOneMoment(MainAppWindow, tp);

                                if (!tpom)
                                    RepositoryCurrencyRelations.Pay(tp, money);
                            }
                        }
                        if (!tpom)
                        {
                            if ((Window.GetWindow((Button) sender)).Owner == null)
                            {
                                var w = new WTypePay();
                                var mReste = new EuroConverter(RepositoryCurrencyRelations.Calc());
                                w.lblReste.Content = mReste.Euro + " euro(s) et " + mReste.Cent + " centime(s)";
                                ClassCustomerDisplay.Reste(RepositoryCurrencyRelations.Calc(), RepositoryCurrencyRelations.GetSumMoney());
                                w.Owner = MainAppWindow;
                                Effect(w);
                            }
                            else
                            {
                                (Window.GetWindow((Button) sender)).Close();
                                var w = (Window.GetWindow((Button) sender)).Owner as WTypePay;
                            }
                        }
                    }
                }
            }
        }

        private static void ShowPaymentEtc(object sender, decimal money)
        {
            ShowMessageCustomerDisplay(null);

            if (!GlobalVar.IsOpen)
                ShowMessageTime(Resources.LabelCashBoxIsClosed);
            else
            {
                var payId = int.Parse(((Button) sender).ToolTip.ToString().Replace("TypesPayDynamic", ""));
                var payType = RepositoryTypePay.GetById(payId);

                if (RepositoryCheck.DocumentProductCheck != null)
                    if (RepositoryCheck.DocumentProductCheck.GetXElements("check","product").Any())
                    {
                        var payWindow = new WPayEtc
                                {
                                    Owner = Window.GetWindow((Button) sender),
                                    TypesPay = payType,
                                    MaxMoney = money
                                };
                        payWindow.numPad.Tag = payWindow;

                        Effect(payWindow);
                    }
            }
        }

        private static void BEnter_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void OpenCashBox()
        {
            if (RepositoryCurrencyRelations.IsExistDrawerTypeInDocument())
                try
                {
                  //  ClassUsbTicket.ReadWrite.Open();
                }
                catch
                {
                    ShowMessageSb("Ошибка с CashBox");
                }
        }

        private static bool PayOneMoment(object window, TypePay typePay)
        {
            var mainWindow = window as MainWindow;
            var r = false;

            if (mainWindow != null)
            {
                var money = GetMoney(mainWindow.qty_label);

                if ((money == 0) || (RepositoryCurrencyRelations.GetSumMoney() <= money))
                    if (typePay.CurMod ?? false)
                    {
                        RepositoryCurrencyRelations.Pay(typePay, money == 0 ? RepositoryCurrencyRelations.GetSumMoney() : money);
                        var calc = ClassEtcFun.RenduCalc();

                        if (calc == 0)
                            PrintCheck(null);
                        else
                        {
                            var m = new EuroConverter(RepositoryCurrencyRelations.Residue());
                            ClassCustomerDisplay.OddMoney(RepositoryCurrencyRelations.Residue(), money);
                            MainAppWindow.lrendu.Content = "Rendu: " + RepositoryCurrencyRelations.Residue().ToString("0.00") + " €";
                            PrintCheck(null);

                            ShowMessageTime("Rendu : " + m.Euro + " euro(s) et " + m.Cent + " centime(s) ");
                        }
                        r = true;
                    }
                    else
                    {
                        if ((money == 0) || (RepositoryCurrencyRelations.GetSumMoney() == money))
                        {
                            RepositoryCurrencyRelations.Pay(typePay, money == 0 ? RepositoryCurrencyRelations.GetSumMoney() : money);
                            var calc = ClassEtcFun.RenduCalc();
                            if (calc == 0)
                            {
                                PrintCheck(null);
                                r = true;
                            }
                        }
                    }
            }

            return r;
        }

        private static void ShowPaymentEtcDynamyc(object sender)
        {
            //ShowMessageCustomerDisplay(null);
            if (!GlobalVar.IsOpen)
                ShowMessageTime("La caisse est fermée");
            else
            {
                if (((Button) sender).ToolTip != null)
                    if (RepositoryCheck.DocumentProductCheck != null)
                        if (RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").ToList().Count > 0)
                        {
                            var money = CheckService.GetTotalPrice();

                            if (Window.GetWindow((Button) sender) is MainWindow)
                                RepositoryCurrencyRelations.SetMoneySum(money);

                            var t = RepositoryTypePay.GetById(int.Parse(((Button) sender).ToolTip.ToString().Replace("TypesPayDynamic", "")));
                            if (!PayOneMoment(Window.GetWindow((Button) sender), t))
                                if (t.CurMod ?? false)
                                {
                                    var ms = new EuroConverter(RepositoryCurrencyRelations.Residue());

                                    var w = new WGridPay
                                            {
                                                Owner = Window.GetWindow((Button) sender),
                                                TypesPay = t,
                                                lblSum = {Content = ms.Euro + "," + ms.Cent + "€"}
                                            };

                                    Effect(w);
                                    RepositoryCurrencyRelations.ClearCurrency(t);
                                }
                                else ShowPaymentEtc(sender, money);
                        }
            }
        }

        private static void PrintCheck(object sender)
        {
            OpenCashBox();

            if (sender != null) (Window.GetWindow((Button) sender)).Close();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (WTypePay))
                    ((WTypePay) window).Close();
                if (window.GetType() == typeof (MainWindow))
                {
                    var dg = MainAppWindow.GridProducts;
                    if (dg.Items.Count > 0)
                    {
                        CheckService.Bay();
                        dg.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");
                        RepositoryCurrencyRelations.Document = null;
                        MainAppWindow.qty_label.Text = "__";
                        RepositoryDiscount.Client.ProcentDefault = 0;
                        MainAppWindow.CheckOriginalDocument = null;
                    }
                }
            }

            WriteTotal();
        }

        private static void EnAttenete()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (MainWindow))
                {
                    var dg = MainAppWindow.GridProducts;
                    if (dg.Items.Count > 0)
                    {
                        CheckService.EnAttenete();
                        ShowMessageTime(Resources.LabelInputInMemory);
                        dg.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");
                        CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
                    }
                    else
                        ShowEnAttente();
                }

                if (window.GetType() == typeof (WTypePay))
                    ((WTypePay) window).Close();
            }
        }

        private static void WriteOff()
        {
            var grid = MainAppWindow.GridProducts;
            if (grid.Items.Count > 0)
            {
                var date = DateTime.Now;
                var elements = grid.Items.Cast<XElement>().ToList();
                foreach (var item in elements)
                {
                    var id = item.GetXElementValue("CustomerId").ToGuid();
                    var count = item.GetXElementValue("qty").ToDecimal();
                    var product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == id);
                    if (product != null)
                        RepositoryProduct.WriteOffProductCount(product, count, date);
                }
                RepositoryCheck.WriteOff(elements);
                var window = new WMessageTime(Resources.LabelOperationComplete);
                window.ShowDialog();
            }
        }

        private static void ShowClosingCash()
        {
            var countBufCheck = RepositoryCheck.GetCountOfProductInCheckFromBuf() + RepositoryCheck.GetCountOfProductInCheckFromEnAttenete();

            if (countBufCheck == 0)
                Effect(new WCloseTicketWindow(""));
            else
                ShowMessageSb(Resources.LabelEditListProducts + Environment.NewLine + Resources.LabelQty.ToLower() + " " +
                              countBufCheck);
        }

        private static void CloseGeneral(object sender)
        {
            var windowCloseTicket = Window.GetWindow((Button) sender) as WCloseTicketWindow;
            if (windowCloseTicket != null)
            {
                var worker = new BackgroundWorker();
                windowCloseTicket.BtnCloseGeneral.IsEnabled = false;
                ProgressHelper.Instance.IsIndeterminate = true;
                ProgressHelper.Instance.Start(1, Resources.LabelClose);

                worker.DoWork += (s, e) =>
                                 {
                                     RepositoryGeneral.IsOpen = !CassieService.CloseWithMessage();
                                 };
                worker.RunWorkerCompleted += (s, e) =>
                                             {
                                                 if (!RepositoryGeneral.IsOpen)
                                                     CloaseMainWindow();

                                                 windowCloseTicket.errorlist.Text = RepositoryGeneral.Mess;
                                                 ProgressHelper.Instance.Stop();
                                             };
                worker.RunWorkerAsync();
            }
        }

        public static void CloseTicketWindow(object sender)
        {
            var closeTicketWindow = Window.GetWindow((Button) sender) as WCloseTicketWindow;

            if (closeTicketWindow != null)
            {
                var worker = new BackgroundWorker();
                closeTicketWindow.BtnCloseLocal.IsEnabled = false;
                ProgressHelper.Instance.IsIndeterminate = true;
                ProgressHelper.Instance.Start(1, Resources.LabelClose);

                worker.DoWork += (s, e) => { CassieService.Close(); };
                worker.RunWorkerCompleted += (s, e) =>
                                             {
                                                 closeTicketWindow.errorlist.Text = CassieService.Mess;
                                                 ProgressHelper.Instance.Stop();
                                             };
                worker.RunWorkerAsync();
            }
        }

        private static int _selectedBefore = -1;
        private static int _selectedAfter = -1;
        private static XDocument ShowMessageCustomerDisplay(XDocument oldXDocument)
        {
            XDocument result = null;

            if (RepositoryCheck.C == null && MainAppWindow != null && RepositoryCheck.DocumentProductCheck != null)
            {
                if (oldXDocument != null)
                {
                    _selectedAfter = MainAppWindow.GridProducts.SelectedIndex;
                    RepositoryCheck.C = oldXDocument;
                }
                else
                {
                    _selectedBefore = MainAppWindow.GridProducts.SelectedIndex;
                    RepositoryCheck.C = new XDocument(RepositoryCheck.DocumentProductCheck);
                    result = new XDocument(RepositoryCheck.DocumentProductCheck);
                    RepositoryCheck.C = RepositoryActionHashBox.MergeProductsInCheck(RepositoryCheck.C);
                }
                MainAppWindow.GridProducts.DataContext = RepositoryCheck.C.Element("check");
                MainAppWindow.GridProducts.Items.Refresh();
                ClassCustomerDisplay.TotalSum(CheckService.GetTotalPrice());

                if (oldXDocument != null)
                {
                    if (_selectedBefore >= 0)
                        MainAppWindow.GridProducts.SelectedIndex = _selectedBefore;
                }
                else
                {
                    if (_selectedAfter >= 0)
                        MainAppWindow.GridProducts.SelectedIndex = _selectedAfter;
                }
            }
            else
            {
                MainAppWindow.GridProducts.DataContext = RepositoryCheck.DocumentProductCheck.Element("check");
                RepositoryCheck.C = null;
                WriteTotal();
            }
            return result;
        }

        private static void ShowDiscountCards()
        {
            Effect(new WInfoClients());
        }

        private static void ShowDiscount()
        {
            Effect(new WDiscount());
        }

        private static void ShowDiscountMini(object sender)
        {
            var dm = new WDiscountMini
                     {
                         Owner = Window.GetWindow((Button) sender)
                     };

            Effect(dm);
        }

        private static void ClickDiscountMini(object sender)
        {
            var b = sender as Button;
            var procent = decimal.Parse(b.Content.ToString().Replace("%", ""));
            var dm = Window.GetWindow(b) as WDiscountMini;
            var mw = dm.Owner as MainWindow;

            if ((mw != null) && (RepositoryCheck.DocumentProductCheck != null))
            {
                RepositoryDiscount.Client.ProcentDefault = procent;
                CheckService.DiscountCalc();
                WriteTotal();
            }

            dm.Close();
        }

        private static void DiscountCardSet(object sender)
        {
            var b = sender as Button;
            if (RepositoryCheck.DocumentProductCheck != null)
                if (!RepositoryDiscount.Client.DiscountSet && (RepositoryDiscount.Client.Barcode != null))
                    if (RepositoryDiscount.Client.Points >= RepositoryDiscount.Client.MaxPoints)
                    {
                        RepositoryDiscount.Client.DiscountSet = true;
                        RepositoryDiscount.Client.Points -= RepositoryDiscount.Client.MaxPoints;
                        b.Content = "20 %";
                        RepositoryDiscount.Client.Procent = 20.0m;
                        CheckService.DiscountCalc();
                        WriteTotal();
                    }
                    else
                        b.Content = "Не хватае " + (RepositoryDiscount.Client.MaxPoints - RepositoryDiscount.Client.Points);
                else
                {
                    RepositoryDiscount.Client.Procent = 0;
                    RepositoryDiscount.Client.DiscountSet = false;
                    RepositoryDiscount.Client.Points += RepositoryDiscount.Client.MaxPoints;
                    b.Content = "Set Discount";
                    CheckService.DiscountCalc();

                    WriteTotal();
                }
        }

        private static void ResetSettings()
        {
            Settings.Default.Reset();
        }

        private static void OpenCashBox2()
        {
       //     ClassUsbTicket.ReadWrite.Open();
        }

        private static void ValidateBallance(object sender)
        {
            var window = Window.GetWindow((Button) sender) as WBallance;
            var p = ProductType.FromXElement(ProductType.ToXElement(window.Product, null));

            decimal outp;
            if (decimal.TryParse(window.xBallance_kg.Text.Replace(".", ","), out outp))
            {
                p.Qty = outp;
                decimal newpice;

                if (decimal.TryParse(window.xPrix.Text.Replace(".", ","), out newpice))
                {
                    if ((p.Qty > 0) && (newpice > 0))
                    {
                        p.Price = newpice;
                        CheckService.AddProductCheck(p, p.Qty);
                        ((WBallance) Window.GetWindow((Button) sender)).Close();
                    }
                    else ShowMessageSb("error QTY=" + p.Qty + " or  Prix=" + newpice);
                }
                else
                    ShowMessageSb("Error prix");
            }
        }

        private static Guid ProductGuid(string tooltip)
        {
            const string findText = "Products id=[";

            if (tooltip.IndexOf(findText) != -1)
            {
                var guid = tooltip.Replace(findText, "").Replace("]", "");
                return Guid.Parse(guid);
            }

            return Guid.Empty;
        }

        private static void ShowModifierPrix(object sender)
        {
            if (RepositoryCheck.C == null)
            {
                var mainWindow = Window.GetWindow((Button) sender) as MainWindow;

                XElement xElement;

                if (mainWindow != null)
                    xElement = mainWindow.GridProducts.SelectedItem as XElement;
                else
                    xElement = ((MainWindow) ClassEtcFun.FindWindow("MainWindow_")).GridProducts.SelectedItem as XElement;

                if (xElement != null)
                {
                    var wp = new WModifierPrix(ProductType.FromXElement(xElement)) {Owner = mainWindow };
                    Effect(wp);
                }
            }
            else
                ShowMessageCustomerDisplay(null);
        }

        private static void ClickModifierPrix(object sender, bool flag)
        {
            var window = Window.GetWindow((Button) sender) as WModifierPrix;

            if (window != null)
            {
                window.Product.Price = window.Price;
                window.Product.Name = window.XNameProduct.Text;

                if (flag) RepositoryProduct.UpdateProductPrice(window.Product);

                if (window.Product.Price != 0)
                    CheckService.ModifProductCheck(window.Product.Ii, window.Product.Price, window.Product.Name, window.Product.Qty);
                else CheckService.DelProductCheck(window.Product.Ii);

                WriteTotal();
            
                window.Close();
            }
        }

        private static void SetBarCode(object sender)
        {
            var mw = (Window.GetWindow((Button) sender) as MainWindow);
            _setBc = !_setBc;
            mw.xProduct.BorderThickness = _setBc ? new Thickness(3.0) : new Thickness(1.0);
        }

        private static void CloseWindow(object sender)
        {
            var f = ClassEtcFun.GetParentWindow((Button) sender);
            f?.Close();
        }

        private static void RunFun(object sender, object arg, object arg1)
        {
            var frameworkElement = (FrameworkElement)sender;
            var elm = frameworkElement.Tag as ClassGridGroup.Elm;

            var bN = string.Empty;

            var button = sender as Button;
            if (button != null)
            {
                bN = button.Name.Substring(0, button.Name.Length > 1 ? 2 : 0);
                if (bN == "m_") ClickCurrency(sender);
                if (bN == "c_") ShowPaymentEtcDynamyc(sender);
            }

            if ((bN != "m_") && (bN != "c_"))
            {
                var typeFun = elm != null ? elm.Func ?? "" : "";

                if (string.IsNullOrEmpty(typeFun) && frameworkElement.Tag != null)
                    typeFun = frameworkElement.Tag.ToString();

                if (string.IsNullOrEmpty(typeFun) && frameworkElement.ToolTip != null)
                    typeFun = frameworkElement.ToolTip.ToString();

                var prodGuid = ProductGuid(typeFun);

                if (prodGuid != Guid.Empty)
                    if (MainAppWindow.GridProducts.Visibility == Visibility.Visible)
                        MoveToCheck(sender, prodGuid);
                    else
                        MainAppWindow.AddStock(prodGuid);
                else
                    switch (typeFun)
                    {
                        case "SetStock":
                            if (MainAppWindow.GridProducts.Visibility == Visibility.Visible)
                            {
                                MainAppWindow.GridProducts.Visibility = Visibility.Collapsed;
                                MainAppWindow.BlockStock.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                MainAppWindow.GridProducts.Visibility = Visibility.Visible;
                                MainAppWindow.BlockStock.Visibility = Visibility.Collapsed;
                            }
                            break;
                        case "Countrys":
                            OpenWCountrys();
                            break;
                        case "Section des Articles":
                            OpenWProduct(sender);
                            break;
                        case "Fermer":
                            CloaseMainWindow();
                            break;
                        case "Supprimer une Ligne":
                            CancelLine(sender);
                            break;
                        case "Supprimer le Ticket":
                            CancelTicket(sender);
                            break;
                        case "Modification des Articles":
                            ShowGridProduct();
                            break;
                        case "Le mode de Paiement":
                            ShowPayment(sender);
                            break;
                        case "Add Product":
                            if (RepositoryTva.Tvases.Count == 0)
                            {
                                MessageBox.Show(Resources.LabelNeedInputNds, Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                                break;
                            }
                            if (RepositoryGroupProduct.GroupProducts.Count == 0)
                            {
                                MessageBox.Show(Resources.LabelNeedInputGroupsAndSubgroups, Resources.LabelWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
                                break;
                            }
                            Effect(
                                new WAddProduct());
                            break;
                        case "Change Product":
                            if (arg != null)
                            {
                                if (RepositoryTva.Tvases.Count == 0)
                                {
                                    MessageBox.Show(Resources.LabelNeedInputNds, Resources.LabelWarning, MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                                    break;
                                }
                                if (RepositoryGroupProduct.GroupProducts.Count == 0)
                                {
                                    MessageBox.Show(Resources.LabelNeedInputGroupsAndSubgroups, Resources.LabelWarning,
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                                    break;
                                }
                                Effect(new WAddProduct((ProductType) arg));
                            }
                            break;
                        case "Set Product":
                            SetProduct(sender, arg);
                            break;
                        //  case "Change Product": show_change_product(sender, arg); break;
                        case "Remove Product":
                            ShowRemoveProduct(arg);
                            break;
                        case "Find Product":
                            ShowFindProduct(sender);
                            break;
                        case "closing_TWG":
                            CloseGeneral(sender);
                            break;
                        case "closing_TW":
                            CloseTicketWindow(sender);
                            break;
                        case "Products":
                            MoveToCheck(sender);
                            break;
                        case "numpad":
                            NumpadClick(sender);
                            break;
                        case "click_ok_pay_type":
                            ClickOkPayType(sender);
                            break;
                        case "Attente +1":
                            EnAttenete();
                            break;
                        case "Clôture":
                            ShowClosingCash();
                            break;
                        case "Bande de Contrôle":
                            ShowHistory();
                            break;
                        case "currencyClear":
                            ClickClearCurrency(sender);
                            break;
                        case "WriteOff":
                            WriteOff();
                            break;
                        case "click_ok_Currency":
                            ClickOkCurrency(sender);
                            break;
                        case "Retours et Remboursements":
                            ShowReturn();
                            break;
                        case "click_return_product":
                            ShowReturnProduct(sender, arg);
                            break;
                        case "ShowBallance":
                            ShowBallanceWindow(arg);
                            break;
                        case "Histoire de ticket":
                            ShowHistoryLocal();
                            break;
                        case "Afficher un Total":
                            MainAppWindow.CheckOriginalDocument = ShowMessageCustomerDisplay(MainAppWindow.CheckOriginalDocument);
                            break;
                        case "TypesPayDynamic0":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic1":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic2":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic3":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic4":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic5":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic6":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic7":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic8":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic9":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic10":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic11":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic12":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic13":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic14":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic15":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic16":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic17":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic18":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic19":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "TypesPayDynamic20":
                            ShowPaymentEtcDynamyc(sender);
                            break;
                        case "_TypesPayDynamic0":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic1":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic2":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic3":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic4":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic5":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic6":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic7":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic8":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic9":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic10":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic11":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic12":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic13":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic14":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic15":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic16":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic17":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic18":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic19":
                            ShowPayment(sender);
                            break;
                        case "_TypesPayDynamic20":
                            ShowPayment(sender);
                            break;
                        case "Carte de Fidélité":
                            ShowDiscountCards();
                            break;
                        case "Reset main Grid":
                            ResetSettings();
                            break;
                        //   case "CaseProducts": ShowCaseProducts(sender); break;
                        //  case "Clavier": show_keyboard(sender); break;
                        //  case "ClavierNumPad": show_NumPad(sender); break;

                        //  case "Mode de clavier": show_WKEYNUM(sender); break;
                        //  case "ProductDivers": show_ProductDivers(sender); break;
                        //   case "ProductDiversBallance": show_ProductDivers(sender); break;
                        //   case "clickProductDivers": clickProductDivers(sender); break;
                        case "General History of closing":
                            ShowGeneralHistoryOfClosing();
                            break;
                        case "Open CashBox":
                            OpenCashBox2();
                            break;
                        case "Modifier le prix":
                            ShowModifierPrix(sender);
                            break;
                        case "Click modifier le prix":
                            ClickModifierPrix(sender, false);
                            break;
                        case "Click modifier le prix de BD":
                            ClickModifierPrix(sender, true);
                            break;
                        case "Discount":
                            ShowDiscount();
                            break;
                        case "DiscountMini":
                            ShowDiscountMini(sender);
                            break;
                        case "discountMiniClick":
                            ClickDiscountMini(sender);
                            break;
                        //   case "discountCalc": discountCalc_(sender); break;
                        case "DiscountSet":
                            if (!RepositoryDiscount.Client.DiscountSet)
                                DiscountCardSet(sender);
                            break;
                        case "svernut":
                            MinimizeWindow(sender);
                            break;
                        case "show_grid_StatNationPopup":
                            ShowGridStatNationPopup(sender);
                            break;
                        case "show_grid_StatPlaceArrond":
                            ShowGridStatPlaceArrond(sender);
                            break;
                        case "show_insert_nation":
                            ShowInsertNation(sender, arg);
                            break;
                        case "show_modif_nation":
                            ShowModifNation(sender, arg, arg1);
                            break;
                        case "show_delete_nation":
                            ShowDeleteNation(sender, arg, arg1);
                            break;
                        case "show_insert_PlaceArrond":
                            ShowInsertPlaceArrond(sender, arg);
                            break;
                        case "show_modif_PlaceArrond":
                            ShowModifPlaceArrond(sender, arg, arg1);
                            break;
                        case "show_delete_PlaceArrond":
                            ShowDeletePlaceArrond(sender, arg, arg1);
                            break;
                        case "SetBarCode":
                            SetBarCode(sender);
                            break;
                        case "validateBallance":
                            ValidateBallance(sender);
                            break;
                        case "Annulation de ticket":
                            AnnulationDeTicket();
                            break;
                        case "clickAnnulationDeTicket":
                            ClickAnnulationDeTicket(sender);
                            break;
                        case "Calculatrice":
                            ShowAppCalculator();
                            break;
                        case "Show Pro":
                            ShowProList(sender);
                            break;
                        case "closeWindow":
                            CloseWindow(sender);
                            break;
                        case "click Set Pro":
                            ClickSetPro(sender);
                            break;
                        case "click SetOff Pro":
                            ClickSetoffPro(sender);
                            break;
                        case "add pro":
                            ShowAddPro(sender);
                            break;
                        case "addPro":
                            ClickAddPro(sender);
                            break;
                        case "toDevis":
                            ToDevis();
                            break;
                        case "UpdateDB":
                            ShowUpdateDb();
                            break;
                        case "clickUpdateDB":
                            ClickUpdateDb();
                            break;
                        case "clickFullUpdateDB":
                            ClickFullUpdateDb();
                            break;
                        case "HistoryChangeProductsPrevious":
                            ClickHistoryChangeProductsPrevious(sender);
                            break;
                        case "HistoryChangeProductsPrint":
                            ClickHistoryChangeProductsPrint(sender);
                            break;
                        case "HistoryChangeProductsNext":
                            ClickHistoryChangeProductsNext(sender);
                            break;
                        default:
                            CloseAnyWindows(sender);
                            break;
                    }
            }
        }

        #region pro mode

        private static void ShowProList(object sender)
        {
            var w = Window.GetWindow((Button) sender) as MainWindow ?? ClassEtcFun.FindWindow("MainWindow_") as MainWindow;

            if (w != null)
            {
                var wp = new WProList {Owner = w};
                Effect(wp);
            }
        }

        private static void ClickSetPro(object sender)
        {
            var w = Window.GetWindow((Button) sender) as WProList;

            var select = w?.dataPro.SelectedItem as Pro;

            if (select != null)
            {
                ClassProMode.ModePro = true;
                ClassProMode.Pro = select;
                RepositoryCheck.DocumentProductCheck = ClassProMode.ReplaceCheck(RepositoryCheck.DocumentProductCheck, true);
                w.Close();
                WriteTotal();
            }
        }

        private static void ClickSetoffPro(object sender)
        {
            var w = Window.GetWindow((Button) sender) as WProList;

            if (w != null)
                if (ClassProMode.ModePro)
                {
                    ClassProMode.ModePro = false;
                    ClassProMode.Pro = null;
                    CancelTicket(null);
                    w.Close();
                }
        }

        private static void ShowAddPro(object sender)
        {
            var w = ClassEtcFun.GetParentWindow((Button) sender) as WProList ?? ClassEtcFun.FindWindow("w_pro_add") as WProList;

            if (w != null)
            {
                var wp = new WProAdd {Owner = w};
                Effect(wp);
            }
        }

        private static void ClickAddPro(object sender)
        {
            var w = Window.GetWindow((Button) sender) as WProAdd;

            if (w != null)
            {
                var pro = new Pro(Guid.NewGuid(),
                    Pro.SexToInt("M."),
                    "",
                    "",
                    w.xNameCompany.Text,
                    "",
                    "",
                    w.xAdress.Text,
                    w.xCodePostal.Text,
                    w.xVille.Text,
                    "",
                    "",
                    "",
                    w.xTel.Text,
                    w.xMail.Text,
                    "",
                    Guid.Empty,
                    Guid.Empty,
                    ""
                    );

                if (RepositoryPro.InsertToDb(pro) > 0)
                {
                    w.Close();
                    var wpl = w.Owner as WProList;
                    wpl.DataProLoaded(null, null);
                    wpl.dataPro.SelectedIndex = 0;
                    wpl.dataPro.SelectionMode = DataGridSelectionMode.Single;
                }
                else ShowMessageSb("error 233");
            }
        }

        private static void ToDevis()
        {
            ClassProMode.Devis = true;
            CheckService.Bay();
        }

        #endregion

        #region updatedata

        private static void ClickFullUpdateDb()
        {
            Config.FromLoadSyncAll = true;
            ClickUpdateDb();
        }

        private static BackgroundWorker _workerupd;

        private static void ShowUpdateDb()
        {
            var w = new WUpdateDb();
            w.Show();
        }

        private static void WorkerUpd(object sender, DoWorkEventArgs e)
        {
            SyncData.SetSunc(true);
            RepositoryProduct.Set();
        }

        private static void WorkerUpdCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var de = ClassEtcFun.FindWindow("_W_Message");
            de?.Close();

            var w = ClassEtcFun.FindWindow("w_upd") as WUpdateDb;

            if (w != null)
            {
                w.IsEnabled = true;
                w.Close();
                ShowUpdateDb();
            }
            SyncData.SetSunc(false);
            Config.FromLoadSyncAll = false;
        }

        private static void ClickUpdateDb()
        {
            if (!SyncData.IsSync)
            {
                var de = ClassEtcFun.FindWindow("_W_Message");
                de?.Close();

                ShowMessage(null, " please  waite....");

                _workerupd = new BackgroundWorker();

                _workerupd.DoWork += WorkerUpd;
                _workerupd.RunWorkerCompleted += WorkerUpdCompleted;

                _workerupd.RunWorkerAsync();
            }
        }

        private static void HistoryChangeProductsWButton(object sender, bool add)
        {
            var w = ClassEtcFun.GetParentWindow((Button) sender) as WUpdateDb;

            if (w != null)
            {
                w.SelectGroup = add ? w.SelectGroup + 1 : w.SelectGroup - 1;

                w.dataGrid1.DataContext =
                    RepositoryHistoryChangeProduct.Document.GetXElements("HistoryChangeProducts", "rec").Where(l => l.GetXElementValue("group") == w.SelectGroup.ToString());

                w.xN.IsEnabled =
                    RepositoryHistoryChangeProduct.Document.GetXElements("HistoryChangeProducts", "rec").Count(l => l.GetXElementValue("group") == (w.SelectGroup - 1).ToString()) !=
                    0;

                w.xP.IsEnabled =
                    RepositoryHistoryChangeProduct.Document.GetXElements("HistoryChangeProducts", "rec").Count(l => l.GetXElementValue("group") == (w.SelectGroup + 1).ToString()) !=
                    0;
            }
        }

        private static void ClickHistoryChangeProductsPrevious(object sender)
        {
            HistoryChangeProductsWButton(sender, true);
        }

        private static void ClickHistoryChangeProductsPrint(object sender)
        {
            var w = ClassEtcFun.GetParentWindow((Button) sender) as WUpdateDb;

            if (w != null)
                CassieService.PrintHistoryChangeProducts(w.SelectGroup);
        }

        private static void ClickHistoryChangeProductsNext(object sender)
        {
            HistoryChangeProductsWButton(sender, false);
        }

        #endregion
    }
}