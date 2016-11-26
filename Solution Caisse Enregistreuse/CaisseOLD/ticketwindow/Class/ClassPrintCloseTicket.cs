using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassPrintCloseTicket
    {
        public class print
        {
            const int size = 20;
            public List<ClassCheck.localTypesPay> typePay { get; set; }
            public string nameTicketWindow { get; set; }
            public class elm
            {
                public string text { get; set; }
                public Rectangle rectangle { get; set; }
                public Font font = new Font("Arial", 9);
                public Brush brush = new SolidBrush(Color.Black);
                public StringFormat stringFormat = new StringFormat();
            }
            public List<elm> E = new List<elm>();
            public print(List<ClassCheck.localTypesPay> typePay, string nameTicketWindow)
            {
                this.typePay = typePay;
                this.nameTicketWindow = nameTicketWindow;
                int X = 0;
                int Y = 0;
                int H = size;
                int W = 280;
                elm n = new elm();
                n.rectangle = new Rectangle(X, Y, W, H);
                n.stringFormat.Alignment = StringAlignment.Near;
                n.stringFormat.LineAlignment = StringAlignment.Near;
                n.text = nameTicketWindow + "(" + DateTime.Now + ")";
                E.Add(n);
                Y += H;

                foreach (ClassCheck.localTypesPay t in typePay)
                {
                    Y += H;
                    elm nl = new elm();
                    nl.rectangle = new Rectangle(X, Y, W/2, H);
                    nl.stringFormat.Alignment = StringAlignment.Near;
                    nl.stringFormat.LineAlignment = StringAlignment.Near;
                    nl.text = t.type.Name;
                    E.Add(nl);

                    elm nr = new elm();
                    nr.rectangle = new Rectangle(X+W/2, Y, W/2, H);
                    nr.stringFormat.Alignment = StringAlignment.Far;
                    nr.stringFormat.LineAlignment = StringAlignment.Near;
                    nr.text = t.value.ToString("0.00");
                    E.Add(nr);
                }
            }
        }
        private void printPage(object o, PrintPageEventArgs e)
        {      
            foreach (var elm in pr.E )          
                e.Graphics.DrawString(elm.text, elm.font, elm.brush, elm.rectangle, elm.stringFormat);     
        }
        public print pr { get; set; }
        public ClassPrintCloseTicket( List< ClassCheck.localTypesPay> typePay, string nameTicketWindow)
        {
            pr = new print(typePay, nameTicketWindow);

            PrintDocument pd = new PrintDocument();

            pd.PrintPage += printPage;

            pd.Print();
        }
    }
 }
