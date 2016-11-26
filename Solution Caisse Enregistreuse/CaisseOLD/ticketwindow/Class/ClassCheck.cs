using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassCheck
    {
        public static XDocument x;
        public static XDocument b;
        public static XDocument c;
        public static XDocument a;
        public static string tmpBarcode { get; set; }
        public static int tmp_Points { get; set; }
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\check.xml";

        public static string path_en_attenete = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\en_attenete.xml";

        public static string path_b = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\b.xml";

        public ClassCheck()
        {
            if (File.Exists(path))
            {
                try
                {
                    x = XDocument.Load(path);
                }
                catch (Exception ex)
                {
                    new ClassFunctuon().showMessageSB("ошибка файла check.xml " + ex.Message);
                }
            }
            else
            {

                x = new XDocument();

            }

            if (File.Exists(path_en_attenete))
            {
                a = XDocument.Load(path_en_attenete);

            }
            else
            {

                a = new XDocument();

                a.Add(new XElement("checks",
                     new XAttribute("ticket", ClassGlobalVar.numberTicket),
                     new XAttribute("openDate", DateTime.Now.ToString())

                     ));

                a.Save(path_en_attenete);

            }
        }
        public static bool openTicket()
        {
            if (!File.Exists(path))
            {
                x.Add(new XElement("checks",
                 new XAttribute("ticket", ClassGlobalVar.nameTicket),
                 new XAttribute("openDate", DateTime.Now.ToString()),
                 new XAttribute("closeDate", ""),
                 new XAttribute("idTicketWindowG", ClassGlobalVar.TicketWindowG),
                 new XAttribute("idTicketWindow", ClassGlobalVar.TicketWindow)
                 ));
                x.Save(path);
                return true;
            }
            else return false;
        }
        private static decimal getPay(IEnumerable<XAttribute> a_)
        {
            decimal ret = 0.0m;
            foreach (XAttribute a in a_)
            {
                ret += decimal.Parse(a.Value.Replace(".", ","));
            }
            return ret;
        }
        public class localTypesPay
        {
            public ClassSync.TypesPayDB type { get; set; }
            public decimal value { get; set; }
            public localTypesPay(ClassSync.TypesPayDB type, decimal value)
            {
                this.type = type;
                this.value = value;
            }
        }
        public static void closeTicket()
        {

            if (File.Exists(path))
            {
                string dir = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\"
                                + DateTime.Now.Year + @"\" + DateTime.Now.Month;

                x.Element("checks").Attribute("closeDate").Value = DateTime.Now.ToString();


                List<localTypesPay> listTP = new List<localTypesPay>();

                foreach (ClassSync.TypesPayDB tp in ClassSync.TypesPayDB.t)
                {
                    localTypesPay ltp = new localTypesPay(tp, getPay(x.Element("checks").Elements("check").Attributes(tp.NameCourt)));

                    listTP.Add(ltp);

                    x.Element("checks").Add(new XAttribute(ltp.type.NameCourt, ltp.value));

                }
                x.Element("checks").Add(new XAttribute("Rendu",
                   getPay(x.Element("checks").Elements("check").Attributes("Rendu"))
                    ));

                x.Element("checks").Add(new XAttribute("sum",
                   getPay(x.Element("checks").Elements("check").Attributes("sum"))
                    ));

                x.Save(path);



                Directory.CreateDirectory(dir);

                string file = dir + @"\" + DateTime.Now.Day + "_"
                    + DateTime.Now.Hour
                    + "_" + DateTime.Now.Minute
                    + "_" + DateTime.Now.Second + ".xml";

                File.Move(path, file);

                new ClassSync.ClassCloseTicket(path, file);

                //    new ClassPrintCloseTicket(listTP, ClassGlobalVar.nameTicket);


            }
        }
        private static string getCodeBarCheck()
        {
            DateTime dt = DateTime.Now;

            string bc = ClassGlobalVar.numberTicket + dt.ToString("HHmmss") + String.Format("{0:00000}", x.Element("checks").Elements("check").Count() + 1);

            return bc;
        }
        public static void openProductsCheck()
        {
            if ((b == null) || (b.Element("check") == null))
            {
                b = new XDocument(
                    new XElement("check",
                        new XAttribute("barcodeCheck", getCodeBarCheck())
                    )
                    );
                new ClassFunctuon().writeToatl();

                b.Save(path_b);

               
            }
            c = null;
        }
        public static void buf_addXElm(XElement x)
        {
            b = null;
            if (b == null)
            {
                b = new XDocument();
            }
            b.Add(x);
            ClassDiscounts.restoreDiscount();
        }
        public static void clearProductsCheck()
        {
            b.Element("check").Remove();
           
            openProductsCheck();
        }
        public static int getProductCheckFromBuf()
        {
            try
            {
                return b.Element("check").Elements("product").Count();
            }
            catch
            {
                return 0;
            }
        }
        public static int getProductCheckFromEn_attenete()
        {
            try
            {
                return a.Element("checks").Elements("check").Count();
            }
            catch
            {
                return 0;
            }
        }
        public static void loadFromFile()
        {
            if (!File.Exists(path_b))
            {
                openProductsCheck();
            }

            b = XDocument.Load(path_b);

            new ClassFunctuon().writeToatl();
        }

        private static void worker_DoWork_Total(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            System.Threading.Thread.Sleep(1000);

            ClassCustomerDisplay.total_sum(getTotalPrice());
        }
        private static void worker_RunWorkerCompleted_Total(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }
        private static System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

        public static void addProductCheck(ClassProducts.product p, decimal qty)
        {
            decimal procent = ClassDiscounts.client.procent + ClassDiscounts.client.procent_default;
            decimal sumDiscount = Math.Truncate(p.price * qty * procent) / 100;
            decimal total = (p.price * qty - sumDiscount);
            int ii = 0;
            openProductsCheck();
            if (b.Element("check").Elements("product").Count() > 0)
            {
                //    string val = b.Element("check").Elements("product").LastOrDefault().Element("ii").Value;
                ii = b.Element("check").Elements("product").Count();   //int.Parse(val) + 1;
            }
            XElement e = (
                   new XElement("product",
                       new XElement("ii", ii),
                        new XElement("CustumerId", p.CustumerId),
                        new XElement("Name", p.Name),
                        new XElement("Desc", p.Desc),
                        new XElement("price", p.price),
                         new XElement("priceGros", p.priceGros),
                        new XElement("tva", p.tva),
                        new XElement("qty", qty),
                        new XElement("CodeBare", p.CodeBare),
                        new XElement("balance", p.balance),
                        new XElement("contenance", p.contenance),
                        new XElement("uniteContenance", p.uniteContenance),
                        new XElement("tare", p.tare),
                        new XElement("date", p.date ?? DateTime.Now),
                        new XElement("cusumerIdRealStock", p.cusumerIdRealStock),
                        new XElement("cusumerIdSubGroup", p.cusumerIdSubGroup),
                        new XElement("ProductsWeb_CustomerId", p.ProductsWeb_CustomerId),
                        new XElement("grp", p.grp),
                        new XElement("sgrp", p.sgrp),
                        new XElement("Discount", procent),
                        new XElement("sumDiscount", sumDiscount),
                       new XElement("total", Math.Truncate(total * 100) / 100)
                       )
                   );

            b.Element("check").Add(e);
          
            ClassCustomerDisplay.writePrice(p.Name, qty, p.price);
            new ClassFunctuon().writeToatl();
            ClassETC_fun.wm_sound(@"Data\Beep.wav");
           
            b.Save(path_b);

            ClassCheck.c = null;

            if (worker.IsBusy)
            {
                //  worker.CancelAsync();

                worker.Dispose();

                worker = new System.ComponentModel.BackgroundWorker();
            }
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork_Total);
            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted_Total);
            worker.RunWorkerAsync();


        }
        private static ClassProducts.product getBallance(ClassProducts.product p)
        {

            Class.ClassBallance.send(p.price, p.tare);



            decimal prix = 0.0m;
            decimal qty = 0.0m;
            try
            {
                prix = decimal.Parse(Class.ClassBallance.prix) / 100;
                qty = decimal.Parse(Class.ClassBallance.poinds) / 1000;

            }
            catch (Exception e)
            {
                new Class.ClassLog("Code 021 Error ballance =" + ClassBallance.error + e.Message);
                p = null;

            }

            if (qty > 0)
            {
                p.contenance = qty;
                p.qty = qty;
                p.price = prix;
            }

            if ((ClassBallance._busy_0x15) || (ClassBallance._error_0x15)) p = null;

            return p;
        }

        public static void addProductCheck(XElement elm, decimal qty)
        {
            openProductsCheck();

            if (ClassProMode.modePro)
            {
                elm = ClassProMode.replace(new XElement[] { elm }).First();
            }

            ClassProducts.product p = ClassProducts.transform(elm);

            if (p.balance)
            {
                ClassProducts.product tmp = getBallance(p);

                if (tmp == null)
                {
                    Button b = new Button();
                    b.ToolTip = "ShowBallance";
                    new ClassFunctuon().Click(b, p);
                }
                else
                {
                    addProductCheck(p, p.qty);
                }
            }
            else
            {
                addProductCheck(p, qty);

            }

        }
        public static bool modifProductCheck(int ii, decimal price, string Name, decimal qty)
        {
            bool res = modifProductCheck_(ii, price, Name, qty);
            new ClassFunctuon().writeToatl();
            return res;
        }
        public static bool modifProductCheck_(int ii, decimal price, string Name, decimal qty)
        {
            XElement e = b.Element("check").Elements("product").SingleOrDefault(l => int.Parse(l.Element("ii").Value) == ii);
            if (e != null)
            {
                decimal sumDuscount = 0.0m;
                if (ClassDiscounts.client.procent_default >= 0)
                {
                    sumDuscount = Math.Truncate((ClassDiscounts.client.procent + ClassDiscounts.client.procent_default) / 100 * price * qty * 100) / 100;
                    e.Element("Discount").SetValue(ClassDiscounts.client.procent + ClassDiscounts.client.procent_default);
                    e.Element("sumDiscount").SetValue(sumDuscount);
                }

                decimal price_old = decimal.Parse(e.Element("price").Value.Replace(".", ","));
                if (price_old != price)
                {
                    e.Add(new XElement("price_old", e.Element("price").Value));
                    e.Element("price").SetValue(price);
                }
                e.Element("Name").SetValue(Name);
                e.Element("total").SetValue(Math.Truncate((price * qty - sumDuscount) * 100) / 100);

                b.Save(path_b);
                return true;
            }
            else return false;
        }
        public static void delProductCheck(int id)
        {
            c = null;
            int si = (ClassETC_fun.findWindow("MainWindow_") as MainWindow).dataGrid1.SelectedIndex;
            IEnumerable<XElement> elms = b.Element("check").Elements("product");
            XElement elm = elms.
              Where(l => int.Parse(l.Element("ii").Value) == id).FirstOrDefault();
            if (elm != null) elm.Remove();
            if (elms.Count() == 0) ClassDiscounts.restoreDiscount();
            new ClassFunctuon().writeToatl(si);
            b.Save(path_b);

        }
        public static void delProductCheck(string barcode)
        {
            int si = (ClassETC_fun.findWindow("MainWindow_") as MainWindow).dataGrid1.SelectedIndex;
            IEnumerable<XElement> elm = b.Element("check").Elements("product").
              Where(l => l.Element("barcode").Value.Replace(" ", "") == barcode.Replace(" ", ""));
            if (elm.Count() > 0)
            {
                decimal d = decimal.Parse(elm.First().Element("qty").Value);

                if ((ClassProducts.listProducts.Find(l => l.CustumerId == Guid.Parse(elm.First().Element("id").Value)).balance) || (d == 0))
                    elm.First().Remove();
                else
                {
                    elm.First().Element("qty").Value = (d - 1).ToString();
                }
            }
            else
                ClassDiscounts.restoreDiscount();
            new ClassFunctuon().writeToatl(si);
            b.Save(path_b);
        }
        public static decimal getTotalPrice()
        {
            decimal getTotalPrice = b != null ?
                Convert.ToDecimal(b.Element("check").Elements("product")
                .Sum(l => double.Parse(l.Element("total").Value.Replace(".", ",")))) : 0.0m;
            // if (!ClassDiscounts.client.discountSet)
            {
                if ((ClassDiscounts.client.barcode != null) && (getTotalPrice >= ClassDiscounts.client.moneyMax))
                {
                    if ((getTotalPrice >= ClassDiscounts.client.moneyMax) && (!ClassDiscounts.client.addPoints))
                    {

                        if ((DateTime.Now.Day == ClassDiscounts.client.lastDateUpd.Day)
                            && (DateTime.Now.Month == ClassDiscounts.client.lastDateUpd.Month)
                            && (DateTime.Now.Year == ClassDiscounts.client.lastDateUpd.Year))
                        {
                            if (ClassDiscounts.client.showMessaget)
                            {

                                new ClassFunctuon().showMessageSB("Cette carte a déjà été utilisé aujourd'hui! " + Environment.NewLine + ClassDiscounts.client.lastDateUpd.ToString());
                            }
                            ClassDiscounts.client.showMessaget = false;
                        }
                        else
                        {
                            if (ClassDiscounts.client.points < ClassDiscounts.client.maxPoints)
                            {
                                ClassDiscounts.client.addPoints = true;
                                ClassDiscounts.client.points += 1;
                            }
                        }



                    }

                    else
                    {
                        if ((getTotalPrice < ClassDiscounts.client.moneyMax) && (ClassDiscounts.client.addPoints))
                        {
                            ClassDiscounts.client.addPoints = false;
                            ClassDiscounts.client.points -= 1;
                        }
                    }
                }
            }
            return getTotalPrice;
        }
        private static void worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {


            ClassSync.sync = true;

            new ClassSync.ClassCloseTicketTmp(x);

        }

     

        private static void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ClassSync.sync = false;
            tmp_Points = 0;
            tmpBarcode = null;
          
        }
        public static void bay()
        {
            if (x.Element("checks") != null && b.Element("check").Elements("product").Count() > 0)
            {
                b = new ClassActionsCaisse().Descendants(b);

                try
                {
                    foreach (ClassSync.TypesPayDB type in ClassSync.TypesPayDB.t)
                    {
                        decimal m = ClassBond.getMoneyFromType(type);

                        b.Element("check").Add(new XAttribute(type.NameCourt.TrimEnd(), m));
                    }

                }
                catch (Exception ex)
                {

                    string text = "KOD 001" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());
                }

                decimal rendu = ClassETC_fun.renduCalc();

                try
                {
                    b.Element("check").Add(new XAttribute("Rendu", rendu.ToString("0.00")));
                }

                catch (Exception ex)
                {

                    string text = "KOD 002" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());

                }


                try
                {
                    b.Element("check").Add(
                     new XAttribute("sum", ClassBond.getSumMoney()),
                     new XAttribute("date", DateTime.Now)
                     );
                }
                catch (Exception ex)
                {

                    string text = "KOD 003" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());
                }

                try
                {

                    if (ClassDiscounts.client.barcode != null && !ClassProMode.modePro)
                    {


                        addSetDiscountCardBareCode(
                            ClassDiscounts.client.barcode,
                            ClassDiscounts.client.points - (ClassDiscounts.client.addPoints ? 1 : 0) + (ClassDiscounts.client.discountSet ? ClassDiscounts.client.maxPoints : 0),
                            ClassDiscounts.client.addPoints ? 1 : 0,
                            ClassDiscounts.client.discountSet ? 8 : 0,
                            ClassDiscounts.client.nameFirst + " " + ClassDiscounts.client.nameLast
                            );
                    }

                }
                catch (Exception ex)
                {

                    string text = "KOD 004" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());
                }


                //  try
                {
                    //   b.Element("check").Add(new XAttribute("barcodeCheck", getCodeBarCheck()));
                    if (ClassProMode.modePro || ClassProMode.devis)
                    {
                        ClassProMode.move(ClassProMode.devis);

                    }
                    else
                    x.Element("checks").Add(
                               b.Element("check")
                               );

                   
                }
                // catch (Exception ex)
                {

                    //   string text = "KOD 005" + ex.Message;

                    // new ClassFunctuon().showMessageSB(text);

                    // new ClassLog(text + b.ToString());
                }

                //    try
                {
                    if (!ClassProMode.modePro && !ClassProMode.devis)

                        new ClassPrintCheck(b, false);
                }
                //  catch (Exception ex)
                {

                    //    string text = "KOD 006" + ex.Message;

                    //  new ClassFunctuon().showMessageSB(text);

                    //new ClassLog(text + b.ToString());
                }

                try
                {
                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();

                    if (!ClassProMode.modePro && !ClassProMode.devis)
                    {
                        worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
                        worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                        worker.RunWorkerAsync();
                      
                    }


                    
                }

                catch (Exception ex)
                {

                    string text = "KOD 007" + ex.Message;

                    // new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());
                }

                try
                {

                    if (!ClassProMode.modePro && !ClassProMode.devis)
                    {

                        x.Save(path);

                        b = null;

                        openProductsCheck();

                        ClassDiscounts.restoreDiscount();
                    }
                  
                }
                catch (Exception ex)
                {

                    string text = "KOD 008" + ex.Message;

                    new ClassFunctuon().showMessageSB(text);

                    new ClassLog(text + b.ToString());
                }

            }
            else
                new ClassFunctuon().showMessageSB("файл check.xml отсутвует или структура не правильная");

        }

        public static void en_attenete()
        {

            new ClassSync.En_attenete().set(b);
            b = null;
            openProductsCheck();
        }
        public static void save_en_attenete(Guid customerId)
        {
            new ClassSync.En_attenete().get(customerId);

        }
        /*   public static XDocument DbToXelemet(ClassSync.ClassCloseTicketTmp.ChecksTicket check)
           {

               XDocument doc;

               doc = new XDocument(
                   new XElement("check",
                       new XAttribute("barcodeCheck", check.BarCode))
                       )
                       ;
               foreach (ClassSync.TypesPayDB type in ClassSync.TypesPayDB.t)
               {

                   decimal m = 0.0m;

                   switch (type.NameCourt)
                   {
                       case "BankChecks": m = check.PayBankChecks; break;
                       case "BankCards": m = check.PayBankCards; break;
                       case "Cash": m = check.PayCash; break;
                       case "Resto": m = check.PayResto; break;
                       case "1": m = check.Pay1; break;
                       case "2": m = check.Pay2; break;
                       case "3": m = check.Pay3; break;
                       case "4": m = check.Pay4; break;
                       case "5": m = check.Pay5; break;
                       case "6": m = check.Pay6; break;
                       case "7": m = check.Pay7; break;
                       case "8": m = check.Pay8; break;
                       case "9": m = check.Pay9; break;
                       case "10": m = check.Pay10; break;
                       case "11": m = check.Pay11; break;
                       case "12": m = check.Pay12; break;
                       case "13": m = check.Pay13; break;
                       case "14": m = check.Pay14; break;
                       case "15": m = check.Pay15; break;
                       case "16": m = check.Pay16; break;
                       case "17": m = check.Pay17; break;
                       case "18": m = check.Pay18; break;
                       case "19": m = check.Pay19; break;
                       case "20": m = check.Pay20; break;
                       default: break;
                   }

                   if (m != 0) doc.Element("check").Add(new XAttribute(type.NameCourt.TrimEnd(), m));
               }
               doc.Element("check").Add(new XAttribute("Rendu", check.Rendu));
               doc.Element("check").Add(new XAttribute("sum", check.TotalTTC));
               doc.Element("check").Add(new XAttribute("date", check.Date));
               doc.Element("check").Add(new XAttribute("DCBC", check.DCBC));
               doc.Element("check").Add(new XAttribute("DCBC_BiloPoints", check.DCBC_BiloPoints));
               doc.Element("check").Add(new XAttribute("DCBC_DobavilePoints", check.DCBC_DobavilePoints));
               doc.Element("check").Add(new XAttribute("DCBC_OtnayliPoints", check.DCBC_OtnayliPoints));
               doc.Element("check").Add(new XAttribute("DCBC_OstalosPoints", check.DCBC_OstalosPoints));
               doc.Element("check").Add(new XAttribute("DCBC_name", check.DCBC_name));


               check.PayProducts = ClassSync.ClassCloseTicketTmp.PayProducts.sel(check.CustumerId);

               foreach (var p in check.PayProducts)
               {
                   ClassProducts.product ptemp = ClassProducts.listProducts.Find(la => la.CustumerId == p.ProductId);


                   XElement el = (
                          new XElement("product",
                              new XElement("CustumerId", p.ProductId),
                              new XElement("grp", ClassGroupProduct.getGrpId(ptemp == null ? 3 : ptemp.cusumerIdSubGroup).grp),
                              new XElement("qty", p.QTY),
                              new XElement("Name", p.Name),
                              new XElement("CodeBare", p.Barcode),
                              new XElement("price", p.PriceHT),
                              new XElement("tva", ClassTVA.listTVA.Find(l => l.val == p.TVA).id),
                              new XElement("total", p.Total),
                              new XElement("Discount", p.Discount),
                              new XElement("sumDiscount", p.sumDiscount)
                              )
                          );
                   doc.Element("check").Add(el);
               }

               return doc;
           }
           */
        public static XDocument DbToXelemet(ClassSync.ClassCloseTicket.ChecksTicket check)
        {

            XDocument doc;

            doc = new XDocument(
                new XElement("check",
                    new XAttribute("barcodeCheck", check.BarCode))
                    )
                    ;
            foreach (ClassSync.TypesPayDB type in ClassSync.TypesPayDB.t)
            {

                decimal m = 0.0m;

                switch (type.NameCourt)
                {
                    case "BankChecks": m = check.PayBankChecks; break;
                    case "BankCards": m = check.PayBankCards; break;
                    case "Cash": m = check.PayCash; break;
                    case "Resto": m = check.PayResto; break;
                    case "1": m = check.Pay1; break;
                    case "2": m = check.Pay2; break;
                    case "3": m = check.Pay3; break;
                    case "4": m = check.Pay4; break;
                    case "5": m = check.Pay5; break;
                    case "6": m = check.Pay6; break;
                    case "7": m = check.Pay7; break;
                    case "8": m = check.Pay8; break;
                    case "9": m = check.Pay9; break;
                    case "10": m = check.Pay10; break;
                    case "11": m = check.Pay11; break;
                    case "12": m = check.Pay12; break;
                    case "13": m = check.Pay13; break;
                    case "14": m = check.Pay14; break;
                    case "15": m = check.Pay15; break;
                    case "16": m = check.Pay16; break;
                    case "17": m = check.Pay17; break;
                    case "18": m = check.Pay18; break;
                    case "19": m = check.Pay19; break;
                    case "20": m = check.Pay20; break;
                    default: break;
                }

                if (m != 0) doc.Element("check").Add(new XAttribute(type.NameCourt.TrimEnd(), m));
            }
            doc.Element("check").Add(new XAttribute("Rendu", check.Rendu));
            doc.Element("check").Add(new XAttribute("sum", check.TotalTTC));
            doc.Element("check").Add(new XAttribute("date", check.Date));

            doc.Element("check").Add(new XAttribute("DCBC", check.CheckDiscount.DCBC ?? ""));
            doc.Element("check").Add(new XAttribute("DCBC_BiloPoints", check.CheckDiscount.DCBC_BiloPoints ?? 0));
            doc.Element("check").Add(new XAttribute("DCBC_DobavilePoints", check.CheckDiscount.DCBC_DobavilePoints ?? 0));
            doc.Element("check").Add(new XAttribute("DCBC_OtnayliPoints", check.CheckDiscount.DCBC_OtnayliPoints ?? 0));
            doc.Element("check").Add(new XAttribute("DCBC_OstalosPoints", check.CheckDiscount.DCBC_OstalosPoints ?? 0));
            doc.Element("check").Add(new XAttribute("DCBC_name", check.CheckDiscount.DCBC_name ?? ""));


            check.PayProducts = new ClassSync.ClassCloseTicket.PayProducts().get(check.CustumerId);

            foreach (var p in check.PayProducts)
            {
                ClassProducts.product ptemp = ClassProducts.listProducts.Find(la => la.CustumerId == p.ProductId);

                XElement el = (
                       new XElement("product",
                           new XElement("CustumerId", p.ProductId),
                           new XElement("grp", ClassGroupProduct.getGrpId(ptemp == null ? 3 : ptemp.cusumerIdSubGroup).grp),
                           new XElement("qty", p.QTY),
                           new XElement("Name", p.Name),
                           new XElement("CodeBare", p.Barcode),
                           new XElement("price", p.PriceHT),
                           new XElement("tva", ClassTVA.listTVA.Find(l => l.val == p.TVA).id),
                           new XElement("total", p.Total),
                           new XElement("Discount", p.Discount),
                           new XElement("sumDiscount", p.sumDiscount)
                           )
                       );
                doc.Element("check").Add(el);
            }

            return doc;
        }
        public static void addSetDiscountCardBareCode(string barcode, int BiloP, int DobavileP, int OtnayliP, string name)
        {
            XAttribute a = b.Element("check").Attribute("DCBC");
            XAttribute a1 = b.Element("check").Attribute("DCBC_BiloPoints");
            XAttribute a2 = b.Element("check").Attribute("DCBC_DobavilePoints");
            XAttribute a3 = b.Element("check").Attribute("DCBC_OtnayliPoints");
            XAttribute a4 = b.Element("check").Attribute("DCBC_OstalosPoints");
            XAttribute a5 = b.Element("check").Attribute("DCBC_name");

            if (a == null)
            {
                b.Element("check").Add(
                    new XAttribute("DCBC", barcode),
                    new XAttribute("DCBC_BiloPoints", BiloP),
                    new XAttribute("DCBC_DobavilePoints", DobavileP),
                    new XAttribute("DCBC_OtnayliPoints", OtnayliP),
                    new XAttribute("DCBC_OstalosPoints", BiloP + DobavileP - OtnayliP),
                    new XAttribute("DCBC_name", name)
                    );
            }
            else
            {
                a.SetValue(barcode);
                a1.SetValue(BiloP);
                a2.SetValue(DobavileP);
                a3.SetValue(OtnayliP);
                a4.SetValue(BiloP + DobavileP - OtnayliP);
                a5.SetValue(name);
            }

            if (barcode != null)
            {
                tmpBarcode = barcode;
                tmp_Points = BiloP + DobavileP - OtnayliP;
                if (DobavileP > 0 || OtnayliP > 0)
                    ClassDiscounts.setDiscountPoint(tmpBarcode, tmp_Points, true);
            }
        }
        public static void discountCalc()
        {
            if (b != null)
            {
                IEnumerable<XElement> elms = ClassCheck.b.Element("check").Elements("product");

                foreach (var e in elms)
                {
                    ClassProducts.product p = ClassProducts.transform(e);
                    ClassCheck.modifProductCheck_(p.ii, p.price, p.Name, p.qty);
                }
                ClassCheck.getTotalPrice();

            }
        }

        public static bool getCheckFromChecksAndDelete(string bc)
        {

            XAttribute atr = x.Element("checks").Elements("check").Attributes("barcodeCheck").FirstOrDefault(l => l.Value.TrimEnd().TrimStart() == bc.TrimStart().TrimEnd());

            if (atr != null)
            {
                XElement elm = atr.Parent;
                openProductsCheck();

                string cmd = "";

                foreach (XElement e in elm.Elements("product"))
                {

                    b.Element("check").Add(e);

                    decimal qty = decimal.Parse(e.Element("qty").Value.Replace('.', ','));

                    Guid CustomerId = Guid.Parse(e.Element("cusumerIdRealStock").Value);

                    cmd += " " + ClassSync.ProductDB.StockReal.query_add_qty(qty, CustomerId);
                }

                int res = ClassSync.ProductDB.StockReal.response(cmd);

                if (res != elm.Elements("product").Count()) new ClassFunctuon().showMessageSB("error 276");

                elm.Remove();

                x.Save(path);

                // ClassSync.ClassCloseTicketTmp.ChecksTicket.del(bc);

                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
