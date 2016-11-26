using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Devis.Class
{
    class ClassProducts
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

            public System.Guid CustomerId { get; set; }
            public bool Visible { get; set; }
            public string Images { get; set; }
            public string DescrCourt { get; set; }
            public string DescrLong { get; set; }
            public Nullable<decimal> Volume { get; set; }
            public Nullable<int> UniteVolume { get; set; }
            public Nullable<decimal> ContenanceBox { get; set; }
            public Nullable<decimal> ContenancePallet { get; set; }
            public Nullable<decimal> Weight { get; set; }
            public Nullable<bool> Frozen { get; set; }
            public virtual List<ClassSync.ProductDB.ProductsBC_> bc { get; set; }
        }

        public static List<XElement> shelfProducts = new List<XElement>();



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
                if (elmts[i] != null)
                {
                    decimal q = 0.0m; 
                        
                    decimal.TryParse(elmts[i].Element("QTY").Value.Replace('.', ','), out q);

                    if (q > 0)
                    {
                        shelfProducts.Add(elmts[i]);
                    }
                }
               
            }
            

        }

        public static void saveLocal()
        {
            x.Save(path);
        }
        public static void save()
        {
            List<product> listProducts = new List<product>();

            foreach (var e in ClassSync.ProductDB.Products.L)
            {

                listProducts.Add(DbToVar(e));
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
                XElement elmbc = new XElement("BC");

                foreach (var v in p.bc)
                {
                    elmbc.Add(
                //        new XElement("CustomerId", v.CustomerId),
                        new XElement("CustomerIdProduct", v.CustomerIdProduct),
                        new XElement("CodeBar", v.CodeBar),
                        new XElement("QTY", v.QTY)
                //        new XElement("Description", v.Description)

                        );
                }


                x.Element("Product").Add(
                    new XElement("rec",
                   //     new XElement("ii", p.ii),
                        new XElement("CustumerId", p.CustumerId),
                        new XElement("Name", p.Name.ToUpper()),
                   //     new XElement("Desc", p.Desc),
                        new XElement("price", p.price),
             //           new XElement("tva", p.tva),
             //           new XElement("qty", p.qty),
                        new XElement("CodeBare", p.CodeBare),
             //           new XElement("balance", p.balance),
             /*           new XElement("contenance", p.contenance),
                        new XElement("uniteContenance", p.uniteContenance),
                        new XElement("tare", p.tare),
                        new XElement("date", p.date ?? DateTime.Now),
                        new XElement("cusumerIdRealStock", p.cusumerIdRealStock),
                        new XElement("cusumerIdSubGroup", p.cusumerIdSubGroup),
                        new XElement("ProductsWeb_CustomerId", p.ProductsWeb_CustomerId),
                        new XElement("grp", ClassGroupProduct.getName(p.grp)),
                        new XElement("sgrp", ClassGroupProduct.getSubName(p.sgrp)),
                        new XElement("Visible", p.Visible),
                */
                        new XElement("Images", p.Images),
              //          new XElement("DescrCourt", p.DescrCourt),
              //          new XElement("DescrLong", p.DescrLong),
               //         new XElement("Volume", p.Volume),
                //        new XElement("UniteVolume", p.UniteVolume),
                        new XElement("ContenanceBox", p.ContenanceBox),
              //          new XElement("Weight", p.Weight),
              //          new XElement("Frozen", p.Frozen),
                        elmbc,
                        new XElement("QTY", null),
                        new XElement("QTY_box", null),
                        new XElement("TOTAL", null)
                        )
                    );

            }

            x.Save(path);

        }

        public static  void clrElm (XElement[] elms)
        {
            foreach (var elm in elms)
            {
                elm.Element("QTY").SetValue("");
                elm.Element("QTY_box").SetValue("");
                elm.Element("TOTAL").SetValue("");
                shelfProducts.Remove(elm);
            }

            x.Save(path);

            
        }

        public static void updQTYProduct(decimal c, XElement e, bool box, int typefind)
        {

            //  XElement e = x.Element("Product").Elements("rec").FirstOrDefault(l => l.Element("CustumerId").Value == CustumerId.ToString());

            if (c >= 0)
            {

                decimal cb = decimal.Parse(e.Element("ContenanceBox").Value.Replace('.', ','));


                decimal pr = decimal.Parse(e.Element("price").Value.Replace('.', ','));

                decimal qty = decimal.Parse(e.Element("QTY").Value.Replace('.', ',').Trim() == "" ? "0" : e.Element("QTY").Value.Replace('.', ',').Trim());

                decimal qty_box = decimal.Parse(e.Element("QTY_box").Value.Replace('.', ',').Trim() == "" ? "0" : e.Element("QTY_box").Value.Replace('.', ',').Trim());

                if (typefind == 1 || typefind == 2)
                {
                    box = false;
                }

                if (box)
                {
                    

                    qty += c * (cb == 0.0m ? 1 : cb);

                    qty_box += c;
                }
                else
                {
                    qty += c;

                    qty_box = qty / (cb == 0.0m ? 1 : cb);
                }
                decimal total = qty * pr;

                e.Element("QTY").SetValue(qty);

                e.Element("QTY_box").SetValue(qty_box);

                e.Element("TOTAL").SetValue(total);
            }
            else
            {
                e.Element("QTY").SetValue("");

                e.Element("QTY_box").SetValue("");

                e.Element("TOTAL").SetValue("");
            }

            x.Save(path);
        }
        public static product DbToVar(ClassSync.ProductDB.Products e)
        {

            product p = new product();

            //  p.ii = listProducts.Count() + 1;

            p.Images = new ClassF().getPathImg(e.CustumerId)[0];

            p.CustumerId = e.CustumerId;

            p.Name = e.Name;

            p.Desc = e.Desc;

            p.tva = int.Parse(e.TVA.Id);

            ClassSync.ProductDB.StockReal sr = null;
            if (e.StockReal_ != null)
                sr = e.StockReal_.First(l => l.IdEstablishment == ClassGlobalVar.IdEstablishment);

            if (sr == null)
            {
                sr = ClassSync.ProductDB.StockReal.addIsNull(p.CustumerId);
            }

            p.price = sr.Price;

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


            p.Visible = e.ProductsWeb_.Visible;

            p.DescrCourt = e.ProductsWeb_.DescrCourt;
            p.DescrLong = e.ProductsWeb_.DescrLong;
            p.Volume = e.ProductsWeb_.Volume;
            p.UniteVolume = e.ProductsWeb_.UniteVolume;
            p.ContenanceBox = e.ProductsWeb_.ContenanceBox;
            p.ContenancePallet = e.ProductsWeb_.ContenancePallet;
            p.Weight = e.ProductsWeb_.Weight;
            p.Frozen = e.ProductsWeb_.Frozen;

            p.bc = e.bc;

            return p;
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

    }
}
