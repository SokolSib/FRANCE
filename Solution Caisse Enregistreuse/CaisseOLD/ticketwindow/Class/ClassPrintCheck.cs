using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;

using System.Xml.Linq;

namespace ticketwindow.Class
{
    public class ClassPrintCheck
    {
        private static string pathH = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkHead.txt";
        private static string pathB = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkBody.txt";
        private static string pathF = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkFooter.txt";
        private string head { get; set; }
        private string barcode { get; set; }
        private List<groupProduct> listProduct;
        private List<string> tva;
        List<Pays> attr = new List<Pays>();
        private string totals { get; set; }
        private string sumDiscount_ { get; set; }
        private string footer { get; set; }
        private decimal rendu { get; set; }
        private bool Duplicate { get; set; }
        public class groupProduct
        {
            public groupProduct(string categories, product p)
            {
                this.categories = categories;
                this.products = new List<product>();
                this.products.Add(p);
            }
            public class product
            {
                public product(Guid customerId, string categories, string barcode, string name, decimal qty, decimal total, decimal price, int tva, decimal procentDiscount, decimal sumDiscount)
                {
                    if (customerId != Guid.Empty)
                    {
                        this.customerId = customerId;
                        this.categories = categories;
                        this.name = name;
                        this.qty = qty;
                        this.total = total;
                        this.barcode = barcode;
                        this.price = price;
                        this.tva = tva;


                        this.HT = Math.Round(  (total / qty) * (100/ (100 + ClassTVA.getTVA(tva))) , 2 ) * qty;


                        /*
                                                
                        Вся проблема в том что мы работаем с суммой продуктов, нужно работать с ценой за штуку



                        ЦенаЕд = 
                        ЦенаЕд + НДС =





                        
                        



                        this.HT = (Math.Ceiling(  (total * (100 / (100 + ClassTVA.getTVA(tva)))) * 10)/10) ;

                         this.HT = decimal.Round(total * ClassTVA.getTVA(tva), 2);
                        this.HT = decimal.Round(total * (100 / (100 + ClassTVA.getTVA(tva))),5);


                        */


                        this.TvaTotal = total - HT;
                       // this.TvaTotal = (total / (100 + ClassTVA.getTVA(tva)) * ClassTVA.getTVA(tva));
                       // this.HT = total - this.TvaTotal;
                        this.procentDiscount = procentDiscount;
                        this.sumDiscount = sumDiscount;
                    }
                    else
                    {
                        this.customerId = customerId;
                        this.categories = categories;
                        this.name = name;
                        this.qty = 1;
                        this.total = sumDiscount;
                        this.barcode = "";
                        this.price = 0;
                        this.tva = tva;



                        this.HT = decimal.Round(sumDiscount * (100 / (100 + ClassTVA.getTVA(tva))), 2);




                        this.TvaTotal = sumDiscount - HT;
                        //this.TvaTotal = (sumDiscount / (100 + ClassTVA.getTVA(tva)) * ClassTVA.getTVA(tva));
                        //this.HT = sumDiscount - this.TvaTotal;
                        this.procentDiscount = procentDiscount;
                        this.sumDiscount = sumDiscount;
                    }
                }
                public Guid customerId { get; set; }
                public string categories { get; set; }
                public string name { get; set; }
                public decimal qty { get; set; }
                public decimal total { get; set; }
                public decimal price { get; set; }
                public string barcode { get; set; }
                public int tva { get; set; }
                public decimal TvaTotal { get; set; }
                public decimal HT { get; set; }
                public decimal procentDiscount { get; set; }
                public decimal sumDiscount { get; set; }
            }

            public string categories { get; set; }
            public List<product> products { get; set; }
        }
        public class Pays
        {
            public ClassSync.TypesPayDB type { get; set; }
            public decimal money { get; set; }

            public Pays(ClassSync.TypesPayDB type, decimal money)
            {
                this.type = type;
                this.money = money;
            }
        }
        public class infoOfClient
        {

            public string DCBC { get; set; }
            public string DCBC_BiloPoints { get; set; }
            public string DCBC_DobavilePoints { get; set; }
            public string DCBC_OtnayliP { get; set; }
            public string DCBC_OstalosPoints { get; set; }
            public string DCBC_name { get; set; }

        }
        private infoOfClient c { get; set; }
        public ClassPrintCheck(XDocument b, bool Duplicate)
        {

            this.Duplicate = Duplicate;
            listProduct = new List<groupProduct>();
            tva = new List<string>();
            foreach (var p in ClassSync.TypesPayDB.t)
            {
                XAttribute a = b.Element("check").Attribute(p.NameCourt.TrimEnd().TrimStart());

                if (a != null)
                {
                    string money = a.Value.Replace(".", ",");

                    attr.Add(new Pays(p, decimal.Parse(money)));
                }
            }

            rendu = decimal.Parse(b.Element("check").Attribute("Rendu").Value.Replace(".", ","));

            IEnumerable<XElement> xe = b.Element("check").Elements("product");

            decimal sum = 0.0m;

            decimal sumDiscounts = 0.0m;


            foreach (XElement e in xe)
            {

                decimal Discount = decimal.Parse(e.Element("Discount").Value.Replace(".", ","));
                decimal sumDiscount = -decimal.Parse(e.Element("sumDiscount").Value.Replace(".", ","));
                string codebare = e.Element("CodeBare").Value.TrimEnd().TrimStart().Trim();
                decimal qty = decimal.Parse(e.Element("qty").Value.Replace(".", ","));
                string name = e.Element("Name").Value;
                decimal total = decimal.Parse(e.Element("total").Value.Replace(".", ",")) - sumDiscount;
                decimal price = decimal.Parse(e.Element("price").Value.Replace(".", ","));
                string categories = ClassGroupProduct.getName(int.Parse(e.Element("grp").Value));
                int tva_ = int.Parse(e.Element("tva").Value);
                Guid customerId = Guid.Parse(e.Element("CustumerId").Value);

                groupProduct.product p = new groupProduct.product(customerId, categories, codebare, name, qty, total, price, tva_, Discount, sumDiscount);


                #region DISCOUNT


                if (Discount > 0)
                {
                    string discountcategories = "Remise " + p.procentDiscount + "%";

                    int discountIndx = listProduct.FindIndex(l => l.categories == discountcategories);

                    if (discountIndx == -1)
                    {

                        groupProduct.product pn = new groupProduct.product(Guid.Empty, discountcategories, barcode, name, qty, total, price, tva_, Discount, sumDiscount);


                        listProduct.Add(new groupProduct(discountcategories, pn));

                    }
                    else
                    {
                        //  groupProduct.product pn = new groupProduct.product(Guid.Empty, discountcategories, "", name, 1, sumDiscount, 0, tva_, Discount, sumDiscount);
                        groupProduct.product pn = new groupProduct.product(Guid.Empty, discountcategories, barcode, name, qty, total, price, tva_, Discount, sumDiscount);

                        int repeat = listProduct[discountIndx].products.FindIndex((l => ((l.name == name))));

                        if (repeat != -1)
                        {
                            listProduct[discountIndx].products[repeat].total += pn.sumDiscount;
                            listProduct[discountIndx].products[repeat].price = pn.price;
                            listProduct[discountIndx].products[repeat].TvaTotal += pn.TvaTotal;
                            listProduct[discountIndx].products[repeat].HT += pn.HT;
                            listProduct[discountIndx].products[repeat].sumDiscount += pn.sumDiscount;
                        }
                        else
                            listProduct[discountIndx].products.Add(pn);
                    }

                    sumDiscounts -= sumDiscount;
                }
                #endregion
                int indx = listProduct.FindIndex(l => l.categories == p.categories);

                if (indx != -1)
                {
                    int f = -1;
                    f = listProduct[indx].products.FindIndex(l => ((l.customerId == p.customerId) && l.price == p.price));

                    if (f == -1)
                        listProduct[indx].products.Add(p);
                    else
                    {
                        if (listProduct[indx].products[f].categories != "Remise " + p.procentDiscount + "%")
                        {
                            listProduct[indx].products[f].qty += p.qty;
                            listProduct[indx].products[f].total += p.total;
                            listProduct[indx].products[f].price = p.price;
                            listProduct[indx].products[f].TvaTotal += p.TvaTotal;
                            listProduct[indx].products[f].HT += p.HT;
                            listProduct[indx].products[f].sumDiscount += p.sumDiscount;
                        }
                    }
                }
                else
                    listProduct.Add(new groupProduct(categories, p));


                sum += total;
            }

            listProduct = listProduct.OrderBy(l => l.categories).ToList();

            List<groupProduct> listProductDiscount = listProduct.FindAll(l => l.categories.IndexOf("Discount") != -1);

            listProduct.RemoveAll(l => l.categories.IndexOf("Discount") != -1);


            List<groupProduct> listProductDiscountRemise = listProduct.FindAll(l => l.categories.IndexOf("Remise") != -1);

            listProduct.RemoveAll(l => l.categories.IndexOf("Remise") != -1);



            listProduct.AddRange(listProductDiscount);
            listProduct.AddRange(listProductDiscountRemise);

            head = File.ReadAllText(pathH);

            footer = File.ReadAllText(pathF);

            barcode = b.Element("check").Attribute("barcodeCheck").Value;

            totals = (sum - sumDiscounts).ToString("0.00");

            sumDiscount_ = sumDiscounts.ToString("0.00");

            #region DiscountPoints
            XAttribute a0 = b.Element("check").Attribute("DCBC");
            XAttribute a1 = b.Element("check").Attribute("DCBC_BiloPoints");
            XAttribute a2 = b.Element("check").Attribute("DCBC_DobavilePoints");
            XAttribute a3 = b.Element("check").Attribute("DCBC_OtnayliPoints");
            XAttribute a4 = b.Element("check").Attribute("DCBC_OstalosPoints");
            XAttribute a5 = b.Element("check").Attribute("DCBC_name");

            string BCDC = (a0 != null  ? a0.Value : null);

            if ((BCDC != null) && (BCDC != ""))
            {
                c = new infoOfClient
                {
                    DCBC = a0.Value,
                    DCBC_BiloPoints = a1.Value,
                    DCBC_DobavilePoints = a2.Value,
                    DCBC_OtnayliP = a3.Value,
                    DCBC_OstalosPoints = a4.Value,
                    DCBC_name = a5.Value
                };
            }
            #endregion
           
  
            ClassDotLiquid.prt(barcode, head, listProduct, totals, sumDiscount_, attr, rendu, footer, c, Duplicate); 

           

        }
     
        private class ClassDrawElm
        {
            public const int sizeLine = 20;
            public class elm
            {
                public string text { get; set; }
                public Font font { get; set; }
                public Brush brush { get; set; }
                public Rectangle rectangle { get; set; }
                public StringFormat stringFormat { get; set; }
            }

            public class mTva
            {
                public int id { get; set; }
                public decimal val { get; set; }
                public decimal ht { get; set; }
                public decimal tva { get; set; }
            }
            /*
            public class Paiements
            {
                public string Name { get; set; }

                public string money { get; set; }

            }
            */
            public List<elm> listEl = new List<elm>();
            public List<mTva> ltva = new List<mTva>();
            public List<elm> listTVA = new List<elm>();
            public List<elm> LPay = new List<elm>();

            //       public List<Paiements> listPaiements = new List<Paiements>();
            public void DrawElm(List<ClassPrintCheck.groupProduct> lProduct, int posX, int posY, out int posXo, out int posYo)
            {
                foreach (ClassTVA.tva t in ClassTVA.listTVA)
                {
                    mTva n = new mTva();
                    n.id = t.id;
                    n.val = t.val;
                    n.ht = 0.0m;
                    n.tva = 0.0m;
                    ltva.Add(n);
                }
                foreach (ClassPrintCheck.groupProduct g in lProduct)
                {
                    int sizeLine = 15;

                    elm e = new elm();
                    e.font = new Font("Arial", 8, FontStyle.Bold);
                    e.text = g.categories;
                    e.rectangle = new Rectangle(posX + 10, posY, 270, sizeLine);
                    e.stringFormat = new StringFormat();
                    e.stringFormat.Alignment = StringAlignment.Near;
                    e.stringFormat.LineAlignment = StringAlignment.Far;
                    posY += sizeLine;
                    listEl.Add(e);
                    for (int i = 0; i < g.products.Count; i++)
                    {
                        elm emName = new elm();
                        emName.text = g.products[i].name;
                        if (emName.text.Length > 30)
                            emName.text = emName.text.Substring(0, 30);
                        emName.font = new Font("Arial", 10);
                        emName.rectangle = new Rectangle(posX, posY, 225, sizeLine);
                        emName.stringFormat = new StringFormat();
                        emName.stringFormat.Alignment = StringAlignment.Near;
                        emName.stringFormat.LineAlignment = StringAlignment.Center;
                        listEl.Add(emName);

                        elm emPrice = new elm();
                        emPrice.text = g.products[i].total.ToString("0.00");

                        emPrice.font = new Font("Arial", 10);
                        emPrice.rectangle = new Rectangle(posX + 220, posY, 60, sizeLine);
                        emPrice.stringFormat = new StringFormat();
                        emPrice.stringFormat.Alignment = StringAlignment.Far;
                        emPrice.stringFormat.LineAlignment = StringAlignment.Center;
                        listEl.Add(emPrice);

                        int indx = ltva.FindIndex(l => l.id == g.products[i].tva);

                        if (indx != -1)
                        {
                            ltva[indx].tva += g.products[i].TvaTotal;
                            ltva[indx].ht += g.products[i].HT;
                        }

                        posY += sizeLine;

                        if (g.products[i].qty != 1)
                        {
                            elm emQTY = new elm();

                            int iqty = (int)g.products[i].qty;

                            if (iqty - g.products[i].qty != 0.0M)
                                emQTY.text = g.products[i].qty.ToString("0.000") + "kg x " + g.products[i].price + " €";
                            else
                                emQTY.text = " " + iqty.ToString() + "   x   " + g.products[i].price + " €";


                            emQTY.font = new Font("Arial", 8);

                            emQTY.rectangle = new Rectangle(posX + 10, posY - 3, 250, sizeLine);
                            emQTY.stringFormat = new StringFormat();
                            emQTY.stringFormat.Alignment = StringAlignment.Near;
                            emQTY.stringFormat.LineAlignment = StringAlignment.Near;
                            listEl.Add(emQTY);
                            posY += 10;
                        }

                    }
                }
                posXo = posX;
                posYo = posY;


            }

            public void DrawPaiements(List<ClassPrintCheck.Pays> LPays, int posX, int posY, out int posXo, out int posYo)
            {
                int countPay = 0;
                int sizeLine = 15;

                foreach (var e in LPays)
                {
                    if (e.money > 0)
                    {
                        countPay++;

                        elm eNamePay = new elm();
                        eNamePay.font = new Font("Arial", 9);
                        eNamePay.text = e.type.Name;
                        eNamePay.rectangle = new Rectangle(posX + 20, posY, 150, sizeLine);
                        eNamePay.stringFormat = new StringFormat();
                        eNamePay.stringFormat.Alignment = StringAlignment.Near;
                        eNamePay.stringFormat.LineAlignment = StringAlignment.Center;


                        elm eMoney = new elm();
                        eMoney.font = new Font("Arial", 9);
                        eMoney.text = e.money.ToString("0.00 €");
                        eMoney.rectangle = new Rectangle(posX + 170, posY, 90, sizeLine);
                        eMoney.stringFormat = new StringFormat();
                        eMoney.stringFormat.Alignment = StringAlignment.Far;
                        eMoney.stringFormat.LineAlignment = StringAlignment.Center;

                        LPay.Add(eNamePay);
                        LPay.Add(eMoney);

                        posY += sizeLine;
                    }


                }


                posXo = posX;
                posYo = posY;


            }
            public void DrawTva(int posX, int posY, out int posXo, out int posYo)
            {
                int count = 0;
                int sizeLine = 11;

                foreach (mTva m in ltva)
                {
                    if (m.tva > 0.0m)
                    {
                        count++;

                        elm eNameTva = new elm();
                        eNameTva.font = new Font("Arial", 8);
                        eNameTva.text = "TVA " + m.val + "% :";
                        eNameTva.rectangle = new Rectangle(posX + 20, posY, 60, sizeLine);
                        eNameTva.stringFormat = new StringFormat();
                        eNameTva.stringFormat.Alignment = StringAlignment.Near;
                        eNameTva.stringFormat.LineAlignment = StringAlignment.Center;

                        elm eTva = new elm();
                        eTva.font = new Font("Arial", 8);
                        eTva.text = m.tva.ToString("0.00 €");
                        eTva.rectangle = new Rectangle(posX + 80, posY, 60, sizeLine);
                        eTva.stringFormat = new StringFormat();
                        eTva.stringFormat.Alignment = StringAlignment.Far;
                        eTva.stringFormat.LineAlignment = StringAlignment.Center;

                        elm eHtName = new elm();
                        eHtName.font = new Font("Arial", 8);
                        eHtName.text = "HT : ";
                        eHtName.rectangle = new Rectangle(posX + 150, posY, 60, sizeLine);
                        eHtName.stringFormat = new StringFormat();
                        eHtName.stringFormat.Alignment = StringAlignment.Near;
                        eHtName.stringFormat.LineAlignment = StringAlignment.Center;

                        elm eHt = new elm();
                        eHt.font = new Font("Arial", 8);
                        eHt.text = m.ht.ToString("0.00 €");
                        eHt.rectangle = new Rectangle(posX + 210, posY, 60, sizeLine);
                        eHt.stringFormat = new StringFormat();
                        eHt.stringFormat.Alignment = StringAlignment.Far;
                        eHt.stringFormat.LineAlignment = StringAlignment.Center;

                        listEl.Add(eHtName);
                        listEl.Add(eHt);
                        listEl.Add(eNameTva);
                        listEl.Add(eTva);
                        posY += sizeLine;
                    }
                }

                if (count > 1)
                {

                    elm eNameTvaS = new elm();
                    eNameTvaS.font = new Font("Arial", 8);
                    eNameTvaS.text = "Total TVA :";
                    eNameTvaS.rectangle = new Rectangle(posX + 20, posY, 60, sizeLine);
                    eNameTvaS.stringFormat = new StringFormat();
                    eNameTvaS.stringFormat.Alignment = StringAlignment.Near;
                    eNameTvaS.stringFormat.LineAlignment = StringAlignment.Center;

                    elm eTvaS = new elm();
                    eTvaS.font = new Font("Arial", 8);
                    eTvaS.text = ltva.Sum(l => l.tva).ToString("0.00 €");
                    eTvaS.rectangle = new Rectangle(posX + 80, posY, 60, sizeLine);
                    eTvaS.stringFormat = new StringFormat();
                    eTvaS.stringFormat.Alignment = StringAlignment.Far;
                    eTvaS.stringFormat.LineAlignment = StringAlignment.Center;

                    elm eHtNameS = new elm();
                    eHtNameS.font = new Font("Arial", 8);
                    eHtNameS.text = "Total HT : ";
                    eHtNameS.rectangle = new Rectangle(posX + 150, posY, 60, sizeLine);
                    eHtNameS.stringFormat = new StringFormat();
                    eHtNameS.stringFormat.Alignment = StringAlignment.Near;
                    eHtNameS.stringFormat.LineAlignment = StringAlignment.Center;

                    elm eHtS = new elm();
                    eHtS.font = new Font("Arial", 8);
                    eHtS.text = ltva.Sum(l => l.ht).ToString("0.00 €");
                    eHtS.rectangle = new Rectangle(posX + 210, posY, 60, sizeLine);
                    eHtS.stringFormat = new StringFormat();
                    eHtS.stringFormat.Alignment = StringAlignment.Far;
                    eHtS.stringFormat.LineAlignment = StringAlignment.Center;

                    listEl.Add(eHtNameS);
                    listEl.Add(eHtS);
                    listEl.Add(eNameTvaS);
                    listEl.Add(eTvaS);
                    posY += sizeLine;
                }
                posXo = posX;
                posYo = posY;
            }



        }
        public Graphics printCheck(string barcode, string head, List<ClassPrintCheck.groupProduct> lProduct, string total, string sumDiscount, List<ClassPrintCheck.Pays> LPay, decimal rendu, string footer, infoOfClient InfoOfClient, Graphics gt)
        {
         
            List<Rectangle> rect = new List<Rectangle>();

            int x1 = 0;
            int y1 = 0;
            int W = 280*20;
            int H = 134*20;

            Rectangle FillRectangleImg = new Rectangle(x1, y1, W, H);

            y1 = H;
            H = 15 * head.Split('\n').Length * 20;
            String Head = head;
            Rectangle FillRectangleHead = new Rectangle(x1, y1, W, H);
            Font TextFontHead = new Font("Arial", 10);
            rect.Add(FillRectangleHead);


            String Duplicate = null;
            Rectangle FillRectangleDuplicate = new Rectangle();
            if (this.Duplicate)
            {
                y1 = y1 + H;
                H = 25;
                Duplicate = "*** DUPLICATA ***";
                FillRectangleDuplicate = new Rectangle(x1, y1, W, H);
                rect.Add(FillRectangleDuplicate);
            }
            y1 = y1 + H;
            H = 25;
            String Vente = "*** VENTE ***";
            Rectangle FillRectangleVente = new Rectangle(x1, y1, W, H);
            rect.Add(FillRectangleVente);

            y1 = y1 + H;
            H = 20;
            ClassDrawElm de = new ClassDrawElm();

            de.DrawElm(lProduct, x1, y1, out x1, out y1);


            H = 10;
            string Total__ = "__________________";
            Rectangle FillRectangleTotal__ = new Rectangle(x1 + 135, y1 - 6, 150, H);
            rect.Add(FillRectangleTotal__);

            H = 20;
            String Total = "TOTAL :";
            Rectangle FillRectangleTotal = new Rectangle(x1 + 145, y1 + 1, 70, H);
            String Total_ = total;
            Rectangle FillRectangleTotal_ = new Rectangle(x1 + 210, y1, 70, H);

            y1 = y1 + H;
            W = 280;
            H = 20;

            de.DrawPaiements(LPay, x1, y1 + 5, out x1, out y1);

            W = 280;
            H = 15;
            String Rendu = "Rendu :";
            Rectangle FillRectangleRendu = new Rectangle(x1 + 20, y1, 140, H);
            Font TFontRendu = new Font("Arial", 9);
            rect.Add(FillRectangleRendu);

            String RenduVal = rendu.ToString("0.00 €");
            Rectangle FillRectangleRenduVal = new Rectangle(x1 + 170, y1, 90, H);
            rect.Add(FillRectangleRenduVal);

            y1 = y1 + H;
            H = 25;
            String TitleTVA = "*** TVA ***";
            Rectangle FillRectangleTitleTVA = new Rectangle(x1, y1, W, H);
            rect.Add(FillRectangleTitleTVA);

            y1 = y1 + H;
            de.DrawTva(x1, y1, out x1, out y1);


            String TitleRemise = null;
            String Discount = null;
            String Discount_ = null;
            Rectangle FillRectangleTitleRemise = new Rectangle();
            Rectangle FillRectangleDiscount = new Rectangle();
            Rectangle FillRectangleDiscount_ = new Rectangle();






            if (Decimal.Parse(sumDiscount) > 0)
            {
                y1 = y1 + H;
                W = 280;
                H = 20;
                TitleRemise = "*** REMISE ***";
                FillRectangleTitleRemise = new Rectangle(x1, y1, W, H);
                rect.Add(FillRectangleTitleRemise);

                y1 = y1 + H;
                W = 280;
                H = 20;
                Discount = "Total remise sur ce ticket : ";
                FillRectangleDiscount = new Rectangle(x1, y1, 200, H);
                Discount_ = sumDiscount;
                FillRectangleDiscount_ = new Rectangle(x1 + 200, y1, 80, H);

            }



            #region INFO OF CLIENT
            String DCBC_name = null;
            String DCBC_BiloPoints = null;
            String DCBC_BiloPoints_ = null;
            String DobavilePoints = null;
            String DobavilePoints_ = null;
            String OtnayliP = null;
            String OtnayliP_ = null;
            String OstalosPoints = null;
            String OstalosPoints_ = null;
            String TCarte = null;
            Rectangle FillRectangleDCBC_name = new Rectangle();
            Rectangle FillRectangleDCBC_BiloPoints = new Rectangle();
            Rectangle FillRectangleDCBC_BiloPoints_ = new Rectangle();
            Rectangle FillRectangleDobavilePoints = new Rectangle();
            Rectangle FillRectangleDobavilePoints_ = new Rectangle();
            Rectangle FillRectangleOtnayliP = new Rectangle();
            Rectangle FillRectangleOtnayliP_ = new Rectangle();
            Rectangle FillRectangleOstalosPoints_ = new Rectangle();
            Rectangle FillRectangleOstalosPoints = new Rectangle(); ;
            Rectangle FillRectangleTCarte = new Rectangle();

            if (InfoOfClient != null)
            {
                y1 = y1 + H;
                W = 280;
                H = 20;
                TCarte = "*** CARTE FIDELITE ***";
                FillRectangleTCarte = new Rectangle(x1, y1, W, H);
                rect.Add(FillRectangleTCarte);

                y1 = y1 + H;
                W = 280;
                H = 20;

                DCBC_name = InfoOfClient.DCBC_name + " - " + InfoOfClient.DCBC;
                FillRectangleDCBC_name = new Rectangle(x1, y1, W, H);



                y1 = y1 + H;
                W = 280;
                H = 12;

                DCBC_BiloPoints = "Ancien solde de points : ";
                FillRectangleDCBC_BiloPoints = new Rectangle(x1, y1, 180, H);
                DCBC_BiloPoints_ = InfoOfClient.DCBC_BiloPoints.ToString();
                FillRectangleDCBC_BiloPoints_ = new Rectangle(x1 + 180, y1, 80, H);

                y1 = y1 + H;
                W = 280;
                H = 12;

                DobavilePoints = "Points fidelité acquis : ";
                FillRectangleDobavilePoints = new Rectangle(x1, y1, 180, H);
                DobavilePoints_ = InfoOfClient.DCBC_DobavilePoints.ToString();
                FillRectangleDobavilePoints_ = new Rectangle(x1 + 180, y1, 80, H);

                /*  y1 = y1 + H;
                  W = 280;
                  H = 20;

                    OtnayliP = "Сколько отняли";
                  FillRectangleOtnayliP = new Rectangle(x1, y1, 200, H);
                  OtnayliP_ = InfoOfClient.DCBC_OtnayliP.ToString();
                  FillRectangleOtnayliP_ = new Rectangle(x1 + 200, y1, 80, H);
                  */

                y1 = y1 + H;
                W = 280;
                H = 12;

                OstalosPoints = "Nouveau solde de points : ";
                FillRectangleOstalosPoints = new Rectangle(x1, y1, 180, H);
                OstalosPoints_ = InfoOfClient.DCBC_OstalosPoints.ToString();
                FillRectangleOstalosPoints_ = new Rectangle(x1 + 180, y1, 80, H);
            }



            #endregion




            y1 = y1 + H;
            W = 260;
            H = 40;
            Rectangle FillRectangleBarCodeImg = new Rectangle(x1 + 10, y1 + 5, W, H);

            y1 = y1 + H + 5;
            W = 280;
            H = 30;
            String NumeroTicket = barcode.ToString() + Environment.NewLine + ClassGlobalVar.nameTicket + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            Rectangle FillRectangleNumeroTicket = new Rectangle(x1, y1, W, H);

            y1 = y1 + H;
            W = 280;
            H = 20 * footer.Split('\n').Length; ;
            String Footer = footer;
            Rectangle FillRectangleFooter = new Rectangle(x1, y1, W, H);

            y1 = y1 + 20;

            Font TextFont = new Font("Arial", 9);
            SolidBrush TextBrush = new SolidBrush(Color.Black);

            StringFormat TextFormatL = new StringFormat();
            TextFormatL.Alignment = StringAlignment.Near;
            TextFormatL.LineAlignment = StringAlignment.Center;


            StringFormat TextFormatR = new StringFormat();
            TextFormatR.Alignment = StringAlignment.Far;
            TextFormatR.LineAlignment = StringAlignment.Center;

            StringFormat TextFormatC = new StringFormat();
            TextFormatC.Alignment = StringAlignment.Center;
            TextFormatC.LineAlignment = StringAlignment.Center;


            /*Total VENTE*/
            Font TFontTotal = new Font("Arial", 10, FontStyle.Bold);

            /*Title*/
            Font TFontTitle = new Font("Arial", 12, FontStyle.Bold);
            StringFormat TFormatTitle = new StringFormat();
            TFormatTitle.Alignment = StringAlignment.Center;
            TFormatTitle.LineAlignment = StringAlignment.Center;


            /* Arial , 8 , Bold*/
            Font TFontArialBold = new Font("Arial", 8, FontStyle.Bold);





            StringFormat TextFormat___ = new StringFormat();
            TextFormat___.Alignment = StringAlignment.Far;
            TextFormat___.LineAlignment = StringAlignment.Far;
           
            int nWidth = W, nHeight = y1;

            Bitmap b1 = new Bitmap(nWidth, nHeight);
            
            using (Graphics g = Graphics.FromImage(b1))
            {

                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                foreach (ClassDrawElm.elm e in de.listEl)
                {
                    g.DrawString(e.text, e.font, TextBrush, e.rectangle, e.stringFormat);

                    rect.Add(e.rectangle);
                }

                g.DrawString(TitleTVA, TFontTitle, TextBrush, FillRectangleTitleTVA, TFormatTitle);

                foreach (ClassDrawElm.elm e in de.listTVA)
                {
                    g.DrawString(e.text, e.font, TextBrush, e.rectangle, e.stringFormat);

                    rect.Add(e.rectangle);
                }

                foreach (ClassDrawElm.elm e in de.LPay)
                {
                    g.DrawString(e.text, e.font, TextBrush, e.rectangle, e.stringFormat);

                    rect.Add(e.rectangle);
                }

                // Pen pen = new Pen(TextBrush, 1.0F);

                // g.DrawRectangles(pen, rect.ToArray());

                //     

                g.DrawImage((Image.FromFile(System.AppDomain.CurrentDomain.BaseDirectory + "\\images\\anahit_9.jpg")), FillRectangleImg);
                g.DrawString(head, TFontRendu, TextBrush, FillRectangleHead, TextFormatC);




                if (Duplicate != null)
                    g.DrawString(Duplicate, TFontTitle, TextBrush, FillRectangleDuplicate, TFormatTitle);

                g.DrawString(Vente, TFontTitle, TextBrush, FillRectangleVente, TFormatTitle);



                g.DrawString(Total__, TFontTotal, TextBrush, FillRectangleTotal__, TextFormat___);

                g.DrawString(Total, TFontTotal, TextBrush, FillRectangleTotal, TextFormatL);

                g.DrawString(Total_, TFontTotal, TextBrush, FillRectangleTotal_, TextFormatR);

                if (rendu != 0.0m)
                {
                    g.DrawString(Rendu, TFontRendu, TextBrush, FillRectangleRendu, TextFormatL);
                    g.DrawString(RenduVal, TFontRendu, TextBrush, FillRectangleRenduVal, TextFormatR);

                }

                if (Decimal.Parse(sumDiscount) > 0)
                {
                    g.DrawString(TitleRemise, TFontTitle, TextBrush, FillRectangleTitleRemise, TFormatTitle);

                    g.DrawString(Discount, TFontArialBold, TextBrush, FillRectangleDiscount, TextFormatL);

                    g.DrawString(Discount_, TFontArialBold, TextBrush, FillRectangleDiscount_, TextFormatR);
                }

                if (InfoOfClient != null)
                {
                    g.DrawString(TCarte, TFontTitle, TextBrush, FillRectangleTCarte, TFormatTitle);

                    g.DrawString(DCBC_name, TFontRendu, TextBrush, FillRectangleDCBC_name, TextFormatL);

                    g.DrawString(DCBC_BiloPoints, TFontRendu, TextBrush, FillRectangleDCBC_BiloPoints, TextFormatL);

                    g.DrawString(DobavilePoints, TFontRendu, TextBrush, FillRectangleDobavilePoints, TextFormatL);

                    g.DrawString(OtnayliP, TFontRendu, TextBrush, FillRectangleOtnayliP, TextFormatL);

                    g.DrawString(OstalosPoints, TFontRendu, TextBrush, FillRectangleOstalosPoints, TextFormatL);


                    g.DrawString(DCBC_BiloPoints_, TFontRendu, TextBrush, FillRectangleDCBC_BiloPoints_, TextFormatL);

                    g.DrawString(DobavilePoints_, TFontRendu, TextBrush, FillRectangleDobavilePoints_, TextFormatL);

                    g.DrawString(OtnayliP_, TFontRendu, TextBrush, FillRectangleOtnayliP_, TextFormatL);

                    g.DrawString(OstalosPoints_, TFontRendu, TextBrush, FillRectangleOstalosPoints_, TextFormatL);

                }





                g.DrawImage(new ClassImageBarCode().get_bc(barcode), FillRectangleBarCodeImg);
                g.DrawString(NumeroTicket, TFontRendu, TextBrush, FillRectangleNumeroTicket, TextFormatC);
                g.DrawString(Footer, TextFont, TextBrush, FillRectangleFooter, TextFormatL);



            }

            b1.Save("D:\\test.bmp");

            printimage( CreateBitmapSourceFromBitmap( b1));

            return gt;
        }
        public static System.Windows.Media.Imaging.BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }

        private void printPage(object o, PrintPageEventArgs e)
        {


           
         /*  printCheck(barcode, head, listProduct, totals, sumDiscount_, attr, rendu, footer,
                c,
                null);
            */

        }
      
        private void printimage(System.Windows.Media.ImageSource bi)
        {

            var vis = new System.Windows.Media.DrawingVisual();
            System.Windows.Media.DrawingContext dc = vis.RenderOpen();
            dc.DrawImage(bi, new System.Windows.Rect { Width = bi.Width, Height = bi.Height });
            dc.Close();

            var pdialog = new System.Windows.Controls.PrintDialog();
            //  if (pdialog.ShowDialog() == true)
            {
                pdialog.PrintVisual(vis, "My Image");
            }
        }
    }
}
