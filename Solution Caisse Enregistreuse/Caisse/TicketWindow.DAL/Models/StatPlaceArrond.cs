using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class StatPlaceArrond
    {
        public StatPlaceArrond(Guid idCustomer, string namePlaceArrond, int qty)
        {
            CustomerId = idCustomer;
            NamePlaceArrond = namePlaceArrond;
            Qty = qty;
        }

        public Guid CustomerId { get; set; }
        public string NamePlaceArrond { get; set; }
        public int Qty { get; set; }

        public static StatPlaceArrond FromXElement(XContainer element)
        {
            return new StatPlaceArrond(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NamePlaceArrond"),
                element.GetXElementValue("Qty").ToInt());
        }

        public static XElement ToXElement(StatPlaceArrond obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NamePlaceArrond", obj.NamePlaceArrond),
                new XElement("Qty", obj.Qty));
        }

        public static void SetXmlValues(XContainer element, StatPlaceArrond obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("NamePlaceArrond").SetValue(obj.NamePlaceArrond);
            element.GetXElement("Qty").SetValue(obj.Qty);
        }

        public override string ToString()
        {
            return NamePlaceArrond;
        }
    }
}