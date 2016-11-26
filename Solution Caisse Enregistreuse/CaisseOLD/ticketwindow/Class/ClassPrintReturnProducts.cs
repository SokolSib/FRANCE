using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{
    class ClassPrintReturnProducts
    {
        public ClassPrintReturnProducts(string bar_code, List<ClassProducts> p, decimal m)
        {
            ClassDotLiquid.prt(bar_code, p, m);
        }
   /*     private static string pathH = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkHead.txt";
        private static string pathB = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkBody.txt";
        private static string pathF = System.AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkFooter.txt";

        const int size = 20;
        public class elm
        {
            public string text { get; set; }
            public Rectangle rectangle { get; set; }
            public Font font = new Font("Arial", 9);
            public Brush brush = new SolidBrush(Color.Black);
            public StringFormat stringFormat = new StringFormat();
        }
        public List<elm> E = new List<elm>();


        /*
        public ClassPrintReturnProducts(string bar_code, List<ClassProducts> p, decimal m, Graphics g)
        {
           
            int X = 0;
            int Y = 0;
            int H = size;
            int W = 280;
            elm n = new elm();
            n.rectangle = new Rectangle(X, Y, W, H);
            n.stringFormat.Alignment = StringAlignment.Near;
            n.stringFormat.LineAlignment = StringAlignment.Near;
            n.text = m.ToString("0.00");
            E.Add(n);
            Y += H;

            g.DrawImage(new ClassImageBarCode().get_bc(bar_code), new Rectangle(X,Y,W,H));

            foreach (var e in E)
            {
                g.DrawString(e.text, e.font, e.brush, e.rectangle, e.stringFormat);
            }


        }
        */
    }
}
