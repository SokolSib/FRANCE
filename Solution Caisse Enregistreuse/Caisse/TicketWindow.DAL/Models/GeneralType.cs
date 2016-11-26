using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class GeneralType
    {
        public GeneralType(Guid id, Guid ticketWindowGeneral, bool? open, string name, string user, DateTime date, Guid establishmentCustomerId)
        {
            Id = id;
            TicketWindowGeneral = ticketWindowGeneral;
            IsOpen = open;
            Name = name;
            User = user;
            Date = date;
            EstablishmentCustomerId = establishmentCustomerId;
        }

        public Guid Id { get; set; }
        public Guid TicketWindowGeneral { get; set; }
        public bool? IsOpen { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public Guid EstablishmentCustomerId { get; set; }

        public static GeneralType FromXElement(XContainer element)
        {
            return new GeneralType(
                element.GetXElementValue("Id").ToGuid(),
                element.GetXElementValue("TicketWindowGeneral").ToGuid(),
                element.GetXElementValue("IsOpen").ToNullableBool(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("User"),
                element.GetXElementValue("Date").ToDateTime(),
                element.GetXElementValue("EstablishmentCustomerId").ToGuid());
        }

        public static XElement ToXElement(GeneralType obj)
        {
            return new XElement("rec",
                new XElement("Id", obj.Id),
                new XElement("TicketWindowGeneral", obj.TicketWindowGeneral),
                new XElement("IsOpen", obj.IsOpen),
                new XElement("Name", obj.Name),
                new XElement("User", obj.User),
                new XElement("Date", obj.Date),
                new XElement("EstablishmentCustomerId", obj.EstablishmentCustomerId));
        }

        public static void SetXmlValues(XContainer element, GeneralType obj)
        {
            element.GetXElement("Id").SetValue(obj.Id);
            element.GetXElement("TicketWindowGeneral").SetValue(obj.TicketWindowGeneral);
            element.GetXElement("IsOpen").SetValue(obj.IsOpen.HasValue ? obj.IsOpen.Value.ToString() : string.Empty);
            element.GetXElement("Name").SetValue(obj.Name);
            element.GetXElement("User").SetValue(obj.User);
            element.GetXElement("Date").SetValue(obj.Date);
            element.GetXElement("EstablishmentCustomerId").SetValue(obj.EstablishmentCustomerId);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}