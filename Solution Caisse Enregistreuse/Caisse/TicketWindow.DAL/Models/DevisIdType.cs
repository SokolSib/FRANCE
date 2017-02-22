using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class DevisIdType
    {
        public DevisIdType(int id, DateTime? date, bool? close, Guid? infoClientsCustomerId, decimal? total)
        {
            Id = id;
            Date = date;
            Close = close;
            InfoClientsCustomerId = infoClientsCustomerId;
            Total = total;

            DivisWebs = new List<DevisWebType>();
        }

        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public bool? Close { get; set; }
        public Guid? InfoClientsCustomerId { get; set; }
        public decimal? Total { get; set; }
        public List<DevisWebType> DivisWebs { get; set; }

        public static DevisIdType FromXElement(XContainer element)
        {
            return new DevisIdType(
                element.GetXElementValue("Id").ToInt(),
                element.GetXElementValue("Date").ToNullableDateTime(),
                element.GetXElementValue("Close").ToNullableBool(),
                element.GetXElementValue("InfoClientsCustomerId").ToNullableGuid(),
                element.GetXElementValue("Total").ToNullableDecimal());
        }

        public static XElement ToXElement(DevisIdType obj)
        {
            return new XElement("rec",
                new XElement("Id", obj.Id),
                new XElement("Date", obj.Date),
                new XElement("Close", obj.Close),
                new XElement("InfoClientsCustomerId", obj.InfoClientsCustomerId),
                new XElement("Total", obj.Total));
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}