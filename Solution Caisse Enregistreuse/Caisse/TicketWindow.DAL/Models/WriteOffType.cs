using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class WriteOffType
    {
        public WriteOffType(Guid productCustomerId, string name, string productCodeBare, decimal count, DateTime date, Guid userId)
        {
            ProductCustomerId = productCustomerId;
            Name = name;
            ProductCodeBare = productCodeBare;
            Count = count;
            Date = date;
            UserId = userId;
        }

        public Guid ProductCustomerId { get; set; }
        public string Name { get; set; }
        public string ProductCodeBare { get; set; }
        public decimal Count { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }

        public static WriteOffType FromXElement(XContainer element)
        {
            return new WriteOffType(
                element.GetXElementValue("ProductCustomerId").ToGuid(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("ProductCodeBare"),
                element.GetXElementValue("Count").ToDecimal(),
                element.GetXElementValue("Date").ToDateTime(),
                element.GetXElementValue("UserId").ToGuid());
        }

        public static XElement ToXElement(WriteOffType obj)
        {
            return new XElement("rec",
                new XElement("ProductCustomerId", obj.ProductCustomerId),
                new XElement("Name", obj.Name),
                new XElement("ProductCodeBare", obj.ProductCodeBare),
                new XElement("Count", obj.Count),
                new XElement("Date", obj.Date),
                new XElement("UserId", obj.UserId)
                );
        }

        public override string ToString()
        {
            return $"{Name}={Count} {Date}";
        }
    }
}