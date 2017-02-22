using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class Establishment
    {
        public Establishment(Guid customerId, int type, string name, string cp, string ville, string adress, string phone, string mail, string siret, string ntva, string codeNaf, string fax)
        {
            CustomerId = customerId;
            Type = type;
            Name = name;
            Cp = cp;
            Ville = ville;
            Adress = adress;
            Phone = phone;
            Mail = mail;
            Siret = siret;
            Ntva = ntva;
            CodeNaf = codeNaf;
            Fax = fax;
        }

        public Guid CustomerId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Cp { get; set; }
        public string Ville { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Siret { get; set; }
        public string Ntva { get; set; }
        public string CodeNaf { get; set; }
        public string Fax { get; set; }

        public static Establishment FromXElement(XContainer element)
        {
            return new Establishment(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Type").ToInt(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("Cp"),
                element.GetXElementValue("Ville"),
                element.GetXElementValue("Adress"),
                element.GetXElementValue("Phone"),
                element.GetXElementValue("Mail"),
                element.GetXElementValue("Siret"),
                element.GetXElementValue("Ntva"),
                element.GetXElementValue("CodeNaf"),
                element.GetXElementValue("Fax"));
        }

        public static XElement ToXElement(Establishment obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Type", obj.Type),
                new XElement("Name", obj.Name),
                new XElement("Cp", obj.Cp),
                new XElement("Ville", obj.Ville),
                new XElement("Adress", obj.Adress),
                new XElement("Phone", obj.Phone),
                new XElement("Mail", obj.Mail),
                new XElement("Siret", obj.Siret),
                new XElement("Ntva", obj.Ntva),
                new XElement("CodeNaf", obj.CodeNaf),
                new XElement("Fax", obj.Fax));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}