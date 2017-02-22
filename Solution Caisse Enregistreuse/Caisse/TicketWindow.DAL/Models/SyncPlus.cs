using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class SyncPlus
    {
        public SyncPlus(Guid customerId, DateTime date, string nameCasse)
        {
            CustomerId = customerId;
            Date = date;
            NameCashBox = nameCasse;
        }

        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string NameCashBox { get; set; }

        public static SyncPlus FromXElement(XContainer element)
        {
            return new SyncPlus(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Date").ToDateTime(),
                element.GetXElementValue("NameCashBox"));
        }

        public static XElement ToXElement(SyncPlus obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Date", obj.Date),
                new XElement("NameCashBox", obj.NameCashBox));
        }

        public override string ToString()
        {
            return NameCashBox;
        }
    }
}