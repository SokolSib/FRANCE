using System;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class PayProduct : PayProductBase
    {
        public PayProduct(Guid idCheckTicket, Guid productId, string name, string barcode, decimal qty, decimal tva, decimal priceHt, decimal total, Guid checksTicketCustomerId,
            Guid checksTicketCloseTicketCustomerId, decimal discount, decimal sumDiscount)
            : base(idCheckTicket, productId, name, barcode, qty, tva, priceHt, total, checksTicketCustomerId, discount, sumDiscount)
        {
            ChecksTicketCloseTicketCustomerId = checksTicketCloseTicketCustomerId;
        }

        public Guid ChecksTicketCloseTicketCustomerId { get; set; }

        public static PayProduct FromCheckXElement(XElement element, Guid customerId, Guid checksTicketCustomerId, Guid checksTicketCloseTicketCustomerId)
        {
            return new PayProduct(
                customerId,
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("CodeBare"),
                element.GetXElementValue("qty").ToDecimal(),
                RepositoryTva.GetById(element.GetXElementValue("tva").ToInt()),
                element.GetXElementValue("price").ToDecimal(),
                element.GetXElementValue("total").ToDecimal(),
                checksTicketCustomerId,
                checksTicketCloseTicketCustomerId,
                element.GetXElementValue("Discount").ToDecimal(),
                element.GetXElementValue("sumDiscount").ToDecimal());
        }

        public static PayProduct FromXElement(XContainer element)
        {
            return new PayProduct(
                element.GetXElementValue("IdCheckTicket").ToGuid(),
                element.GetXElementValue("ProductId").ToGuid(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("Barcode"),
                element.GetXElementValue("Qty").ToDecimal(),
                element.GetXElementValue("Tva").ToDecimal(),
                element.GetXElementValue("PriceHt").ToDecimal(),
                element.GetXElementValue("Total").ToDecimal(),
                element.GetXElementValue("ChecksTicketCustomerId").ToGuid(),
                element.GetXElementValue("ChecksTicketCloseTicketCustomerId").ToGuid(),
                element.GetXElementValue("Discount").ToDecimal(),
                element.GetXElementValue("SumDiscount").ToDecimal());
        }

        public static XElement ToXElement(PayProduct obj)
        {
            return new XElement("rec",
                new XElement("IdCheckTicket", obj.IdCheckTicket),
                new XElement("ProductId", obj.ProductId),
                new XElement("Name", obj.Name),
                new XElement("Barcode", obj.Barcode),
                new XElement("Qty", obj.Qty),
                new XElement("Tva", obj.Tva),
                new XElement("PriceHt", obj.PriceHt),
                new XElement("Total", obj.Total),
                new XElement("ChecksTicketCustomerId", obj.ChecksTicketCustomerId),
                new XElement("ChecksTicketCloseTicketCustomerId", obj.ChecksTicketCloseTicketCustomerId),
                new XElement("Discount", obj.Discount),
                new XElement("SumDiscount", obj.SumDiscount));
        }
    }
}