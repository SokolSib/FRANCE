using System.Collections.Generic;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Print.Additional
{
    public class PrintTypePay
    {
        public PrintTypePay(TypePay type, decimal money)
        {
            Type = type;
            Money = money;
        }

        public TypePay Type { get; set; }
        public decimal Money { get; set; }

        public static List<PrintTypePay> GetPrintTypePays(CloseTicket closeTicket)
        {
            var typePays = new List<PrintTypePay>();

            if (closeTicket.PayCash > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "Cash"), closeTicket.PayCash));

            if (closeTicket.PayBankCards > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "BankCards"), closeTicket.PayBankCards));

            if (closeTicket.PayBankChecks > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "BankChecks"), closeTicket.PayBankChecks));

            if (closeTicket.PayResto > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "Resto"), closeTicket.PayResto));

            if (closeTicket.Pay1 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "1"), closeTicket.Pay1));

            if (closeTicket.Pay2 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "2"), closeTicket.Pay2));

            if (closeTicket.Pay3 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "3"), closeTicket.Pay3));

            if (closeTicket.Pay4 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "4"), closeTicket.Pay4));

            if (closeTicket.Pay5 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "5"), closeTicket.Pay5));

            if (closeTicket.Pay6 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "6"), closeTicket.Pay6));

            if (closeTicket.Pay7 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "7"), closeTicket.Pay7));

            if (closeTicket.Pay8 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "8"), closeTicket.Pay8));

            if (closeTicket.Pay9 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "9"), closeTicket.Pay9));

            if (closeTicket.Pay10 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "10"), closeTicket.Pay10));

            if (closeTicket.Pay11 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "11"), closeTicket.Pay11));

            if (closeTicket.Pay12 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "12"), closeTicket.Pay12));

            if (closeTicket.Pay13 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "13"), closeTicket.Pay13));

            if (closeTicket.Pay14 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "14"), closeTicket.Pay14));

            if (closeTicket.Pay15 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "15"), closeTicket.Pay15));

            if (closeTicket.Pay16 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "16"), closeTicket.Pay16));

            if (closeTicket.Pay17 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "17"), closeTicket.Pay17));

            if (closeTicket.Pay18 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "18"), closeTicket.Pay18));

            if (closeTicket.Pay19 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "19"), closeTicket.Pay19));

            if (closeTicket.Pay20 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(tp => tp.NameCourt == "20"), closeTicket.Pay20));

            return typePays;
        }

        public static List<PrintTypePay> GetPrintTypePays(CloseTicketG closeTicketG)
        {
            var typePays = new List<PrintTypePay>();

            if (closeTicketG.PayCash > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "Cash"), closeTicketG.PayCash));

            if (closeTicketG.PayBankCards > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "BankCards"), closeTicketG.PayBankCards));

            if (closeTicketG.PayBankChecks > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "BankChecks"), closeTicketG.PayBankChecks));

            if (closeTicketG.PayResto > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "Resto"), closeTicketG.PayResto));

            if (closeTicketG.Pay1 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "1"), closeTicketG.Pay1));

            if (closeTicketG.Pay2 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "2"), closeTicketG.Pay2));

            if (closeTicketG.Pay3 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "3"), closeTicketG.Pay3));

            if (closeTicketG.Pay4 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "4"), closeTicketG.Pay4));

            if (closeTicketG.Pay5 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "5"), closeTicketG.Pay5));

            if (closeTicketG.Pay6 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "6"), closeTicketG.Pay6));

            if (closeTicketG.Pay7 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "7"), closeTicketG.Pay7));

            if (closeTicketG.Pay8 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "8"), closeTicketG.Pay8));

            if (closeTicketG.Pay9 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "9"), closeTicketG.Pay9));

            if (closeTicketG.Pay10 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "10"), closeTicketG.Pay10));

            if (closeTicketG.Pay11 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "11"), closeTicketG.Pay11));

            if (closeTicketG.Pay12 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "12"), closeTicketG.Pay12));

            if (closeTicketG.Pay13 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "13"), closeTicketG.Pay13));

            if (closeTicketG.Pay14 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "14"), closeTicketG.Pay14));

            if (closeTicketG.Pay15 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "15"), closeTicketG.Pay15));

            if (closeTicketG.Pay16 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "16"), closeTicketG.Pay16));

            if (closeTicketG.Pay17 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "17"), closeTicketG.Pay17));

            if (closeTicketG.Pay18 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "18"), closeTicketG.Pay18));

            if (closeTicketG.Pay19 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "19"), closeTicketG.Pay19));

            if (closeTicketG.Pay20 > 0)
                typePays.Add(new PrintTypePay(RepositoryTypePay.TypePays.Find(l => l.NameCourt == "20"), closeTicketG.Pay20));

            return typePays;
        }
    }
}