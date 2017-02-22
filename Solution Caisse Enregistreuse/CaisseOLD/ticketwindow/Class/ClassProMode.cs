using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    public class ClassProMode
    {
        public static bool modePro = false;
        public static bool devis = false;

        public static int ndevis = -1;
        public static int nfact = -1;
        public static ClassSync.classPro pro { get; set; }

        public static XDocument check;

        public static decimal PrixOnlyTVA(decimal prix, decimal tva)
        {

            decimal TTC = Math.Round(prix * ((100 + tva) / 100), 2);

            decimal TVA = TTC - prix;

            return TVA;

        }


        public static decimal PrixTTCOnlyTVA(decimal prix, decimal tva)
        {

            decimal HT = Math.Round((prix / (100 + tva)) * 100, 2);

            decimal TVA = prix - HT;

            return TVA;

        }



        public static XDocument replaceCheck(XDocument b, bool back)
        {


            var p = b.Element("check").Elements("product");

            var x = new List<XElement>();

            foreach (var elm in p)
            {
                var f = x.FirstOrDefault(l => Guid.Parse(l.Element("CustumerId").Value) == Guid.Parse(elm.Element("CustumerId").Value));

                if (f == null)
                {
                    x.Add(elm);
                }
                else
                {
                    decimal qty = decimal.Parse(f.Element("qty").Value.Replace('.', ','));

                    decimal total = decimal.Parse(f.Element("total").Value.Replace('.', ','));

                    decimal qty_ = decimal.Parse(elm.Element("qty").Value.Replace('.', ','));

                    decimal total_ = decimal.Parse(elm.Element("total").Value.Replace('.', ','));

                    f.Element("qty").SetValue(qty + qty_);

                    f.Element("total").SetValue(total + total_);
                }
            }


            IEnumerable<XElement> elms = replace(x.ToArray());

            b.Element("check").RemoveAll();

            foreach (var item in elms)
                b.Element("check").Add(item);

            return b;
        }

        private static ClassSync.classPriceGros[] get(Guid[] g)
        {
            var res = new ClassSync.classPriceGros[g.Length];

            for (int i = 0; i < res.Length; i++)
            {
                XElement x = ClassProducts.findCustomerId(g[i]);


                decimal procent = (decimal)(ClassSync.classPro.getDiscountValue(ClassProMode.pro)) / 100;

                decimal prix = decimal.Parse(x.Element("price").Value.Replace('.', ','));

                res[i] = new ClassSync.classPriceGros(new object[] {

                    Guid.Parse( x.Element("cusumerIdRealStock").Value),

                    Guid.Parse (x.Element("CustumerId").Value),

                    prix + prix * procent
            });
            }

            return res;
        }

        private static ClassSync.classPriceGros[] get_gros(Guid[] g)
        {
            var res = new ClassSync.classPriceGros[g.Length];

            for (int i = 0; i < res.Length; i++)
            {
                XElement x = ClassProducts.findCustomerId(g[i]);


                decimal pg = decimal.Parse(x.Element("priceGros").Value.Replace('.', ','));


                decimal procent = (decimal)(ClassSync.classPro.getDiscountValue(ClassProMode.pro));



                pg = pg + (pg * procent / 100);



                decimal tva_ = ClassTVA.getTVA(int.Parse(x.Element("tva").Value.Replace('.', ',')));

                decimal tva = PrixOnlyTVA(pg, tva_);

                pg = pg + tva;


                res[i] = new ClassSync.classPriceGros(new object[] {

                    Guid.Parse( x.Element("cusumerIdRealStock").Value),

                    Guid.Parse (x.Element("CustumerId").Value),

                    pg


            });
            }

            return res;
        }


        private static XElement XelementCopy(XElement xe)
        {
            XDocument x = new XDocument();

            x.Add(new XElement("rec"));

            ClassProducts.product p = ClassProducts.transform(xe);

            x.Element("rec").Add(
                      new XElement("ii", p.ii),
                      new XElement("CustumerId", p.CustumerId),
                      new XElement("Name", p.Name.ToUpper()),
                      new XElement("Desc", p.Desc),
                      new XElement("price", p.price),
                      new XElement("priceGros", p.priceGros),
                      new XElement("tva", p.tva),
                      new XElement("qty", p.qty),
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
                      new XElement("sgrp", p.sgrp)
                  );

            return x.Element("rec");
        }

        public static XElement[] replace(XElement[] p_)
        {

            XElement[] p = new XElement[p_.Length];



            Guid[] g = new Guid[p.Length];

            for (int i = 0; i < g.Length; i++)
            {
                XElement nx = XelementCopy(p_[i]);

                p[i] = nx;

                g[i] = Guid.Parse(p[i].Element("CustumerId").Value);


            }

            int typePro = ClassSync.classPro.InfoClientsDiscountsType.L.First(l => l.Id == ClassSync.classPro.getDiscountType(pro)).Type;

            var sg = modePro && typePro == 1 ? // ClassSync.classPriceGros.get(g) 
                get_gros(g)
                : get(g);

            for (int i = 0; i < p.Length; i++)
            {
                p[i].Element("cusumerIdRealStock").SetValue(sg[i].CustomerId);

                p[i].Element("price").SetValue(sg[i].Prix);

                if (p[i].Element("total") != null)
                {
                    decimal qty = Decimal.Parse(p[i].Element("qty").Value.Replace('.', ','));

                    p[i].Element("total").SetValue(qty * sg[i].Prix);
                }
            }



            return p;
        }

        public static string replaceCodeBare (string cb)
        {
            int b = cb.IndexOf(']');

            int a = cb.IndexOf('[');

            if (a != -1 && b != -1)

                return cb.Substring(a+1, b-1);

            else return cb;
        }

        public static void xmlToDbSave(XDocument x)
        {
            ClassSync.TES Tes = new ClassSync.TES();

            Tes.CustomerId = Guid.NewGuid();

            Tes.a_CodeFournisseur = 0; //short.Parse( pro.Nclient);

            Tes.a_FRTVA = ClassSync.classPro.getFRTVA(pro);

            Tes.a_Mail = pro.Mail;

            Tes.a_Name = ClassSync.classPro.getName(pro) ?? "";

            Tes.a_Sex = ClassSync.classPro.SexToInt(ClassSync.classPro.getSex(pro));

            Tes.a_Surname = ClassSync.classPro.getSurName(pro) ?? "";

            Tes.a_NameCompany = pro.NameCompany;

            Tes.a_OfficeAddress = ClassSync.classPro.getOfficeAddress(pro);

            Tes.a_OfficeCity = ClassSync.classPro.getOfficeCity(pro);

            Tes.a_OfficeZipCode = ClassSync.classPro.getOfficeZipCode(pro);



            Tes.a_SIRET = ClassSync.classPro.getSIRET(pro);



            Tes.a_Telephone = pro.Telephone;

            Tes.CustomerId = Guid.NewGuid();

            Tes.DateTime = DateTime.Now;

            Tes.Description = "";

            Tes.Id = ClassSync.TES.maxId(0) + 1;

            Tes.Livraison = false;

            Tes.Montant = decimal.Parse(x.Element("check").Attribute("sum").Value.Replace('.', ','));

            Tes.Nclient = pro.Nclient.ToString();

            Tes.Payement = false;

            Tes.Type = 0;

            Tes.v_Adresse = ClassGlobalVar.Establishment.Adresse;

            Tes.v_CodeNAF = "0";

            Tes.v_CP = ClassGlobalVar.Establishment.CP;

            Tes.v_Fax = ClassGlobalVar.Establishment.Phone;

            Tes.v_FRTVA = "";

            Tes.v_Mail = ClassGlobalVar.Establishment.Mail;

            Tes.v_NameCompany = ClassGlobalVar.Establishment.Name;

            Tes.v_Phone = ClassGlobalVar.Establishment.Phone;

            Tes.v_SIRET = "";

            Tes.v_Ville = ClassGlobalVar.Establishment.Ville;


            IEnumerable<XElement> products = x.Element("check").Elements("product");

            string cmdSr = " ";

            List<XElement> p = new List<XElement>();

            foreach (var product in products)
            {
                var pf = p.Find(l => (l.Element("CustumerId").Value == product.Element("CustumerId").Value));


                if (pf == null || (pf.Element("price").Value != product.Element("price").Value))
                    p.Add(product);

                else
                {
                    decimal qty = decimal.Parse(pf.Element("qty").Value.Replace('.', ','));
                    decimal qty_ = decimal.Parse(product.Element("qty").Value.Replace('.', ','));
                    decimal total = decimal.Parse(pf.Element("total").Value.Replace('.', ','));
                    decimal total_ = decimal.Parse(product.Element("total").Value.Replace('.', ','));
                    pf.Element("qty").SetValue(qty + qty_);
                    pf.Element("total").SetValue(total + total_);

                }
            }

            x.Element("check").Elements("product").Remove();



            foreach (var product in p)
            {
                x.Element("check").Add(product);

                ClassSync.TESproducts prod = new ClassSync.TESproducts();

                prod.Balance = bool.Parse(product.Element("balance").Value);

                prod.CodeBar = replaceCodeBare (product.Element("CodeBare").Value);

                prod.ConditionAchat = 1;// decimal.Parse( product.Element("qty").Value);

                prod.CustomerId = -1;

                prod.CustomerIdProduct = Guid.Parse(product.Element("CustumerId").Value);

                prod.Date = DateTime.Now;

                prod.Description = ClassGlobalVar.Name;

                prod.Group = ClassGroupProduct.getName(int.Parse(product.Element("grp").Value));

                prod.NameProduct = product.Element("Name").Value;

                prod.PrixHT = Math.Round(decimal.Parse(product.Element("price").Value.Replace('.', ',')), 2);

                prod.ProductsWeb = Guid.Parse(product.Element("ProductsWeb_CustomerId").Value);

                prod.QTY = decimal.Parse(product.Element("qty").Value.Replace('.', ','));

                prod.SubGroup = ClassGroupProduct.getName(int.Parse(product.Element("grp").Value));

                prod.TESCustomerId = Tes.CustomerId;

                prod.TVA = ClassTVA.getTVA(int.Parse(product.Element("tva").Value));

                decimal tva = PrixTTCOnlyTVA(decimal.Parse(product.Element("price").Value.Replace('.', ',')), prod.TVA);

                prod.PrixHT = prod.PrixHT - tva;

                prod.TotalHT = prod.QTY * prod.PrixHT; //decimal.Parse(product.Element("total").Value.Replace('.', ',')) - totalOnlyTVA(product);

                prod.TypeID = 1;

                Tes.TESproducts.Add(prod);

                decimal qty = prod.QTY;

                Guid cusumerIdRealStock = Guid.Parse(product.Element("cusumerIdRealStock").Value.Replace('.', ','));

                cmdSr += ClassSync.ProductDB.StockReal.query_add_qty(-qty, cusumerIdRealStock);
            }

            foreach (ClassSync.TypesPayDB type in ClassSync.TypesPayDB.t)
            {

                ClassSync.TESreglement reglement = new ClassSync.TESreglement();

                var r = reglement.TypePayItems.FirstOrDefault(l => l.Text == type.NameCourt);

                var elm = x.Element("check").Attribute(type.NameCourt);

                if (elm != null && r != null)
                {
                    reglement.Caisse = ClassGlobalVar.nameTicket;

                    reglement.CustomerId = Guid.NewGuid();

                    reglement.DateTime = DateTime.Now;

                    if (r.Value == "En espèces")
                    {

                        var rendux = x.Element("check").Attribute("Rendu");

                        if (rendux != null)
                        {
                            decimal rendu = decimal.Parse(rendux.Value.Replace('.', ','));

                            reglement.Montant = decimal.Parse(elm.Value.Replace('.', ',')) + rendu;
                        }
                        else

                            reglement.Montant = decimal.Parse(elm.Value.Replace('.', ','));
                    }
                    else
                    {
                        reglement.Montant = decimal.Parse(elm.Value.Replace('.', ','));
                    }
                    reglement.TESCustomerId = Tes.CustomerId;

                    reglement.TypePay = r.Value;

                    Tes.TESreglement.Add(reglement);


                }
                Tes.Payement = (Tes.TESreglement.Sum(l => l.Montant) >= Tes.Montant);
            }


            int c_tes = 0;
            int c_tes_ = 0;
            int c_sr = 0;

            if (Tes.TESreglement.Count > 0)
            {
                ClassSync.TES Tes_ = new ClassSync.TES();

                Tes_.CustomerId = Guid.NewGuid();

                Tes_.a_CodeFournisseur = (short)(pro.Nclient);

                Tes_.a_FRTVA = ClassSync.classPro.getFRTVA(pro);

                Tes_.a_Mail = pro.Mail;

                Tes_.a_Name = ClassSync.classPro.getName(pro) ?? "";

                Tes_.a_Sex = ClassSync.classPro.SexToInt(ClassSync.classPro.getSex(pro));

                Tes_.a_Surname = ClassSync.classPro.getSurName(pro) ?? "";


                Tes_.a_NameCompany = pro.NameCompany;

                Tes_.a_OfficeAddress = ClassSync.classPro.getOfficeAddress(pro);

                Tes_.a_OfficeCity = ClassSync.classPro.getOfficeCity(pro);

                Tes_.a_OfficeZipCode = ClassSync.classPro.getOfficeZipCode(pro);



                Tes_.a_SIRET = ClassSync.classPro.getSIRET(pro);

                Tes_.a_Telephone = pro.Telephone;

                Tes_.CustomerId = Guid.NewGuid();

                Tes_.DateTime = DateTime.Now;

                Tes_.Description = "{" + Tes.CustomerId + "}{" + Tes.Id + "}{" + Tes.Type + "}";

                Tes_.Id = ClassSync.TES.maxId(1) + 1;


                Tes_.Livraison = Tes.Livraison;

                Tes_.Montant = decimal.Parse(x.Element("check").Attribute("sum").Value.Replace('.', ','));

                Tes_.Nclient = pro.Nclient.ToString();

                Tes_.Payement = Tes.Payement;

                Tes_.Type = 1;

                Tes_.v_Adresse = Tes.v_Adresse;

                Tes_.v_CodeNAF = Tes.v_CodeNAF;

                Tes_.v_CP = Tes.v_CP;

                Tes_.v_Fax = Tes.v_Fax;

                Tes_.v_FRTVA = Tes.v_FRTVA;

                Tes_.v_Mail = Tes.v_Mail;

                Tes_.v_NameCompany = Tes.v_NameCompany;

                Tes_.v_Phone = Tes.v_Phone;

                Tes_.v_SIRET = Tes.v_SIRET;

                Tes_.v_Ville = Tes.v_Ville;

                Tes.Description = "{" + Tes_.CustomerId + "}{" + Tes_.Id + "}{" + Tes_.Type + "}";

                foreach (var t in Tes.TESreglement)
                {
                    if (t.Montant > 0)
                    {
                        t.TESCustomerId = Tes_.CustomerId;

                        Tes_.TESreglement.Add(t);
                    }
                }

                Tes.TESreglement.Clear();
                //   foreach (var t in Tes.TESproducts)
                //     Tes_.TESproducts.Add(t);

                c_tes_ = ClassSync.TES.ins(Tes_);

            }

            c_tes = ClassSync.TES.ins(Tes);

            c_sr = ClassSync.ProductDB.StockReal.response(cmdSr);

            nfact = Tes.Id ?? -1;


            fileMove();

        }

        public static void fileMove()
        {

            DateTime dt = DateTime.Now;

            string bc = ClassGlobalVar.numberTicket + dt.ToString("HHmmss") + String.Format("{0:0000}", ClassProMode.devis ? ClassProMode.ndevis : ClassProMode.nfact);

            if (ClassCheck.b.Element("check").Attribute("barcodeCheck") != null)
            {
                ClassCheck.b.Element("check").Attribute("barcodeCheck").SetValue(bc);
            }
            else
            {
                ClassCheck.b.Element("check").Add(new XAttribute("barcodeCheck", bc));
            }


            string dir = AppDomain.CurrentDomain.BaseDirectory + (devis ? @"\Data\ProDevis" : @"\Data\ProFacture")
                            + DateTime.Now.Year + @"\" + DateTime.Now.Month;


            Directory.CreateDirectory(dir);

            string file = dir + @"\" + DateTime.Now.Day + "_"
                    + DateTime.Now.Hour
                    + "_" + DateTime.Now.Minute
                    + "_" + DateTime.Now.Second + "_" + (pro != null ? pro.NameCompany : "unkwn") + ".xml";

            string path = AppDomain.CurrentDomain.BaseDirectory + @"Data\b.xml";

            File.Move(path, file);

        }

        public static void xmlToDbSaveD(XDocument x)
        {
            ClassSync.DevisId di = new ClassSync.DevisId();

            di.Close = false;

            di.Date = DateTime.Now;

            di.Id = -1;

            di.infoClientsCustomerId = modePro ? ClassSync.classPro.getcustumerId(pro) : new Guid("87da7ba2-5e52-4d31-8def-5a2dad508e94");

            di.Total = 0;




            IEnumerable<XElement> products = x.Element("check").Elements("product");

            List<XElement> p = new List<XElement>();

            foreach (var product in products)
            {
                var pf = p.Find(l => (l.Element("CustumerId").Value == product.Element("CustumerId").Value));


                if (pf == null || (pf.Element("price").Value != product.Element("price").Value))
                    p.Add(product);

                else
                {
                    decimal qty = decimal.Parse(pf.Element("qty").Value.Replace('.', ','));
                    decimal qty_ = decimal.Parse(product.Element("qty").Value.Replace('.', ','));
                    decimal total = decimal.Parse(pf.Element("total").Value.Replace('.', ','));
                    decimal total_ = decimal.Parse(product.Element("total").Value.Replace('.', ','));
                    pf.Element("qty").SetValue(qty + qty_);
                    pf.Element("total").SetValue(total + total_);

                }
            }

            x.Element("check").Elements("product").Remove();


            di.divisWeb = new List<ClassSync.DevisWeb>();

            string cmd_ = "";

            foreach (var product in p)
            {
                x.Element("check").Add(product);

                ClassSync.DevisWeb dw = new ClassSync.DevisWeb();

                dw.CustomerId = Guid.NewGuid();

                dw.IdDevis = -1;

                dw.InfoClients_custumerId = di.infoClientsCustomerId ?? Guid.Empty;

                decimal prixHT = Math.Round(decimal.Parse(product.Element("price").Value.Replace('.', ',')), 2);

                decimal TVA = ClassTVA.getTVA(int.Parse(product.Element("tva").Value));

                decimal tva = PrixTTCOnlyTVA(decimal.Parse(product.Element("price").Value.Replace('.', ',')), TVA);

                dw.QTY = decimal.Parse(product.Element("qty").Value.Replace('.', ','));

                dw.PrixHT = prixHT - tva;

                dw.TotalHT = dw.QTY * dw.PrixHT;

                dw.Operator = false;

                dw.PayementType = 0;

                dw.ProductsCustumerId = Guid.Parse(product.Element("CustumerId").Value);

                dw.MonPrixHT = decimal.Parse( product.Element("contenance").Value.Replace('.',',')) ;

                dw.MonPrixHT = dw.MonPrixHT == 0 ? 1 : dw.MonPrixHT;

                Guid cusumerIdRealStock = Guid.Parse(product.Element("cusumerIdRealStock").Value.Replace('.', ','));

                cmd_ += ClassSync.ProductDB.StockReal.query_add_qty(-dw.QTY, cusumerIdRealStock) +
                                ClassSync.ProductDB.StockReal.query_add_qty_from_est(dw.QTY, ClassGlobalVar.IdEstablishment_GROS, dw.ProductsCustumerId);

                di.divisWeb.Add(dw);
            }

            di.Total = di.divisWeb.Sum(l => l.TotalHT);


            ClassSync.InsDevis(di);

            ClassSync.ProductDB.StockReal.response(cmd_);

            fileMove();

        }

        private static System.ComponentModel.BackgroundWorker worker;

        private static void worker_DoWorkFact(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ClassSync.sync = true;

            xmlToDbSave(ClassCheck.b);



        }

        private static void worker_DoWorkDevis(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            ClassSync.sync = true;

            xmlToDbSaveD(ClassCheck.b);

            
        }

        private static void worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ClassSync.sync = false;

            new ClassPrintCheck(ClassCheck.b, false);

            ClassCheck.b = null;

            ClassCheck.openProductsCheck();

            pro = null;

            ndevis = -1;

            nfact = -1;

            ClassProMode.check = null;

            ClassProMode.devis = false;

            ClassProMode.modePro = false;

            ClassDiscounts.restoreDiscount();

            var de = ClassETC_fun.findWindow("_W_Message");
             if (de!=null)
                de.Close();

            var mw = ClassETC_fun.findWindow("MainWindow_");

            if (mw != null)

                mw.IsEnabled = true;
        }


      

        public static void move(bool devis)
        {

            new ClassFunctuon().showMessage(null," please  waite....");

            var mw = ClassETC_fun.findWindow("MainWindow_");

            if (mw != null)
            {
                mw.IsEnabled = false;
                
            }
            worker = new System.ComponentModel.BackgroundWorker();

            if (devis)
            {
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWorkDevis);
                worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            }
            else
            {
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWorkFact);
                worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            }

            worker.RunWorkerAsync();

        }

    }
}
