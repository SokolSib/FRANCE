using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class CheckTicketTmp : CheckTicketBase
    {
        public CheckTicketTmp(Guid customerId, string barCode, DateTime date, decimal payBankChecks, decimal payBankCards, decimal payCash, decimal payResto, decimal pay1,
            decimal pay2, decimal pay3, decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10, decimal pay11, decimal pay12,
            decimal pay13, decimal pay14, decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20, Guid closeTicketCustomerId, decimal totalTtc,
            decimal rendu, string dcbc, int dcbcBiloPoints, int dcbcDobavilePoints, int dcbcOtnayliPoints, int dcbcOstalosPoints, string dcbcName)
            : base(
                customerId, barCode, date, payBankChecks, payBankCards, payCash, payResto, pay1, pay2, pay3, pay4, pay5, pay6, pay7, pay8, pay9, pay10, pay11, pay12, pay13, pay14,
                pay15, pay16, pay17, pay18, pay19, pay20, closeTicketCustomerId, totalTtc, rendu)
        {
            Dcbc = dcbc;
            DcbcBiloPoints = dcbcBiloPoints;
            DcbcDobavilePoints = dcbcDobavilePoints;
            DcbcOtnayliPoints = dcbcOtnayliPoints;
            DcbcOstalosPoints = dcbcOstalosPoints;
            DcbcName = dcbcName;

            PayProducts = new List<PayProductTmp>();
        }

        public CheckTicketTmp(PayTicketBase payTicket):this(payTicket.CustomerId, null, DateTime.Now, payTicket.PayBankChecks, payTicket.PayBankCards, payTicket.PayCash, payTicket.PayResto, payTicket.Pay1,payTicket.Pay2, payTicket.Pay3, payTicket.Pay4, payTicket.Pay5, payTicket.Pay6, payTicket.Pay7, payTicket.Pay8, payTicket.Pay9, payTicket.Pay10, payTicket.Pay11, payTicket.Pay12,payTicket.Pay13, payTicket.Pay14, payTicket.Pay15, payTicket.Pay16, payTicket.Pay17, payTicket.Pay18, payTicket.Pay19, payTicket.Pay20, Guid.Empty, 0,0, null, 0, 0, 0, 0, null)
        {
        }

        public string Dcbc { get; set; }
        public int DcbcBiloPoints { get; set; }
        public int DcbcDobavilePoints { get; set; }
        public int DcbcOtnayliPoints { get; set; }
        public int DcbcOstalosPoints { get; set; }
        public string DcbcName { get; set; }
        public List<PayProductTmp> PayProducts { get; set; }

        public static CheckTicketTmp FromCheckXElement(XElement element, Guid customerId, Guid closeTicketCustomerId)
        {
            var checkTicket = new CheckTicketTmp(FromXElement(element, customerId))
            {
                BarCode = element.GetXAttributeValue("barcodeCheck"),
                Date = element.GetXAttributeValue("date").ToDateTime(),
                CloseTicketCustomerId = closeTicketCustomerId,
                TotalTtc = element.GetXAttributeValue("sum").ToDecimal(),
                Rendu = element.GetXAttributeValue("Rendu").ToDecimal()
            };

            if (element.Attribute("DCBC") != null)
            {
                checkTicket.Dcbc = element.GetXAttributeValue("DCBC");
                checkTicket.DcbcBiloPoints = element.GetXAttributeValue("DCBC_BiloPoints").ToInt();
                checkTicket.DcbcDobavilePoints = element.GetXAttributeValue("DCBC_DobavilePoints").ToInt();
                checkTicket.DcbcOtnayliPoints = element.GetXAttributeValue("DCBC_OtnayliPoints").ToInt();
                checkTicket.DcbcOstalosPoints = element.GetXAttributeValue("DCBC_OstalosPoints").ToInt();
                checkTicket.DcbcName = element.GetXAttributeValue("DCBC_name");
            }

            foreach (var el in element.Elements("product"))
                checkTicket.PayProducts.Add(PayProductTmp.FromCheckXElement(el, Guid.NewGuid(), checkTicket.CustomerId));

            return checkTicket;
        }

        public static CheckTicketTmp FromXElement(XContainer element)
        {
            var checkTicket = new CheckTicketTmp(FromXElementBase(element))
            {
                BarCode = element.GetXElementValue("BarCode"),
                Date = element.GetXElementValue("Date").ToDateTime(),
                CloseTicketCustomerId = element.GetXElementValue("CloseTicketCustomerId").ToGuid(),
                TotalTtc = element.GetXElementValue("TotalTtc").ToDecimal(),
                Rendu = element.GetXElementValue("Rendu").ToDecimal(),
                Dcbc = element.GetXElementValue("Dcbc"),
                DcbcBiloPoints = element.GetXElementValue("DcbcBiloPoints").ToInt(),
                DcbcDobavilePoints = element.GetXElementValue("DcbcDobavilePoints").ToInt(),
                DcbcOtnayliPoints = element.GetXElementValue("DcbcOtnayliPoints").ToInt(),
                DcbcOstalosPoints = element.GetXElementValue("DcbcOstalosPoints").ToInt(),
                DcbcName = element.GetXElementValue("DcbcName")
            };

            return checkTicket;
        }

        public static XElement ToXElement(CheckTicketTmp obj)
        {
            var element = ToXElementBase(obj);
            element.Add(new XElement("BarCode", obj.BarCode));
            element.Add(new XElement("Date", obj.Date));
            element.Add(new XElement("CloseTicketCustomerId", obj.CloseTicketCustomerId));
            element.Add(new XElement("TotalTtc", obj.TotalTtc));
            element.Add(new XElement("Rendu", obj.Rendu));
            element.Add(new XElement("Dcbc", obj.Dcbc));
            element.Add(new XElement("DcbcBiloPoints", obj.DcbcBiloPoints));
            element.Add(new XElement("DcbcDobavilePoints", obj.DcbcDobavilePoints));
            element.Add(new XElement("DcbcOtnayliPoints", obj.DcbcOtnayliPoints));
            element.Add(new XElement("DcbcOstalosPoints", obj.DcbcOstalosPoints));
            element.Add(new XElement("DcbcName", obj.DcbcName));
            return element;
        }
    }
}