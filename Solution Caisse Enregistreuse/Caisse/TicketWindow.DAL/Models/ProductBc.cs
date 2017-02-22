using System;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
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
            GeneratedFromProduct = false;

            if (RepositoryProduct.Products.Count == 0)
                RepositoryProduct.Sync();

            if (CustomerIdProduct.HasValue)
                Product = RepositoryProduct.Products.FirstOrDefault(p => p.CustomerId == CustomerIdProduct);
        }

        public ProductBc(ProductType product, decimal count)
            : this(Guid.NewGuid(), product.CustomerId, product.CodeBare, count, product.Name)
        {
            GeneratedFromProduct = true;
        }

        public Guid CustomerId { get; set; }
        public Guid? CustomerIdProduct { get; set; }
        public string CodeBar { get; set; }
        public decimal Qty { get; set; }
        public string Description { get; set; }
        public ProductType Product { get; set; }
        public bool GeneratedFromProduct { get; set; }

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