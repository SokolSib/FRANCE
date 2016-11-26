using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class OpenTicketWindow
    {
        public OpenTicketWindow(Guid customerId, Guid idTicketWindow, string nameTicket, string user, int numberTicket, bool isOpen, DateTime dateOpen, Guid? idTicketWindowG,
            Guid establishmentCustomerId)
        {
            CustomerId = customerId;
            IdTicketWindow = idTicketWindow;
            NameTicket = nameTicket;
            User = user;
            NumberTicket = numberTicket;
            IsOpen = isOpen;
            DateOpen = dateOpen;
            IdTicketWindowG = idTicketWindowG;
            EstablishmentCustomerId = establishmentCustomerId;
        }

        public Guid CustomerId { get; set; }
        public Guid IdTicketWindow { get; set; }
        public string NameTicket { get; set; }
        public string User { get; set; }
        public int NumberTicket { get; set; }
        public bool IsOpen { get; set; }
        public DateTime DateOpen { get; set; }
        public Guid? IdTicketWindowG { get; set; }
        public Guid EstablishmentCustomerId { get; set; }

        public static OpenTicketWindow FromXElement(XContainer element)
        {
            return new OpenTicketWindow(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("IdTicketWindow").ToGuid(),
                element.GetXElementValue("NameTicket"),
                element.GetXElementValue("User"),
                element.GetXElementValue("NumberTicket").ToInt(),
                element.GetXElementValue("IsOpen").ToBool(),
                element.GetXElementValue("DateOpen").ToDateTime(),
                element.GetXElementValue("IdTicketWindowG").ToNullableGuid(),
                element.GetXElementValue("EstablishmentCustomerId").ToGuid());
        }

        public static XElement ToXElement(OpenTicketWindow obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("IdTicketWindow", obj.IdTicketWindow),
                new XElement("NameTicket", obj.NameTicket),
                new XElement("User", obj.User),
                new XElement("NumberTicket", obj.NumberTicket),
                new XElement("IsOpen", obj.IsOpen),
                new XElement("DateOpen", obj.DateOpen),
                new XElement("IdTicketWindowG", obj.IdTicketWindowG),
                new XElement("EstablishmentCustomerId", obj.EstablishmentCustomerId));
        }

        public static void SetXmlValues(XContainer element, OpenTicketWindow obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("IdTicketWindow").SetValue(obj.IdTicketWindow);
            element.GetXElement("NameTicket").SetValue(obj.NameTicket);
            element.GetXElement("User").SetValue(obj.User);
            element.GetXElement("NumberTicket").SetValue(obj.NumberTicket);
            element.GetXElement("IsOpen").SetValue(obj.IsOpen);
            element.GetXElement("DateOpen").SetValue(obj.DateOpen);
            element.GetXElement("IdTicketWindowG").SetValue(obj.IdTicketWindowG.HasValue ? obj.IdTicketWindowG.Value.ToString() : string.Empty);
            element.GetXElement("EstablishmentCustomerId").SetValue(obj.EstablishmentCustomerId);
        }

        public override string ToString()
        {
            return NameTicket;
        }
    }
}