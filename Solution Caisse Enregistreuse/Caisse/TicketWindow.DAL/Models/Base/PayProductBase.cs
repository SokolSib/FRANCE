using System;

namespace TicketWindow.DAL.Models.Base
{
    public abstract class PayProductBase
    {
        protected PayProductBase(Guid idCheckTicket, Guid productId, string name, string barcode, decimal qty, decimal tva, decimal priceHt, decimal total, Guid checksTicketCustomerId,
            decimal discount, decimal sumDiscount)
        {
            IdCheckTicket = idCheckTicket;
            ProductId = productId;
            Name = name;
            Barcode = barcode;
            Qty = qty;
            Tva = tva;
            PriceHt = priceHt;
            Total = total;
            ChecksTicketCustomerId = checksTicketCustomerId;
            Discount = discount;
            SumDiscount = sumDiscount;
        }

        public Guid IdCheckTicket { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Qty { get; set; }
        public decimal Tva { get; set; }
        public decimal PriceHt { get; set; }
        public decimal Total { get; set; }
        public Guid ChecksTicketCustomerId { get; set; }
        public decimal Discount { get; set; }
        public decimal SumDiscount { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}