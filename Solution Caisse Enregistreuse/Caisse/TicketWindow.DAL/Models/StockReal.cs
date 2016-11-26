using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class StockReal
    {
        public StockReal(Guid customerId, decimal qty, decimal minQty, decimal price, Guid productsCustomerId, Guid idEstablishment)
        {
            CustomerId = customerId;
            Qty = qty;
            MinQty = minQty;
            Price = price;
            ProductsCustomerId = productsCustomerId;
            IdEstablishment = idEstablishment;
        }

        public Guid CustomerId { get; set; }
        public decimal Qty { get; set; }
        public decimal MinQty { get; set; }
        public decimal Price { get; set; }
        public Guid ProductsCustomerId { get; set; }
        public Guid IdEstablishment { get; set; }

        public static StockReal FromXElement(XContainer element)
        {
            return new StockReal(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Qty").ToDecimal(),
                element.GetXElementValue("MinQty").ToDecimal(),
                element.GetXElementValue("Price").ToDecimal(),
                element.GetXElementValue("ProductsCustomerId").ToGuid(),
                element.GetXElementValue("IdEstablishment").ToGuid());
        }

        public static XElement ToXElement(StockReal obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Qty", obj.Qty),
                new XElement("MinQty", obj.MinQty),
                new XElement("Price", obj.Price),
                new XElement("ProductsCustomerId", obj.ProductsCustomerId),
                new XElement("IdEstablishment", obj.IdEstablishment));
        }

        public static void SetXmlValues(XContainer element, StockReal obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("Qty").SetValue(obj.Qty);
            element.GetXElement("MinQty").SetValue(obj.MinQty);
            element.GetXElement("Price").SetValue(obj.Price);
            element.GetXElement("ProductsCustomerId").SetValue(obj.ProductsCustomerId);
            element.GetXElement("IdEstablishment").SetValue(obj.IdEstablishment);
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}