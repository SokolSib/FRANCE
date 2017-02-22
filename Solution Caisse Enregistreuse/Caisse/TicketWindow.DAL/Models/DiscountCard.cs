using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class DiscountCard
    {
        public DiscountCard(Guid customerId, string numberCard, int points, bool active, Guid infoClientsCustomerId, DateTime dateTimeLastAddProduct)
        {
            CustomerId = customerId;
            NumberCard = numberCard;
            Points = points;
            IsActive = active;
            InfoClientsCustomerId = infoClientsCustomerId;
            DateTimeLastAddProduct = dateTimeLastAddProduct;
        }

        public Guid CustomerId { get; set; }
        public string NumberCard { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }
        public Guid InfoClientsCustomerId { get; set; }
        public DateTime DateTimeLastAddProduct { get; set; }

        public static DiscountCard FromXElement(XContainer element)
        {
            return new DiscountCard(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("NumberCard"),
                element.GetXElementValue("Points").ToInt(),
                element.GetXElementValue("IsActive").ToBool(),
                element.GetXElementValue("InfoClientsCustomerId").ToGuid(),
                element.GetXElementValue("DateTimeLastAddProduct").ToDateTime());
        }

        public static XElement ToXElement(DiscountCard obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("NumberCard", obj.NumberCard),
                new XElement("Points", obj.Points),
                new XElement("IsActive", obj.IsActive),
                new XElement("InfoClientsCustomerId", obj.InfoClientsCustomerId),
                new XElement("DateTimeLastAddProduct", obj.DateTimeLastAddProduct));
        }

        public static void SetXmlValues(XContainer element, DiscountCard obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("NumberCard").SetValue(obj.NumberCard);
            element.GetXElement("Points").SetValue(obj.Points);
            element.GetXElement("IsActive").SetValue(obj.IsActive);
            element.GetXElement("InfoClientsCustomerId").SetValue(obj.InfoClientsCustomerId);
            element.GetXElement("DateTimeLastAddProduct").SetValue(obj.DateTimeLastAddProduct);
        }

        public override string ToString()
        {
            return NumberCard;
        }
    }
}