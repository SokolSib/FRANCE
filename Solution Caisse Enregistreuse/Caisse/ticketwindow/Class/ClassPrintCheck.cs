using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using TicketWindow.Classes;
using TicketWindow.DAL.Repositories;
using TicketWindow.Extensions;
using TicketWindow.Global;
using TicketWindow.Print.Additional;
using TicketWindow.Print.Templates;
using TicketWindow.Services;
using Color = System.Drawing.Color;
using FontStyle = System.Drawing.FontStyle;
using Image = System.Drawing.Image;
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace TicketWindow.Class
{
    public class ClassPrintCheck
    {
        private static readonly string PathH = AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkHead.txt";
        private static readonly string PathF = AppDomain.CurrentDomain.BaseDirectory + @"\Data\checkFooter.txt";
        private readonly List<PrintTypePay> _attr = new List<PrintTypePay>();
        private List<string> _tva;

        public ClassPrintCheck(XContainer rootElement, bool duplicate)
        {
            Duplicate = duplicate;
            var listProduct = new List<PrintGroupProduct>();
            _tva = new List<string>();
            foreach (var p in RepositoryTypePay.TypePays)
            {
                var money = rootElement.GetXAttribute("check", p.NameCourt.Trim());

                if (money != null)
                    _attr.Add(new PrintTypePay(p, money.Value.ToDecimal()));
            }

            Rendu = rootElement.GetXAttributeValue("check", "Rendu").ToDecimal();

            var xe = rootElement.GetXElements("check", "product");

            var sum = 0.0m;
            var sumDiscounts = 0.0m;

            foreach (var e in xe)
            {
                var discount = e.GetXElementValue("Discount").ToDecimal();
                var sumDiscount = -e.GetXElementValue("sumDiscount").ToDecimal();
                var codebare = e.GetXElementValue("CodeBare").Trim();
                var qty = e.GetXElementValue("qty").ToDecimal();
                var name = e.GetXElementValue("Name");
                var total = e.GetXElementValue("total").ToDecimal() - sumDiscount;
                var price = e.GetXElementValue("price").ToDecimal();
                var categories = RepositoryGroupProduct.GetGroupNameById(e.GetXElementValue("grp").ToInt());
                var tva = e.GetXElementValue("tva").ToInt();
                var customerId = e.GetXElementValue("CustomerId").ToGuid();

                var p = new PrintProduct(customerId, categories, codebare, name, qty, total, price, tva, discount, sumDiscount);

                #region DISCOUNT

                if (discount > 0)
                {
                    var discountcategories = "Remise " + p.ProcentDiscount + "%";

                    var discountIndx = listProduct.FindIndex(l => l.Categories == discountcategories);
                    if (discountIndx == -1)
                    {
                        var pn = new PrintProduct(Guid.Empty, discountcategories, Barcode, name, qty, total, price, tva, discount, sumDiscount);
                        listProduct.Add(new PrintGroupProduct(discountcategories, pn));
                    }
                    else
                    {
                        //  groupProduct.product pn = new groupProduct.product(Guid.Empty, discountcategories, "", name, 1, sumDiscount, 0, tva_, Discount, sumDiscount);
                        var pn = new PrintProduct(Guid.Empty, discountcategories, Barcode, name, qty, total, price, tva, discount, sumDiscount);

                        var repeat = listProduct[discountIndx].Products.FindIndex((l => ((l.Name == name))));

                        if (repeat != -1)
                        {
                            listProduct[discountIndx].Products[repeat].Total += pn.SumDiscount;
                            listProduct[discountIndx].Products[repeat].Price = pn.Price;
                            listProduct[discountIndx].Products[repeat].TvaTotal += pn.TvaTotal;
                            listProduct[discountIndx].Products[repeat].Ht += pn.Ht;
                            listProduct[discountIndx].Products[repeat].SumDiscount += pn.SumDiscount;
                        }
                        else
                            listProduct[discountIndx].Products.Add(pn);
                    }

                    sumDiscounts -= sumDiscount;
                }

                #endregion

                var indx = listProduct.FindIndex(l => l.Categories == p.Categories);

                if (indx != -1)
                {
                    var f = listProduct[indx].Products.FindIndex(l => ((l.CustomerId == p.CustomerId) && l.Price == p.Price));

                    if (f == -1)
                        listProduct[indx].Products.Add(p);
                    else
                    {
                        if (listProduct[indx].Products[f].Categories != "Remise " + p.ProcentDiscount + "%")
                        {
                            listProduct[indx].Products[f].Qty += p.Qty;
                            listProduct[indx].Products[f].Total += p.Total;
                            listProduct[indx].Products[f].Price = p.Price;
                            listProduct[indx].Products[f].TvaTotal += p.TvaTotal;
                            listProduct[indx].Products[f].Ht += p.Ht;
                            listProduct[indx].Products[f].SumDiscount += p.SumDiscount;
                        }
                    }
                }
                else
                    listProduct.Add(new PrintGroupProduct(categories, p));


                sum += total;
            }

            listProduct = listProduct.OrderBy(l => l.Categories).ToList();

            var listProductDiscount = listProduct.FindAll(l => l.Categories.IndexOf("Discount") != -1);
            listProduct.RemoveAll(l => l.Categories.IndexOf("Discount") != -1);

            var listProductDiscountRemise = listProduct.FindAll(l => l.Categories.IndexOf("Remise") != -1);
            listProduct.RemoveAll(l => l.Categories.IndexOf("Remise") != -1);

            listProduct.AddRange(listProductDiscount);
            listProduct.AddRange(listProductDiscountRemise);

            Head = File.ReadAllText(PathH);
            Footer = File.ReadAllText(PathF);

            Barcode = rootElement.GetXAttributeValue("check","barcodeCheck");

            Totals = (sum - sumDiscounts).ToString("0.00");
            SumDiscount = sumDiscounts.ToString("0.00");

            #region DiscountPoints

            var a0 = rootElement.GetXAttribute("check","DCBC");
            var a1 = rootElement.GetXAttribute("check","DCBC_BiloPoints");
            var a2 = rootElement.GetXAttribute("check","DCBC_DobavilePoints");
            var a3 = rootElement.GetXAttribute("check","DCBC_OtnayliPoints");
            var a4 = rootElement.GetXAttribute("check","DCBC_OstalosPoints");
            var a5 = rootElement.GetXAttribute("check","DCBC_name");

            var bcdc = (a0 != null ? a0.Value : null);

            if (!string.IsNullOrEmpty(bcdc))
            {
                C = new PrintClientInfo
                    {
                        Dcbc = a0.Value,
                        DcbcBiloPoints = a1.Value,
                        DcbcDobavilePoints = a2.Value,
                        DcbcOtnayliP = a3.Value,
                        DcbcOstalosPoints = a4.Value,
                        DcbcName = a5.Value
                    };
            }

            #endregion

            DotLiquidService.Print(Barcode, Head, listProduct, Totals, SumDiscount, _attr, Rendu, Footer, C, duplicate);
        }

        private string Head { get; set; }
        private string Barcode { get; set; }
        private string Totals { get; set; }
        private string SumDiscount { get; set; }
        private string Footer { get; set; }
        private decimal Rendu { get; set; }
        private bool Duplicate { get; set; }
        private PrintClientInfo C { get; set; }

        public Graphics PrintCheck(string barcode, string head, List<PrintGroupProduct> lProduct, string total, string sumDiscount, List<PrintTypePay> lPay, decimal rendu,
            string footer,
            PrintClientInfo infoOfClient, Graphics gt)
        {
            var rect = new List<Rectangle>();

            var x1 = 0;
            var y1 = 0;
            var w = 280*20;
            var h = 134*20;

            var fillRectangleImg = new Rectangle(x1, y1, w, h);

            y1 = h;
            h = 15*head.Split('\n').Length*20;
            var fillRectangleHead = new Rectangle(x1, y1, w, h);
            rect.Add(fillRectangleHead);

            string duplicate = null;
            var fillRectangleDuplicate = new Rectangle();
            if (Duplicate)
            {
                y1 = y1 + h;
                h = 25;
                duplicate = "*** DUPLICATA ***";
                fillRectangleDuplicate = new Rectangle(x1, y1, w, h);
                rect.Add(fillRectangleDuplicate);
            }
            y1 = y1 + h;
            h = 25;
            const string vente = "*** VENTE ***";
            var fillRectangleVente = new Rectangle(x1, y1, w, h);
            rect.Add(fillRectangleVente);

            y1 = y1 + h;
            var de = new TemplateCheck();

            de.DrawElements(lProduct, x1, y1, out x1, out y1);
            
            h = 10;
            const string totalLine = "__________________";
            var fillRectangleTotal = new Rectangle(x1 + 135, y1 - 6, 150, h);
            rect.Add(fillRectangleTotal);

            h = 20;
            const string totalLabel = "TOTAL :";
            var fillRectangleTotal1 = new Rectangle(x1 + 145, y1 + 1, 70, h);
            var totalText = total;
            var fillRectangleTotal2 = new Rectangle(x1 + 210, y1, 70, h);

            y1 = y1 + h;

            de.DrawPays(lPay, x1, y1 + 5, out x1, out y1);

            w = 280;
            h = 15;
            var Rendu = "Rendu :";
            var fillRectangleRendu = new Rectangle(x1 + 20, y1, 140, h);
            var fontRendu = new Font("Arial", 9);
            rect.Add(fillRectangleRendu);

            var renduVal = rendu.ToString("0.00 €");
            var fillRectangleRenduVal = new Rectangle(x1 + 170, y1, 90, h);
            rect.Add(fillRectangleRenduVal);

            y1 = y1 + h;
            h = 25;
            var titleTva = "*** TVA ***";
            var fillRectangleTitleTva = new Rectangle(x1, y1, w, h);
            rect.Add(fillRectangleTitleTva);

            y1 = y1 + h;
            de.DrawTvases(x1, y1, out x1, out y1);


            string titleRemise = null;
            string discount = null;
            string discount1 = null;
            var fillRectangleTitleRemise = new Rectangle();
            var fillRectangleDiscount = new Rectangle();
            var fillRectangleDiscount1 = new Rectangle();

            if (decimal.Parse(sumDiscount) > 0)
            {
                y1 = y1 + h;
                w = 280;
                h = 20;
                titleRemise = "*** REMISE ***";
                fillRectangleTitleRemise = new Rectangle(x1, y1, w, h);
                rect.Add(fillRectangleTitleRemise);

                y1 = y1 + h;
                h = 20;
                discount = "Total remise sur ce ticket : ";
                fillRectangleDiscount = new Rectangle(x1, y1, 200, h);
                discount1 = sumDiscount;
                fillRectangleDiscount1 = new Rectangle(x1 + 200, y1, 80, h);
            }

            #region INFO OF CLIENT

            string dcbcName = null;
            string dcbcBiloPoints = null;
            string dcbcBiloPoints1 = null;
            string dobavilePoints = null;
            string dobavilePoints1 = null;
            string ostalosPoints = null;
            string ostalosPoints1 = null;
            string carte = null;
            var fillRectangleDcbcName = new Rectangle();
            var fillRectangleDcbcBiloPoints = new Rectangle();
            var fillRectangleDcbcBiloPoints1 = new Rectangle();
            var fillRectangleDobavilePoints = new Rectangle();
            var fillRectangleDobavilePoints1 = new Rectangle();
            var fillRectangleOtnayliP = new Rectangle();
            var fillRectangleOtnayliP1 = new Rectangle();
            var fillRectangleOstalosPoints = new Rectangle();
            var fillRectangleOstalosPoints1 = new Rectangle();
            var fillRectangleTCarte = new Rectangle();

            if (infoOfClient != null)
            {
                y1 = y1 + h;
                w = 280;
                h = 20;
                carte = "*** CARTE FIDELITE ***";
                fillRectangleTCarte = new Rectangle(x1, y1, w, h);
                rect.Add(fillRectangleTCarte);

                y1 = y1 + h;
                w = 280;
                h = 20;

                dcbcName = infoOfClient.DcbcName + " - " + infoOfClient.Dcbc;
                fillRectangleDcbcName = new Rectangle(x1, y1, w, h);
                
                y1 = y1 + h;
                h = 12;

                dcbcBiloPoints = "Ancien solde de points : ";
                fillRectangleDcbcBiloPoints = new Rectangle(x1, y1, 180, h);
                dcbcBiloPoints1 = infoOfClient.DcbcBiloPoints;
                fillRectangleDcbcBiloPoints1 = new Rectangle(x1 + 180, y1, 80, h);

                y1 = y1 + h;
                h = 12;

                dobavilePoints = "Points fidelité acquis : ";
                fillRectangleDobavilePoints = new Rectangle(x1, y1, 180, h);
                dobavilePoints1 = infoOfClient.DcbcDobavilePoints;
                fillRectangleDobavilePoints1 = new Rectangle(x1 + 180, y1, 80, h);
                
                y1 = y1 + h;
                h = 12;

                ostalosPoints = "Nouveau solde de points : ";
                fillRectangleOstalosPoints1 = new Rectangle(x1, y1, 180, h);
                ostalosPoints1 = infoOfClient.DcbcOstalosPoints;
                fillRectangleOstalosPoints = new Rectangle(x1 + 180, y1, 80, h);
            }

            #endregion

            y1 = y1 + h;
            w = 260;
            h = 40;
            var fillRectangleBarCodeImg = new Rectangle(x1 + 10, y1 + 5, w, h);

            y1 = y1 + h + 5;
            w = 280;
            h = 30;
            var numeroTicket = barcode + Environment.NewLine + Config.NameTicket + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            var fillRectangleNumeroTicket = new Rectangle(x1, y1, w, h);

            y1 = y1 + h;
            w = 280;
            h = 20*footer.Split('\n').Length;
            
            var fillRectangleFooter = new Rectangle(x1, y1, w, h);

            y1 = y1 + 20;

            var textFont = new Font("Arial", 9);
            var textBrush = new SolidBrush(Color.Black);

            var textFormatL = new StringFormat
                              {
                                  Alignment = StringAlignment.Near,
                                  LineAlignment = StringAlignment.Center
                              };


            var textFormatR = new StringFormat
                              {
                                  Alignment = StringAlignment.Far,
                                  LineAlignment = StringAlignment.Center
                              };

            var textFormatC = new StringFormat
                              {
                                  Alignment = StringAlignment.Center,
                                  LineAlignment = StringAlignment.Center
                              };

            /*Total VENTE*/
            var fontTotal = new Font("Arial", 10, FontStyle.Bold);

            /*Title*/
            var fontTitle = new Font("Arial", 12, FontStyle.Bold);
            var formatTitle = new StringFormat
                              {
                                  Alignment = StringAlignment.Center,
                                  LineAlignment = StringAlignment.Center
                              };


            /* Arial , 8 , Bold*/
            var fontArialBold = new Font("Arial", 8, FontStyle.Bold);


            var textFormat = new StringFormat
                             {
                                 Alignment = StringAlignment.Far,
                                 LineAlignment = StringAlignment.Far
                             };

            int nWidth = w, nHeight = y1;
            var b1 = new Bitmap(nWidth, nHeight);

            using (var g = Graphics.FromImage(b1))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;

                foreach (var e in de.PrintElements)
                {
                    g.DrawString(e.Text, e.Font, textBrush, e.Rectangle, e.StringFormat);
                    rect.Add(e.Rectangle);
                }

                g.DrawString(titleTva, fontTitle, textBrush, fillRectangleTitleTva, formatTitle);

                foreach (var e in de.PrintTvases)
                {
                    g.DrawString(e.Text, e.Font, textBrush, e.Rectangle, e.StringFormat);
                    rect.Add(e.Rectangle);
                }

                foreach (var e in de.PrintPays)
                {
                    g.DrawString(e.Text, e.Font, textBrush, e.Rectangle, e.StringFormat);
                    rect.Add(e.Rectangle);
                }

                g.DrawImage((Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\images\\anahit_9.jpg")), fillRectangleImg);
                g.DrawString(head, fontRendu, textBrush, fillRectangleHead, textFormatC);

                if (duplicate != null)
                    g.DrawString(duplicate, fontTitle, textBrush, fillRectangleDuplicate, formatTitle);

                g.DrawString(vente, fontTitle, textBrush, fillRectangleVente, formatTitle);
                g.DrawString(totalLine, fontTotal, textBrush, fillRectangleTotal, textFormat);
                g.DrawString(totalLabel, fontTotal, textBrush, fillRectangleTotal1, textFormatL);
                g.DrawString(totalText, fontTotal, textBrush, fillRectangleTotal2, textFormatR);

                if (rendu != 0.0m)
                {
                    g.DrawString(Rendu, fontRendu, textBrush, fillRectangleRendu, textFormatL);
                    g.DrawString(renduVal, fontRendu, textBrush, fillRectangleRenduVal, textFormatR);
                }

                if (decimal.Parse(sumDiscount) > 0)
                {
                    g.DrawString(titleRemise, fontTitle, textBrush, fillRectangleTitleRemise, formatTitle);
                    g.DrawString(discount, fontArialBold, textBrush, fillRectangleDiscount, textFormatL);
                    g.DrawString(discount1, fontArialBold, textBrush, fillRectangleDiscount1, textFormatR);
                }

                if (infoOfClient != null)
                {
                    g.DrawString(carte, fontTitle, textBrush, fillRectangleTCarte, formatTitle);
                    g.DrawString(dcbcName, fontRendu, textBrush, fillRectangleDcbcName, textFormatL);
                    g.DrawString(dcbcBiloPoints, fontRendu, textBrush, fillRectangleDcbcBiloPoints, textFormatL);
                    g.DrawString(dobavilePoints, fontRendu, textBrush, fillRectangleDobavilePoints, textFormatL);
                    g.DrawString(null, fontRendu, textBrush, fillRectangleOtnayliP, textFormatL);
                    g.DrawString(ostalosPoints, fontRendu, textBrush, fillRectangleOstalosPoints1, textFormatL);
                    g.DrawString(dcbcBiloPoints1, fontRendu, textBrush, fillRectangleDcbcBiloPoints1, textFormatL);
                    g.DrawString(dobavilePoints1, fontRendu, textBrush, fillRectangleDobavilePoints1, textFormatL);
                    g.DrawString(null, fontRendu, textBrush, fillRectangleOtnayliP1, textFormatL);
                    g.DrawString(ostalosPoints1, fontRendu, textBrush, fillRectangleOstalosPoints, textFormatL);
                }


                g.DrawImage(new ClassImageBarCode().GetBc(barcode), fillRectangleBarCodeImg);
                g.DrawString(numeroTicket, fontRendu, textBrush, fillRectangleNumeroTicket, textFormatC);
                g.DrawString(Footer, textFont, textBrush, fillRectangleFooter, textFormatL);
            }

            b1.Save("D:\\test.bmp");

            Printimage(CreateBitmapSourceFromBitmap(b1));

            return gt;
        }

        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
        
        private static void Printimage(ImageSource bi)
        {
            var vis = new DrawingVisual();
            var dc = vis.RenderOpen();
            dc.DrawImage(bi, new Rect {Width = bi.Width, Height = bi.Height});
            dc.Close();

            var pdialog = new PrintDialog();
            pdialog.PrintVisual(vis, "My Image");
        }
    }
}