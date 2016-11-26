
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using BarcodeLib;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using System.Threading;

namespace printCheck
{
    class Program
    {
        class ClassImageBarCode
        {

            private BarcodeLib.TYPE selectTYPE(string barcode)
            {

                int caseSwitch = barcode.Replace(" ", "").Length;

                switch (caseSwitch)
                {
                    case 11:
                        return TYPE.CODE11;
                    case 8:
                        return TYPE.EAN8;
                    case 12:
                        return TYPE.UCC12;
                    case 18:
                        return TYPE.CODE128;
                    default:
                        return TYPE.EAN13;
                }
            }

            public Image get_bc(string bc)
            {
                BarcodeLib.Barcode b = new Barcode(bc, selectTYPE(bc));

                b.Encode(selectTYPE(bc), bc, Color.Black, Color.White, 135, 30);

                return b.EncodedImage;
            }

            public Graphics printCheck(string barcode, string produit, int count, string total, Graphics g)
            {


                String Messagechp_des = produit;
                Color FillColor = Color.FromArgb(0, 255, 255, 255);
                SolidBrush FillBrush = new SolidBrush(FillColor);
                Rectangle FillRectangle = new Rectangle(0, 0, 300, 50 );
                Font TextFont = new Font("Arial", 12);
                SolidBrush TextBrush = new SolidBrush(Color.Black);
                StringFormat TextFormat = new StringFormat();
                TextFormat.Alignment = StringAlignment.Center;
                TextFormat.LineAlignment = StringAlignment.Near;


                String Message_prix = total;
                Color FillColor_prix = Color.FromArgb(255, 0, 0, 0);
                SolidBrush FillBrush_prix = new SolidBrush(FillColor_prix);
                Rectangle FillRectangle_prix = new Rectangle(220, 50, 0, 0);
                Font TextFont_prix = new Font("Arial", 21);
                SolidBrush TextBrush_prix = new SolidBrush(Color.Black);
                StringFormat TextFormat_prix = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                TextFormat_prix.Alignment = StringAlignment.Near;
                TextFormat_prix.LineAlignment = StringAlignment.Near;


                String Message_barcode = barcode + Environment.NewLine;
                Color FillColor_barcode = Color.FromArgb(255, 0, 0, 0);
                SolidBrush FillBrush_barcode = new SolidBrush(FillColor_prix);
                Rectangle FillRectangle_barcode = new Rectangle(0, 75, 100, 30);
                Font TextFont_barcode = new Font("Arial", 9);
                SolidBrush TextBrush_barcode = new SolidBrush(Color.Black);
                StringFormat TextFormat_barcode = new StringFormat();
                TextFormat_barcode.Alignment = StringAlignment.Center;
                TextFormat_barcode.LineAlignment = StringAlignment.Center;


                PointF p = new PointF();
                p.X = 0;
                p.Y = 0;
                Pen blackPen = new Pen(Color.Black, 10);

                // Create points that define line.
                Point point1 = new Point(0, 0);
                Point point2 = new Point(285, 0);

                // Draw line to screen.
               g.DrawLine(blackPen, point1, point2);

                g.DrawImageUnscaled(get_bc(barcode), 0, 30);
                g.DrawString(Messagechp_des, TextFont, TextBrush, p, TextFormat);

              //  g.DrawString(Message_prix, TextFont_prix, TextBrush_prix, FillRectangle_prix, TextFormat_prix);
                //     g.DrawString(Message_barcode, TextFont_barcode, TextBrush_barcode, FillRectangle_barcode, TextFormat_barcode);

                return g;
            }

        }
        private static void printPage(object o, PrintPageEventArgs e)
        {
            new ClassImageBarCode().printCheck("1231312312312345", "productproductproductproductproductproduct", 1, "total", e.Graphics);

        }
     
        static void Main(string[] args)
        {
           

            PrintDocument pd = new PrintDocument();



           pd.PrintPage += printPage;

           pd.Print();

        }
    }
}
