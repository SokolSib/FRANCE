using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using DotLiquid;
using TicketWindow.Class;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.Print.Additional;
using dotTemplate = DotLiquid.Template;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.Services
{
    internal class DotLiquidService
    {
        private static readonly string PathCheckPrint = AppDomain.CurrentDomain.BaseDirectory + @"Template\CheckPrint.mxaml";
        private static readonly string PathReturnPrint = AppDomain.CurrentDomain.BaseDirectory + @"Template\ReturnPrint.mxaml";
        private static readonly string PathRaportPricePrint = AppDomain.CurrentDomain.BaseDirectory + @"Template\RaportPrice.mxaml";

        private static dotTemplate TemplatePathCheckPrint { get; set; }
        private static dotTemplate TemplatePathReturnPrint { get; set; }
        private static dotTemplate TemplatePathRaportPricePrint { get; set; }

        private static dotTemplate Func(string path)
        {
            try
            {
                var stream = new FileStream(path, FileMode.Open);
                dotTemplate template;
                using (stream)
                {
                    using (var reader = new StreamReader(stream))
                        template = dotTemplate.Parse(reader.ReadToEnd());
                }
                return template;
            }
            catch (System.Exception ex)
            {
                var s = "Ошибка " + path + ex.Message+".";
                FunctionsService.ShowMessageSb(s);
                LogService.Log(TraceLevel.Error, 92, s);
                return null;
            }
        }

        public static void SetPath(int select)
        {
            switch (select)
            {
                case 0:
                    TemplatePathCheckPrint = Func(PathCheckPrint);
                    break;
                case 1:
                    TemplatePathReturnPrint = Func(PathReturnPrint);
                    break;
                case 2:
                    TemplatePathRaportPricePrint = Func(PathRaportPricePrint);
                    break;
            }
        }

        public static void Print(string barcode, string head, List<PrintGroupProduct> printGroupProducts, string total, string sumDiscount, List<PrintTypePay> printPays,
            decimal rendu, string footer, PrintClientInfo infoOfClient, bool duplicateF)
        {
            var printDialog = new PrintDialog();

            var docContext = CreateDocumentContext(barcode, head, printGroupProducts, total, sumDiscount, printPays, rendu, footer, infoOfClient, duplicateF);
            var docString = TemplatePathCheckPrint.Render(docContext);

            try
            {
                var doc = (FlowDocument) XamlReader.Parse(docString);
                var paginator = ((IDocumentPaginatorSource) doc).DocumentPaginator;

                doc.PageWidth = 285;
                doc.PagePadding = new Thickness(11,0,0,0);
                doc.ColumnGap = 0;
                doc.ColumnWidth = 0;

                var queue = printDialog.PrintQueue;
                var writer = PrintQueue.CreateXpsDocumentWriter(queue);
                writer.Write(paginator);
            }
            catch (System.Exception ex)
            {
                var text = ex.Message + docString+".";
                FunctionsService.ShowMessageSb(text);
                LogService.Log(TraceLevel.Error, 11, text);
            }
        }

        public static void Print(string barCode, decimal money)
        {
            var printDialog = new PrintDialog();

            var docContext = CreateDocumentContext(barCode, money);
            var docString = TemplatePathReturnPrint.Render(docContext);

            try
            {
                var doc = (FlowDocument) XamlReader.Parse(docString);
                var paginator = ((IDocumentPaginatorSource) doc).DocumentPaginator;

                doc.PageWidth = 280;
                doc.PagePadding = new Thickness(0);
                doc.ColumnGap = 0;
                doc.ColumnWidth = 0;

                var queue = printDialog.PrintQueue;
                var writer = PrintQueue.CreateXpsDocumentWriter(queue);
                writer.Write(paginator);
            }
            catch (System.Exception ex)
            {
                var text =  ex.Message + docString+".";
                FunctionsService.ShowMessageSb(text);
                LogService.Log(TraceLevel.Error, 11, text);
            }
        }

        public static string SpecTransform(string s)
        {
            return s.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("\"", "&quot;");
        }

        private static Hash CreateDocumentContext(string barcode, decimal money)
        {
            var incodeBarcode = "*" + barcode + "*";

            var context = new
                          {
                              BARCODE = barcode,
                              IncodeBarcode = incodeBarcode,
                              MONEY = money
                          };
            return Hash.FromAnonymousObject(context);
        }

        private static Hash CreateDocumentContext(string barcode, string head, IEnumerable<PrintGroupProduct> printGroupProducts, string total, string sumDiscount,
            IEnumerable<PrintTypePay> printPays, decimal rendu, string footer, PrintClientInfo printClientInfo, bool duplicateF)
        {
            var tvases = RepositoryTva.Tvases.Select(t => new PrintTva(t, 0, t.Value, 0)).ToList();
            var listGroup = new List<dynamic>();

            foreach (var group in printGroupProducts)
            {
                var listProducts = new List<dynamic>();

                foreach (var product in group.Products)
                {
                    listProducts.Add(new
                                     {
                                         name = product.Name.Length > 30 ? SpecTransform(product.Name.Substring(0, 30)) : SpecTransform(product.Name),
                                         total = product.Total.ToString("0.00"),
                                         qty = product.Qty,
                                         price = product.Price.ToString("0.00")
                                     });

                    var indx = tvases.FindIndex(l => l.Tva.Id == product.TvaId);

                    if (indx != -1)
                    {
                        tvases[indx].Ttc += product.TvaTotal;
                        tvases[indx].Ht += product.Ht;
                    }
                }

                listGroup.Add(new
                              {
                                  categories = SpecTransform(group.Categories),
                                  Products = listProducts
                              });
            }

            var listPay = new List<dynamic>();

            foreach (var pay in printPays.Where(pay => pay.Money != 0))
            {
                listPay.Add(new {name = pay.Type.CheckName, money = pay.Money.ToString("0.00")});
            }

            var listTva = new List<dynamic>();

            decimal totalTva = 0;
            decimal totalTvaht = 0;

            foreach (var tva in tvases.Where(tva => tva.Ttc != 0))
            {
                listTva.Add(new {name = tva.TvaDecimal + "%", money = tva.Ttc.ToString("0.00"), ht = tva.Ht.ToString("0.00")});
                totalTva += tva.Ttc;
                totalTvaht += tva.Ht;
            }

            if (listTva.Count == 1)
            {
                totalTva = 0;
                totalTvaht = 0;
            }

            var numeroTicket = Environment.NewLine + Config.NameTicket + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            var incodeBarcode = "*" + barcode + "*";

            var context = new
                          {
                              Duplicate = duplicateF ? "*** DUPLICATA ***" : null,
                              GroupProducts = listGroup,
                              Header = head,
                              TotalTTC = total,
                              Pays = listPay,
                              TVA = listTva,
                              TotalTVA = totalTva == 0 ? null : totalTva.ToString("0.00"),
                              TotalTVAHT = totalTvaht == 0 ? null : totalTvaht.ToString("0.00"),
                              BARCODE = barcode,
                              Rendu = (rendu == 0 ? null : rendu.ToString()),
                              Discount = sumDiscount == "0,00" ? null : sumDiscount,
                              Footer = footer,
                              NumeroTicket = numeroTicket,
                              DCBC_name = printClientInfo != null ? printClientInfo.DcbcName : null,
                              DCBC =
                                  printClientInfo != null
                                      ? printClientInfo.Dcbc.Substring(0, 5) + "xx-xxx-xx" + printClientInfo.Dcbc.Substring(printClientInfo.Dcbc.Length - 1, 1)
                                      : null,
                              DCBC_BiloPoints = printClientInfo != null ? printClientInfo.DcbcBiloPoints : null,
                              DCBC_DobavilePoints = printClientInfo != null ? printClientInfo.DcbcDobavilePoints : null,
                              DCBC_OstalosPoints = printClientInfo != null ? printClientInfo.DcbcOstalosPoints : null,
                              DCBC_OtnayliP = printClientInfo != null ? printClientInfo.DcbcOtnayliP : null,
                              IncodeBarcode = incodeBarcode,
                              Facture = ClassProMode.ModePro ? "" : null
                          };


            return Hash.FromAnonymousObject(context);
        }


        public static void Print(List<object[]> objects, DateTime date, int group)
        {
            var printDialog = new PrintDialog();

            var docContext = CreateDocumentContext(objects, date, group);
            var docString = TemplatePathRaportPricePrint.Render(docContext);

            try
            {
                var doc = (FlowDocument) XamlReader.Parse(docString);
                var paginator = ((IDocumentPaginatorSource) doc).DocumentPaginator;

                doc.PageWidth = 280;
                doc.PagePadding = new Thickness(0);
                doc.ColumnGap = 0;
                doc.ColumnWidth = 0;

                var queue = printDialog.PrintQueue;
                var writer = PrintQueue.CreateXpsDocumentWriter(queue);
                writer.Write(paginator);
            }
            catch (System.Exception ex)
            {
                var text =  ex.Message + docString+".";
                FunctionsService.ShowMessageSb(text);
                LogService.Log(TraceLevel.Error, 13, text);
            }
        }

        private static Hash CreateDocumentContext(List<object[]> o, DateTime dt, int group)
        {
            var context = new
                          {
                              dt = dt.ToString(),
                              group,
                              products = o
                          };

            return Hash.FromAnonymousObject(context);
        }
    }
}