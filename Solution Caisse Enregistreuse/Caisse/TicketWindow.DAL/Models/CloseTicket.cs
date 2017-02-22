using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Models
{
    public class CloseTicket : CloseTicketBase
    {
        public CloseTicket(Guid customerId, string nameTicket, DateTime dateOpen, DateTime dateClose, decimal payBankChecks, decimal payBankCards, decimal payCash,
            decimal payResto, decimal pay1, decimal pay2, decimal pay3, decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10,
            decimal pay11, decimal pay12, decimal pay13, decimal pay14, decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20,
            Guid closeTicketGCustomerId)
            : base(
                customerId, dateOpen, dateClose, payBankChecks, payBankCards, payCash, payResto, pay1, pay2, pay3, pay4, pay5, pay6, pay7, pay8, pay9, pay10, pay11, pay12, pay13,
                pay14, pay15, pay16, pay17, pay18, pay19, pay20)
        {
            NameTicket = nameTicket;
            CloseTicketGCustomerId = closeTicketGCustomerId;

            ChecksTicket = new List<CheckTicket>();
        }

        public CloseTicket(PayTicketBase payTicket)
            : this(
                payTicket.CustomerId, null, DateTime.Now, DateTime.Now, payTicket.PayBankChecks, payTicket.PayBankCards, payTicket.PayCash, payTicket.PayResto, payTicket.Pay1,
                payTicket.Pay2, payTicket.Pay3, payTicket.Pay4, payTicket.Pay5, payTicket.Pay6, payTicket.Pay7, payTicket.Pay8, payTicket.Pay9, payTicket.Pay10, payTicket.Pay11,
                payTicket.Pay12, payTicket.Pay13, payTicket.Pay14, payTicket.Pay15, payTicket.Pay16, payTicket.Pay17, payTicket.Pay18, payTicket.Pay19, payTicket.Pay20,
                Guid.Empty)
        {
            ChecksTicket = new List<CheckTicket>();
        }

        public void SetPaysFromChecks()
        {
            PayCash = 0;
            PayResto = 0;
            PayBankCards = 0;
            PayBankChecks = 0;
            Pay1 = 0;
            Pay2 = 0;
            Pay3 = 0;
            Pay4 = 0;
            Pay5 = 0;
            Pay6 = 0;
            Pay7 = 0;
            Pay8 = 0;
            Pay9 = 0;
            Pay10 = 0;
            Pay11 = 0;
            Pay12 = 0;
            Pay13 = 0;
            Pay14 = 0;
            Pay15 = 0;
            Pay16 = 0;
            Pay17 = 0;
            Pay18 = 0;
            Pay19 = 0;
            Pay20 = 0;

            foreach (var check in ChecksTicket)
            {
                PayCash += check.PayCash - check.Rendu;
                PayResto += check.PayResto;
                PayBankCards += check.PayBankCards;
                PayBankChecks += check.PayBankChecks;
                Pay1 += check.Pay1;
                Pay2 += check.Pay2;
                Pay3 += check.Pay3;
                Pay4 += check.Pay4;
                Pay5 += check.Pay5;
                Pay6 += check.Pay6;
                Pay7 += check.Pay7;
                Pay8 += check.Pay8;
                Pay9 += check.Pay9;
                Pay10 += check.Pay10;
                Pay11 += check.Pay11;
                Pay12 += check.Pay12;
                Pay13 += check.Pay13;
                Pay14 += check.Pay14;
                Pay15 += check.Pay15;
                Pay16 += check.Pay16;
                Pay17 += check.Pay17;
                Pay18 += check.Pay18;
                Pay19 += check.Pay19;
                Pay20 += check.Pay20;
            }
        }

        public string NameTicket { get; set; }
        public Guid CloseTicketGCustomerId { get; set; }
        public List<CheckTicket> ChecksTicket { get; set; }

        public static CloseTicket FromCheckXElement(XElement element)
        {
            var customerId = GlobalVar.TicketWindow;
            var closeTicket = new CloseTicket(FromXElement(element.GetXElement("check"), customerId))
                              {
                                  NameTicket = element.GetXAttributeValue("ticket"),
                                  DateOpen = element.GetXAttributeValue("openDate").ToDateTime(),
                                  DateClose = !string.IsNullOrEmpty(element.GetXAttributeValue("closeDate")) ? element.GetXAttributeValue("closeDate").ToDateTime() : DateTime.Now,
                                  CloseTicketGCustomerId = GlobalVar.TicketWindowG
                              };
            
            foreach (var checkTicket in element.GetXElements("check").Select(check => CheckTicket.FromCheckXElement(check, Guid.NewGuid(), closeTicket.CustomerId)))
            {
                closeTicket.ChecksTicket.Add(checkTicket);
            }

            closeTicket.SetPaysFromChecks();

            return closeTicket;
        }

        public static CloseTicket FromXElement(XContainer element)
        {
            var closeTicket = new CloseTicket(FromXElementElBase(element))
                              {
                                  DateOpen = element.GetXElementValue("DateOpen").ToDateTime(),
                                  DateClose = element.GetXElementValue("DateClose").ToDateTime(),
                                  NameTicket = element.GetXElementValue("NameTicket"),
                                  CloseTicketGCustomerId = element.GetXElementValue("CloseTicketGCustomerId").ToGuid()
                              };

            return closeTicket;
        }

        public static XElement ToXElement(CloseTicket obj)
        {
            var element = ToXElementBase(obj);
            element.Add(new XElement("DateOpen", obj.DateOpen));
            element.Add(new XElement("DateClose", obj.DateClose));
            element.Add(new XElement("NameTicket", obj.NameTicket));
            element.Add(new XElement("CloseTicketGCustomerId", obj.CloseTicketGCustomerId));
            return element;
        }

        public override string ToString()
        {
            return NameTicket;
        }
    }
}