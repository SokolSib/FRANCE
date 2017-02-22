using System;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class Currency
    {
        public Currency(Guid customerId, decimal currencyMoney, string desc, int typesPayId)
        {
            CustomerId = customerId;
            CurrencyMoney = currencyMoney;
            Desc = desc;
            TypesPayId = typesPayId;
            TypesPay = RepositoryTypePay.TypePays.Find(tp => tp.Id == typesPayId);
        }

        public Guid CustomerId { get; set; }
        public decimal CurrencyMoney { get; set; }
        public string Desc { get; set; }
        public int TypesPayId { get; set; }
        public TypePay TypesPay { get; set; }

        public static Currency FromXElement(XContainer element)
        {
            return new Currency(
                element.GetXElementValue("CustomerId").ToGuid(),
                element.GetXElementValue("Currency_money").ToDecimal(),
                element.GetXElementValue("Desc"),
                element.GetXElementValue("TypesPayId").ToInt());
        }

        public static XElement ToXElement(Currency obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Currency_money", obj.CurrencyMoney),
                new XElement("Desc", obj.Desc),
                new XElement("TypesPayId", obj.TypesPayId)
                );
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}