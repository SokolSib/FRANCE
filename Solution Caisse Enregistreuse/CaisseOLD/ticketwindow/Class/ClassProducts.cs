using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ticketwindow.Winows.Product.FindProduct;

namespace ticketwindow.Class
{
    public class ClassProducts
    {
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Product.xml";

        public class product
        {
            public int ii { get; set; }
            public Guid CustumerId { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
            public int tva { get; set; }
            public decimal price { get; set; }
            public decimal priceGros { get; set; }
            public string CodeBare { get; set; }
            public decimal qty { get; set; }
            public bool balance { get; set; }
            public decimal contenance { get; set; }
            public int uniteContenance { get; set; }
            public int tare { get; set; }
            public int grp { get; set; }
            public int sgrp { get; set; }
            public System.Guid ProductsWeb_CustomerId { get; set; }
            public Nullable<DateTime> date { get; set; }
            public Guid cusumerIdRealStock { get; set; }
            public int cusumerIdSubGroup { get; set; }
            public decimal total { get; set; }
        }

        public static List<product> listProducts = new List<product>();

        public static XDocument x;
        public ClassProducts()
        {

        }
        public static void loadFronFile()
        {

            x = XDocument.Load(path);

            List<XElement> elmts = x.Element("Product").Elements("rec").ToList();


            for (int i = 0; i < elmts.Count; i++)
            {

                product p = new product();
                if (elmts[i] != null)
                {
                    XElement elm = elmts[i];

                    p.CustumerId = Guid.Parse(elm.Element("CustumerId").Value);
                    p.Name = elm.Element("Name").Value.ToString().ToUpper();
                    p.Desc = elm.Element("Desc").Value.ToString().ToUpper();
                    p.tva = int.Parse(elm.Element("tva").Value);
                    p.price = decimal.Parse(elm.Element("price").Value.Replace('.', ','));
                    p.priceGros = decimal.Parse(elm.Element("priceGros").Value.Replace('.', ','));
                    p.CodeBare = elm.Element("CodeBare").Value;
                    p.qty = decimal.Parse(elm.Element("qty").Value.Replace('.', ','));
                    p.balance = bool.Parse(elm.Element("balance").Value);
                    p.contenance = decimal.Parse(elm.Element("contenance").Value.Replace('.', ','));
                    p.uniteContenance = int.Parse(elm.Element("uniteContenance").Value);
                    p.tare = int.Parse(elm.Element("tare").Value);
                    p.cusumerIdRealStock = Guid.Parse(elm.Element("cusumerIdRealStock").Value);
                    p.grp = int.Parse(elm.Element("grp").Value);
                    p.sgrp = int.Parse(elm.Element("sgrp").Value);
                    p.cusumerIdSubGroup = int.Parse(elm.Element("cusumerIdSubGroup").Value);
                }
                listProducts.Add(p);
             //   prod.Add(p.Name);
            }

        }
        public static List<string> prod = new List<string>();
        public static List<product> getProduct(string name)
        {
            List<product> res = new List<product>();

            if (name.TrimEnd() != "")
            {
                List<XElement> g = x.Element("Product").Elements("rec").ToList().FindAll(l => l.Element("Name").Value == name);

                foreach (XElement elm in g)
                {

                    res.Add(transform(elm));
                }
            }

            return res;
        }
        public static void save()
        {

            foreach (var e in ClassSync.ProductDB.Products.L)
            {
                var x = DbToVar(e);
                if (x!=null)
                listProducts.Add(x);
            }

            listProducts = listProducts.OrderBy(l => l.qty).ToList();

            if (x == null)
            {
                x = new XDocument();

                x.Add(new XElement("Product"));
            }
            else
            {
                x.Element("Product").RemoveAll();
            }

            foreach (product p in listProducts)
            {
                prod.Add(p.Name);
                p.ii = x.Element("Product").Elements("rec").Count();
                x.Element("Product").Add(
                    new XElement("rec",
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
                        )
                    );

            }
            
            x.Save(path);

        }
        public static void add(List<product> pro)
        {

            foreach (product p in pro)
            {
                p.ProductsWeb_CustomerId = Guid.NewGuid();
                p.CustumerId = Guid.NewGuid();
                p.cusumerIdRealStock = Guid.NewGuid();
                p.ii = x.Element("Product").Elements("rec").Count();
                x.Element("Product").Add(
                    new XElement("rec",
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
                        )
                    );
                new ClassSync.ProductDB().ins(p);

                prod.Add(p.Name.ToUpper());

                int indx = listProducts.FindIndex(l => l.CustumerId == p.CustumerId);

                if (indx == -1)
                {
                    listProducts.Add(p);
                }
            }
            x.Save(path);

        }
        public static product DbToVar(ClassSync.ProductDB.Products e)
        {
            try
            {
                product p = new product();

                p.CustumerId = e.CustumerId;

                p.Name = e.Name.ToUpper();

                p.Desc = e.Desc;

                p.tva = int.Parse(e.TVA.Id);

                ClassSync.ProductDB.StockReal sr = null;
                ClassSync.ProductDB.StockReal sr2 = null;
                if (e.StockReal_ != null)
                {
                    sr = e.StockReal_.First(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment);
                    sr2 = e.StockReal_.First(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment_GROS);

                }

                if (sr == null)
                {
                    sr = ClassSync.ProductDB.StockReal.addIsNull(p.CustumerId, ClassGlobalVar.IdEstablishment);

                    sr2 = ClassSync.ProductDB.StockReal.addIsNull(p.CustumerId, ClassGlobalVar.IdEstablishment_GROS);

                }

                p.price = sr.Price;

                p.priceGros = sr2.Price;

                p.qty = sr.QTY;

                p.cusumerIdRealStock = sr.CustomerId;

                p.uniteContenance = e.UniteContenance;

                p.contenance = e.Contenance;

                p.tare = e.Tare;

                p.date = e.Date;

                p.CodeBare = e.CodeBare;

                p.balance = e.Balance;

                p.cusumerIdSubGroup = e.SubGrpProduct.Id;

                ClassGroupProduct.grp_subGrp gs = ClassGroupProduct.getGrpId(p.cusumerIdSubGroup);

                p.grp = gs.grp;

                p.sgrp = gs.subGrp;

                p.ProductsWeb_CustomerId = e.ProductsWeb_CustomerId;

                return p;
            }

            catch (Exception ex)
            {
                new ClassLog("error 2012" + ex.Message + " " + e.Name + " " + e.CodeBare);

                return null;
            }
        }
        public static void modifAddOnlyFile(product p)
        {
            XElement elm = x.Element("Product").Elements("rec").
               Where(l => Guid.Parse(l.Element("CustumerId").Value) == p.CustumerId).FirstOrDefault();

            if (elm != null)
            {
                string o_name = elm.Element("Name").Value;
                int indx = prod.FindIndex(l => l == o_name);
                if (indx > 0) prod[indx] = p.Name;
                elm.Element("Name").SetValue(p.Name);
                elm.Element("Desc").SetValue(p.Desc);
                elm.Element("price").SetValue(p.price);
                elm.Element("priceGros").SetValue(p.priceGros);
                elm.Element("tva").SetValue(p.tva);
                elm.Element("qty").SetValue(p.qty);
                elm.Element("CodeBare").SetValue(p.CodeBare);
                elm.Element("balance").SetValue(p.balance);
                elm.Element("contenance").SetValue(p.contenance);
                elm.Element("uniteContenance").SetValue(p.uniteContenance);
                elm.Element("tare").SetValue(p.tare);
                elm.Element("cusumerIdRealStock").SetValue(p.cusumerIdRealStock);
                elm.Element("date").SetValue(DateTime.Now);
                elm.Element("cusumerIdSubGroup").SetValue(p.cusumerIdSubGroup);
                elm.Element("ProductsWeb_CustomerId").SetValue(p.ProductsWeb_CustomerId);
                elm.Element("grp").SetValue(p.grp);
                elm.Element("sgrp").SetValue(p.sgrp);
            }

            else
            {
                p.ProductsWeb_CustomerId = p.ProductsWeb_CustomerId;
                p.CustumerId = p.CustumerId;
                p.cusumerIdRealStock = p.cusumerIdRealStock;
                x.Element("Product").Add(
                    new XElement("rec",
                        new XElement("CustumerId", p.CustumerId),
                         new XElement("Name", p.Name),
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
                        )
                    );
                prod.Add(p.Name);

                int indx = listProducts.FindIndex(l => l.CustumerId == p.CustumerId);

                if (indx == -1)
                {
                    listProducts.Add(p);
                }
            }
            x.Save(path);
        }
        public static void modif(product p)
        {
            XElement elm = x.Element("Product").Elements("rec").
                Where(l => Guid.Parse(l.Element("CustumerId").Value) == p.CustumerId).Single();
            string o_name = elm.Element("Name").Value;
            int indx = prod.FindIndex(l => l == o_name);
            if (indx > 0) prod[indx] = p.Name;
            elm.Element("Name").SetValue(p.Name);
            elm.Element("Desc").SetValue(p.Desc);
            elm.Element("price").SetValue(p.price);
            elm.Element("priceGros").SetValue(p.priceGros);
            elm.Element("tva").SetValue(p.tva);
            elm.Element("qty").SetValue(p.qty);
            elm.Element("CodeBare").SetValue(p.CodeBare);
            elm.Element("balance").SetValue(p.balance);
            elm.Element("contenance").SetValue(p.contenance);
            elm.Element("uniteContenance").SetValue(p.uniteContenance);
            elm.Element("tare").SetValue(p.tare);
            elm.Element("cusumerIdRealStock").SetValue(p.cusumerIdRealStock);
            elm.Element("date").SetValue(DateTime.Now);
            elm.Element("cusumerIdSubGroup").SetValue(p.cusumerIdSubGroup);
            elm.Element("ProductsWeb_CustomerId").SetValue(p.ProductsWeb_CustomerId);
            elm.Element("grp").SetValue(p.grp);
            elm.Element("sgrp").SetValue(p.sgrp);
            new ClassSync.ProductDB().mod(p);
            x.Save(path);

        }
        public static void updQTYProduct(decimal qty, Guid CustumerId)
        {
            XElement elm = x.Element("Product").Elements("rec").
                  Where(l => Guid.Parse(l.Element("CustumerId").Value) == CustumerId).Single();
            elm.Element("qty").SetValue(qty);
            x.Save(path);
        }
        public static void remove(product p)
        {
            XElement elm = x.Element("Product").Elements("rec").
                Where(l => Guid.Parse(l.Element("CustumerId").Value) == p.CustumerId).First();
            elm.Remove();
            string o_name = elm.Element("Name").Value;
            int indx = prod.FindIndex(l => l == o_name);
            if (indx > 0) prod.RemoveAt(indx);
            new ClassSync.ProductDB().del(p);
            x.Save(path);
        }
        public static XElement findCodeBar(string codebar)
        {
            XElement res = x.Element("Product").Elements("rec").FirstOrDefault(l => l.Element("CodeBare").Value.IndexOf("[" + codebar + "]") != -1);

            return res;
        }        
        public static XElement findName(string Name)
        {
            if (Name.TrimEnd().Length > 3)
                return x.Element("Product").Elements("rec").Where(l => l.Element("Name").Value.ToUpper().TrimEnd().TrimStart().Trim() == Name.TrimEnd().TrimStart().Trim().ToUpper()).FirstOrDefault();
            else return null;
        }
        public static XElement findCustomerId(Guid g)
        {
            try
            {
                return x.Element("Product").Elements("rec").First(l => Guid.Parse(l.Element("CustumerId").Value) == g);
            }
            catch
            {
                return null;
            }
        }
        public static product transform(XElement elm)
        {
            product p = new product();
            if (elm != null)
            {
                try

                {
                    p.ii = int.Parse(elm.Element("ii").Value);
                }

                catch
                
                {
                    p.ii = 0;

                    new ClassLog(" public static product transform(XElement elm) исправить на будущее");
                }

                p.CustumerId = Guid.Parse(elm.Element("CustumerId").Value);
                p.Name = elm.Element("Name").Value;
                p.Desc = elm.Element("Desc").Value;
                p.tva = int.Parse(elm.Element("tva").Value);
                p.price = decimal.Parse(elm.Element("price").Value.Replace('.', ','));
                p.priceGros = decimal.Parse(elm.Element("priceGros").Value.Replace('.', ','));
                p.CodeBare = elm.Element("CodeBare").Value;
                p.date = DateTime.Parse(elm.Element("date").Value);
                p.qty = decimal.Parse(elm.Element("qty").Value.Replace('.', ','));
                p.balance = bool.Parse(elm.Element("balance").Value);
                p.contenance = decimal.Parse(elm.Element("contenance").Value.Replace('.', ','));
                p.uniteContenance = int.Parse(elm.Element("uniteContenance").Value);
                p.tare = int.Parse(elm.Element("tare").Value);
                p.cusumerIdRealStock = Guid.Parse(elm.Element("cusumerIdRealStock").Value);
                p.cusumerIdSubGroup = int.Parse(elm.Element("cusumerIdSubGroup").Value);
                p.ProductsWeb_CustomerId = Guid.Parse(elm.Element("ProductsWeb_CustomerId").Value);
                p.grp = int.Parse(elm.Element("grp").Value);
                p.sgrp = int.Parse(elm.Element("sgrp").Value);
                p.total = elm.Element("toatl") == null ? 0.0m : decimal.Parse(elm.Element("total").Value); 
            }
            return p;
        }
        public static IEnumerable<XElement> filtrCodeBare(string code_bare, IEnumerable<XElement> x)
        {
            string com = code_bare.Trim().TrimEnd().TrimStart();

            return x.Where(l => l.Element("CodeBare").Value.Length > com.Length ?
                l.Element("CodeBare").Value.TrimEnd().TrimStart().Trim().Substring(0, com.Length) == com : false);
        }
        public static IEnumerable<XElement> filtrName(string Name, IEnumerable<XElement> x)
        {
            string name = Name.ToUpper();

            return x.Where(l => l.Element("Name").Value.ToUpper().IndexOf (name) != -1 );
        }
        public static IEnumerable<XElement> filtrCodeBare(product a, product b)
        {
            return x.Elements("rec").Where(l => l.Element("CodeBare").Value.TrimEnd().TrimStart().Trim() == a.CodeBare);
        }
        public static IEnumerable<XElement> filtrPrice(decimal price_a, decimal price_b, IEnumerable<XElement> x)
        {
            return x.Where(l => ((decimal.Parse(l.Element("price").Value) >= price_a) && ((decimal.Parse(l.Element("price").Value) <= price_b))));
        }
        public static IEnumerable<XElement> filtrTVA(int tva, IEnumerable<XElement> x)
        {
            return x.Where(l => int.Parse(l.Element("tva").Value) == tva);
        }
        public static IEnumerable<XElement> filtrQTY(decimal qty_a, decimal qty_b, IEnumerable<XElement> x)
        {
            return x.Where((l => (decimal.Parse(l.Element("qty").Value) >= qty_a) && (decimal.Parse(l.Element("qty").Value) <= qty_b)));
        }
        public static IEnumerable<XElement> filtrGroup(int group, IEnumerable<XElement> x)
        {
            return x.Where((l => int.Parse(l.Element("grp").Value) == group));
        }
        public static IEnumerable<XElement> filtrSubGroup(int sub_group, IEnumerable<XElement> x)
        {
            return x.Where((l => int.Parse(l.Element("sgrp").Value) == sub_group));
        }
        public static IEnumerable<XElement> filtrBalance(bool ballance, IEnumerable<XElement> x)
        {
            return x.Where((l => bool.Parse(l.Element("balance").Value) == ballance));
        }
        public static IEnumerable<XElement> filtrContenance(decimal contenance_a, decimal contenance_b, IEnumerable<XElement> x)
        {
            return x.Where(l => ((decimal.Parse(l.Element("contenance").Value) >= contenance_a) && ((decimal.Parse(l.Element("contenance").Value) <= contenance_b))));
        }
        public static IEnumerable<XElement> filtrUniteContenance(decimal UniteCcontenance_a, decimal UniteCcontenance_b, IEnumerable<XElement> x)
        {
            return x.Where(l => ((decimal.Parse(l.Element("contenance").Value) >= UniteCcontenance_a) && ((decimal.Parse(l.Element("contenance").Value) <= UniteCcontenance_b))));
        }
        public static IEnumerable<XElement> filtrTare(decimal tare_a, decimal tare_b, IEnumerable<XElement> x)
        {
            return x.Where(l => ((decimal.Parse(l.Element("contenance").Value) >= tare_a) && ((decimal.Parse(l.Element("contenance").Value) <= tare_b))));
        }
        public static IEnumerable<XElement> filtr(W_Find_Product W)
        {
            IEnumerable<XElement> ie = x.Element("Product").Elements("rec");

            if (W.xName.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrName(W.xName.Text, ie);
            }
            if (W.xCodeBar.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrCodeBare(W.xCodeBar.Text, ie);
            }
            if (W.spPrice.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrPrice(decimal.Parse(W.xPricea.Text), decimal.Parse(W.xPriceb.Text), ie);
            }
            if (W.xTVA.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrTVA(int.Parse(W.xTVA.Text), ie);
            }
            if (W.spQTY.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrQTY(int.Parse(W.xQTYa.Text), int.Parse(W.xQTYb.Text), ie);
            }
            if (W.xGroup.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrGroup(int.Parse(W.xGroup.SelectedValue.ToString()), ie);
            }
            if (W.xSub_group.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrSubGroup(int.Parse(W.xSub_group.SelectedValue.ToString()), ie);
            }
            if (!W.xBalance.IsChecked ?? false)
            {
                ie = filtrBalance(true, ie);
            }
            if (W.spContenance.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrContenance(decimal.Parse(W.xContenancea.Text), decimal.Parse(W.xContenanceb.Text), ie);
            }
            if (W.spUnit_contenance.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrUniteContenance(decimal.Parse(W.xUnit_contenancea.Text), decimal.Parse(W.xUnit_contenanceb.Text), ie);
            }
            if (W.spTare.Visibility == System.Windows.Visibility.Visible)
            {
                ie = filtrTare(decimal.Parse(W.xTarea.Text), decimal.Parse(W.xTareb.Text), ie);
            }

            //  ie.Elements("rec").Remove();
            return ie;
        }
    }
}
