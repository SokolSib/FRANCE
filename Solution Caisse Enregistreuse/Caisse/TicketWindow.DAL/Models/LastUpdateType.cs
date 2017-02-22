using System;
using System.Globalization;
using System.Xml.Linq;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Models
{
    public class LastUpdateType
    {
        public LastUpdateType(Guid customerId, string nameTicket, bool upd, string date, string user, Guid idEstablishment)
        {
            CustomerId = customerId;
            NameTicket = nameTicket;
            Upd = upd;
            DateTime convertedDate;
            if (!DateTime.TryParseExact(date, Config.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out convertedDate))
            {
                if (!DateTime.TryParseExact(date, Config.DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out convertedDate))
                    convertedDate = DateTime.ParseExact(date, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            LastDate = convertedDate;
            User = user;
            IdEstablishment = idEstablishment;
        }

        public Guid CustomerId { get; set; }
        public string NameTicket { get; set; }
        public bool Upd { get; set; }
        public DateTime LastDate { get; set; }
        public string User { get; set; }
        public Guid IdEstablishment { get; set; }

        public static LastUpdateType FromXElement(XContainer element)
        {
            return new LastUpdateType(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NameTicket"),
                element.GetXElementValue("Upd").ToBool(),
                element.GetXElementValue("LastDate"),
                element.GetXElementValue("User"),
                element.GetXElementValue("IdEstablishment").ToGuid());
        }

        public static XElement ToXElement(LastUpdateType obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NameTicket", obj.NameTicket),
                new XElement("Upd", obj.Upd),
                new XElement("LastDate", obj.LastDate),
                new XElement("User", obj.User),
                new XElement("IdEstablishment", obj.IdEstablishment));
        }

        public static void SetXmlValues(XContainer element, LastUpdateType obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("NameTicket").SetValue(obj.NameTicket);
            element.GetXElement("Upd").SetValue(obj.Upd);
            element.GetXElement("LastDate").SetValue(obj.LastDate);
            element.GetXElement("User").SetValue(obj.User);
            element.GetXElement("IdEstablishment").SetValue(obj.IdEstablishment);
        }
    }
}