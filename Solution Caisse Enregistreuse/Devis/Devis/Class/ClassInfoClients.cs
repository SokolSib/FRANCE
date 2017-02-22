using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Devis.Class
{
    class ClassInfoClients
    {
        public static XDocument x;
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\ic.xml";
        public static void save()
        {
            x = new XDocument();

            x.Add(new XElement("tva"));

            foreach (var e in ClassSync.infoClient.list)
            {
                x.Element("tva").Add(new XElement("rec",
                    new XElement("custumerId", e.custumerId),
                    new XElement("TypeClient", e.TypeClient),
                    new XElement("Sex", e.Sex),
                    new XElement("Name", e.Name),
                    new XElement("Surname", e.Surname),
                    new XElement("NameCompany", e.NameCompany),
                    new XElement("SIRET", e.SIRET),
                    new XElement("FRTVA", e.FRTVA),
                    new XElement("OfficeAddress", e.OfficeAddress),
                    new XElement("OfficeZipCode", e.OfficeZipCode),
                    new XElement("OfficeCity", e.OfficeCity),
                    new XElement("HomeAddress", e.HomeAddress),
                    new XElement("HomeZipCode", e.HomeZipCode),
                    new XElement("HomeCity", e.HomeCity),
                    new XElement("Telephone", e.Telephone),
                    new XElement("Mail", e.Mail),
                    new XElement("Password", e.Password),
                    new XElement("Countrys_CustomerId", e.Countrys_CustomerId),
                    new XElement("FavoritesProductAutoCustomerId", e.FavoritesProductAutoCustomerId),
                    new XElement("Nclient", e.Nclient)
                   
                    ));
            }
            x.Save(path);
        }
        public static void loadFromFile()
        {
            x = XDocument.Load(path);
        }
    }
}
