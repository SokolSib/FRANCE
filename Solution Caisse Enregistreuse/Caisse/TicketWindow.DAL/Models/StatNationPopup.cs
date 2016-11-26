using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class StatNationPopup
    {
        public StatNationPopup(Guid idCustomer, string nameNation, int qty)
        {
            CustomerId = idCustomer;
            NameNation = nameNation;
            Qty = qty;
        }

        public Guid CustomerId { get; set; }
        public string NameNation { get; set; }
        public int Qty { get; set; }

        public static StatNationPopup FromXElement(XContainer element)
        {
            return new StatNationPopup(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NameNation"),
                element.GetXElementValue("Qty").ToInt());
        }

        public static XElement ToXElement(StatNationPopup obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NameNation", obj.NameNation),
                new XElement("Qty", obj.Qty));
        }

        public static void SetXmlValues(XContainer element, StatNationPopup obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("NameNation").SetValue(obj.NameNation);
            element.GetXElement("Qty").SetValue(obj.Qty);
        }

        public override string ToString()
        {
            return NameNation;
        }
    }
}