using System;

namespace TicketWindow.DAL.Additional
{
    public class TesProduct
    {
        public int CustomerId { get; set; }
        public int TypeId { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerIdProduct { get; set; }
        public string NameProduct { get; set; }
        public string CodeBar { get; set; }
        public bool Balance { get; set; }
        public decimal PrixHt { get; set; }
        public decimal Qty { get; set; }
        public decimal Tva { get; set; }
        public decimal TotalHt { get; set; }
        public string Description { get; set; }
        public Guid ProductsWeb { get; set; }
        public string SubGroup { get; set; }
        public string Group { get; set; }
        public Guid TesCustomerId { get; set; }
        public decimal? ConditionAchat { get; set; }
    }
}