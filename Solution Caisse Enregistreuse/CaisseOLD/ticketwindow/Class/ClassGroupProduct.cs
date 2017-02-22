using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassGroupProduct
    {
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\GroupProduct.xml";

        private static XDocument x;

        public class group
        {
            public int id;
            public string Name;
            public List< group> sub = new List<group>();
        }

        public static List< ClassSync.ProductDB.GrpProduct> groupXMLToGrpProductTabl ()
        {
            List<ClassSync.ProductDB.GrpProduct> R = new List<ClassSync.ProductDB.GrpProduct>();

            if (listGroupName != null)
            {

                foreach (group g in listGroupName)
                {

                    ClassSync.ProductDB.GrpProduct gp = new ClassSync.ProductDB.GrpProduct();

                    gp.GroupName = g.Name;
                    gp.Id = g.id;

                    foreach (group sg in g.sub)
                    {
                        ClassSync.ProductDB.SubGrpProduct sgp = new ClassSync.ProductDB.SubGrpProduct();
                        sgp.GrpProductId = g.id;
                        sgp.SubGroupName = sg.Name;
                        sgp.Id = sg.id;
                        gp.SubGrpNameSet.Add(sgp);
                    }

                    R.Add(gp);
                }
                return R;
            }
            else
            return null;
        }

        public static List<group> listGroupName = new List<group>();

        public static string getName(int id)
        {
            if (id == 0) id = 1;
            return listGroupName.Find(l => l.id == id).Name;
        }
       
        public struct grp_subGrp
        {
            public int grp;
            public int subGrp;
        }
        public static grp_subGrp getGrpId (int customer)
        {
            grp_subGrp r = new grp_subGrp();

            foreach (var e in listGroupName)
            {
                r.grp = e.id;

                if (e.sub.Exists(l => l.id == customer))
                {
                    r.subGrp = customer;

                    break;
                    
                }
            }

            return r;
            
        }

        public static void loadFromFile()
        {
            x = XDocument.Load(path);

            IEnumerable<XElement> e = x.Element("Palettes").Elements("Palette");


           

            foreach (XElement el in e)
            {
                group g = new group();
                
                XElement xe = el.Element("Group");
                g.id = int.Parse(xe.Attribute("ID").Value);
                g.Name = (xe.Attribute("Name").Value);
                IEnumerable< XElement> xeg = el.Elements("SubGroup");
                foreach (var s in xeg)
                {
                    group gs = new group();
                    gs.id = int.Parse(s.Attribute("ID").Value);
                    gs.Name = (s.Attribute("Name").Value);
                    g.sub.Add(gs);
                }
                
                listGroupName.Add(g);

            }
        }

        public static void save ()
        {
            x = new XDocument();

            x.Add(new XElement("Palettes"));
            
            foreach (var g in ClassSync.ProductDB.GrpProduct.L)
            {
                x.Element("Palettes").Add(new XElement("Palette",
                                          new XElement("Group",
                                          new XAttribute("Name", g.GroupName),
                                          new XAttribute("ID", g.Id)
                                          )));

                foreach (var sg in g.SubGrpNameSet)
                {
                    IEnumerable<XElement> el = x.Element("Palettes").Elements("Palette").Where(l => l.Element("Group").Attribute("Name").Value == g.GroupName);
                    
                   // if (el.Count() == 1)
                      el.First().Add(new XElement("SubGroup", new XAttribute("Name", sg.SubGroupName), new XAttribute("ID", sg.Id)));
                 //   else
                     //   el.First().Add(new XElement("SubGroup", new XAttribute("Name", sg.SubGroupName), new XAttribute("ID", sg.Id)));
 
                }
            }

            x.Save(path);
        }

        public ClassGroupProduct()
        {

        }
    }

}
