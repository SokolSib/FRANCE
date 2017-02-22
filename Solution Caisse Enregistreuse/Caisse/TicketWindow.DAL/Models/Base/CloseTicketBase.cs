using System;

namespace TicketWindow.DAL.Models.Base
{
    public abstract class CloseTicketBase : PayTicketBase
    {
        protected CloseTicketBase(Guid customerId, DateTime dateOpen, DateTime dateClose, decimal payBankChecks, decimal payBankCards, decimal payCash, decimal payResto,
            decimal pay1, decimal pay2, decimal pay3, decimal pay4, decimal pay5, decimal pay6, decimal pay7, decimal pay8, decimal pay9, decimal pay10, decimal pay11,
            decimal pay12, decimal pay13, decimal pay14, decimal pay15, decimal pay16, decimal pay17, decimal pay18, decimal pay19, decimal pay20)
            : base(
                customerId, payBankChecks, payBankCards, payCash, payResto, pay1, pay2, pay3, pay4, pay5, pay6, pay7, pay8, pay9, pay10, pay11, pay12, pay13, pay14, pay15, pay16,
                pay17, pay18, pay19, pay20)
        {
            DateOpen = dateOpen;
            DateClose = dateClose;
        }

        public DateTime DateOpen { get; set; }
        public DateTime DateClose { get; set; }
    }
}