using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class StatNation
    {
        public StatNation(Guid customerId, DateTime date, Guid idCaisse, Guid idNation, Guid idArrond)
        {
            CustomerId = customerId;
            Date = date;
            IdCaisse = idCaisse;
            IdNation = idNation;
            IdArrond = idArrond;
        }

        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public Guid IdCaisse { get; set; }
        public Guid IdNation { get; set; }
        public Guid IdArrond { get; set; }

        public static StatNation FromXElement(XContainer element)
        {
            return new StatNation(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Date").ToDateTime(),
                element.GetXElementValue("IdCaisse").ToGuid(),
                element.GetXElementValue("IdNation").ToGuid(),
                element.GetXElementValue("IdArrond").ToGuid());
        }

        public static XElement ToXElement(StatNation obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Date", obj.Date),
                new XElement("IdCaisse", obj.IdCaisse),
                new XElement("IdNation", obj.IdNation),
                new XElement("IdArrond", obj.IdArrond));
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}