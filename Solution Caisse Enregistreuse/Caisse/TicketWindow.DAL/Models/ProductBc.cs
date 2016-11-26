using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class ProductBc
    {
        public ProductBc(Guid customerId, Guid? customerIdProduct, string codeBar, decimal qty, string description)
        {
            CustomerId = customerId;
            CustomerIdProduct = customerIdProduct;
            CodeBar = codeBar;
            Qty = qty;
            Description = description;
        }

        public Guid CustomerId { get; set; }
        public Guid? CustomerIdProduct { get; set; }
        public string CodeBar { get; set; }
        public decimal Qty { get; set; }
        public string Description { get; set; }

        public static ProductBc FromXElement(XContainer element)
        {
            return new ProductBc(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("CustomerIdProduct").ToNullableGuid(),
                element.GetXElementValue("CodeBar"),
                element.GetXElementValue("Qty").ToDecimal(),
                element.GetXElementValue("Description"));
        }

        public static XElement ToXElement(ProductBc obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("CustomerIdProduct", obj.CustomerIdProduct),
                new XElement("CodeBar", obj.CodeBar),
                new XElement("Qty", obj.Qty),
                new XElement("Description", obj.Description));
        }

        public override string ToString()
        {
            return CodeBar;
        }
    }
}