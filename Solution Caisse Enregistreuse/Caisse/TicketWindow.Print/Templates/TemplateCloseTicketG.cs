using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.Print.Additional;

namespace TicketWindow.Print.Templates
{
    public class TemplateCloseTicketG
    {
        public string NameTicketWindow;
        public List<PrintTextElement> TextElements = new List<PrintTextElement>();

        public TemplateCloseTicketG(CloseTicketG closeTicketG, ICollection<CloseTicket> closeTickets)
        {
            var dateText = closeTicketG.DateClose.ToShortDateString();
            var timetext = closeTicketG.DateClose.ToShortTimeString();

            const int x = 0;
            var y = 134;
            const int h = 20;
            const int w = 280;
            const int sizeLine = 12;

            TextElements.Add(new PrintTextElement("Date : " + dateText + "  -  Heure : " + timetext,
                x, y, w, h, new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near}));
            y += h;

            var printTotals = new List<PrintTotal>();
            var totalCaseTva = new List<List<PrintTva>>();

            foreach (var closeTicket in closeTickets.OrderBy(ct => ct.NameTicket))
            {
                TextElements.Add(new PrintTextElement(closeTicket.NameTicket.ToUpper(),
                    x, y + 15, w, h, new StringFormat {Alignment = StringAlignment.Center}, new Font("Arial", 12, FontStyle.Bold)));
                y += h + 15;

                TextElements.Add(new PrintTextElement("**********************************************************",
                    x, y, 300, h));
                y += h;

                TextElements.Add(new PrintTextElement(closeTicket.DateClose.ToShortDateString(),
                    x, y - 10, w, h, new StringFormat {Alignment = StringAlignment.Center}));
                y += h;

                var typePays = PrintTypePay.GetPrintTypePays(closeTicket);
                var sumMoney = typePays.Sum(tp => tp.Money);

                foreach (var typePay in typePays)
                {
                    TextElements.Add(new PrintTextElement(typePay.Type.Name,
                        x, y, 120, h, new StringFormat {Alignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(typePay.Money.ToString("C"),
                        x + 120, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));

                    TextElements.Add(new PrintTextElement((typePay.Money / sumMoney).ToString("P"),
                        x + 200, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));
                    y += h;
                }

                TextElements.Add(new PrintTextElement("TOTAL",
                    x, y, 120, h, new StringFormat {Alignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(sumMoney.ToString("C"),
                    x + 120, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(1.ToString("P"),
                    x + 200, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}, new Font("Arial", 9, FontStyle.Bold)));
                y += h;

                TextElements.Add(new PrintTextElement("*** TVA ***",
                    x, y + 10, w, h, new StringFormat {Alignment = StringAlignment.Center}, new Font("Arial", 10, FontStyle.Bold)));
                y += h + 10;

                TextElements.Add(new PrintTextElement("  TAUX               HT              TVA                 TTC",
                    x, y, w, h, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 9)));
                y += h;

                var ht = 0.0m;
                var tva = 0.0m;
                var ttc = 0.0m;

                var caseTva = PrintTva.GetPrintTvases(closeTicket);
                totalCaseTva.Add(caseTva);

                foreach (var printTva in caseTva)
                {
                    TextElements.Add(new PrintTextElement(printTva.Tva.Value + "%",
                        x, y, 45, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(printTva.Ht.ToString("C"),
                        x + 50, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(printTva.TvaDecimal.ToString("C"),
                        x + 130, y, 70, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(printTva.Ttc.ToString("C"),
                        x + 200, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    y += h;
                    ht += printTva.Ht;
                    tva += printTva.TvaDecimal;
                    ttc += printTva.Ttc;
                }

                TextElements.Add(new PrintTextElement("TOTAL",
                    x, y, 50, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(ht.ToString("C"),
                    x + 50, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(tva.ToString("C"),
                    x + 130, y, 70, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(ttc.ToString("C"),
                    x + 200, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));
                y += h;

                var printTotal = new PrintTotal
                                 {
                                     Count = decimal.ToInt32(closeTicket.ChecksTicket.Sum(l => l.PayProducts.Sum(la => la.Qty))),
                                     SrTotal = printTotals.Count != 0 ? ttc/printTotals.Count : 0,
                                     Name = closeTicket.NameTicket,
                                     Total = sumMoney
                                 };
                printTotals.Add(printTotal);
            }

            if (closeTickets.Count > 1)
            {
                TextElements.Add(new PrintTextElement("TOTAL GLOBAL",
                    x, y + 15, w, h, new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near}, new Font("Arial", 12, FontStyle.Bold)));
                y += h + 15;

                TextElements.Add(new PrintTextElement("**********************************************************",
                    x, y, 300, h));
                y += h;

                var printTypePays = PrintTypePay.GetPrintTypePays(closeTicketG);
                var sumMoneyG = printTypePays.Sum(l => l.Money);

                foreach (var printTypePay in printTypePays)
                {
                    TextElements.Add(new PrintTextElement(printTypePay.Type.Name,
                        x, y, 120, h, new StringFormat {Alignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(printTypePay.Money.ToString("C"),
                        x + 120, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));

                    TextElements.Add(new PrintTextElement((printTypePay.Money / sumMoneyG).ToString("P"),
                        x + 200, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));
                    y += h;
                }

                TextElements.Add(new PrintTextElement("TOTAL",
                    x, y, 120, h, new StringFormat {Alignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(sumMoneyG.ToString("C"),
                    x + 120, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(1.ToString("P"),
                    x + 200, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}, new Font("Arial", 9, FontStyle.Bold)));
                y += h;

                var ght = 0.0m;
                var gtva = 0.0m;
                var gttc = 0.0m;

                var printTvases = new List<PrintTva>();

                foreach (var p in totalCaseTva)
                {
                    foreach (var pTva in p)
                    {
                        var indx = printTvases.FindIndex(l => l.Tva == pTva.Tva);

                        if (indx == -1)
                        {
                            var j = new PrintTva(pTva.Tva, pTva.Ht, pTva.TvaDecimal, pTva.Ttc);

                            printTvases.Add(j);
                        }
                        else
                        {
                            printTvases[indx].Ht += pTva.Ht;
                            printTvases[indx].Ttc += pTva.Ttc;
                            printTvases[indx].TvaDecimal += pTva.TvaDecimal;
                        }
                    }
                }

                TextElements.Add(new PrintTextElement("*** TVA ***",
                    x, y + 10, w, h, new StringFormat {Alignment = StringAlignment.Center}, new Font("Arial", 10, FontStyle.Bold)));
                y += h + 10;

                TextElements.Add(new PrintTextElement("  TAUX               HT              TVA                 TTC",
                    x, y, w, h, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 9)));
                y += h;

                foreach (var pTva in printTvases)
                {
                    TextElements.Add(new PrintTextElement(pTva.Tva.Value + "%",
                        x, y, 45, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(pTva.Ht.ToString("C"),
                        x + 50, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(pTva.TvaDecimal.ToString("C"),
                        x + 130, y, 70, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    TextElements.Add(new PrintTextElement(pTva.Ttc.ToString("C"),
                        x + 200, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));

                    y += h;
                    ght += pTva.Ht;
                    gtva += pTva.TvaDecimal;
                    gttc += pTva.Ttc;
                }

                TextElements.Add(new PrintTextElement("TOTAL",
                    x, y, 50, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(ght.ToString("C"),
                    x + 50, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(gtva.ToString("C"),
                    x + 130, y, 70, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));

                TextElements.Add(new PrintTextElement(gttc.ToString("C"),
                    x + 200, y, 80, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}, new Font("Arial", 9, FontStyle.Bold)));
            }

            TextElements.Add(new PrintTextElement("STATISTIQUES",
                x, y + 15, w, h, new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near}, new Font("Arial", 12, FontStyle.Bold)));
            y += h + 15;

            TextElements.Add(new PrintTextElement("**********************************************************",
                x, y, 300, h));
            y += h;

            TextElements.Add(new PrintTextElement("Description             QTY           Moyen             %",
                x, y, w, h, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 9)));
            y += h;

            var sumM = printTotals.Sum(l => l.Total);

            foreach (var printTotal in printTotals)
            {
                TextElements.Add(new PrintTextElement(printTotal.Name,
                    x, y, 60, h, new StringFormat {Alignment = StringAlignment.Near}));

                TextElements.Add(new PrintTextElement(printTotal.Count.ToString(),
                    x + 60, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));

                TextElements.Add(new PrintTextElement(printTotal.SrTotal.ToString("C"),
                    x + 140, y, 70, h, new StringFormat {Alignment = StringAlignment.Far}));

                TextElements.Add(new PrintTextElement((sumM == 0 ? 0 : (printTotal.Total/sumM)).ToString("P"),
                    x + 210, y, 70, h, new StringFormat {Alignment = StringAlignment.Far}));

                y += h;
            }

            if (closeTickets.Count > 0)
            {
                TextElements.Add(new PrintTextElement("TOTAL",
                    x, y, 60, h, new StringFormat {Alignment = StringAlignment.Near}));

                TextElements.Add(new PrintTextElement(printTotals.Sum(l => l.Count).ToString(),
                    x + 60, y, 80, h, new StringFormat {Alignment = StringAlignment.Far}));

                TextElements.Add(new PrintTextElement(Math.Round(printTotals.Sum(l => l.SrTotal), 2).ToString("C"),
                    x + 140, y, 70, h, new StringFormat {Alignment = StringAlignment.Far}));

                TextElements.Add(new PrintTextElement(1.ToString("P"),
                    x + 210, y, 70, h, new StringFormat {Alignment = StringAlignment.Far}));
            }
        }
    }
}