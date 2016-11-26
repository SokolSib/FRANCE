using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class CloseTicketCheckDiscount
    {
        public CloseTicketCheckDiscount(Guid customerId, Guid closeTicketCheckcCustomer, Guid discountCardsCustomerId, string dcbc, int? dcbcBiloPoints, int? dcbcDobavilePoints,
            int? dcbcOtnayliPoints, int? dcbcOstalosPoints, string dcbcname)
        {
            CustomerId = customerId;
            CloseTicketCheckcCustomer = closeTicketCheckcCustomer;
            DiscountCardsCustomerId = discountCardsCustomerId;
            Dcbc = dcbc;
            DcbcBiloPoints = dcbcBiloPoints;
            DcbcDobavilePoints = dcbcDobavilePoints;
            DcbcOtnayliPoints = dcbcOtnayliPoints;
            DcbcOstalosPoints = dcbcOstalosPoints;
            DcbcName = dcbcname;
        }

        public Guid CustomerId { get; set; }
        public Guid CloseTicketCheckcCustomer { get; set; }
        public Guid DiscountCardsCustomerId { get; set; }
        public string Dcbc { get; set; }
        public int? DcbcBiloPoints { get; set; }
        public int? DcbcDobavilePoints { get; set; }
        public int? DcbcOtnayliPoints { get; set; }
        public int? DcbcOstalosPoints { get; set; }
        public string DcbcName { get; set; }

        public static CloseTicketCheckDiscount FromXElement(XElement element, Guid customerId, Guid closeTicketCheckcCustomer)
        {
            return new CloseTicketCheckDiscount(customerId,
                closeTicketCheckcCustomer,
                Guid.Empty,
                element.Attribute("DCBC").Value,
                element.GetXAttributeValue("DCBC_BiloPoints").ToInt(),
                element.GetXAttributeValue("DCBC_DobavilePoints").ToInt(),
                element.GetXAttributeValue("DCBC_OtnayliPoints").ToInt(),
                element.GetXAttributeValue("DCBC_OstalosPoints").ToInt(),
                element.GetXAttributeValue("DCBC_name"));
        }

        public static CloseTicketCheckDiscount FromXElement(XContainer element)
        {
            return new CloseTicketCheckDiscount(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("CloseTicketCheckcCustomer").ToGuid(),
                element.GetXElementValue("DiscountCardsCustomerId").ToGuid(),
                element.GetXElementValue("Dcbc"),
                element.GetXElementValue("DcbcBiloPoints").ToNullableInt(),
                element.GetXElementValue("DcbcDobavilePoints").ToNullableInt(),
                element.GetXElementValue("DcbcOtnayliPoints").ToNullableInt(),
                element.GetXElementValue("DcbcOstalosPoints").ToNullableInt(),
                element.GetXElementValue("DcbcName"));
        }

        public static XElement ToXElement(CloseTicketCheckDiscount obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("CloseTicketCheckcCustomer", obj.CloseTicketCheckcCustomer),
                new XElement("DiscountCardsCustomerId", obj.DiscountCardsCustomerId),
                new XElement("Dcbc", obj.Dcbc),
                new XElement("DcbcBiloPoints", obj.DcbcBiloPoints),
                new XElement("DcbcDobavilePoints", obj.DcbcDobavilePoints),
                new XElement("DcbcOtnayliPoints", obj.DcbcOtnayliPoints),
                new XElement("DcbcOstalosPoints", obj.DcbcOstalosPoints),
                new XElement("DcbcName", obj.DcbcName));
        }

        public override string ToString()
        {
            return Dcbc;
        }
    }
}