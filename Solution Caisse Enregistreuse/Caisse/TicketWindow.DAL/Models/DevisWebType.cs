using System;
using System.Xml.Linq;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class DevisWebType
    {
        public DevisWebType(Guid customerId, int idDevis, decimal prixHt, decimal monPrixHt, decimal qty,
            decimal totalHt, short payementType, bool operatorData, Guid productsCustomerId,
            Guid infoClientsCustomerId)
        {
            CustomerId = customerId;
            IdDevis = idDevis;
            PrixHt = prixHt;
            MonPrixHt = monPrixHt;
            Qty = qty;
            TotalHt = totalHt;
            PayementType = payementType;
            Operator = operatorData;
            ProductsCustomerId = productsCustomerId;
            InfoClientsCustomerId = infoClientsCustomerId;
        }

        public Guid CustomerId { get; set; }
        public int IdDevis { get; set; }
        public decimal PrixHt { get; set; }
        public decimal MonPrixHt { get; set; }
        public decimal Qty { get; set; }
        public decimal TotalHt { get; set; }
        public short PayementType { get; set; }
        public bool Operator { get; set; }
        public Guid ProductsCustomerId { get; set; }
        public Guid InfoClientsCustomerId { get; set; }

        public static DevisWebType FromXElement(XContainer element)
        {
            return new DevisWebType(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("IdDevis").ToInt(),
                element.GetXElementValue("PrixHt").ToDecimal(),
                element.GetXElementValue("MonPrixHt").ToDecimal(),
                element.GetXElementValue("Qty").ToDecimal(),
                element.GetXElementValue("TotalHt").ToDecimal(),
                element.GetXElementValue("PayementType").ToShort(),
                element.GetXElementValue("Operator").ToBool(),
                element.GetXElementValue("ProductsCustomerId").ToGuid(),
                element.GetXElementValue("InfoClientsCustomerId").ToGuid());
        }

        public static XElement ToXElement(DevisWebType obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("IdDevis", obj.IdDevis),
                new XElement("PrixHt", obj.PrixHt),
                new XElement("MonPrixHt", obj.MonPrixHt),
                new XElement("Qty", obj.Qty),
                new XElement("TotalHt", obj.TotalHt),
                new XElement("PayementType", obj.PayementType),
                new XElement("Operator", obj.Operator),
                new XElement("ProductsCustomerId", obj.ProductsCustomerId),
                new XElement("InfoClientsCustomerId", obj.InfoClientsCustomerId));
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}
