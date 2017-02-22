using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TicketWindow.DAL.Models
{
    public class CheckType
    {
        public CheckType(string barcodeCheck, decimal cash, decimal bankCards, decimal bankChecks, decimal resto,
            decimal bondAchat, decimal rendu, decimal sum, DateTime date)
        {
            BarcodeCheck = barcodeCheck;
            Cash = cash;
            BankCards = bankCards;
            BankChecks = bankChecks;
            Resto = resto;
            BondAchat = bondAchat;
            Rendu = rendu;
            Sum = sum;
            Date = date;
            Products = new List<ProductType>();
        }

        public string BarcodeCheck { get; }
        public decimal Cash { get; }
        public decimal BankCards { get; }
        public decimal BankChecks { get; }
        public decimal Resto { get; }
        public decimal BondAchat { get; }
        public decimal Rendu { get; }
        public decimal Sum { get; }
        public DateTime Date { get; }
        public List<ProductType> Products { get; }

        public static XElement ToXElement(CheckType obj)
        {
            var element = new XElement("check",
                              new XAttribute("barcodeCheck", obj.BarcodeCheck),
                              new XAttribute("Cash", obj.Cash),
                              new XAttribute("BankCards", obj.BankCards),
                              new XAttribute("BankChecks", obj.BankChecks),
                              new XAttribute("Resto", obj.Resto),
                              new XAttribute("BondAchat", obj.BondAchat),
                              new XAttribute("Rendu", obj.Rendu),
                              new XAttribute("sum", obj.Sum),
                              new XAttribute("date", obj.Date));

            foreach (var product in obj.Products)
                element.Add(ProductType.ToCheckXElement(product, null));

            return element;
        }
    }
}