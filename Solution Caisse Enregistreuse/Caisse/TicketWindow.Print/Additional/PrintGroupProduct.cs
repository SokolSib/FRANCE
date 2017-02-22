using System.Collections.Generic;

namespace TicketWindow.Print.Additional
{
    public class PrintGroupProduct
    {
        public PrintGroupProduct(string categories, PrintProduct printProduct)
        {
            Categories = categories;
            Products = new List<PrintProduct> {printProduct};
        }

        public string Categories { get; set; }
        public List<PrintProduct> Products { get; set; }
    }
}