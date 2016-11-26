using System;

namespace TicketWindow.DAL.Models
{
    public class Discount
    {
        public bool AddPoints;
        public bool DiscountSet;
        public DateTime Dt = new DateTime().AddHours(24);
        public Guid InfoClientsCustomerId;
        public int MaxPoints = 8;
        public decimal MoneyMax = 20.0m;
        public int Points;
        public decimal Procent;
        public decimal ProcentDefault = 0.0m;
        public bool ShowMessaget = true;
        public DateTime LastDateUpd { get; set; }
        public string Barcode { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
    }
}