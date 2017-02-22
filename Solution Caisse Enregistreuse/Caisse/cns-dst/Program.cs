using cns_dst.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace cns_dst
{
    class Program
    {
        static void r_xml_file()
        {
            string path = @"D:\Product.xml";
            //  string pat = @"D:\GroupProduct.xml";
            XDocument x = XDocument.Load(path);
            IEnumerable<XElement> elms = x.Element("Product").Elements("rec");
            int i = 0;

            new ClassDB(null).queryResonse("DELETE FROM StockReal");
            new ClassDB(null).queryResonse("DELETE FROM Products");
            
           
            foreach (XElement e in elms)
            {
                Products p = new Products();
                p.CustumerId = Guid.NewGuid();
               // e.Element("id").Value = p.CustumerId.ToString();
                p.Name = e.Element("Name").Value;
                
                p.TVACustumerId = e.Element("tva").Value == "1" ? Guid.Parse("7e73aa7e-a6a6-4ed5-8dad-3d47aa5cdbc0") : Guid.Parse("0eb6dd27-e9f8-4f50-8e7e-e7c81874ff56");
                p.ProductsAwaitingDeliveryCustomerId = Guid.Empty;
                p.ProductsWeb_CustomerId = Guid.NewGuid();
                p.SubGrpProduct_Id = 3;//Convert.ToInt32(e.Element("chp_fam").Value);
                p.CodeBare = e.Element("CodeBare").Value;
                p.Balance = Convert.ToBoolean(e.Element("balance").Value);
                p.Contenance = Convert.ToDecimal(e.Element("contenance").Value.Replace(".", ","));
                p.UniteContenance = Convert.ToInt32(e.Element("uniteContenance").Value);
                p.Tare = 0;
                p.Desc = e.Element("Desc").Value;
                p.Date = DateTime.Now;

                Products.StockReal s = new Products.StockReal();

                s.CustomerId = Guid.NewGuid();
                s.IdEstablishment = new Guid("e27d5a4d-d6d3-4ee5-810b-f95b32e0bb93");
                s.IdProduct = p.CustumerId;
                s.MinQTY = 10;
                s.QTY = 0;
                s.Price = Convert.ToDecimal(e.Element("price").Value.Replace(".", ","));
                s.ProductsCustumerId = p.CustumerId;
                
                p.SR.Add(s);
           
                Products.ins(p);
               
                Console.WriteLine(i++ );
            }

            x.Save(path);


        }

        static void r_xml_file_ ()
        {
            int i = 0;
            
            string path = @"D:\Product.xml";
            XDocument x = XDocument.Load(path);
            IEnumerable<XElement> elms = x.Element("Product").Elements("rec");
            int c = elms.Count();
            foreach (XElement e in elms)
            {

                Guid pg = Products.getProdCustomerId(e.Element("CodeBare").Value.TrimEnd().TrimStart().Trim());
                i++;
                Console.WriteLine(i + " of " + c);
                if (pg != Guid.Empty)
                {
                    Products.StockReal s = new Products.StockReal();
                    s.CustomerId = Guid.NewGuid();
                    s.IdEstablishment = new Guid("42e89ba9-1012-4327-95f9-40712e232849");
                    s.IdProduct = pg ;
                    s.MinQTY = 10;
                    s.QTY = 0;
                    s.Price = Convert.ToDecimal(e.Element("price").Value.Replace(".", ","));
                    s.ProductsCustumerId = pg;
                    Products.StockReal.ins(s);

                }
            }
        }

        static void Main(string[] args)
        {
            r_xml_file_();
        }
    }
}
