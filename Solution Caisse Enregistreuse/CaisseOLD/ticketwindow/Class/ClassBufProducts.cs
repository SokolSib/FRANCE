using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassBufProducts
    {
        public  XDocument x;
        public  ClassBufProducts()
        {
            x = new XDocument();
            x.Add(new XElement ("Product") );
            pb = new List<ClassProducts.product>();
        }

        public List<Class.ClassProducts.product> pb;
        

        public  void add (Class.ClassProducts.product p )
        {

            pb.Add(p);

              x.Element("Product").Add(
                    new XElement("rec",
                        new XElement("id", p.CustumerId),
                        new XElement("name", p.Name),
                        new XElement("price", p.price),
                        new XElement("tva", p.tva),
                        new XElement("qty", p.qty),
                        new XElement("code_bare", p.CodeBare),
                   
                        new XElement("balance", p.balance),
                        new XElement("contenance", p.contenance),
                        new XElement("uniteContenance", p.uniteContenance),
                        new XElement("tare", p.tare)
                        ));
        }
    }
}
