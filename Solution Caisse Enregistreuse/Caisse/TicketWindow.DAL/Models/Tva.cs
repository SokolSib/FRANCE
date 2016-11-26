using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class Tva
    {
        public Tva(Guid customerId, string id, string val) : this(customerId, id.ToInt(), val.ToDecimal())
        {
        }

        public Tva(Guid customerId, int id, decimal value)
        {
            CustomerId = customerId;
            Id = id;
            Value = value;
        }

        public Guid CustomerId { get; set; }
        public int Id { get; set; }
        public decimal Value { get; set; }

        public static Tva FromXElement(XContainer element)
        {
            return new Tva(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("id").ToInt(),
                element.GetXElementValue("value").ToDecimal());
        }

        public static XElement ToXElement(Tva obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("id", obj.Id),
                new XElement("value", obj.Value));
        }

        public override string ToString()
        {
            return string.Concat(Id, " ", Value);
        }
    }
}