using System;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Print.Additional
{
    public class PrintProduct
    {
        public PrintProduct(Guid customerId, string categories, string barcode, string name, decimal qty, decimal total, decimal price, int tvaId, decimal procentDiscount,
            decimal sumDiscount)
        {
            CustomerId = customerId;
            Categories = categories;
            Name = name;
            TvaId = tvaId;
            ProcentDiscount = procentDiscount;
            SumDiscount = sumDiscount;

            if (customerId != Guid.Empty)
            {
                Qty = qty;
                Total = total;
                Barcode = barcode;
                Price = price;
                Ht = Math.Round((total / qty) * (100 / (100 + RepositoryTva.GetById(tvaId))), 2) * qty;
                TvaTotal = total - Ht;
            }
            else
            {
                Qty = 1;
                Total = sumDiscount;
                Barcode = string.Empty;
                Price = 0;
                Ht = decimal.Round(sumDiscount * (100 / (100 + RepositoryTva.GetById(tvaId))), 2);
                TvaTotal = sumDiscount - Ht;
            }
        }

        public Guid CustomerId { get; set; }
        public string Categories { get; set; }
        public string Name { get; set; }
        public decimal Qty { get; set; }
        public decimal Total { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public int TvaId { get; set; }
        public decimal TvaTotal { get; set; }
        public decimal Ht { get; set; }
        public decimal ProcentDiscount { get; set; }
        public decimal SumDiscount { get; set; }
    }
}