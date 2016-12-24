using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;
using TicketWindow.Class;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.PortClasses;
using TicketWindow.Winows.AdditionalClasses;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Services
{
    internal class CheckService
    {
        public static string TmpBarcode { get; set; }
        public static int TmpPoints { get; set; }

        public static void BufAddXElm(XElement element)
        {
            RepositoryCheck.DocumentProductCheck = new XDocument();
            RepositoryCheck.DocumentProductCheck.Add(element);
            RepositoryDiscount.RestoreDiscount();
            DiscountCalc();
            FunctionsService.WriteTotal();
        }

        public static void AddProductCheck(ProductType product, decimal qty)
        {
            product.Discount = RepositoryDiscount.Client.Procent + RepositoryDiscount.Client.ProcentDefault;
            product.SumDiscount = Math.Truncate(product.Price*qty*product.Discount)/100;
            product.Qty = qty;
            product.Total = Math.Truncate((product.Price*product.Qty - product.SumDiscount)*100)/100;

            CassieService.OpenProductsCheck();

            var productElements = RepositoryCheck.DocumentProductCheck.GetXElements("check", "product");
            var newProductXElement = ProductType.ToCheckXElement(product, productElements);

            RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(newProductXElement);

            ClassCustomerDisplay.WritePrice(product.Name, qty, product.Price);
            FunctionsService.WriteTotal();
            ClassEtcFun.WmSound(@"Data\Beep.wav");

            RepositoryCheck.DocumentProductCheck.Save(RepositoryCheck.PathProductCheck);
        }

        private static ProductType GetBallance(ProductType product)
        {
            ClassBallance.Send(product.Price, product.Tare);

            try
            {
                var prix = ClassBallance.Prix.ToDecimal()/100;
                var qty = ClassBallance.Poinds.ToDecimal()/1000;

                if (qty > 0)
                {
                    product.Contenance = qty;
                    product.Qty = qty;
                    product.Price = prix;
                }
            }
            catch (System.Exception e)
            {
                LogService.Log(TraceLevel.Error, 21, "Error ballance =" + ClassBallance.Error + e.Message + ".");
                product = null;
            }

            if ((ClassBallance.Busy_0X15) || (ClassBallance.Error_0X15)) product = null;
            return product;
        }

        public static void AddProductCheck(XElement element, decimal qty)
        {
            CassieService.OpenProductsCheck();

            if (ClassProMode.ModePro)
                element = ClassProMode.Replace(new[] {element}).First();

            var product = ProductType.FromXElement(element);
            product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == product.CustomerId);

            if (product.Balance)
            {
                if (GetBallance(product) == null)
                {
                    var button = new Button {ToolTip = "ShowBallance"};
                    FunctionsService.Click(button, product);
                }
                else AddProductCheck(product, product.Qty);
            }
            else AddProductCheck(product, qty);
        }

        public static bool ModifProductCheck(int ii, decimal price, string name, decimal qty)
        {
            var res = ModifProductCheckInXml(ii, price, name, qty);
            FunctionsService.WriteTotal();
            return res;
        }

        public static bool ModifProductCheckInXml(int ii, decimal price, string name, decimal qty)
        {
            var productElement = RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").FirstOrDefault(l => l.GetXElementValue("ii").ToInt() == ii);

            if (productElement != null)
            {
                var sumDuscount = 0.0m;
                if (RepositoryDiscount.Client.ProcentDefault >= 0)
                {
                    sumDuscount = Math.Truncate((RepositoryDiscount.Client.Procent + RepositoryDiscount.Client.ProcentDefault)/100*price*qty*100)/100;
                    productElement.GetXElement("Discount").SetValue(RepositoryDiscount.Client.Procent + RepositoryDiscount.Client.ProcentDefault);
                    productElement.GetXElement("sumDiscount").SetValue(sumDuscount);
                }

                var priceOld = productElement.GetXElementValue("price").ToDecimal();
                if (priceOld != price)
                {
                    productElement.Add(new XElement("price_old", productElement.GetXElementValue("price")));
                    productElement.GetXElement("price").SetValue(price);
                }
                productElement.GetXElement("Name").SetValue(name);
                productElement.GetXElement("total").SetValue(Math.Truncate((price*qty - sumDuscount)*100)/100);

                RepositoryCheck.DocumentProductCheck.Save(RepositoryCheck.PathProductCheck);
                return true;
            }
            return false;
        }

        public static void DelProductCheck(int id)
        {
            RepositoryCheck.C = null;
            var selectedIndex = ((MainWindow)ClassEtcFun.FindWindow("MainWindow_")).GridProducts.SelectedIndex;
            var productElements = RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").ToList();
            if (productElements.Count == 0)
            {
                RepositoryDiscount.RestoreDiscount();
                DiscountCalc();
                FunctionsService.WriteTotal();
            }
            else
            {
                var curElement = productElements.FirstOrDefault(l => l.GetXElementValue("ii").ToInt() == id);
                if (curElement != null) curElement.Remove();
            }
            FunctionsService.WriteToatl(selectedIndex);
            RepositoryCheck.DocumentProductCheck.Save(RepositoryCheck.PathProductCheck);
        }

        public static void DelProductCheck(string barcode)
        {
            var selectedIndex = ((MainWindow)ClassEtcFun.FindWindow("MainWindow_")).GridProducts.SelectedIndex;
            var productElements = RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").Where(p => p.GetXElementValue("CodeBare").Trim() == barcode.Trim()).ToList();

            if (productElements.Count > 0)
            {
                var firstQty = productElements.First().GetXElementValue("qty").ToDecimal();

                if ((RepositoryProduct.Products.Find(l => l.CustomerId == productElements.First().GetXElementValue("id").ToGuid()).Balance) || (firstQty == 0))
                    productElements.First().Remove();
                else
                    productElements.First().GetXElement("qty").Value = (firstQty - 1).ToString();
            }
            else
            {
                RepositoryDiscount.RestoreDiscount();
                DiscountCalc();
                FunctionsService.WriteTotal();
            }
            FunctionsService.WriteToatl(selectedIndex);
            RepositoryCheck.DocumentProductCheck.Save(RepositoryCheck.PathProductCheck);
        }

        public static decimal GetTotalPrice()
        {
            decimal getTotalPrice;

            RepositoryCheck.C = XDocument.Parse(RepositoryCheck.DocumentProductCheck.ToString());
            RepositoryCheck.C = RepositoryActionHashBox.MergeProductsInCheck(RepositoryCheck.C);

            if (RepositoryCheck.C == null)
                getTotalPrice = RepositoryCheck.DocumentProductCheck != null
                    ? Convert.ToDecimal(RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").Sum(l => l.GetXElementValue("total").ToDouble()))
                    : 0.0m;
            else
                getTotalPrice = RepositoryCheck.C != null
                    ? Convert.ToDecimal(RepositoryCheck.C.GetXElements("check", "product").Sum(l => l.GetXElementValue("total").ToDouble()))
                    : 0.0m;

            RepositoryCheck.C = null;

            if (RepositoryDiscount.Client.Barcode != null && getTotalPrice >= RepositoryDiscount.Client.MoneyMax)
            {
                if ((getTotalPrice >= RepositoryDiscount.Client.MoneyMax) && (!RepositoryDiscount.Client.AddPoints))
                {
                    if ((DateTime.Now.Day == RepositoryDiscount.Client.LastDateUpd.Day)
                        && (DateTime.Now.Month == RepositoryDiscount.Client.LastDateUpd.Month)
                        && (DateTime.Now.Year == RepositoryDiscount.Client.LastDateUpd.Year))
                    {
                        if (RepositoryDiscount.Client.ShowMessaget)
                            FunctionsService.ShowMessageSb("Cette carte a déjà été utilisé aujourd'hui! " + Environment.NewLine + RepositoryDiscount.Client.LastDateUpd);

                        RepositoryDiscount.Client.ShowMessaget = false;
                    }
                    else
                    {
                        if (RepositoryDiscount.Client.Points < RepositoryDiscount.Client.MaxPoints)
                        {
                            RepositoryDiscount.Client.AddPoints = true;
                            RepositoryDiscount.Client.Points += 1;
                        }
                    }
                }
                else
                {
                    if ((getTotalPrice < RepositoryDiscount.Client.MoneyMax) && (RepositoryDiscount.Client.AddPoints))
                    {
                        RepositoryDiscount.Client.AddPoints = false;
                        RepositoryDiscount.Client.Points -= 1;
                    }
                }
            }

            return getTotalPrice;
        }

        private static void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            SyncData.SetSunc(true);
            CassieService.RemoveProductCountFromStockReal(RepositoryCheck.GetCloseTicketTmp());
        }
        
        private static void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SyncData.SetSunc(false);
            TmpPoints = 0;
            TmpBarcode = null;
        }

        public static void Bay()
        {
            RepositoryCheck.GetDucument();

            if (RepositoryCheck.Document.Element("checks") != null && RepositoryCheck.DocumentProductCheck.GetXElements("check","product").Any())
            {
                RepositoryCheck.DocumentProductCheck = RepositoryActionHashBox.MergeProductsInCheck(RepositoryCheck.DocumentProductCheck);

                try
                {
                    foreach (var type in RepositoryTypePay.TypePays)
                    {
                        var money = RepositoryCurrencyRelations.GetMoneyFromType(type);
                        RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(new XAttribute(type.NameCourt.TrimEnd(), money));
                    }
                }
                catch (System.Exception ex)
                {
                    var text = ex.Message;
                    FunctionsService.ShowMessageSb(text);
                    LogService.Log(TraceLevel.Error, 1, text + RepositoryCheck.DocumentProductCheck);
                }

                var rendu = ClassEtcFun.RenduCalc();

                try
                {
                    RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(new XAttribute("Rendu", rendu.ToString("0.00")));
                }
                catch (System.Exception ex)
                {
                    var text =  ex.Message;
                    FunctionsService.ShowMessageSb(text);
                    LogService.Log(TraceLevel.Error, 2, text + RepositoryCheck.DocumentProductCheck + ".");
                }

                try
                {
                    RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(
                        new XAttribute("sum", RepositoryCurrencyRelations.GetSumMoney()),
                        new XAttribute("date", DateTime.Now));
                }
                catch (System.Exception ex)
                {
                    var text =  ex.Message;
                    FunctionsService.ShowMessageSb(text);
                    LogService.Log(TraceLevel.Error, 3, text + RepositoryCheck.DocumentProductCheck + ".");
                }

                try
                {
                    if (RepositoryDiscount.Client.Barcode != null && !ClassProMode.ModePro)
                    {
                        AddSetDiscountCardBareCode(
                            RepositoryDiscount.Client.Barcode,
                            RepositoryDiscount.Client.Points - (RepositoryDiscount.Client.AddPoints ? 1 : 0) +
                            (RepositoryDiscount.Client.DiscountSet ? RepositoryDiscount.Client.MaxPoints : 0),
                            RepositoryDiscount.Client.AddPoints ? 1 : 0,
                            RepositoryDiscount.Client.DiscountSet ? 8 : 0,
                            RepositoryDiscount.Client.NameFirst + " " + RepositoryDiscount.Client.NameLast);
                    }
                }
                catch (System.Exception ex)
                {
                    var text =  ex.Message;
                    FunctionsService.ShowMessageSb(text);
                    LogService.Log(TraceLevel.Error, 4, text + RepositoryCheck.DocumentProductCheck + ".");
                }
                
                    if (ClassProMode.ModePro || ClassProMode.Devis)
                        ClassProMode.Move(ClassProMode.Devis);
                    else
                        RepositoryCheck.Document.GetXElement("checks").Add(RepositoryCheck.DocumentProductCheck.Element("check"));
              
                    if (!ClassProMode.ModePro && !ClassProMode.Devis)
                        new ClassPrintCheck(RepositoryCheck.DocumentProductCheck, false);
                
                try
                {
                    var worker = new BackgroundWorker();

                    if (!ClassProMode.ModePro && !ClassProMode.Devis)
                    {
                        worker.DoWork += WorkerDoWork;
                        worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
                        worker.RunWorkerAsync();
                    }
                }
                catch (System.Exception ex)
                {
                    var text =  ex.Message;
                    LogService.Log(TraceLevel.Error, 7, text + RepositoryCheck.DocumentProductCheck + ".");
                }

                try
                {
                    if (!ClassProMode.ModePro && !ClassProMode.Devis)
                    {
                        RepositoryCheck.Document.Save(RepositoryCheck.Path);
                        RepositoryCheck.DocumentProductCheck = null;
                        CassieService.OpenProductsCheck();
                        RepositoryDiscount.RestoreDiscount();
                        DiscountCalc();
                        FunctionsService.WriteTotal();
                    }
                }
                catch (System.Exception ex)
                {
                    var text =  ex.Message;
                    FunctionsService.ShowMessageSb(text);
                    LogService.Log(TraceLevel.Error, 8, text + RepositoryCheck.DocumentProductCheck + ".");
                }
            }
            else FunctionsService.ShowMessageSb("файл check.xml отсутвует или структура не правильная");
        }

        public static void EnAttenete()
        {
            RepositorySyncPlusProduct.SetCheck(RepositoryCheck.DocumentProductCheck);
            RepositoryCheck.DocumentProductCheck = null;
            CassieService.OpenProductsCheck();
        }

        public static void SaveEnAttenete(Guid customerId)
        {
            RepositorySyncPlusProduct.GetCheck(customerId);
        }
        
        public static void AddSetDiscountCardBareCode(string barcode, int biloP, int dobavileP, int otnayliP, string name)
        {
            var a = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC");
            var a1 = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC_BiloPoints");
            var a2 = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC_DobavilePoints");
            var a3 = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC_OtnayliPoints");
            var a4 = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC_OstalosPoints");
            var a5 = RepositoryCheck.DocumentProductCheck.GetXAttribute("check","DCBC_name");

            if (a == null)
            {
                RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(
                    new XAttribute("DCBC", barcode),
                    new XAttribute("DCBC_BiloPoints", biloP),
                    new XAttribute("DCBC_DobavilePoints", dobavileP),
                    new XAttribute("DCBC_OtnayliPoints", otnayliP),
                    new XAttribute("DCBC_OstalosPoints", biloP + dobavileP - otnayliP),
                    new XAttribute("DCBC_name", name));
            }
            else
            {
                a.SetValue(barcode);
                a1.SetValue(biloP);
                a2.SetValue(dobavileP);
                a3.SetValue(otnayliP);
                a4.SetValue(biloP + dobavileP - otnayliP);
                a5.SetValue(name);
            }

            if (barcode != null)
            {
                TmpBarcode = barcode;
                TmpPoints = biloP + dobavileP - otnayliP;
                if (dobavileP > 0 || otnayliP > 0)
                    RepositoryDiscount.SetDiscountPoint(TmpBarcode, TmpPoints, true);
            }
        }

        public static void DiscountCalc()
        {
            if (RepositoryCheck.DocumentProductCheck != null)
            {
                foreach (var p in RepositoryCheck.DocumentProductCheck.GetXElements("check", "product").Select(ProductType.FromXElement))
                {
                    ModifProductCheckInXml(p.Ii, p.Price, p.Name, p.Qty);
                }
                GetTotalPrice();
            }
        }

        public static bool GetCheckFromChecksAndDelete(string bc)
        {
            var atr = RepositoryCheck.Document.GetXAttributes("checks", "check", "barcodeCheck").FirstOrDefault(l => l.Value.Trim() == bc.Trim());

            if (atr != null)
            {
                var elm = atr.Parent;
                CassieService.OpenProductsCheck();
                
                foreach (var e in elm.Elements("product"))
                {
                    RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(e);

                    var qty = e.GetXElementValue("qty").ToDecimal();
                    var customerId = e.GetXElementValue("cusumerIdRealStock").ToGuid();

                    RepositoryStockReal.UpdateProductCount(qty, customerId);
                }

                elm.Remove();
                RepositoryCheck.Document.Save(RepositoryCheck.Path);
                return true;
            }
            return false;
        }
    }
}