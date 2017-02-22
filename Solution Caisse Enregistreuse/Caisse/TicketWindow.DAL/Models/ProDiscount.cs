using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class ProDiscount
    {
        public ProDiscount(Guid customerId, Guid customerIdInfoClients, int idInfoClientsDiscountsType)
        {
            CustomerId = customerId;
            CustomerIdInfoClients = customerIdInfoClients;
            IdInfoClientsDiscountsType = idInfoClientsDiscountsType;
        }

        public Guid CustomerId { get; set; }
        public Guid CustomerIdInfoClients { get; set; }
        public int IdInfoClientsDiscountsType { get; set; }

        public static ProDiscount FromXElement(XContainer element)
        {
            return new ProDiscount(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("CustomerIdInfoClients").ToGuid(),
                element.GetXElementValue("IdInfoClientsDiscountsType").ToInt());
        }

        public static XElement ToXElement(ProDiscount obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("CustomerIdInfoClients", obj.CustomerIdInfoClients),
                new XElement("IdInfoClientsDiscountsType", obj.IdInfoClientsDiscountsType));
        }

    }

}
