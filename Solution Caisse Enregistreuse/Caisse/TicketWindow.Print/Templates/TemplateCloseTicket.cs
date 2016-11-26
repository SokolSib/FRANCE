using System;
using System.Collections.Generic;
using System.Drawing;
using TicketWindow.Print.Additional;

namespace TicketWindow.Print.Templates
{
    public class TemplateCloseTicket
    {
        private const int Size = 20;
        public string NameTicketWindow;
        public List<PrintTextElement> TextElements = new List<PrintTextElement>();
        public List<PrintTypePay> TypePay;

        public TemplateCloseTicket(List<PrintTypePay> typePay, string nameTicketWindow)
        {
            TypePay = typePay;
            NameTicketWindow = nameTicketWindow;
            const int x = 0;
            var y = 0;
            const int h = Size;
            const int w = 280;

            TextElements.Add(new PrintTextElement(nameTicketWindow + "(" + DateTime.Now + ")",
                x, y, w, h, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}));
            y += h;

            foreach (var t in typePay)
            {
                y += h;

                TextElements.Add(new PrintTextElement(t.Type.Name,
                    x, y, w/2, h, new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near}));

                TextElements.Add(new PrintTextElement(t.Money.ToString("0.00"),
                    x + w/2, y, w/2, h, new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near}));
            }
        }
    }
}