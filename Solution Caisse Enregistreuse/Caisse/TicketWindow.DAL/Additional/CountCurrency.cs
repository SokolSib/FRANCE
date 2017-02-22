using TicketWindow.DAL.Models;

namespace TicketWindow.DAL.Additional
{
    public class CountCurrency
    {
        public CountCurrency(int count, Currency currency)
        {
            Count = count;
            Currency = currency;
        }

        public int Count { get; set; }
        public Currency Currency { get; set; }
    }
}