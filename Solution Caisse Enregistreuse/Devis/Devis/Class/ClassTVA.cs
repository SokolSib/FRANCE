using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Devis.Class
{
    class ClassTVA
    {
        private static XDocument x;
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\TVA.xml";
        public class tva
        {
            public Guid CustumerId;
            public int id;
            public decimal val;
        }

        public static List<tva> listTVA;


        public static List<ClassSync.ProductDB.TVA_> xmlToDb()
        {
            List<ClassSync.ProductDB.TVA_> R = new List<ClassSync.ProductDB.TVA_>();

            if (listTVA != null)
            {

                foreach (tva mtva in listTVA)
                {
                    ClassSync.ProductDB.TVA_ tva = new ClassSync.ProductDB.TVA_();

                    tva.CustumerId = mtva.CustumerId;
                    tva.Id = mtva.id.ToString();
                    tva.val = mtva.val.ToString();

                    R.Add(tva);
                }
                return R;
            }
            else return null;

        }

        public static decimal getTVA(int id)
        {
            int ix = listTVA.FindIndex(l => l.id == id);
            if (ix != -1)
                return listTVA[ix].val;
            else return 0.0m;
        }
        public static void loadFromFile()
        {
            x = XDocument.Load(path);

            IEnumerable<XElement> e = x.Element("tva").Elements("rec");

            listTVA = new List<tva>();

            foreach (XElement el in e)
            {
                tva g = new tva();
                g.id = int.Parse(el.Element("id").Value);
                g.val = decimal.Parse(el.Element("value").Value.Replace(".", ","));
                g.CustumerId = Guid.Parse(el.Element("CustumerId").Value);
                listTVA.Add(g);
            }
        }

        public static void save()
        {
            x = new XDocument();

            x.Add(new XElement("tva"));

            foreach (var e in ClassSync.ProductDB.TVA_.L)
            {
                x.Element("tva").Add(new XElement("rec",
                    new XElement("CustumerId", e.CustumerId),
                    new XElement("id", e.Id),
                    new XElement("value", e.val)
                    ));
            }
            x.Save(path);
        }
        public ClassTVA()
        {

        }
    }
}
