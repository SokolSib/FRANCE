using System;
using TicketWindow.Global;

namespace TicketWindow.DAL.Additional
{
    public class TesReglament
    {
        public TesReglament()
        {
            Caisse = Config.Name;
        }

        public Guid CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public string Caisse { get; set; }
        public string TypePay { get; set; }
        public decimal? Montant { get; set; }
        public Guid TesCustomerId { get; set; }
    }
}