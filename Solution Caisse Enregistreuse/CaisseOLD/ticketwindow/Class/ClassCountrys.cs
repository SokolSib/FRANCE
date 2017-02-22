using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    class ClassCountrys
    {
        private static XDocument x;
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\Countrys.xml";
        public static List<ClassSync.Countrys> listCountrys;
        public static List<string> listStringCountrys;

        public static void loadFromFile()
        {
            try
            {
                x = XDocument.Load(path);
            }
            catch
            {
                saveFronDB();
            }
            IEnumerable<XElement> e = x.Element("Countrys").Elements("rec");

            listCountrys = new List<ClassSync.Countrys>();
            listStringCountrys = new List<string>();

            foreach (XElement el in e)
            {
                ClassSync.Countrys g = new ClassSync.Countrys();
                g.CustomerId = Guid.Parse(el.Element("CustomerId").Value);
                g.NameCountry = el.Element("NameCountry").Value;
                g.Capital = el.Element("Capital").Value;
                g.Continent = el.Element("Continent").Value;
                listCountrys.Add(g);
                listStringCountrys.Add(g.NameCountry);
            }
        }

        private static void saveFronDB()
        {
            x = new XDocument();

            x.Add(new XElement("Countrys"));

            foreach (var e in new  ClassSync.Countrys().selAll())
            {
                x.Element("Countrys").Add(new XElement("rec",
                    new XElement("CustomerId", e.CustomerId),
                    new XElement("NameCountry", e.NameCountry),
                    new XElement("Capital", e.Capital),
                    new XElement("Continent", e.Continent)
                    ));
            }
            x.Save(path);
        }
    }
}
