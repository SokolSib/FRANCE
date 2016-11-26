using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    public class ClassActionsCaisse
    {
        private static XDocument doc { get; set; }

        public static string path = AppDomain.CurrentDomain.BaseDirectory + @"Data\ActionsCaisse.xml";
        public void saveFile()
        {
            string s = "<ActionsCaisse>" + Environment.NewLine;

            foreach (var item in ClassSync.ActionsCaisseDB.L)
            {
                s += "<item>" + item.XML + "</item>" + Environment.NewLine; 
            }

            s += "</ActionsCaisse>" + Environment.NewLine;

            System.IO.File.WriteAllText(path, s);
        }


        public XDocument Descendants (XDocument check)
        {

            List<XElement> lxp = check.Element("check").Elements("product").Reverse().ToList();

            List<XElement> d = new List<XElement>();


                        

            foreach (var xp in lxp)
            {
                var CustumerId = xp.Element("CustumerId").Value;
                var price = xp.Element("price").Value;

                var x = d.Find(l => l.Element("CustumerId").Value == CustumerId &&  l.Element("price").Value == price );

                if ( x == null)
                {
                    xp.Element("ii").SetValue(d.Count);

                    d.Add( new XElement( xp));
                }
                else
                {
                    x.Element("qty").SetValue( decimal.Parse(x.Element("qty").Value.Replace('.', ',')) + decimal.Parse(xp.Element("qty").Value.Replace('.', ',')));

                    x.Element("sumDiscount").SetValue(decimal.Parse(x.Element("sumDiscount").Value.Replace('.', ',')) + decimal.Parse(xp.Element("sumDiscount").Value.Replace('.', ',')));

                    x.Element("total").SetValue(decimal.Parse(x.Element("total").Value.Replace('.',',')) + decimal.Parse(xp.Element("total").Value.Replace('.', ',')));
                }
                
            }

            check.Element("check").Elements("product").Remove();

            foreach (var elm in d)
            {
                check.Element("check").Add(elm);
            }
            return actions( check);
        }

        public XDocument actions (XDocument check)
        {
            IEnumerable<XElement> lxp = check.Element("check").Elements("product");

            IEnumerable<XElement> actions = doc.Element("ActionsCaisse").Elements("item");

            List<XElement> res = new List<XElement>();

            foreach (var xp in lxp)
            {
                var CustumerId = Guid.Parse(xp.Element("CustumerId").Value);

                bool flag = false;

                foreach (var a in actions)
                {
                  
                    var type = int.Parse( a.Element("DeActions").Element("type").Value);
                    var enabled = bool.Parse( a.Element("DeActions").Element("enabled").Value);
                    var A = DateTime.Parse( a.Element("DeActions").Element("A").Value);
                    var B = DateTime.Parse( a.Element("DeActions").Element("B").Value);
                    var establishmentCustomerId = Guid.Parse( a.Element("DeActions").Element("establishmentCustomerId").Value);
                    var productCustomerId = Guid.Parse( a.Element("DeActions").Element("productCustomerId").Value);
                    var qty = decimal.Parse( a.Element("DeActions").Element("qty").Value.Replace('.',','));
                    var prix = decimal.Parse( a.Element("DeActions").Element("prix").Value.Replace('.', ','));
                    var discount = decimal.Parse( a.Element("DeActions").Element("discount").Value.Replace('.', ','));

                    var dtn = DateTime.Now;

                   
                    if (CustumerId == productCustomerId && dtn >= A && dtn <= B && prix !=  decimal.Parse (xp.Element("price").Value.Replace('.',',') ))
                    {
                        if (type == 1)
                        {

                            decimal qty_ = decimal.Parse(xp.Element("qty").Value.Replace('.', ','));

                            if (qty == 1)
                            {
                                XElement n = new XElement(xp);

                                n.Element("qty").SetValue(qty_);

                                n.Element("price").SetValue(prix);

                                n.Element("total").SetValue(prix * qty_);

                                decimal prix_old = decimal.Parse(xp.Element("price").Value.Replace('.', ','));

                                n.Element("sumDiscount").SetValue(prix_old * qty_  - prix * qty_ );
                                n.Element("ii").SetValue(res.Count);
                                res.Add(n);
                                flag = true;
                            }

                            if (qty_ >= qty && qty > 1 )
                            {
                                int c = (int)(qty_ / qty);

                                decimal f = qty_ - qty* c;

                                decimal prix_old = decimal.Parse(xp.Element("price").Value.Replace('.', ','));

                                XElement n = new XElement(xp);

                                n.Element("qty").SetValue(qty * c);

                              //  n.Element("price").SetValue(prix);

                                n.Element("total").SetValue(prix * qty * c);

                                n.Element("sumDiscount").SetValue((prix_old * qty * c - prix * qty * c));

                                n.Element("Discount").SetValue( Math.Truncate( 100 * prix / prix_old));

                                XElement n2;

                                if (f > 0)
                                {
                                    n2 = new XElement(xp);

                                    n2.Element("qty").SetValue(f);

                                 //   n2.Element("price").SetValue(prix_old);

                                    n2.Element("total").SetValue(prix_old * f);

                                    n2.Element("sumDiscount").SetValue(0);

                                    n2.Element("ii").SetValue(res.Count);
                                    res.Add(n2);
                                }
                                n.Element("ii").SetValue(res.Count);
                                res.Add(n);
                                flag = true;
                            }
                        }

                       
                    }
                   
                }
                if (!flag)

                    res.Add(xp);

            }
            check.Element("check").Elements("product").Remove();

            foreach (var elm in res)
            {
                check.Element("check").AddFirst(elm);
            }
            return check;
        }

        public void loadFile()
        {
            doc = XDocument.Load(path);    
        }
    }
}
