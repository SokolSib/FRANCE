using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.Services;
using TicketWindow.Winows.AdditionalClasses;

namespace TicketWindow.Class
{
    public class ClassProMode
    {
        public static bool ModePro;
        public static bool Devis;
        public static int Ndevis = -1;
        public static int Nfact = -1;
        public static XDocument Check;
        private static BackgroundWorker _worker;
        public static Pro Pro { get; set; }

        public static decimal PrixOnlyTva(decimal prix, decimal tva)
        {
            var ttc = Math.Round(prix*((100 + tva)/100), 2);
            return ttc - prix;
        }

        public static decimal PrixTtcOnlyTva(decimal prix, decimal tva)
        {
            var ht = Math.Round((prix/(100 + tva))*100, 2);
            return prix - ht;
        }

        public static XDocument ReplaceCheck(XDocument document, bool back)
        {
            var productsElements = document.GetXElements("check", "product");
            var x = new List<XElement>();

            foreach (var product in productsElements)
            {
                var copyProduct = x.FirstOrDefault(l => l.GetXElementValue("CustomerId").ToGuid() == product.GetXElementValue("CustomerId").ToGuid());

                if (copyProduct == null)
                    x.Add(product);
                else
                {
                    var qty = copyProduct.GetXElementValue("qty").ToDecimal();
                    var total = copyProduct.GetXElementValue("total").ToDecimal();
                    var qtyProduct = product.GetXElementValue("qty").ToDecimal();
                    var totalProduct = product.GetXElementValue("total").ToDecimal();

                    copyProduct.GetXElement("qty").SetValue(qty + qtyProduct);
                    copyProduct.GetXElement("total").SetValue(total + totalProduct);
                }
            }

            IEnumerable<XElement> elements = Replace(x.ToArray());

            document.GetXElement("check").RemoveAll();

            foreach (var item in elements)
                document.GetXElement("check").Add(item);

            return document;
        }

        private static PriceGrosType[] Get(Guid[] g)
        {
            var res = new PriceGrosType[g.Length];

            for (var i = 0; i < res.Length; i++)
            {
                var x = RepositoryProduct.GetXElementByElementName("CustomerId", g[i].ToString());
                var procent = (decimal) (Pro.DiscountValue)/100;
                var prix = x.GetXElementValue("price").ToDecimal();

                res[i] = new PriceGrosType(x.GetXElementValue("cusumerIdRealStock").ToGuid(), x.GetXElementValue("CustomerId").ToGuid(), prix + prix * procent);
            }

            return res;
        }

        private static PriceGrosType[] GetGros(Guid[] g)
        {
            var res = new PriceGrosType[g.Length];

            for (var i = 0; i < res.Length; i++)
            {
                var x = RepositoryProduct.GetXElementByElementName("CustomerId", g[i].ToString());
                var pg = x.GetXElementValue("priceGros").ToDecimal();
                decimal procent = Pro.DiscountValue;
                pg = pg + (pg*procent/100);
                var tva1 = RepositoryTva.GetById(x.GetXElementValue("tva").ToInt());
                var tva = PrixOnlyTva(pg, tva1);
                pg = pg + tva;

                res[i] = new PriceGrosType(x.GetXElementValue("cusumerIdRealStock").ToGuid(), x.GetXElementValue("CustomerId").ToGuid(), pg);
            }
            return res;
        }

        private static XElement XelementCopy(XElement xe)
        {
            var x = new XDocument();
            x.Add(new XElement("rec"));
            var p = ProductType.FromXElement(xe);

            x.GetXElement("rec").Add(
                new XElement("ii", p.Ii),
                new XElement("CustomerId", p.CustomerId),
                new XElement("Name", p.Name.ToUpper()),
                new XElement("Desc", p.Desc),
                new XElement("price", p.Price),
                new XElement("priceGros", p.PriceGros),
                new XElement("tva", p.Tva),
                new XElement("qty", p.Qty),
                new XElement("CodeBare", p.CodeBare),
                new XElement("balance", p.Balance),
                new XElement("contenance", p.Contenance),
                new XElement("uniteContenance", p.UniteContenance),
                new XElement("tare", p.Tare),
                new XElement("date", p.Date ?? DateTime.Now),
                new XElement("cusumerIdRealStock", p.CusumerIdRealStock),
                new XElement("cusumerIdSubGroup", p.CusumerIdSubGroup),
                new XElement("ProductsWeb_CustomerId", p.ProductsWebCustomerId),
                new XElement("grp", p.Grp),
                new XElement("sgrp", p.Sgrp));

            return x.Element("rec");
        }

        public static XElement[] Replace(XElement[] products)
        {
            var p = new XElement[products.Length];
            var g = new Guid[p.Length];

            for (var i = 0; i < g.Length; i++)
            {
                var nx = XelementCopy(products[i]);
                p[i] = nx;
                g[i] = p[i].GetXElementValue("CustomerId").ToGuid();
            }

            var typePro = RepositoryInfoClientsDiscountsType.InfoClientsDiscounts.First(l => l.Id == Pro.DiscountType).Type;

            var sg = ModePro && typePro == 1 ? GetGros(g) : Get(g);

            for (var i = 0; i < p.Length; i++)
            {
                p[i].GetXElement("cusumerIdRealStock").SetValue(sg[i].CustomerId);
                p[i].GetXElement("price").SetValue(sg[i].Prix);

                if (p[i].Element("total") != null)
                {
                    var qty = p[i].GetXElementValue("qty").ToDecimal();
                    p[i].GetXElement("total").SetValue(qty * sg[i].Prix);
                }
            }
            return p;
        }

        public static string ReplaceCodeBare(string cb)
        {
            var b = cb.IndexOf(']');
            var a = cb.IndexOf('[');
            if (a != -1 && b != -1)
                return cb.Substring(a + 1, b - 1);

            return cb;
        }

        public static void XmlToDbSave(XDocument x)
        {
            var tes = new Tes
                      {
                          CustomerId = Guid.NewGuid(),
                          ACodeFournisseur = 0,
                          AFrtva = Pro.Frtva ?? string.Empty,
                          AMail = Pro.Mail,
                          AName = Pro.Name ?? string.Empty,
                          ASex = Pro.SexToInt(Pro.Sex),
                          ASurname = Pro.Surname ?? string.Empty,
                          ANameCompany = Pro.NameCompany,
                          AOfficeAddress = Pro.OfficeAddress ?? string.Empty,
                          AOfficeCity = Pro.OfficeCity ?? string.Empty,
                          AOfficeZipCode = Pro.OfficeZipCode ?? string.Empty,
                          ASiret = Pro.Siret ?? string.Empty,
                          ATelephone = Pro.Telephone
                      };

            tes.CustomerId = Guid.NewGuid();
            tes.DateTime = DateTime.Now;
            tes.Description = string.Empty;
            tes.Id = RepositoryTes.MaxId(0) + 1;
            tes.Livraison = false;
            tes.Montant = x.GetXAttributeValue("check", "sum").ToDecimal();
            tes.Nclient = Pro.Nclient.ToString();
            tes.Payement = false;
            tes.Type = 0;
            tes.VAdresse = RepositoryEstablishment.Establishment.Adress;
            tes.VCodeNaf = "0";
            tes.VCp = RepositoryEstablishment.Establishment.Cp;
            tes.VFax = RepositoryEstablishment.Establishment.Phone;
            tes.VFrtva = "";
            tes.VMail = RepositoryEstablishment.Establishment.Mail;
            tes.VNameCompany = RepositoryEstablishment.Establishment.Name;
            tes.VPhone = RepositoryEstablishment.Establishment.Phone;
            tes.VSiret = "";
            tes.VVille = RepositoryEstablishment.Establishment.Ville;
            var products = x.GetXElements("check","product");

            var p = new List<XElement>();

            foreach (var product in products)
            {
                var pf = p.Find(l => l.GetXElementValue("CustomerId") == product.GetXElementValue("CustomerId"));

                if (pf == null || (pf.GetXElementValue("price") != product.GetXElementValue("price")))
                    p.Add(product);
                else
                {
                    var qty = pf.GetXElementValue("qty").ToDecimal();
                    var qty1 = product.GetXElementValue("qty").ToDecimal();
                    var total = pf.GetXElementValue("total").ToDecimal();
                    var total1 = product.GetXElementValue("total").ToDecimal();
                    pf.GetXElement("qty").SetValue(qty + qty1);
                    pf.GetXElement("total").SetValue(total + total1);
                }
            }
            x.GetXElements("check","product").Remove();

            foreach (var product in p)
            {
                x.GetXElement("check").Add(product);
                var prod = new TesProduct
                           {
                               Balance = product.GetXElementValue("balance").ToBool(),
                               CodeBar = ReplaceCodeBare(product.GetXElementValue("CodeBare")),
                               ConditionAchat = 1,
                               CustomerId = -1,
                               CustomerIdProduct = product.GetXElementValue("CustomerId").ToGuid(),
                               Date = DateTime.Now,
                               Description = Config.Name,
                               Group = RepositoryGroupProduct.GetGroupNameById(product.GetXElementValue("grp").ToInt()),
                               NameProduct = product.GetXElementValue("Name"),
                               PrixHt = Math.Round(product.GetXElementValue("price").ToDecimal(), 2),
                               ProductsWeb = product.GetXElementValue("ProductsWeb_CustomerId").ToGuid(),
                               Qty = product.GetXElementValue("qty").Replace('.', ',').ToDecimal(),
                               SubGroup = RepositoryGroupProduct.GetGroupNameById(product.GetXElementValue("grp").ToInt()),
                               TesCustomerId = tes.CustomerId,
                               Tva = RepositoryTva.GetById(product.GetXElementValue("tva").ToInt())
                           };

                var tva = PrixTtcOnlyTva(product.GetXElementValue("price").ToDecimal(), prod.Tva);
                prod.PrixHt = prod.PrixHt - tva;
                prod.TotalHt = prod.Qty*prod.PrixHt; 
                prod.TypeId = 1;
                tes.TesProducts.Add(prod);

                var qty = prod.Qty;
                var cusumerIdRealStock = product.GetXElementValue("cusumerIdRealStock").ToGuid();

                RepositoryStockReal.AddProductCount(-qty, cusumerIdRealStock);
            }

            foreach (var type in RepositoryTypePay.TypePays)
            {
                var reglement = new TesReglament();
                var r = RepositoryTypePay.TypePays.FirstOrDefault(l => l.NameCourt == type.NameCourt);
                var elm = x.GetXAttribute("check",type.NameCourt);

                if (elm != null && r != null)
                {
                    reglement.Caisse = Config.NameTicket;
                    reglement.CustomerId = Guid.NewGuid();
                    reglement.DateTime = DateTime.Now;

                    if (r.Name == "En espèces")
                    {
                        var rendux = x.GetXAttribute("check", "Rendu");

                        if (rendux != null)
                        {
                            var rendu = rendux.Value.ToDecimal();
                            reglement.Montant = elm.Value.ToDecimal() + rendu;
                        }
                        else reglement.Montant = elm.Value.ToDecimal();
                    }
                    else reglement.Montant = elm.Value.ToDecimal();
                    
                    reglement.TesCustomerId = tes.CustomerId;
                    reglement.TypePay = r.Name;
                    tes.TesReglaments.Add(reglement);
                }
                tes.Payement = (tes.TesReglaments.Sum(l => l.Montant) >= tes.Montant);
            }

            if (tes.TesReglaments.Count > 0)
            {
                var newTes = new Tes
                             {
                                 CustomerId = Guid.NewGuid(),
                                 ACodeFournisseur = (short) (Pro.Nclient),
                                 AFrtva = Pro.Frtva ?? string.Empty,
                                 AMail = Pro.Mail,
                                 AName = Pro.Name ?? string.Empty,
                                 ASex = Pro.SexToInt(Pro.Sex),
                                 ASurname = Pro.Surname ?? string.Empty,
                                 ANameCompany = Pro.NameCompany,
                                 AOfficeAddress = Pro.OfficeAddress ?? string.Empty,
                                 AOfficeCity = Pro.OfficeCity ?? string.Empty,
                                 AOfficeZipCode = Pro.OfficeZipCode ?? string.Empty,
                                 ASiret = Pro.Siret ?? string.Empty,
                                 ATelephone = Pro.Telephone,
                                 DateTime = DateTime.Now,
                                 Description = "{" + tes.CustomerId + "}{" + tes.Id + "}{" + tes.Type + "}",
                                 Id = RepositoryTes.MaxId(1) + 1,
                                 Livraison = tes.Livraison,
                                 Montant = decimal.Parse(x.Element("check").Attribute("sum").Value.Replace('.', ',')),
                                 Nclient = Pro.Nclient.ToString(),
                                 Payement = tes.Payement,
                                 Type = 1,
                                 VAdresse = tes.VAdresse,
                                 VCodeNaf = tes.VCodeNaf,
                                 VCp = tes.VCp,
                                 VFax = tes.VFax,
                                 VFrtva = tes.VFrtva,
                                 VMail = tes.VMail,
                                 VNameCompany = tes.VNameCompany,
                                 VPhone = tes.VPhone,
                                 VSiret = tes.VSiret,
                                 VVille = tes.VVille
                             };
                tes.Description = "{" + newTes.CustomerId + "}{" + newTes.Id + "}{" + newTes.Type + "}";

                foreach (var t in tes.TesReglaments)
                {
                    if (t.Montant > 0)
                    {
                        t.TesCustomerId = newTes.CustomerId;
                        newTes.TesReglaments.Add(t);
                    }
                }
                tes.TesReglaments.Clear();

                RepositoryTes.AddToDb(newTes);
            }

            RepositoryTes.AddToDb(tes);

            Nfact = tes.Id ?? -1;
            FileMove();
        }

        public static void FileMove()
        {
            var dt = DateTime.Now;
            var bc = Config.NumberTicket + dt.ToString("HHmmss") + string.Format("{0:0000}", Devis ? Ndevis : Nfact);

            if (RepositoryCheck.DocumentProductCheck.GetXAttribute("check","barcodeCheck") != null)
                RepositoryCheck.DocumentProductCheck.GetXAttribute("check","barcodeCheck").SetValue(bc);
            else
                RepositoryCheck.DocumentProductCheck.GetXElement("check").Add(new XAttribute("barcodeCheck", bc));

            var dir = AppDomain.CurrentDomain.BaseDirectory + (Devis ? @"\Data\ProDevis" : @"\Data\ProFacture")
                      + DateTime.Now.Year + @"\" + DateTime.Now.Month;

            Directory.CreateDirectory(dir);

            var file = dir + @"\" + DateTime.Now.Day + "_"
                       + DateTime.Now.Hour
                       + "_" + DateTime.Now.Minute
                       + "_" + DateTime.Now.Second + "_" + (Pro != null ? Pro.NameCompany : "unkwn") + ".xml";

            var path = AppDomain.CurrentDomain.BaseDirectory + @"Data\b.xml";

            File.Move(path, file);
        }

        public static void XmlToDbSaveD(XDocument x)
        {
            var infoClientsCustomerId = ModePro ? Pro.CustomerId : new Guid("87da7ba2-5e52-4d31-8def-5a2dad508e94");

            var di = new DevisIdType(-1, DateTime.Now, false, infoClientsCustomerId, 0);

            var products = x.GetXElements("check","product");
            var p = new List<XElement>();

            foreach (var product in products)
            {
                var pf = p.Find(l => (l.GetXElementValue("CustomerId") == product.GetXElementValue("CustomerId")));

                if (pf == null || (pf.GetXElementValue("price") != product.GetXElementValue("price")))
                    p.Add(product);
                else
                {
                    var qty = pf.GetXElementValue("qty").ToDecimal();
                    var qty1 = product.GetXElementValue("qty").ToDecimal();
                    var total = pf.GetXElementValue("total").ToDecimal();
                    var total1 = product.GetXElementValue("total").ToDecimal();
                    pf.GetXElement("qty").SetValue(qty + qty1);
                    pf.GetXElement("total").SetValue(total + total1);
                }
            }

            x.GetXElements("check","product").Remove();

            di.DivisWebs = new List<DevisWebType>();

            foreach (var product in p)
            {
                x.GetXElement("check").Add(product);
                var prixHt = Math.Round(product.GetXElementValue("price").ToDecimal(), 2);
                var tva = RepositoryTva.GetById(product.GetXElementValue("tva").ToInt());
                var tva1 = PrixTtcOnlyTva(product.GetXElementValue("price").ToDecimal(), tva);
                var monPrixHt = product.GetXElementValue("contenance").ToDecimal();
                var qty = product.GetXElementValue("qty").ToDecimal();
                var prixHtValue = prixHt - tva1;
                var dw = new DevisWebType(Guid.NewGuid(), -1, prixHtValue, monPrixHt == 0 ? 1 : monPrixHt, qty,
                    qty * prixHtValue, 0, true, product.GetXElementValue("CustomerId").ToGuid(), di.InfoClientsCustomerId ?? Guid.Empty);

                var cusumerIdRealStock = Guid.Parse(product.GetXElementValue("cusumerIdRealStock").Replace('.', ','));

                RepositoryStockReal.AddProductCount(-dw.Qty, cusumerIdRealStock);

                RepositoryStockReal.UpdateProductCountByEstablishment(dw.Qty, Config.IdEstablishmentGros, dw.ProductsCustomerId);
                di.DivisWebs.Add(dw);
            }

            di.Total = di.DivisWebs.Sum(l => l.TotalHt);
            SyncService.InsDevis(di);
            FileMove();
        }

        private static void WorkerDoWorkFact(object sender, DoWorkEventArgs e)
        {
            SyncData.SetSunc(true);
            XmlToDbSave(RepositoryCheck.DocumentProductCheck);
        }

        private static void WorkerDoWorkDevis(object sender, DoWorkEventArgs e)
        {
            SyncData.SetSunc(true);
            XmlToDbSaveD(RepositoryCheck.DocumentProductCheck);
        }

        private static void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SyncData.SetSunc(false);
            new ClassPrintCheck(RepositoryCheck.DocumentProductCheck, false);
            RepositoryCheck.DocumentProductCheck = null;
            CassieService.OpenProductsCheck();
            Pro = null;
            Ndevis = -1;
            Nfact = -1;
            Check = null;
            Devis = false;
            ModePro = false;

            RepositoryDiscount.RestoreDiscount();
            CheckService.DiscountCalc();
            FunctionsService.WriteTotal();

            var de = ClassEtcFun.FindWindow("_W_Message");
            if (de != null)
                de.Close();

            var mw = ClassEtcFun.FindWindow("MainWindow_");

            if (mw != null)
                mw.IsEnabled = true;
        }

        public static void Move(bool devis)
        {
            FunctionsService.ShowMessage(null, " please  waite....");

            var mw = ClassEtcFun.FindWindow("MainWindow_");

            if (mw != null)
            {
                mw.IsEnabled = false;
            }
            _worker = new BackgroundWorker();

            if (devis)
            {
                _worker.DoWork += WorkerDoWorkDevis;
                _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            }
            else
            {
                _worker.DoWork += WorkerDoWorkFact;
                _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            }

            _worker.RunWorkerAsync();
        }
    }
}