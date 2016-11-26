using System;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;

namespace TicketWindow.DAL.Models
{
    public class StockLogs
    {
        public StockLogs(string name, string barcode, decimal qty)
        {
            CustomerId = Guid.NewGuid();
            DateTime = DateTime.Now;
            TypeOperation = true;
            Name = name;
            Barcode = barcode;
            Qty = qty;
            User = Config.User;
            Details = string.Concat(Config.NameTicket, " ", Config.Name, " ", RepositoryEstablishment.Establishment);
        }

        public Guid CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public bool TypeOperation { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Qty { get; set; }
        public string User { get; set; }
        public string Details { get; set; }
    }
}