using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TicketWindow.DAL.Repositories;
using TicketWindow.Print.Additional;

namespace TicketWindow.Print.Templates
{
    public class TemplateCheck
    {
        public readonly List<PrintTextElement> PrintElements = new List<PrintTextElement>();
        public readonly List<PrintTextElement> PrintPays = new List<PrintTextElement>();
        public readonly List<PrintTva> PrintTvaObjects = new List<PrintTva>();
        public readonly List<PrintTextElement> PrintTvases = new List<PrintTextElement>();

        public void DrawElements(List<PrintGroupProduct> printGroupProducts, int x, int y, out int outX, out int outY)
        {
            foreach (var tva in RepositoryTva.Tvases)
                PrintTvaObjects.Add(new PrintTva(tva, 0, tva.Value, 0));

            foreach (var printGroupProduct in printGroupProducts)
            {
                const int sizeLine = 15;

                y += sizeLine;
                PrintElements.Add(new PrintTextElement(printGroupProduct.Categories,
                    x + 10, y, 270, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far}, new Font("Arial", 8, FontStyle.Bold)));

                foreach (var product in printGroupProduct.Products)
                {
                    var name = product.Name.Length > 30 ? product.Name.Substring(0, 30) : product.Name;

                    PrintElements.Add(new PrintTextElement(name,
                        x, y, 225, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 10)));

                    PrintElements.Add(new PrintTextElement(product.Total.ToString("0.00"),
                        x + 220, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 10)));

                    var idx = PrintTvaObjects.FindIndex(t => t.Tva.Id == product.TvaId);
                    if (idx != -1)
                    {
                        PrintTvaObjects[idx].Ttc += product.TvaTotal;
                        PrintTvaObjects[idx].Ht += product.Ht;
                    }

                    y += sizeLine;

                    if (product.Qty != 1)
                    {
                        var countOfProduct = (int) product.Qty;

                        var emQtyText = countOfProduct - product.Qty != 0
                            ? product.Qty.ToString("0.000") + "kg x " + product.Price + " €"
                            : " " + countOfProduct + "   x   " + product.Price + " €";

                        PrintElements.Add(new PrintTextElement(emQtyText,
                            x + 10, y - 3, 250, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}, new Font("Arial", 8)));
                        y += 10;
                    }
                }
            }
            outX = x;
            outY = y;
        }

        public void DrawPays(IEnumerable<PrintTypePay> printPays, int x, int y, out int outX, out int outY)
        {
            const int sizeLine = 15;

            foreach (var pay in printPays.Where(pay => pay.Money > 0))
            {
                PrintPays.Add(new PrintTextElement(pay.Type.Name,
                    x + 20, y, 150, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 9)));

                PrintPays.Add(new PrintTextElement(pay.Money.ToString("0.00 €"),
                    x + 170, y, 90, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 9)));

                y += sizeLine;
            }

            outX = x;
            outY = y;
        }

        public void DrawTvases(int x, int y, out int outX, out int outY)
        {
            var count = 0;
            const int sizeLine = 11;

            foreach (var m in PrintTvaObjects.Where(m => m.Ttc > 0))
            {
                count++;

                PrintElements.Add(new PrintTextElement("HT : ",
                    x + 150, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement(m.Ht.ToString("0.00 €"),
                    x + 210, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement("TVA " + m.TvaDecimal + "% :",
                    x + 20, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement(m.Ttc.ToString("0.00 €"),
                    x + 80, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                y += sizeLine;
            }

            if (count > 1)
            {
                PrintElements.Add(new PrintTextElement("Total HT : ",
                    x + 150, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement(PrintTvaObjects.Sum(l => l.Ht).ToString("0.00 €"),
                    x + 210, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement("Total TVA :",
                    x + 20, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                PrintElements.Add(new PrintTextElement(PrintTvaObjects.Sum(l => l.Ttc).ToString("0.00 €"),
                    x + 80, y, 60, sizeLine, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center}, new Font("Arial", 8)));

                y += sizeLine;
            }

            outX = x;
            outY = y;
        }
    }
}