using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class Country
    {
        public Country(Guid customerId, string nameCountry, string capital, string continent)
        {
            CustomerId = customerId;
            NameCountry = nameCountry;
            Capital = capital;
            Continent = continent;
        }

        public Guid CustomerId { get; set; }
        public string NameCountry { get; set; }
        public string Capital { get; set; }
        public string Continent { get; set; }

        public static Country FromXElement(XContainer element)
        {
            return new Country(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NameCountry"),
                element.GetXElementValue("Capital"),
                element.GetXElementValue("Continent"));
        }

        public static XElement ToXElement(Country obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NameCountry", obj.NameCountry),
                new XElement("Capital", obj.Capital),
                new XElement("Continent", obj.Continent)
                );
        }

        public override string ToString()
        {
            return NameCountry;
        }
    }
}