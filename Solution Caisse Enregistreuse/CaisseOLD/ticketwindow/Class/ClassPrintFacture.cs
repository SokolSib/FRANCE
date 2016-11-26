using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    public class ClassPrintFacture
    {
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
                        this.HT = decimal.Round(total * (100 / (100 + ClassTVA.getTVA(tva))), 2);
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
        private List<groupProduct> listProduct;
        private List<string> tva;
 /*       List<Pays> attr = new List<Pays>();

        public  ClassPrintFacture(XDocument b, bool Duplicate)
        {


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

            string BCDC = (a0 != null ? a0.Value : null);

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




        } */
    }
}
