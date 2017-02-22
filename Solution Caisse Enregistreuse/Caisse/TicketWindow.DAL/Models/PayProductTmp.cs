using System;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class PayProductTmp : PayProductBase
    {
        public PayProductTmp(Guid idCheckTicket, Guid productId, string name, string barcode, decimal qty, decimal tva, decimal priceHt, decimal total, Guid checksTicketCustomerId,
            decimal discount, decimal sumDiscount) : base(idCheckTicket, productId, name, barcode, qty, tva, priceHt, total, checksTicketCustomerId, discount, sumDiscount)
        {
        }

        public static PayProductTmp FromCheckXElement(XElement element, Guid customerId, Guid checksTicketCustomerId)
        {
            return new PayProductTmp(
                customerId,
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("CodeBare"),
                element.GetXElementValue("qty").ToDecimal(),
                RepositoryTva.GetById(element.GetXElementValue("tva").ToInt()),
                element.GetXElementValue("price").ToDecimal(),
                element.GetXElementValue("total").ToDecimal(),
                checksTicketCustomerId,
                0,
                0);
        }

        public static PayProductTmp FromXElement(XContainer element)
        {
            return new PayProductTmp(
                element.GetXElementValue("IdCheckTicket").ToGuid(),
                element.GetXElementValue("ProductId").ToGuid(),
                element.GetXElementValue("Name"),
                element.GetXElementValue("Barcode"),
                element.GetXElementValue("Qty").ToDecimal(),
                element.GetXElementValue("Tva").ToDecimal(),
                element.GetXElementValue("PriceHt").ToDecimal(),
                element.GetXElementValue("Total").ToDecimal(),
                element.GetXElementValue("ChecksTicketCustomerId").ToGuid(),
                element.GetXElementValue("Discount").ToDecimal(),
                element.GetXElementValue("SumDiscount").ToDecimal());
        }

        public static XElement ToXElement(PayProductTmp obj)
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
                new XElement("Discount", obj.Discount),
                new XElement("SumDiscount", obj.SumDiscount));
        }
    }
}