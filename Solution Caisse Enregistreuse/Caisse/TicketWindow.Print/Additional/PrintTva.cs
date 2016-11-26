using System.Collections.Generic;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;

namespace TicketWindow.Print.Additional
{
    public sealed class PrintTva
    {
        public PrintTva(Tva tva, decimal ht, decimal tvaDecimal, decimal ttc)
        {
            Tva = tva;
            Ht = ht;
            TvaDecimal = tvaDecimal;
            Ttc = ttc;
        }

        public Tva Tva { get; set; }
        public decimal Ht { get; set; }
        public decimal TvaDecimal { get; set; }
        public decimal Ttc { get; set; }

        public static List<PrintTva> GetPrintTvases(CloseTicket closeTicket)
        {
            var mtvases = new List<PrintTva>();
            foreach (var tva in RepositoryTva.Tvases)
            {
                var mtva = new PrintTva(tva, 0, 0, 0);

                foreach (var check in closeTicket.ChecksTicket)
                {
                    foreach (var product in check.PayProducts)
                    {
                        if (product.Tva == tva.Value)
                        {
                            var ht = (product.Total/(100 + product.Tva))*100;

                            mtva.Ht += ht;
                            mtva.Ttc += product.Total;
                            mtva.TvaDecimal += (product.Total/(100 + product.Tva)*product.Tva);
                        }
                    }
                }

                mtvases.Add(mtva);
            }

            return mtvases;
        }
    }
}