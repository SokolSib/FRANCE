using System;

namespace TicketWindow.DAL.Additional
{
    public class PriceGrosType
    {
        public PriceGrosType(Guid customerId, Guid productsCustomerId, decimal prix)
        {
            CustomerId = customerId;
            ProductsCustomerId = productsCustomerId;
            Prix = prix;
        }

        public Guid CustomerId { get; set; }
        public Guid ProductsCustomerId { get; set; }
        public decimal Prix { get; set; }
    }
}