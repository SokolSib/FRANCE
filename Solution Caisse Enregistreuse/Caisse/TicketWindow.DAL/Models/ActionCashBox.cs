using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class ActionCashBox
    {
        public ActionCashBox(Guid customerId, string nameAction, DateTime? date, DateTime? a, DateTime? b, Guid? est, bool? enabled, string xml)
        {
            CustomerId = customerId;
            NameAction = nameAction;
            Date = date;
            A = a;
            B = b;
            Est = est;
            Enabled = enabled;
            Xml = xml;
        }

        public Guid CustomerId { get; set; }
        public string NameAction { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? A { get; set; }
        public DateTime? B { get; set; }
        public Guid? Est { get; set; }
        public bool? Enabled { get; set; }
        public string Xml { get; set; }

        public static ActionCashBox FromXElement(XContainer element)
        {
            return new ActionCashBox(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NameAction"),
                element.GetXElementValue("Date").ToNullableDateTime(),
                element.GetXElementValue("A").ToNullableDateTime(),
                element.GetXElementValue("B").ToNullableDateTime(),
                element.GetXElementValue("Est").ToNullableGuid(),
                element.GetXElementValue("Enabled").ToBool(),
                element.GetXElementValue("Xml"));
        }

        public static XElement ToXElement(ActionCashBox obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NameAction", obj.NameAction),
                new XElement("Date", obj.Date),
                new XElement("A", obj.A),
                new XElement("B", obj.B),
                new XElement("Est", obj.Est),
                new XElement("Enabled", obj.Enabled),
                new XElement("Xml", obj.Xml));
        }

        public override string ToString()
        {
            return NameAction;
        }
    }
}
