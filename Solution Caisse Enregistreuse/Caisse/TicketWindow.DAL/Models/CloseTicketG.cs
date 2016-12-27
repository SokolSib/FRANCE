using System;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Models.Base;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Models
{
    public class CloseTicketG : CloseTicketBase
    {
        private static readonly DateTime DefoultDateClose = new DateTime(2014, 1, 1);

        public CloseTicketG(Guid customerId, DateTime dateOpen, DateTime dateClose, decimal payBankChecks, decimal payBankCards, decimal payCash, decimal payResto, decimal pay1,
            decimal pay2, decimal pay3, decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10, decimal pay11, decimal pay12,
            decimal pay13, decimal pay14, decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20, Guid establishmentCustomerId)
            : base(
                customerId, dateOpen, dateClose, payBankChecks, payBankCards, payCash, payResto, pay1, pay2, pay3, pay4, pay5, pay6, pay7, pay8, pay9, pay10, pay11, pay12, pay13,
                pay14, pay15, pay16, pay17, pay18, pay19, pay20)
        {
            EstablishmentCustomerId = establishmentCustomerId;
        }

        public CloseTicketG(Guid customerId, DateTime dateOpen, Guid establishmentCustomerId)
            : base(customerId, dateOpen, DefoultDateClose, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        {
            SetPaysToNull();
            EstablishmentCustomerId = establishmentCustomerId;
        }

        public CloseTicketG(PayTicketBase payTicket)
            : this(
                 payTicket.CustomerId, DateTime.Now, DateTime.Now, payTicket.PayBankChecks, payTicket.PayBankCards, payTicket.PayCash, payTicket.PayResto, payTicket.Pay1, payTicket.Pay2,
                 payTicket.Pay3, payTicket.Pay4, payTicket.Pay5, payTicket.Pay6, payTicket.Pay7, payTicket.Pay8, payTicket.Pay9, payTicket.Pay10, payTicket.Pay11, payTicket.Pay12,
                 payTicket.Pay13, payTicket.Pay14, payTicket.Pay15, payTicket.Pay16, payTicket.Pay17, payTicket.Pay18, payTicket.Pay19, payTicket.Pay20, Guid.Empty)
        {
        }

        public Guid EstablishmentCustomerId { get; set; }

        public void SetPaysToNull()
        {
            // Прям эта дата ?
            DateClose =  DefoultDateClose;
            PayBankChecks = 0;
            PayBankCards = 0;
            PayCash = 0;
            PayResto = 0;
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
        }

        public string CassieName
        {
            get
            {
                return
                    RepositoryEstablishment.Establishments.FirstOrDefault(e => e.CustomerId == EstablishmentCustomerId)?
                        .Name;
            }
        }

        public static CloseTicketG FromXElement(XContainer element)
        {
            var closeTicketG = new CloseTicketG(FromXElementBase(element))
            {
                EstablishmentCustomerId = element.GetXElementValue("EstablishmentCustomerId").ToGuid(),
                DateOpen = element.GetXElementValue("DateOpen").ToDateTime(),
                DateClose = element.GetXElementValue("DateClose").ToDateTime()
            };

            return closeTicketG;
        }

        public static XElement ToXElement(CloseTicketG obj)
        {
            var element = ToXElementBase(obj);
            element.Add(new XElement("EstablishmentCustomerId", obj.EstablishmentCustomerId));
            element.Add(new XElement("DateOpen", obj.DateOpen));
            element.Add(new XElement("DateClose", obj.DateClose));
            return element;
        }

        public static void SetXmlValues(XContainer element, CloseTicketG obj)
        {
            SetXmlValuesBase(element, obj);
            element.GetXElement("EstablishmentCustomerId").SetValue(obj.EstablishmentCustomerId);
            element.GetXElement("DateOpen").SetValue(obj.DateOpen);
            element.GetXElement("DateClose").SetValue(obj.DateClose);
        }

        public override string ToString()
        {
            return CustomerId.ToString();
        }
    }
}