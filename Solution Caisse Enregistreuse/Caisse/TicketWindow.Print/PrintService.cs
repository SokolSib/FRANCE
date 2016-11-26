using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using TicketWindow.DAL.Models;
using TicketWindow.Print.Additional;
using TicketWindow.Print.Templates;

namespace TicketWindow.Print
{
    public class PrintService
    {
        private static TemplateCloseTicketG _templateCloseTicketG;
        private static TemplateCloseTicket _templateCloseTicket;

        public static void PrintCloseTicketG(CloseTicketG g, List<CloseTicket> c)
        {
            _templateCloseTicketG = new TemplateCloseTicketG(g, c);
            var document = new PrintDocument();
            document.PrintPage += PrintPageCloseTicketG;
            document.Print();
        }

        public static void PrintCloseTicket(CloseTicket closeTicket, string nameTicketWindow)
        {
            var typePay = PrintTypePay.GetPrintTypePays(closeTicket);
            _templateCloseTicket = new TemplateCloseTicket(typePay, nameTicketWindow);
            var pd = new PrintDocument();
            pd.PrintPage += PrintPageCloseTicket;
            pd.Print();
        }

        private static void PrintPageCloseTicketG(object o, PrintPageEventArgs e)
        {
            const int x = 0;
            const int y = 0;
            const int width = 280;
            const int heigth = 134;

            var imageRectangle = new Rectangle(x, y, width, heigth);

            e.Graphics.DrawImage((Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\images\\anahit_9.jpg")), imageRectangle);

            foreach (var textElement in _templateCloseTicketG.TextElements)
                e.Graphics.DrawString(textElement.Text, textElement.Font, textElement.Brush, textElement.Rectangle, textElement.StringFormat);
        }

        private static void PrintPageCloseTicket(object o, PrintPageEventArgs e)
        {
            foreach (var elm in _templateCloseTicket.TextElements)
                e.Graphics.DrawString(elm.Text, elm.Font, elm.Brush, elm.Rectangle, elm.StringFormat);
        }
    }
}