using System;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models.Base
{
    public class PayTicketBase
    {
        private static readonly string[] PayXmlNames =
        {
            "BankChecks", "BankCards", "Cash", "Resto", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15",
            "16", "17", "18", "19", "20"
        };

        protected PayTicketBase(Guid customerId, decimal payBankChecks, decimal payBankCards, decimal payCash, decimal payResto, decimal pay1, decimal pay2, decimal pay3,
            decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10, decimal pay11, decimal pay12, decimal pay13, decimal pay14,
            decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20)
        {
            CustomerId = customerId;
            PayBankChecks = payBankChecks;
            PayBankCards = payBankCards;
            PayCash = payCash;
            PayResto = payResto;
            Pay1 = pay1;
            Pay2 = pay2;
            Pay3 = pay3;
            Pay4 = pay4;
            Pay5 = pay5;
            Pay6 = pay6;
            Pay7 = pay7;
            Pay8 = pay8;
            Pay9 = pay9;
            Pay10 = pay10;
            Pay11 = pay11;
            Pay12 = pay12;
            Pay13 = pay13;
            Pay14 = pay14;
            Pay15 = pay15;
            Pay16 = pay16;
            Pay17 = pay17;
            Pay18 = pay18;
            Pay19 = pay19;
            Pay20 = pay20;
        }

        public Guid CustomerId { get; set; }
        public decimal PayBankChecks { get; set; }
        public decimal PayBankCards { get; set; }
        public decimal PayCash { get; set; }
        public decimal PayResto { get; set; }
        public decimal Pay1 { get; set; }
        public decimal Pay2 { get; set; }
        public decimal Pay3 { get; set; }
        public decimal Pay4 { get; set; }
        public decimal Pay5 { get; set; }
        public decimal Pay6 { get; set; }
        public decimal Pay7 { get; set; }
        public decimal Pay8 { get; set; }
        public decimal Pay9 { get; set; }
        public decimal Pay10 { get; set; }
        public decimal Pay11 { get; set; }
        public decimal Pay12 { get; set; }
        public decimal Pay13 { get; set; }
        public decimal Pay14 { get; set; }
        public decimal Pay15 { get; set; }
        public decimal Pay16 { get; set; }
        public decimal Pay17 { get; set; }
        public decimal Pay18 { get; set; }
        public decimal Pay19 { get; set; }
        public decimal Pay20 { get; set; }

        public static PayTicketBase FromXElement(XContainer element, Guid customerId)
        {
            var payValues = new decimal[PayXmlNames.Length];
            var idx = 0;
            foreach (var attribute in PayXmlNames.Select(name => element.GetXAttributeOrNull(name)))
                payValues[idx++] = attribute != null ? attribute.Value.ToDecimal() : 0;

            return new PayTicketBase(customerId,
                payValues[0],
                payValues[1],
                payValues[2],
                payValues[3],
                payValues[4],
                payValues[5],
                payValues[6],
                payValues[7],
                payValues[8],
                payValues[9],
                payValues[10],
                payValues[11],
                payValues[12],
                payValues[13],
                payValues[14],
                payValues[15],
                payValues[16],
                payValues[17],
                payValues[18],
                payValues[19],
                payValues[20],
                payValues[21],
                payValues[22],
                payValues[23]);
        }

        public static PayTicketBase FromXElementBase(XContainer element)
        {
            var payValues = new decimal[PayXmlNames.Length];
            var idx = 0;
            foreach (var attribute in PayXmlNames.Select(name => element.GetXAttributeOrNull("Pay" + name)))
                payValues[idx++] = attribute != null ? attribute.Value.ToDecimal() : 0;

            return new PayTicketBase(
                element.GetXElementValue("CustomerId").ToGuid(),
                payValues[0],
                payValues[1],
                payValues[2],
                payValues[3],
                payValues[4],
                payValues[5],
                payValues[6],
                payValues[7],
                payValues[8],
                payValues[9],
                payValues[10],
                payValues[11],
                payValues[12],
                payValues[13],
                payValues[14],
                payValues[15],
                payValues[16],
                payValues[17],
                payValues[18],
                payValues[19],
                payValues[20],
                payValues[21],
                payValues[22],
                payValues[23]);
        }

        public static XElement ToXElementBase(PayTicketBase obj)
        {
            return new XElement("rec",
                new XElement("CustomerId", obj.CustomerId),
                new XElement("Pay" + PayXmlNames[0], obj.PayBankChecks),
                new XElement("Pay" + PayXmlNames[1], obj.PayBankCards),
                new XElement("Pay" + PayXmlNames[2], obj.PayCash),
                new XElement("Pay" + PayXmlNames[3], obj.PayResto),
                new XElement("Pay" + PayXmlNames[4], obj.Pay1),
                new XElement("Pay" + PayXmlNames[5], obj.Pay2),
                new XElement("Pay" + PayXmlNames[6], obj.Pay3),
                new XElement("Pay" + PayXmlNames[7], obj.Pay4),
                new XElement("Pay" + PayXmlNames[8], obj.Pay5),
                new XElement("Pay" + PayXmlNames[9], obj.Pay6),
                new XElement("Pay" + PayXmlNames[10], obj.Pay7),
                new XElement("Pay" + PayXmlNames[11], obj.Pay8),
                new XElement("Pay" + PayXmlNames[12], obj.Pay9),
                new XElement("Pay" + PayXmlNames[13], obj.Pay10),
                new XElement("Pay" + PayXmlNames[14], obj.Pay11),
                new XElement("Pay" + PayXmlNames[15], obj.Pay12),
                new XElement("Pay" + PayXmlNames[16], obj.Pay13),
                new XElement("Pay" + PayXmlNames[17], obj.Pay14),
                new XElement("Pay" + PayXmlNames[18], obj.Pay15),
                new XElement("Pay" + PayXmlNames[19], obj.Pay16),
                new XElement("Pay" + PayXmlNames[20], obj.Pay17),
                new XElement("Pay" + PayXmlNames[21], obj.Pay18),
                new XElement("Pay" + PayXmlNames[22], obj.Pay19),
                new XElement("Pay" + PayXmlNames[23], obj.Pay20));
        }

        public static void SetXmlValuesBase(XContainer element, PayTicketBase obj)
        {
            element.GetXElement("CustomerId").SetValue(obj.CustomerId);
            element.GetXElement("Pay" + PayXmlNames[0]).SetValue(obj.PayBankChecks);
            element.GetXElement("Pay" + PayXmlNames[1]).SetValue(obj.PayBankCards);
            element.GetXElement("Pay" + PayXmlNames[2]).SetValue(obj.PayCash);
            element.GetXElement("Pay" + PayXmlNames[3]).SetValue(obj.PayResto);
            element.GetXElement("Pay" + PayXmlNames[4]).SetValue(obj.Pay1);
            element.GetXElement("Pay" + PayXmlNames[5]).SetValue(obj.Pay2);
            element.GetXElement("Pay" + PayXmlNames[6]).SetValue(obj.Pay3);
            element.GetXElement("Pay" + PayXmlNames[7]).SetValue(obj.Pay4);
            element.GetXElement("Pay" + PayXmlNames[8]).SetValue(obj.Pay5);
            element.GetXElement("Pay" + PayXmlNames[9]).SetValue(obj.Pay6);
            element.GetXElement("Pay" + PayXmlNames[10]).SetValue(obj.Pay7);
            element.GetXElement("Pay" + PayXmlNames[11]).SetValue(obj.Pay8);
            element.GetXElement("Pay" + PayXmlNames[12]).SetValue(obj.Pay9);
            element.GetXElement("Pay" + PayXmlNames[13]).SetValue(obj.Pay10);
            element.GetXElement("Pay" + PayXmlNames[14]).SetValue(obj.Pay11);
            element.GetXElement("Pay" + PayXmlNames[15]).SetValue(obj.Pay12);
            element.GetXElement("Pay" + PayXmlNames[16]).SetValue(obj.Pay13);
            element.GetXElement("Pay" + PayXmlNames[17]).SetValue(obj.Pay14);
            element.GetXElement("Pay" + PayXmlNames[18]).SetValue(obj.Pay15);
            element.GetXElement("Pay" + PayXmlNames[19]).SetValue(obj.Pay16);
            element.GetXElement("Pay" + PayXmlNames[20]).SetValue(obj.Pay17);
            element.GetXElement("Pay" + PayXmlNames[21]).SetValue(obj.Pay18);
            element.GetXElement("Pay" + PayXmlNames[22]).SetValue(obj.Pay19);
            element.GetXElement("Pay" + PayXmlNames[23]).SetValue(obj.Pay20);
        }

        public static XElement SetPaysAttributesBase(XElement checkElement, CheckTicket check)
        {
            foreach (var type in RepositoryTypePay.TypePays)
            {
                decimal val = 0;

                switch (type.NameCourt)
                {
                    case "BankChecks":
                        val = check.PayBankChecks;
                        break;
                    case "BankCards":
                        val = check.PayBankCards;
                        break;
                    case "Cash":
                        val = check.PayCash;
                        break;
                    case "Resto":
                        val = check.PayResto;
                        break;
                    case "1":
                        val = check.Pay1;
                        break;
                    case "2":
                        val = check.Pay2;
                        break;
                    case "3":
                        val = check.Pay3;
                        break;
                    case "4":
                        val = check.Pay4;
                        break;
                    case "5":
                        val = check.Pay5;
                        break;
                    case "6":
                        val = check.Pay6;
                        break;
                    case "7":
                        val = check.Pay7;
                        break;
                    case "8":
                        val = check.Pay8;
                        break;
                    case "9":
                        val = check.Pay9;
                        break;
                    case "10":
                        val = check.Pay10;
                        break;
                    case "11":
                        val = check.Pay11;
                        break;
                    case "12":
                        val = check.Pay12;
                        break;
                    case "13":
                        val = check.Pay13;
                        break;
                    case "14":
                        val = check.Pay14;
                        break;
                    case "15":
                        val = check.Pay15;
                        break;
                    case "16":
                        val = check.Pay16;
                        break;
                    case "17":
                        val = check.Pay17;
                        break;
                    case "18":
                        val = check.Pay18;
                        break;
                    case "19":
                        val = check.Pay19;
                        break;
                    case "20":
                        val = check.Pay20;
                        break;
                }

                if (val != 0) checkElement.Add(new XAttribute(type.NameCourt.Trim(), val));
            }
            return checkElement;
        }
    }
}