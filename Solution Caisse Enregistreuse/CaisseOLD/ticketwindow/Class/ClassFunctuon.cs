using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using ticketwindow.Winows.AnnulationDeTicket;
using ticketwindow.Winows.Ballance;
using ticketwindow.Winows.DetailsProducts;
using ticketwindow.Winows.Discount;
using ticketwindow.Winows.Divers;
using ticketwindow.Winows.EnAttente;
using ticketwindow.Winows.History;
using ticketwindow.Winows.Message;
using ticketwindow.Winows.Payment;
using ticketwindow.Winows.Product;
using ticketwindow.Winows.Product.AddProduct;
using ticketwindow.Winows.Product.ChangeProduct;
using ticketwindow.Winows.Product.FindProduct;
using ticketwindow.Winows.Product.RemoveProduct;
using ticketwindow.Winows.Return;
using ticketwindow.Winows.Setting;
using ticketwindow.Winows.Statistique;
using ticketwindow.Winows.WPFCalculator;
using ticketwindow.Winows.Pro;

namespace ticketwindow.Class
{
    class ClassFunctuon
    {
        private static bool setBC = false;


        public void Click(object sender)
        {
            runFun(sender, null, null);
        }
        public void Click(object sender, object arg)
        {
            runFun(sender, arg, null);
        }
        public void Click(object sender, object arg, object arg1)
        {
            runFun(sender, arg, arg1);
        }
        public void showMessage(string b_ok_tool_tip, string mes)
        {

            W_Message m = new W_Message();
            if (b_ok_tool_tip != null)
                m.bok.ToolTip = b_ok_tool_tip;
            else
                m.bok.Visibility = Visibility.Hidden;
            m.message.Text = mes;
            m.WindowStyle = WindowStyle.None;
            m.message.TextWrapping = TextWrapping.WrapWithOverflow;
            m.Topmost = true;
            m.Show();
        }
        public void showMessageSB(string mes)
        {

            W_MessageSB m = new W_MessageSB();
            m.WindowStyle = WindowStyle.None;
            m.message.Text = mes;
            m.message.TextWrapping = TextWrapping.WrapWithOverflow;

            m.ShowDialog();

        }
        public void showMessageTime(string mes)
        {
            W_MessageTime m = new W_MessageTime();
            m.WindowStyle = WindowStyle.None;
            m.message.Text = mes;
            m.message.TextWrapping = TextWrapping.WrapWithOverflow;
            m.Topmost = true;
            m.Show();
        }
        public void showMessageTimeList(string mes)
        {
            W_MessageTimeList m = new W_MessageTimeList();
            m.WindowStyle = WindowStyle.None;
            m.message.Text = mes;
            m.message.TextWrapping = TextWrapping.WrapWithOverflow;
            m.Topmost = true;
            m.Show();
        }
        public decimal getQTY(TextBlock tb)
        {
            decimal qty;
            if (!decimal.TryParse(tb.Text.Replace(" X ", ""), NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("fr-FR"), out qty)) qty = 1;
            tb.Text = "__";
            return qty;
        }
        public decimal getMoney(TextBlock tb)
        {
            decimal m;
            if (!decimal.TryParse(tb.Text.Replace(" X ", ""), out m)) m = 0.0m;

            return m;
        }
        public void add_Currency(ClassSync.Currency currency, int count, W_GridPay ws)
        {

            decimal sumMoney = ClassBond.addCurrency(currency, count);
            decimal odd = (ClassBond.residue() - sumMoney);


            if (odd < 0)
            {
                if (!ws.typesPay.Rendu_Avoir ?? false)
                {
                    decimal money = ClassBond.getSumMoney();

                    decimal d = 0.0m;
                    foreach (var tm in ClassSync.TypesPayDB.t)
                        d += tm.Rendu_Avoir ?? false ? ClassBond.getMoneyFromType(tm) : 0.0m;

                    decimal etc = 0.0m;

                    foreach (var tm in ClassSync.TypesPayDB.t)
                        etc += !tm.Rendu_Avoir ?? false ? ClassBond.getMoneyFromType(tm) : 0.0m;

                    odd = money - etc - sumMoney - d;

                    if (money - sumMoney - etc < 0)
                        odd = -d;
                }

            }

            ClassMoney m = new ClassMoney(ClassBond.residue());
            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent + " centime(s)";




            foreach (Label ls in ClassETC_fun.FindVisualChildren<Label>(ws))
            {
                if ((ls.Name != "") && (ls.Name.Substring(0, 4) == "mla_") && ((Button)((StackPanel)ls.Parent).Parent).ToolTip != null)
                {
                    ls.Content = "";
                    ws.countCurrency = "";
                }
            }
            ws.ListBoxlog.Items.Clear();

            ws.ListBoxlog.Items.Add("Montant : " + m.Euro + "," + m.Cent);

            if (odd < 0)
            {
                ClassMoney ms = new ClassMoney(odd);

                ws.lblGet.Content = ms.Euro + " euro(s) et " + ms.Cent.ToString("00");

                ws.ListBoxlog.Items.Add("Rendu : " + ms.Euro + "," + ms.Cent.ToString("00"));
            }

            ws.ListBoxlog.Items.Add("Détails du paiement");

            foreach (ClassBond.count_Currency c in ClassBond.cc)
                ws.ListBoxlog.Items.Add(c.count + " X " + c.currency.Desc.ToString());
            ws.ListBoxlog.Items.Add("      ______________");
            ws.ListBoxlog.Items.Add(sumMoney.ToString("0.00"));
        }
        public void writeToatl(int indx_)
        {
            MainWindow w = (ClassETC_fun.findWindow("MainWindow_") as MainWindow);
            if (w != null)
            {


                if (Class.ClassCheck.b != null)
                {
                    ClassMoney mTotal = new ClassMoney(Class.ClassCheck.getTotalPrice());

                    w.ltotal.Content = "Total : " + mTotal.Euro.ToString() + "," + mTotal.Cent.ToString("00") + " €";

                    if (w.dataGrid1.Items.Count == 1)
                    {
                        w.dataGrid1.SelectedIndex = 0;
                    }

                    int indx = w.dataGrid1.SelectedIndex;

                    if (ClassCheck.c == null)

                    w.dataGrid1.DataContext = ClassCheck.b.Element("check");

                    else
                        w.dataGrid1.DataContext = ClassCheck.c.Element("check");



                    if (indx != -1)
                        w.dataGrid1.SelectedIndex = w.dataGrid1.Items.Count - 1;
                    else

                        w.dataGrid1.SelectedIndex = indx_ > w.dataGrid1.Items.Count - 1 ? w.dataGrid1.Items.Count - 1 : indx_;
                    XElement x = w.dataGrid1.SelectedItem as XElement;

                    if (x == null)
                    {
                        w.dataGrid1.SelectedIndex = w.dataGrid1.Items.Count - 1;
                        x = w.dataGrid1.SelectedItem as XElement;
                    }

                    if (w.dataGrid1.Items.Count > 0)
                    {
                        try
                        {
                            var border = VisualTreeHelper.GetChild(w.dataGrid1, 0) as Decorator;

                            if (border != null)
                            {
                                var scroll = border.Child as ScrollViewer;
                                if (scroll != null) scroll.ScrollToEnd();
                            }
                        }
                        catch
                        {
                            new ClassLog("kode 990 ");
                        }
                    }

                    if (x != null)
                    {


                        if (decimal.Parse(x.Element("price").Value.Replace('.', ',')) == 0)
                        {
                            var p = ClassProducts.transform(x);

                            W_ModifierPrix wp = new W_ModifierPrix(p);

                            effect(wp);
                        }
                    }

                }
                if ((ClassDiscounts.client.barcode != null) || ClassDiscounts.client.procent_default > 0)
                {
                    if (ClassDiscounts.client.points > (ClassDiscounts.client.maxPoints - 1))
                    {
                        if ((!ClassDiscounts.client.discountSet) && ClassDiscounts.client.showMessaget)
                        {
                            new ClassFunctuon().showMessage("DiscountSet", "Vous avez " + ClassDiscounts.client.points + " points." + Environment.NewLine + "Vous voulez vous servir de reduction de 20%?");
                            ClassDiscounts.client.showMessaget = false;
                        }
                    }

                    w.lProcentDiscount.Content = ClassDiscounts.client.procent + ClassDiscounts.client.procent_default + "%";
                    w.lNameClient.Content = ClassDiscounts.client.nameFirst ?? "Inconnue " + " " + ClassDiscounts.client.nameLast;
                    w.lPointsClient.Content = ClassDiscounts.client.points;
                }
                else
                {
                    w.lProcentDiscount.Content = "";
                    w.lNameClient.Content = "";
                    w.lPointsClient.Content = "";
                }

                if (ClassProMode.modePro)
                {
                    w.lProcentDiscount.Content = ClassProMode.pro.NameCompany;
                    // w.lNameClient.Content = ClassProMode.pro.Sex +" " + ClassProMode.pro.Name + " " + ClassProMode.pro.SurName;
                    // w.lPointsClient.Content = ClassProMode.pro.Mail;
                }
            }

        }
        public void writeToatl()
        {
            writeToatl(-1);
        }
        private void svernut(object sender)
        {
            Window.GetWindow((Button)sender).WindowState = WindowState.Minimized;
        }
        private void effect(object sender)
        {

            Window w = (Window)sender;
            w.WindowStyle = WindowStyle.None;
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (w.WindowState != WindowState.Maximized)
            {
                w.ShowDialog();
            }
            else
                w.Show();

        }
        private void cloaseMainWindow(object sender)
        {
            try
            {
                int countBufCheck = ClassCheck.getProductCheckFromBuf() + ClassCheck.getProductCheckFromEn_attenete();
                if (countBufCheck == 0)
                {
                    if (!ClassSync.sync)
                    {
                        Application.Current.Shutdown();
                        ClassCustomerDisplay.bye();
                        ClassGlobalVar.break_ = true;
                    }
                    else
                    {
                        showMessageSB("Synchronisation en cours, a quitté plus tard");
                    }
                }
                else
                    showMessageSB("Veuillez d'abord éditer la note!" + Environment.NewLine + countBufCheck + " article(s).");
            }
            catch
            {
                Application.Current.Shutdown();
            }

        }
        private void open_W_product(object sender)
        {
            string[] name = ((Button)sender).Name.Split('x');

            int X = int.Parse(name[0].Substring(2, name[0].Length - 2));

            int Y = int.Parse(name[1].Substring(0, name[1].Length));

            xGrid_Loaded(X, Y, sender);

            /*            W_product p = new W_product(X, Y);

                        MainWindow mw = (MainWindow)ClassETC_fun.GetParents(sender, 0);

                        p.Owner = mw;

                        p.Width = mw.gidLeft.ActualWidth;
                        p.Height = mw.gidLeft.ActualHeight;


                        Point relativePoint = mw.gidLeft.TransformToAncestor(mw)
                                          .Transform(new Point(0, 0));

                        p.Top = relativePoint.Y;

                        p.Left = relativePoint.X;

                        // p.Topmost = true;
                        p.WindowStyle = WindowStyle.None;
                        p.ShowDialog();

                        */
            //effect(p);
        }
        private void xGrid_Loaded(int I, int J, object sender)
        {

            MainWindow mw = (MainWindow)(Window.GetWindow((Button)(sender)));
            mw.I = I;
            mw.J = J;
            mw.gidLeft.Visibility = Visibility.Hidden;
            mw.xGrid.Visibility = Visibility.Visible;
            for (int i = 0; i < Class.ClassGridProduct.grid.GetLength(2); i++)
            {
                for (int j = 0; j < Class.ClassGridProduct.grid.GetLength(3); j++)
                {
                    if (Class.ClassGridProduct.grid[I, J, i, j] != null)
                    {
                        Button b = (Button)mw.FindName("_b_" + i + "x" + j);

                        if (Class.ClassGridProduct.grid[I, J, i, j].customerId == Guid.Empty)
                        {

                            b.ToolTip = null;
                        }
                        else
                        {
                            b.ToolTip = "Products id=[" + Class.ClassGridProduct.grid[I, J, i, j].customerId.ToString() + "]";

                            ((TextBlock)b.Content).Text = Class.ClassGridProduct.grid[I, J, i, j].Description;
                            b.Background = Class.ClassGridProduct.grid[I, J, i, j].background;
                            b.Foreground = Class.ClassGridProduct.grid[I, J, i, j].font;
                        }
                    }
                }
            }
        }

        public void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = (MainWindow)(Window.GetWindow((Button)(sender)));
            if (((Button)sender).Name.IndexOf('_') == 0)
            {
                int I = mw.I;
                int J = mw.J;
                ticketwindow.Winows.Product.W_setProduct w = new ticketwindow.Winows.Product.W_setProduct();
                w.Owner = mw;
                string[] s = ((Button)sender).Name.Remove(0, 1).Split('x');
                string x = s[0].Substring(2, s[0].Length - 2);
                string y = s[1];

                w.x = int.Parse(x);

                w.y = int.Parse(y);


                int X = Convert.ToInt16(x);
                int Y = Convert.ToInt16(y);
                var gridElm = Class.ClassGridProduct.grid;

                if (gridElm[I, J, X, Y] != null)
                {
                    w.xColor.Background = gridElm[I, J, X, Y].background;
                    w.xFontColor.Background = gridElm[I, J, X, Y].font;
                    w.xDescription.Text = gridElm[I, J, X, Y].Description;


                }


                w.WindowStyle = WindowStyle.None;
                w.AllowsTransparency = true;
                w.ShowDialog();

            }

            else
            {

                var we = new W_edit();

                var b = (Button)sender;

                string[] t = b.Name.Split('x');

                int[] xy = { int.Parse(t[0].Substring(2, t[0].Length - 2)), int.Parse(t[1]) };

                we.xName.Content = xy[0];

                we.yName.Content = xy[1];

                we.xCaption.Text = b.Content == null ? "" : b.Content.ToString();

                we.xColor.Background = b.Background;

                we.xColorFont.Background = b.Foreground;

                if (b.ToolTip != null)
                {
                    we.setSelected(we.xGBFunction, b.ToolTip.ToString());
                }
                else
                {
                    we.setSelected(we.xGBFunction, "None");
                }
                try
                {
                    we.xColor.SelectedColor = (b.Background as SolidColorBrush).Color;
                    we.xColorFont.SelectedColor = (b.Foreground as SolidColorBrush).Color;
                }

                catch
                {

                }
                we.Owner = mw;
                we.sub = b.Name.Substring(0, 1);
                we.WindowStyle = WindowStyle.None;
                we.AllowsTransparency = true;
                we.ShowDialog();

            }

        }

        /*     private void open_W_Countrys(object sender)
             {
                 string[] name = ((Button)sender).Name.Split('x');

                 int X = int.Parse(name[0].Substring(2, name[0].Length - 2));

                 int Y = int.Parse(name[1].Substring(0, name[1].Length));

                 W_Statistique_Region_et_Pays p = new W_Statistique_Region_et_Pays(X, Y);

                 p.Owner = (MainWindow)ClassETC_fun.GetParents(sender, 0);

                 effect(p);
             }
         */
        private void open_W_Countrys(object sender)
        {
            effect(new W_Stat());
        }
        private void show_grid_StatNationPopup(object sender)
        {
            ticketwindow.Winows.Statistique.ModifStatNationPopup.W_Grid w = new ticketwindow.Winows.Statistique.ModifStatNationPopup.W_Grid();

            w.Owner = (Window.GetWindow((Button)sender) as W_Stat);

            effect(w);
        }
        private void show_grid_StatPlaceArrond(object sender)
        {
            ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_Grid w = new ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_Grid();

            w.Owner = (Window.GetWindow((Button)sender) as W_Stat);

            effect(w);
        }
        private void show_insert_nation(object sender, object arg)
        {
            ticketwindow.Winows.Statistique.ModifStatNationPopup.W_add w = new ticketwindow.Winows.Statistique.ModifStatNationPopup.W_add();

            w.SNP = (List<Class.ClassSync.Stat.StatNationPopup>)arg;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatNationPopup.W_Grid);

            effect(w);
        }
        private void show_modif_nation(object sender, object arg, object arg1)
        {
            ticketwindow.Winows.Statistique.ModifStatNationPopup.W_mod w = new ticketwindow.Winows.Statistique.ModifStatNationPopup.W_mod();

            w.SNP = (List<Class.ClassSync.Stat.StatNationPopup>)arg;

            Class.ClassSync.Stat.StatNationPopup elm = arg1 as Class.ClassSync.Stat.StatNationPopup;

            w.xNameNation.Text = elm.NameNation;

            w.xQTY.Text = elm.QTY.ToString();

            w.CustomerId = elm.IdCustomer;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatNationPopup.W_Grid);

            effect(w);
        }
        private void show_delete_nation(object sender, object arg, object arg1)
        {
            ticketwindow.Winows.Statistique.ModifStatNationPopup.W_del w = new ticketwindow.Winows.Statistique.ModifStatNationPopup.W_del();

            w.SNP = (List<Class.ClassSync.Stat.StatNationPopup>)arg;

            Class.ClassSync.Stat.StatNationPopup elm = arg1 as Class.ClassSync.Stat.StatNationPopup;

            w.xNameNation.Text = elm.NameNation;

            w.xQTY.Text = elm.QTY.ToString();

            w.CustomerId = elm.IdCustomer;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatNationPopup.W_Grid);

            effect(w);
        }
        private void show_insert_PlaceArrond(object sender, object arg)
        {
            ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_add w = new ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_add();

            w.SNP = (List<Class.ClassSync.Stat.StatPlaceArrond>)arg;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_Grid);

            effect(w);
        }
        private void show_modif_PlaceArrond(object sender, object arg, object arg1)
        {
            ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_mod w = new ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_mod();

            w.SNP = (List<Class.ClassSync.Stat.StatPlaceArrond>)arg;

            Class.ClassSync.Stat.StatPlaceArrond elm = arg1 as Class.ClassSync.Stat.StatPlaceArrond;

            w.xNamePlaceArrond.Text = elm.NamePlaceArrond;

            w.xQTY.Text = elm.QTY.ToString();

            w.CustomerId = elm.IdCustomer;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_Grid);

            effect(w);
        }
        private void show_delete_PlaceArrond(object sender, object arg, object arg1)
        {
            ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_del w = new ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_del();

            w.SNP = (List<Class.ClassSync.Stat.StatPlaceArrond>)arg;

            Class.ClassSync.Stat.StatPlaceArrond elm = arg1 as Class.ClassSync.Stat.StatPlaceArrond;

            w.xNamePlaceArrond.Text = elm.NamePlaceArrond;

            w.xQTY.Text = elm.QTY.ToString();

            w.CustomerId = elm.IdCustomer;

            w.Owner = (Window.GetWindow((Button)sender) as ticketwindow.Winows.Statistique.ModifStatPlaceArrond.W_Grid);

            effect(w);
        }
        private void show_grid_product(object sender)
        {
            W_Grid_Product gp = new W_Grid_Product();

            effect(gp);
        }
        private void show_return(object sender)
        {
            effect(new W_Return());
        }
        private void show_return_product(object sender, object arg)
        {

            effect(new W_ReturnProduct(arg));
            (Window.GetWindow((Button)sender) as W_Return).Close();
        }
        private void AnnulationDeTicket(object sender)
        {
            if (ClassCheck.b.Element("check").Elements("product").Count() == 0)

                effect(new W_AnnulationDeTicket());

            else showMessageSB("Veuillez d'abord éditer la note!");
        }
        private void clickAnnulationDeTicket(object sender)
        {
            if (!ClassSync.sync)
            {
                W_AnnulationDeTicket w = Window.GetWindow((Button)sender) as W_AnnulationDeTicket;

                string bc = w.codebare_.Text.Trim().TrimEnd().TrimStart();

                if (ClassCheck.getCheckFromChecksAndDelete(bc))
                {
                    w.Close();
                    w.codebare_.Text = "";
                }
                else
                {
                    new ClassFunctuon().showMessageTime("pas trouvé");
                    w.codebare_.Text = "";
                }
            }
            else
            {
                showMessageSB("Synchronisation en cours, a quitté plus tard");
            }
        }
        private void show_add_product(object sender)
        {

            effect(new W_AddProduct());

        }
        private void show_change_product(object sender, object arg)
        {
            effect(new W_Change_Product(arg));
        }
        private void show_remove_product(object sender, object arg)
        {

            effect(new W_Remove_product(arg));
        }
        private void show_find_product(object sender)
        {
            W_Find_Product w = new W_Find_Product();
            w.Owner = (Window.GetWindow((Button)sender) as W_Grid_Product);
            effect(w);
        }
        private void set_product(object sender, object arg)
        {
            W_Grid_Product wp = ClassETC_fun.GetParents(((Button)sender), 0) as W_Grid_Product;

            if (wp != null)
            {
                MainWindow w = ClassETC_fun.findWindow("MainWindow_") as MainWindow;


                if (arg != null)
                {
                    XElement x = (XElement)arg;
                    if (x != null)
                    {
                        ClassCheck.addProductCheck(x, getQTY(w.qty_label));
                        //  writeToatl();
                    }
                }
                wp.Close();
            }
        }
        private void show_ballance_window(object sender, object arg)
        {
            effect(new W_Ballance(arg));
        }
        private void show_AppCalculator(object sender)
        {
            effect(new AppCalculator());
        }
        private void show_en_attente(object sender)
        {
            effect(new W_enAttente());
        }
        private void show_history(object sender)
        {
            effect(new W_AllHistory());
        }
        private void show_history_local(object sender)
        {
            effect(new W_history());
        }

        private void ShowGeneralHistoryOfClosing(object sender)
        {
            effect(


                new W_G_History()
                );
        }
        private void close_any_windows(object sender)
        {
            Window w = Window.GetWindow((Button)sender);

            if (w is MainWindow)
            {
                MainWindow mw = w as MainWindow;
                if (mw.gidLeft.Visibility == Visibility.Hidden)
                {
                    mw.gidLeft.Visibility = Visibility.Visible;
                    mw.xGrid.Visibility = Visibility.Hidden;
                    mw.I = -1;
                    mw.J = -1;
                    for (int i = 0; i < Class.ClassGridProduct.grid.GetLength(2); i++)
                    {
                        for (int j = 0; j < Class.ClassGridProduct.grid.GetLength(3); j++)
                        {

                            Button b = (Button)mw.FindName("_b_" + i + "x" + j);

                            if (b != null)
                            {
                                b.ClearValue(Button.ForegroundProperty);
                                b.ClearValue(Button.BackgroundProperty);

                                b.ToolTip = null;
                                ((TextBlock)b.Content).Text = "";
                                //        b.Background = Class.ClassGridProduct.grid[I, J, i, j].background;
                                //      b.Foreground = Class.ClassGridProduct.grid[I, J, i, j].font;
                            }
                        }
                    }
                }

            }

            if (w is W_product)
                w.Close();
            if (w is W_Grid_Product)
                w.Close();
        }
        private void moveToCheck(object sender)
        {

            W_product wp = ClassETC_fun.GetParents(((Button)sender), 0) as W_product;

            if (wp != null)
            {
                MainWindow w = wp.Owner as MainWindow;

                string productName = ((TextBlock)((Button)sender).Content).Text;
                if (productName.Trim().Length > 0)
                {
                    XElement x = Class.ClassProducts.findName(productName);



                    if (x != null)
                    {
                        ClassCheck.addProductCheck(x, getQTY(w.qty_label));
                        //writeToatl();


                    }
                }
                wp.Close();
            }

        }
        private void moveToCheck(object sender, Guid guidProduct)
        {

            W_product wp = ClassETC_fun.GetParents(((Button)sender), 0) as W_product;

            MainWindow mw = Window.GetWindow((Button)sender) as MainWindow;

            if (mw != null)
            {
                XElement x = Class.ClassProducts.findCustomerId(guidProduct);

                if (x != null)
                {
                    ClassCheck.addProductCheck(x, getQTY(mw.qty_label));
                    //  writeToatl();

                }
            }

            if (wp != null)
            {
                MainWindow w = wp.Owner as MainWindow;

                XElement x = Class.ClassProducts.findCustomerId(guidProduct);

                if (x != null)
                {
                    ClassCheck.addProductCheck(x, getQTY(w.qty_label));
                    // writeToatl();

                }
                // wp.Close();
            }

        }
        private void changePayButton(MainWindow w, string sumbol)
        {
            foreach (Button bs in ClassETC_fun.FindVisualChildren<Button>(w))
            {
                if (bs.ToolTip != null)
                {
                    if (bs.ToolTip.ToString().IndexOf("TypesPayDynamic") != -1)
                    {
                        string bc = bs.Content.ToString();

                        switch (sumbol.TrimEnd())
                        {
                            case "cl":
                                if (bc.IndexOf("(") != -1)
                                {
                                    int a = bc.IndexOf("(");

                                    int b = bc.ToString().IndexOf(")");

                                    string s = bc.Substring(a - 1);

                                    bs.Content = bc.Replace(s, "");

                                }


                                break;
                            default:

                                string e_str = "({%})".Replace("{%}", sumbol);

                                if (bc.IndexOf("(") != -1)
                                {
                                    int a = bc.IndexOf("(");

                                    int b = bc.IndexOf(")");

                                    string s = bc.Substring(a, bc.Length - b);

                                    s = e_str;

                                    bs.Content += e_str;
                                }

                                else
                                {
                                    bs.Content += e_str;
                                }

                                break;
                        }

                    }
                }
            }
        }
        private void numpadClick(object sender)
        {
            Button b = (Button)sender;
            string number = (b).Content.ToString();
            Window w = Window.GetWindow(b);
            TextBlock tb;
            if (w is MainWindow)
            {
                MainWindow mw = w as MainWindow;
                tb = (mw).qty_label;
                // changePayButton(mw, number);
                if (setBC)
                {
                    TextBox txb = mw.xProduct;
                    switch (number.TrimEnd())
                    {
                        case "cl": txb.Text = ""; break;
                        case ",enter":
                            mw.keyReturn();
                            SetBarCode(b);
                            break;
                        default: txb.Text += int.Parse(number); break;
                    }
                }
                else
                {
                    switch (number.TrimEnd())
                    {
                        case "cl": tb.Text = "__"; break;
                        case ",enter":


                            tb.Text = (tb.Text.Replace(" X ", "").Replace("_", "")) + "," + " X ";

                            break;
                        default: tb.Text = (tb.Text.Replace(" X ", "").Replace("_", "")) + int.Parse(number) + " X "; break;
                    }
                }

            }

            if (w is W_GridPay)
            {
                W_GridPay mw = w as W_GridPay;

                string c = b.Content.ToString();

                string cc = mw.countCurrency;

                string s = (cc + c);

                if ((b.Content.ToString() == "cl"))
                {
                    s = "";

                }

                foreach (Label ls in ClassETC_fun.FindVisualChildren<Label>(mw))
                {



                    if ((ls.Name != "") && (ls.Name.Substring(0, 4) == "mla_") && ((Button)((StackPanel)ls.Parent).Parent).ToolTip != null)
                    {


                        ls.Content = s.Length > 0 ? s + " X " : "";

                        mw.countCurrency = s;

                    }


                }
            }
        }
        private void click_clearCurrency(object sender)
        {
            W_GridPay ws = Window.GetWindow((Button)sender) as W_GridPay;
            ClassBond.clearCurrency(ws.typesPay);
            decimal sumMoney = ClassBond.List_Currency.Sum(l => l.Currency_money);
            ClassMoney m = new ClassMoney(sumMoney);
            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent + " centime(s)";

            ClassMoney ms = new ClassMoney(0.0m);

            ws.lblGet.Content = ms.Euro + " euro(s) et " + ms.Cent.ToString("00") + " centime(s)";

            ws.lblSet.Content = m.Euro + " euro(s) et " + m.Cent.ToString("00") + " centime(s)";
            ws.ListBoxlog.Items.Clear();
        }
        private void click_Currency(object sender)
        {

            if (((Button)sender).ToolTip != null)
            {
                Guid g;

                if (Guid.TryParse(((Button)sender).ToolTip.ToString(), out g))
                {
                    W_GridPay ws = Window.GetWindow((Button)sender) as W_GridPay;
                    ClassSync.Currency currency = ClassSync.Currency.List_Currency.Find(l => l.CustomerId == g);
                    int count = int.Parse((ws.countCurrency == "") || (ws.countCurrency == null) ? "1" : ws.countCurrency);
                    add_Currency(currency, count, ws);
                }
            }

        }
        private void click_ok_Currency(object sender)
        {

            W_GridPay ws = Window.GetWindow((Button)sender) as W_GridPay;

            W_PayETC w = null;// = new W_PayETC();

            W_TypePay wind = null;

            ClassSync.TypesPayDB typePay = null;

            bool f = true;

            if (ws == null)
            {
                w = Window.GetWindow((Button)sender) as W_PayETC;

                decimal d = 0.0m;

                decimal resis = ClassBond.residue();

                bool df = decimal.TryParse(w.numPad.textBox.Text, out d);

                if (((sender as Button).ToolTip.ToString() == "click_ok_Currency") && (d <= 0) || (d > resis))
                {
                    new ClassFunctuon().showMessageSB("Montant ne doit pas dépasser de la somme total");

                    w.tbS.Text = ClassBond.residue().ToString();

                    f = false;
                }

                else
                {
                    typePay = w.typesPay;


                    wind = w.Owner as W_TypePay;

                    ClassBond.pay(typePay, decimal.Parse(w.tbS.Text));

                }

            }
            else
            {
                typePay = ws.typesPay;

                if (ClassBond.List_Currency.Count > 0)
                {
                    ClassBond.pay(typePay, ClassBond.List_Currency.Sum(l => l.Currency_money));

                    ClassBond.List_Currency.Clear();
                }



                wind = ws.Owner as W_TypePay;

            }
            if (f)
            {
                decimal calc = ClassETC_fun.renduCalc();



                if (calc == 0)
                    printCheck(sender, null);
                if (calc < 0)
                {

                    ClassMoney m = new ClassMoney(calc);


                    ClassCustomerDisplay.odd_money(calc, ClassBond.getMoneyFromType(typePay));

                    MainWindow mw = ClassETC_fun.findWindow("MainWindow_") as MainWindow;
                    if (mw != null)
                        mw.lrendu.Content = "Rendu: " + calc.ToString("0.00") + " €";

                    printCheck(sender, null);

                    new ClassFunctuon().showMessageTime("Rendu : " + m.Euro + " euro(s) et " + m.Cent + " centime(s) ");
                }
                if (calc > 0)
                {
                    if (ws != null) ws.Close();
                    if (w != null) w.Close();

                    if (wind == null)
                    {
                        wind = new W_TypePay();

                        effect(wind);
                    }
                    else
                    {
                        CollectionViewSource.GetDefaultView(wind.dataGrid1.ItemsSource).Refresh();
                        ClassMoney mReste = new ClassMoney(Class.ClassBond.calc());
                        wind.lblReste.Content = mReste.Euro.ToString() + " euro(s) et " + mReste.Cent.ToString() + " centime(s)";
                        ClassCustomerDisplay.Reste(ClassBond.calc(), ClassBond.getSumMoney());
                    }

                }

            }

        }
        private void click_ok_pay_type(object sender)
        {
            if (ClassBond.calc() > 0)
            {
                new ClassFunctuon().showMessageTime("Pas assez d'argent pour reglement d'achat");
            }
            else
            {
                printCheck(sender, null);
            }
        }
        private void cancelLine(object sender)
        {

            XElement x = null;

            MainWindow mw = ClassETC_fun.findWindow("MainWindow_") as MainWindow;

            W_DetailsProducts details = Window.GetWindow((Button)sender) as W_DetailsProducts;

            x = details != null ? (XElement)details.dg.SelectedItem : (XElement)mw.dataGrid1.SelectedItem;

            if (x != null)
            {
                ClassCheck.delProductCheck(int.Parse(x.Element("ii").Value));

                ClassDiscounts.client.procent_default = 0;

                mw.qty_label.Text = "__";
            }

            if (ClassDiscounts.client.procent > 0)
            {
                ClassCheck.discountCalc();
            }
        }
        private void cancelTicket(object sender)
        {
            MainWindow mw;
            if (sender == null)
                mw = ClassETC_fun.findWindow("MainWindow_") as MainWindow;

            else
                mw = Window.GetWindow((Button)sender) as MainWindow;



            if (mw.dataGrid1.Items.Count > 0)
            {
                ClassDiscounts.restoreDiscount();
                ClassCheck.clearProductsCheck();
                ClassDiscounts.client.procent_default = 0;

                if (ClassProMode.modePro)
                {
                    ClassProMode.modePro = false;

                    ClassProMode.pro = null;

                    writeToatl();
                }

                if (ClassDiscounts.client.procent > 0)
                {

                    ClassCheck.discountCalc();

                }
                mw.qty_label.Text = "__";
            }
        }
        private void show_payment(object sender)
        {
            bool tpom = false;
            show_message_customer_display(sender);
            if (!ClassGlobalVar.open)
                new ClassFunctuon().showMessageTime("La caisse est fermée");
            else
            {
                if (ClassCheck.b != null)
                {
                    if (Class.ClassCheck.b.Element("check").Elements("product").ToList().Count > 0)
                    {

                        decimal d = Class.ClassCheck.getTotalPrice();

                        if ((Window.GetWindow((Button)sender)) is MainWindow)
                        {
                            ClassBond.setMoneySum(d);

                            int idTP = 0;

                            bool b = int.TryParse(((Button)sender).ToolTip.ToString().Replace("_TypesPayDynamic", ""), out idTP);

                            if (b)
                            {
                                MainWindow mw = ((Window.GetWindow((Button)sender)) as MainWindow);

                                decimal money = getMoney(mw.qty_label);

                                ClassSync.TypesPayDB tp = ClassBond.getTypesPayDBFromId(idTP);

                                tpom = payOneMoment(mw, tp);

                                if (!tpom)
                                    ClassBond.pay(tp, money);
                            }
                        }
                        if (!tpom)
                        {
                            if ((Window.GetWindow((Button)sender)).Owner == null)
                            {

                                W_TypePay w = new W_TypePay();
                                ClassMoney mReste = new ClassMoney(Class.ClassBond.calc());
                                w.lblReste.Content = mReste.Euro.ToString() + " euro(s) et " + mReste.Cent.ToString() + " centime(s)";
                                ClassCustomerDisplay.Reste(ClassBond.calc(), ClassBond.getSumMoney());
                                w.Owner = (Window.GetWindow((Button)sender) as MainWindow);
                                effect(w);

                            }
                            else
                            {
                                (Window.GetWindow((Button)sender)).Close();
                                W_TypePay w = (Window.GetWindow((Button)sender)).Owner as W_TypePay;


                            }
                        }

                    }
                }
            }
        }
        private void show_payment_ETC(object sender)
        {
            show_message_customer_display(sender);
            if (!ClassGlobalVar.open)
                new ClassFunctuon().showMessageTime("La caisse est fermée");
            else
            {
                var t = ClassBond.getTypesPayDBFromId(int.Parse(((Button)sender).ToolTip.ToString().Replace("TypesPayDynamic", "")));

                if (ClassCheck.b != null)
                {
                    if (Class.ClassCheck.b.Element("check").Elements("product").ToList().Count > 0)
                    {
                        W_PayETC w = new W_PayETC();
                        w.Owner = (Window.GetWindow((Button)sender));
                        w.typesPay = t;
                        effect(w);
                    }
                }

            }
        }
        private void openCashBox()
        {
            if (ClassBond.getType(null))
            {
                try
                {
                    ClassUsbTicket.ReadWrite.open();

                    //    new ClassFunctuon().showMessageSB("CashBox is open");
                }
                catch
                {
                    new ClassFunctuon().showMessageSB("Ошибка с CashBox");
                }
            }
        }
        private bool payOneMoment(object window, ClassSync.TypesPayDB typePay)
        {



            MainWindow mw = window as MainWindow;

            bool r = false;

            if (mw != null)
            {
                decimal money = getMoney((mw).qty_label);

                if ((money == 0) || (ClassBond.getSumMoney() <= money))
                {
                    if (typePay.CurMod ?? false)
                    {
                        ClassBond.pay(typePay, money == 0 ? ClassBond.getSumMoney() : money);
                        decimal calc = ClassETC_fun.renduCalc();

                        if ((calc == 0))
                        {

                            printCheck(null, null);

                        }
                        else
                        {

                            ClassMoney m = new ClassMoney(ClassBond.residue());
                            ClassCustomerDisplay.odd_money(ClassBond.residue(), money);
                            mw.lrendu.Content = "Rendu: " + ClassBond.residue().ToString("0.00") + " €";
                            printCheck(null, null);



                            new ClassFunctuon().showMessageTime("Rendu : " + m.Euro + " euro(s) et " + m.Cent + " centime(s) ");



                        }
                        r = true;
                    }
                    else
                    {
                        if ((money == 0) || (ClassBond.getSumMoney() == money))
                        {
                            ClassBond.pay(typePay, money == 0 ? ClassBond.getSumMoney() : money);
                            decimal calc = ClassETC_fun.renduCalc();
                            if ((calc == 0))
                            {
                                printCheck(null, null);
                                r = true;
                            }

                        }

                    }
                }

            }

            return r;
        }
        private void show_payment_ETC_dynamyc(object sender)
        {
            show_message_customer_display(sender);
            if (!ClassGlobalVar.open)
                new ClassFunctuon().showMessageTime("La caisse est fermée");
            else
            {
                if (((Button)sender).ToolTip != null)
                {
                    if (ClassCheck.b != null)
                    {


                        if (Class.ClassCheck.b.Element("check").Elements("product").ToList().Count > 0)
                        {

                            decimal d = Class.ClassCheck.getTotalPrice();

                            if ((Window.GetWindow((Button)sender)) is MainWindow)
                                ClassBond.setMoneySum(d);


                            ClassSync.TypesPayDB t = ClassBond.getTypesPayDBFromId(int.Parse(((Button)sender).ToolTip.ToString().Replace("TypesPayDynamic", "")));
                            if (!payOneMoment((Window.GetWindow((Button)sender)), t))
                            {
                                if (t.CurMod ?? false)
                                {
                                    ClassMoney ms = new ClassMoney(ClassBond.residue());

                                    W_GridPay w = new W_GridPay();
                                    w.Owner = (Window.GetWindow((Button)sender));
                                    w.typesPay = t;

                                    w.lblSum.Content = ms.Euro + "," + ms.Cent + "€";
                                    effect(w);
                                    ClassBond.clearCurrency(t);
                                }
                                else show_payment_ETC(sender);

                            }
                        }
                    }
                }
            }
        }
        private void printCheck(object sender, object arg)
        {

            openCashBox();

            if (sender != null) (Window.GetWindow((Button)sender)).Close();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(W_TypePay))
                {
                    (window as W_TypePay).Close();

                }
                if (window.GetType() == typeof(MainWindow))
                {
                    DataGrid dg = (window as MainWindow).dataGrid1;
                    if (dg.Items.Count > 0)
                    {

                        ClassCheck.bay();
                        dg.DataContext = Class.ClassCheck.b.Element("check");
                        ClassBond.x = null;
                        (window as MainWindow).qty_label.Text = "__";
                        ClassDiscounts.client.procent_default = 0;
                    }
                }

            }

            writeToatl();
        }
        private void en_attenete(object sender)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    DataGrid dg = (window as MainWindow).dataGrid1;
                    if (dg.Items.Count > 0)
                    {
                        Class.ClassCheck.en_attenete();

                        new ClassFunctuon().showMessageTime(" Mis en attente +1 ");

                        dg.DataContext = Class.ClassCheck.b.Element("check");

                        CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
                    }
                    else
                    {
                        show_en_attente(sender);
                    }
                }
                if (window.GetType() == typeof(W_TypePay))
                {
                    (window as W_TypePay).Close();

                }
            }
        }
        private void show_closing_cash(object sender)
        {
            int countBufCheck = ClassCheck.getProductCheckFromBuf() + ClassCheck.getProductCheckFromEn_attenete();
            if (countBufCheck == 0)
                effect(new W_Close_TicketWindow(""));
            else
                new ClassFunctuon().showMessageSB("Veuillez d'abord éditer la note!" + Environment.NewLine + countBufCheck + " article(s).");
        }
        private void CloseGeneral(object sender)
        {
            W_Close_TicketWindow w = (Window.GetWindow((Button)sender) as W_Close_TicketWindow);


            if (ticketwindow.Class.ClassSync.General.cls())
                cloaseMainWindow(sender);
            else
                w.errorlist.Text = ticketwindow.Class.ClassSync.General.mess;



        }
        private void CloseTicketWindow(object sender)
        {
            W_Close_TicketWindow w = (Window.GetWindow((Button)sender) as W_Close_TicketWindow);


            ticketwindow.Class.ClassSync.OpenTicketWindow.close();

            w.errorlist.Text = ticketwindow.Class.ClassSync.OpenTicketWindow.mess;




        }
        private void show_message_customer_display(object sender)
        {

            MainWindow w = (ClassETC_fun.findWindow("MainWindow_") as MainWindow);


            if (ClassCheck.c == null && w != null && Class.ClassCheck.b != null)
            {
                ClassCheck.c = new XDocument(ClassCheck.b);

                ClassCheck.c = new ClassActionsCaisse().Descendants(ClassCheck.c);

                w.dataGrid1.DataContext = Class.ClassCheck.c.Element("check");

                ClassCustomerDisplay.total_sum(ClassCheck.getTotalPrice());

               
            }


            else
            {
                w.dataGrid1.DataContext = Class.ClassCheck.b.Element("check");

                ClassCheck.c = null;

              
            }

            new ClassFunctuon().writeToatl();
        }
        private void showDiscountCards(object sender)
        {
            effect(new W_InfoClients());
        }
        private void showDiscount(object sender)
        {
            effect(new W_discount());
        }
        private void showDiscountMini(object sender)
        {

            W_DiscountMini dm = new W_DiscountMini();
            dm.Owner = Window.GetWindow((Button)sender);
            effect(dm);
        }
        private void clickDiscountMini(object sender)
        {
            Button b = sender as Button;

            decimal procent = decimal.Parse(b.Content.ToString().Replace("%", ""));

            W_DiscountMini dm = Window.GetWindow(b) as W_DiscountMini;

            MainWindow mw = dm.Owner as MainWindow;

            if ((mw != null) && (ClassCheck.b != null))
            {
                ClassDiscounts.client.procent_default = procent;

                ClassCheck.discountCalc();

                writeToatl();
            }

            (dm).Close();


        }
        private void discountCardSet(object sender)
        {
            Button b = sender as Button;
            if (ClassCheck.b != null)
            {

                if ((!ClassDiscounts.client.discountSet) && (ClassDiscounts.client.barcode != null))
                {
                    decimal d = Math.Truncate((ClassCheck.getTotalPrice()
                        * (ClassDiscounts.client.procent
                        + ClassDiscounts.client.procent_default) / 100) * 100) / 100;


                    if (ClassDiscounts.client.points >= ClassDiscounts.client.maxPoints)
                    {

                        ClassDiscounts.client.discountSet = true;

                        ClassDiscounts.client.points -= ClassDiscounts.client.maxPoints;

                        b.Content = "20 %";

                        ClassDiscounts.client.procent = 20.0m;

                        ClassCheck.discountCalc();


                        writeToatl();
                    }
                    else
                    {
                        b.Content = "Не хватае " + (ClassDiscounts.client.maxPoints - ClassDiscounts.client.points).ToString();
                    }
                }
                else
                {
                    ClassDiscounts.client.procent = 0;

                    ClassDiscounts.client.discountSet = false;

                    ClassDiscounts.client.points += ClassDiscounts.client.maxPoints;

                    b.Content = "Set Discount";

                    ClassCheck.discountCalc();

                    writeToatl();
                }
            }
        }
        private void ResetMainGrid(object sender)
        {
            Properties.Settings.Default.Reset();
        }
        /*      private void ShowCaseProducts(object sender)
           {
               MainWindow mw = (Window.GetWindow((Button)sender) as MainWindow);

               effect(new W_DetailsProducts(mw.dataGrid1));
           }
           private void show_ProductDivers(object sender)
           {
               W_Divers w = new W_Divers();

               w.nameProduct = ((Button)sender).ToolTip.ToString();

               effect(w);
           }
               private void createDiversProduct(int tva, decimal price, bool ballance)
               {
                   Class.ClassProducts.product p = new Class.ClassProducts.product();
                   p.balance = ballance;
                   p.CodeBare = "";
                   p.Name = ballance ? "ProductDiversBallance" : "ProductDivers";
                   p.price = price;
                   p.qty = 0;
                   ClassGroupProduct.grp_subGrp g_s = ClassGroupProduct.getGrpId(3);
                   p.grp = g_s.grp;
                   p.sgrp = g_s.subGrp;
                   p.cusumerIdSubGroup = p.sgrp;
                   p.tare = 0;
                   p.tva = tva;
                   p.uniteContenance = 0;
                   p.Desc = "";
                   p.qty = 0;
                   ClassProducts.add(new List<ClassProducts.product>() { p });
               }
            private void clickProductDivers(object sender)
               {
                   W_Divers w = (Window.GetWindow((Button)sender) as W_Divers);

                   w.tva = int.Parse(w.xTVA.SelectedValue.ToString());

                   bool b = decimal.TryParse(w.xValue.Text, out w.prix);

                   if (w.prix <= 0)
                       new Class.ClassFunctuon().showMessageSB("Не верные данные");


                   else
                   {

                       string productName = w.nameProduct;

                       XElement x = Class.ClassProducts.findName(productName);

                       if (x == null)
                       {
                           createDiversProduct(w.tva, w.prix, (productName == "ProductDiversBallance"));
                           x = ClassProducts.x.Element("Product").Elements("rec").First(l => l.Element("Name").Value == productName);
                       }
                       else
                       {
                           x.Element("tva").SetValue(w.tva);
                           x.Element("price").SetValue(w.prix);
                       }

                       if (x != null)
                       {
                           foreach (Window window in Application.Current.Windows)
                           {
                               if (window.GetType() == typeof(MainWindow))
                               {

                                   decimal qty = getQTY((window as MainWindow).qty_label);
                                   DataGrid dg = (window as MainWindow).dataGrid1;
                                   ClassCheck.addProductCheck(x, qty);
                                   dg.DataContext = Class.ClassCheck.b.Element("check");
                                   CollectionViewSource.GetDefaultView(dg.ItemsSource).Refresh();
                                   dg.SelectedIndex = dg.Items.Count - 1;
                               }

                           }

                           w.Close();

                       }
                   }
               }*/
        private void openCashBox(object sender)
        {
            ClassUsbTicket.ReadWrite.open();
        }
        private void validateBallance(object sender)
        {
            W_Ballance w = (Window.GetWindow((Button)sender) as W_Ballance);

            ClassProducts.product p = w.po as ClassProducts.product;

            decimal outp = 0.0m;
            if (decimal.TryParse(w.xBallance_kg.Text.Replace(".", ","), out outp))
            {
                p.qty = outp;
                decimal newpice = 0.0m;

                if (decimal.TryParse(w.xPrix.Text.Replace(".", ","), out newpice))
                {
                    if ((p.qty > 0) && (newpice > 0))
                    {
                        p.price = newpice;



                        ClassCheck.addProductCheck(p, p.qty);


                        ((W_Ballance)Window.GetWindow((Button)sender)).Close();
                    }
                    else new ClassFunctuon().showMessageSB("error QTY=" + p.qty + " or  Prix=" + newpice);
                }

                else
                {
                    new ClassFunctuon().showMessageSB("Error prix");
                }
            }
        }
        private Guid productGuid(string tooltip)
        {
            string findText = "Products id=[";

            if (tooltip.IndexOf(findText) != -1)
            {
                string guid = tooltip.Replace(findText, "").Replace("]", "");

                return Guid.Parse(guid);
            }
            else return Guid.Empty;
        }
        private void showModifier_prix(object sender)
        {
            if (ClassCheck.c == null)
            {

                MainWindow w = (Window.GetWindow((Button)sender) as MainWindow);

                XElement x = null;

                if (w != null)

                    x = w.dataGrid1.SelectedItem as XElement;
                else
                    x = (ClassETC_fun.findWindow("MainWindow_") as MainWindow).dataGrid1.SelectedItem as XElement;


                if (x != null)
                {
                    W_ModifierPrix wp = new W_ModifierPrix(ClassProducts.transform(x));


                    wp.Owner = w;

                    effect(wp);
                }

            }
            else
            {
                show_message_customer_display(null);
            }
        }
        private void clickModifier_prix(object sender, bool flag)
        {

            W_ModifierPrix w = (Window.GetWindow((Button)sender) as W_ModifierPrix);




            decimal d = 0.0m;
            if (decimal.TryParse(w.xValue.Text.Replace(".", ","), out d))
            {
                ClassProducts.product product = w.product as Class.ClassProducts.product;
                product.price = d;
                product.Name = w.xNameProduct.Text;

                if (flag) ClassProducts.modif(product);


                if (product.price != 0)
                    ClassCheck.modifProductCheck(product.ii, product.price, product.Name, product.qty);
                else ClassCheck.delProductCheck(product.ii);

                writeToatl();
            }

            w.Close();
        }
        private void SetBarCode(object sender)
        {
            MainWindow mw = (Window.GetWindow((Button)sender) as MainWindow);

            setBC = !setBC;

            mw.xProduct.BorderThickness = setBC ? new Thickness(3.0) : new Thickness(1.0);
        }

        #region pro mode
        private void show_ProList(object sender)
        {

            MainWindow w = (Window.GetWindow((Button)sender) as MainWindow);

            if (w == null)

                w = (ClassETC_fun.findWindow("MainWindow_") as MainWindow);


            if (w != null)
            {
                W_ProList wp = new W_ProList();


                wp.Owner = w;

                effect(wp);
            }
        }

        private void click_set_pro(object sender)
        {
            W_ProList w = (Window.GetWindow((Button)sender) as W_ProList);

            if (w != null)
            {
                var select = w.dataPro.SelectedItem as ClassSync.classPro;

                if (select != null)
                {

                    ClassProMode.modePro = true;

                    ClassProMode.pro = select;


                    ClassCheck.b = ClassProMode.replaceCheck(ClassCheck.b, true);

                    w.Close();

                    writeToatl();
                }
            }
        }
        private void click_setoff_pro(object sender)
        {
            W_ProList w = (Window.GetWindow((Button)sender) as W_ProList);

            if (w != null)
            {
                var select = w.dataPro.SelectedItem as ClassSync.classPro;

                if (ClassProMode.modePro)
                {
                    /*   ClassDiscounts.client.procent_default =0;

                       ClassCheck.discountCalc();
                       */
                    ClassProMode.modePro = false;

                    ClassProMode.pro = null;

                    cancelTicket(null);

                    w.Close();

                }
            }
        }

        private void show_add_pro(object sender)
        {
            W_ProList w = (ClassETC_fun.GetParentWindow((Button)sender) as W_ProList);

            if (w == null)

                w = (ClassETC_fun.findWindow("w_pro_add") as W_ProList);


            if (w != null)
            {
                W_Pro_add wp = new W_Pro_add();

                wp.Owner = w;

                effect(wp);
            }
        }

        private void click_add_pro(object sender)
        {
            W_Pro_add w = (Window.GetWindow((Button)sender) as W_Pro_add);

            if (w != null)
            {

                var pro = new ClassSync.classPro(new object[]
                {
                    Guid.NewGuid(),
                    ClassSync.classPro.SexToInt( "M."),
                    "",
                    "",
                    w.xNameCompany.Text,
                    w.xMail.Text,
                    w.xTel.Text,
                    -1,
                    "",
                    "",
                    w.xAdress.Text,
                    w.xCodePostal.Text,
                    w.xVille.Text,
                    "",
                    "",
                    "",
                    "",
                    Guid.Empty,
                    Guid.Empty,
                    0
                });

                if (ClassSync.classPro.add(pro) > 0)
                {
                    w.Close();

                    var wpl = w.Owner as W_ProList;

                    wpl.dataPro_Loaded(null, null);

                    wpl.dataPro.SelectedIndex = 0;

                    wpl.dataPro.SelectionMode = DataGridSelectionMode.Single;
                }

                else showMessageSB("error 233");
            }
        }

        private void toDevis(object sender)
        {
            ClassProMode.devis = true;

            ClassCheck.bay();

        }
        #endregion
        private void closeWindow(object sender)
        {
            var f = ClassETC_fun.GetParentWindow((Button)sender);
            if (f != null) f.Close();
        }

        private void runFun(object sender, object arg, object arg1)
        {
            // try
            {
                string b_n = "";

                if (sender is Button)
                    b_n = ((Button)sender).Name.Substring(0, ((Button)sender).Name.Length > 1 ? 2 : 0);

                if (sender is DataGrid)
                    b_n = "";

                if (b_n == "m_") click_Currency(sender);

                if (b_n == "c_") show_payment_ETC_dynamyc(sender);

                if ((b_n != "m_") && (b_n != "c_"))
                {

                    string typeFun = "";
                    if (sender is Button)
                        typeFun = ((Button)sender).ToolTip == null ? "" : ((Button)sender).ToolTip.ToString();

                    if (sender is DataGrid)
                        typeFun = ((DataGrid)sender).ToolTip == null ? "" : ((DataGrid)sender).ToolTip.ToString(); ;

                    Guid prodGuid = productGuid(typeFun);

                    if (prodGuid != Guid.Empty)
                    {
                        moveToCheck(sender, prodGuid);
                    }
                    else
                    {
                        switch (typeFun)
                        {
                            case "Countrys": open_W_Countrys(sender); break;
                            case "Section des Articles": open_W_product(sender); break;
                            case "Fermer": cloaseMainWindow(sender); break;
                            case "Supprimer une Ligne": cancelLine(sender); break;
                            case "Supprimer le Ticket": cancelTicket(sender); break;
                            case "Modification des Articles": show_grid_product(sender); break;
                            case "Le mode de Paiement": show_payment(sender); break;
                            //case "Add Product": show_add_product(sender); break;
                            case "Set Product": set_product(sender, arg); break;
                            //  case "Change Product": show_change_product(sender, arg); break;
                            case "Remove Product": show_remove_product(sender, arg); break;
                            case "Find Product": show_find_product(sender); break;
                            case "closing_TWG": CloseGeneral(sender); break;
                            case "closing_TW": CloseTicketWindow(sender); break;
                            case "Products": moveToCheck(sender); break;
                            case "numpad": numpadClick(sender); break;
                            case "click_ok_pay_type": click_ok_pay_type(sender); break;
                            case "Attente +1": en_attenete(sender); break;
                            case "Clôture": show_closing_cash(sender); break;
                            case "Bande de Contrôle": show_history(sender); break;
                            case "currencyClear": click_clearCurrency(sender); break;
                            case "click_ok_Currency": click_ok_Currency(sender); break;
                            case "Retours et Remboursements": show_return(sender); break;
                            case "click_return_product": show_return_product(sender, arg); break;
                            case "ShowBallance": show_ballance_window(sender, arg); break;

                            case "Histoire de ticket": show_history_local(sender); break;

                            case "Afficher un Total": show_message_customer_display(sender); break;
                            case "TypesPayDynamic0": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic1": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic2": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic3": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic4": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic5": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic6": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic7": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic8": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic9": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic10": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic11": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic12": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic13": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic14": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic15": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic16": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic17": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic18": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic19": show_payment_ETC_dynamyc(sender); break;
                            case "TypesPayDynamic20": show_payment_ETC_dynamyc(sender); break;
                            case "_TypesPayDynamic0": show_payment(sender); break;
                            case "_TypesPayDynamic1": show_payment(sender); break;
                            case "_TypesPayDynamic2": show_payment(sender); break;
                            case "_TypesPayDynamic3": show_payment(sender); break;
                            case "_TypesPayDynamic4": show_payment(sender); break;
                            case "_TypesPayDynamic5": show_payment(sender); break;
                            case "_TypesPayDynamic6": show_payment(sender); break;
                            case "_TypesPayDynamic7": show_payment(sender); break;
                            case "_TypesPayDynamic8": show_payment(sender); break;
                            case "_TypesPayDynamic9": show_payment(sender); break;
                            case "_TypesPayDynamic10": show_payment(sender); break;
                            case "_TypesPayDynamic11": show_payment(sender); break;
                            case "_TypesPayDynamic12": show_payment(sender); break;
                            case "_TypesPayDynamic13": show_payment(sender); break;
                            case "_TypesPayDynamic14": show_payment(sender); break;
                            case "_TypesPayDynamic15": show_payment(sender); break;
                            case "_TypesPayDynamic16": show_payment(sender); break;
                            case "_TypesPayDynamic17": show_payment(sender); break;
                            case "_TypesPayDynamic18": show_payment(sender); break;
                            case "_TypesPayDynamic19": show_payment(sender); break;
                            case "_TypesPayDynamic20": show_payment(sender); break;
                            case "Carte de Fidélité": showDiscountCards(sender); break;
                            case "Reset main Grid": ResetMainGrid(sender); break;
                            //   case "CaseProducts": ShowCaseProducts(sender); break;
                            //  case "Clavier": show_keyboard(sender); break;
                            //  case "ClavierNumPad": show_NumPad(sender); break;

                            //  case "Mode de clavier": show_WKEYNUM(sender); break;
                            //  case "ProductDivers": show_ProductDivers(sender); break;
                            //   case "ProductDiversBallance": show_ProductDivers(sender); break;
                            //   case "clickProductDivers": clickProductDivers(sender); break;
                            case "General History of closing": ShowGeneralHistoryOfClosing(sender); break;
                            case "Open CashBox": openCashBox(sender); break;
                            case "Modifier le prix": showModifier_prix(sender); break;
                            case "Click modifier le prix": clickModifier_prix(sender, false); break;
                            case "Click modifier le prix de BD": clickModifier_prix(sender, true); break;
                            case "Discount": showDiscount(sender); break;
                            case "DiscountMini": showDiscountMini(sender); break;
                            case "discountMiniClick": clickDiscountMini(sender); break;
                            //   case "discountCalc": discountCalc_(sender); break;
                            case "DiscountSet":
                                if (!ClassDiscounts.client.discountSet)
                                    discountCardSet(sender); break;
                            case "svernut": svernut(sender); break;
                            case "show_grid_StatNationPopup": show_grid_StatNationPopup(sender); break;
                            case "show_grid_StatPlaceArrond": show_grid_StatPlaceArrond(sender); break;
                            case "show_insert_nation": show_insert_nation(sender, arg); break;
                            case "show_modif_nation": show_modif_nation(sender, arg, arg1); break;
                            case "show_delete_nation": show_delete_nation(sender, arg, arg1); break;
                            case "show_insert_PlaceArrond": show_insert_PlaceArrond(sender, arg); break;
                            case "show_modif_PlaceArrond": show_modif_PlaceArrond(sender, arg, arg1); break;
                            case "show_delete_PlaceArrond": show_delete_PlaceArrond(sender, arg, arg1); break;
                            case "SetBarCode": SetBarCode(sender); break;
                            case "validateBallance": validateBallance(sender); break;
                            case "Annulation de ticket": AnnulationDeTicket(sender); break;
                            case "clickAnnulationDeTicket": clickAnnulationDeTicket(sender); break;
                            case "Calculatrice": show_AppCalculator(sender); break;
                            case "Show Pro": show_ProList(sender); break;
                            case "closeWindow": closeWindow(sender); break;
                            case "click Set Pro": click_set_pro(sender); break;
                            case "click SetOff Pro": click_setoff_pro(sender); break;
                            case "add pro": show_add_pro(sender); break;
                            case "addPro": click_add_pro(sender); break;
                            case "toDevis": toDevis(sender); break;
                            default: close_any_windows(sender); break;
                        }
                    }

                }
            }
            /*
        catch (Exception exp)
        {
            string str = "code 0000 " + (sender != null ? ((Button)sender).ToolTip : "nuul ") + arg + " || " + arg1 + exp.Message;

            new ClassFunctuon().showMessageTimeList(str);

            new ClassLog(str + exp.Source + "  " + exp.StackTrace + "   ");

        }*/
        }
    }
}
