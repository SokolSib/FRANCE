﻿using System;
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

            closeTicket.PayCash = 0;
            closeTicket.PayResto = 0;
            closeTicket.PayBankCards = 0;
            closeTicket.PayBankChecks = 0;

            foreach (var checkTicket in element.GetXElements("check").Select(check => CheckTicket.FromCheckXElement(check, Guid.NewGuid(), closeTicket.CustomerId)))
            {
                closeTicket.PayCash += checkTicket.PayCash + checkTicket.Rendu;
                closeTicket.PayResto += checkTicket.PayResto;
                closeTicket.PayBankCards += checkTicket.PayBankCards;
                closeTicket.PayBankChecks += checkTicket.PayBankChecks;
                closeTicket.ChecksTicket.Add(checkTicket);
            }

            return closeTicket;
        }

        public static CloseTicket FromXElement(XContainer element)
        {
            var closeTicket = new CloseTicket(FromXElementBase(element))
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