using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using System.Windows.Xps;
using dotTemplate = DotLiquid.Template;
using System.Windows.Markup;
using System.Printing;


namespace ticketwindow.Class
{
    class ClassDotLiquid
    {
        //    private static  Stream stream { get; set; }
        private static string pathCheckPrint = System.AppDomain.CurrentDomain.BaseDirectory + @"Template\CheckPrint.mxaml";
        private static string pathReturnPrint = System.AppDomain.CurrentDomain.BaseDirectory + @"Template\ReturnPrint.mxaml";

        private static dotTemplate template_pathCheckPrint { get; set; }
        private static dotTemplate template_pathReturnPrint { get; set; }

        private static dotTemplate _func(string path)
        {
            try
            {
                var stream = new FileStream(path, FileMode.Open);
                dotTemplate template;
                using (stream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var templateString = reader.ReadToEnd();

                        template = dotTemplate.Parse(templateString);

                    }
                }
                return template;
            }
            catch (Exception ex)
            {
                string s = "Ошибка code 0092 " + path + ex.Message;
                new ClassFunctuon().showMessageSB(s);
                new ClassLog(s);

                return null;
            }
        }

        public static void setPath(int select)
        {
            switch (select)
            {
                case 0: 
                    template_pathCheckPrint = _func(pathCheckPrint); 
                    break;
                case 1: 
                    template_pathReturnPrint = _func(pathReturnPrint); 
                    break;
            }

            
        }

        public static void prt(string barcode, string head, List<ClassPrintCheck.groupProduct> lProduct, string total, string sumDiscount, List<ClassPrintCheck.Pays> LPay, decimal rendu, string footer, ticketwindow.Class.ClassPrintCheck.infoOfClient InfoOfClient, bool DuplicateF)
        {


            PrintDialog printDialog = new PrintDialog();

            var docContext = CreateDocumentContext(barcode, head, lProduct, total, sumDiscount, LPay, rendu, footer, InfoOfClient, DuplicateF);
            var docString = template_pathCheckPrint.Render(docContext);

            try
            {
                var doc = (FlowDocument)XamlReader.Parse(docString);

                var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;


                doc.PageWidth = 280;
                doc.PagePadding = new Thickness(0);
                doc.ColumnGap = 0;
                doc.ColumnWidth = 0;

                PrintQueue queue = printDialog.PrintQueue;
                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(queue);
                writer.Write(paginator);
            }
            catch (Exception ex)
            {

                string text = "KOD 011" + ex.Message + docString;

                new ClassFunctuon().showMessageSB(text);

                new ClassLog(text );
            }
        }

        public static void prt(string bar_code, List<ClassProducts> p, decimal m)
        {
            PrintDialog printDialog = new PrintDialog();

            var docContext = CreateDocumentContext(bar_code, m);
            var docString = template_pathReturnPrint.Render(docContext);

            try
            {
                var doc = (FlowDocument)XamlReader.Parse(docString);

                var paginator = ((IDocumentPaginatorSource)doc).DocumentPaginator;


                doc.PageWidth = 280;
                doc.PagePadding = new Thickness(0);
                doc.ColumnGap = 0;
                doc.ColumnWidth = 0;

                PrintQueue queue = printDialog.PrintQueue;
                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(queue);
                writer.Write(paginator);
            }
            catch (Exception ex)
            {

                string text = "KOD 011" + ex.Message + docString;

                new ClassFunctuon().showMessageSB(text);

                new ClassLog(text);
            }
   
        }

        public class mTva
        {
            public int id { get; set; }
            public decimal val { get; set; }
            public decimal ht { get; set; }
            public decimal tva { get; set; }
        }

        private static string specTransform (string s)
        {
            return s.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("\"", "&quot;");
        }

        private static DotLiquid.Hash CreateDocumentContext(string barcode, decimal money)
        {
            string incodeBarcode = "*" + barcode + "*";

            var context = new
            {
                BARCODE = barcode,
                IncodeBarcode = incodeBarcode,
                MONEY = money
            };
            return DotLiquid.Hash.FromAnonymousObject(context);
        }
        private static DotLiquid.Hash CreateDocumentContext(string barcode, string head, List<ClassPrintCheck.groupProduct> lProduct, string total, string sumDiscount, List<ClassPrintCheck.Pays> LPay, decimal rendu, string footer, ticketwindow.Class.ClassPrintCheck.infoOfClient InfoOfClient, bool DuplicateF )
        {
            List<mTva> ltva = new List<mTva>();

            foreach (ClassTVA.tva t in ClassTVA.listTVA)
            {
                mTva n = new mTva();
                n.id = t.id;
                n.val = t.val;
                n.ht = 0.0m;
                n.tva = 0.0m;
                ltva.Add(n);
            }

            List<dynamic> listGroup = new List<dynamic>();

            foreach (ClassPrintCheck.groupProduct group in lProduct)
            {
                List<dynamic> listProducts = new List<dynamic>();

                foreach (ClassPrintCheck.groupProduct.product product in group.products)
                {
                    listProducts.Add(new
                    {
                        name = product.name.Length > 30 ? specTransform(product.name.Substring(0, 30)) : specTransform(product.name),
                        total = product.total.ToString("0.00"),
                        qty = product.qty,
                        price = product.price.ToString("0.00")

                    });

                    int indx = ltva.FindIndex(l => l.id == product.tva);

                    if (indx != -1)
                    {
                        ltva[indx].tva += product.TvaTotal;
                        ltva[indx].ht += product.HT;
                    }
                }

                listGroup.Add(new
                {
                    categories = specTransform( group.categories),
                    Products = listProducts
                });
            }

            List<dynamic> listPay = new List<dynamic>();

            foreach (ClassPrintCheck.Pays pay in LPay)
            {
                if (pay.money != 0)
                    listPay.Add(new { name = pay.type.Name, money = pay.money.ToString("0.00") });
            }

            List<dynamic> listTVA = new List<dynamic>();

            decimal totalTva = 0;
            decimal totalTVAHT = 0;

            foreach (mTva tva in ltva)
            {
                if (tva.tva != 0)
                {
                    listTVA.Add(new { name = tva.val + "%", money = tva.tva.ToString("0.00"), ht = tva.ht.ToString("0.00") });
                    totalTva += tva.tva;
                    totalTVAHT += tva.ht;
                }
            }

            if (listTVA.Count == 1)
            {
                totalTva = 0;
                totalTVAHT = 0;
            }

            string numeroTicket = Environment.NewLine + ClassGlobalVar.nameTicket + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

            /*      System.Drawing.Image img = new ClassImageBarCode().get_bc(barcode);

                  var streamMemory = new System.IO.MemoryStream();
                  img.Save(streamMemory, System.Drawing.Imaging.ImageFormat.Jpeg);
                  streamMemory.Position = 0;
                  string path =  @"d:\bc" + barcode.Substring(8, 10) +".jpg";

                  FileStream fileStream = File.Create(path, (int)streamMemory.Length);
            
                  byte[] bytesInStream = new byte[streamMemory.Length];
                  streamMemory.Read(bytesInStream, 0, bytesInStream.Length);
           
                  fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                 */
            string incodeBarcode = "*"+barcode+"*";// BarCode.BarcodeConverter128.StringToBarcode(barcode);

            /*
            incodeBarcode = barcode; 

            if (incodeBarcode.IndexOfAny(new char[] { '<', '>', '&' }) != -1)
            {


                new ClassLog("BARCODE " + barcode + " INCORRECT '<','>'  " + incodeBarcode);

                incodeBarcode = barcode;// incodeBarcode.Replace("<", "&lt;").Replace(">", "&gt;");
            }
            */

            var context = new
            {
                Duplicate = DuplicateF ? "*** DUPLICATA ***" : null,
                GroupProducts = listGroup,
                Header = head,
                TotalTTC = total,
                Pays = listPay,
                TVA = listTVA,
                TotalTVA = totalTva == 0 ? null : totalTva.ToString("0.00"),
                TotalTVAHT = totalTVAHT == 0 ? null : totalTVAHT.ToString("0.00"),
                BARCODE = barcode,
                Rendu = (rendu == 0 ? null : rendu.ToString()),
                Discount = sumDiscount == "0,00" ? null : sumDiscount,
                Footer = footer,
                NumeroTicket = numeroTicket,
                DCBC_name = InfoOfClient != null ? InfoOfClient.DCBC_name : null,
                DCBC = InfoOfClient != null ? InfoOfClient.DCBC.Substring(0, 5) + "xx-xxx-xx" + InfoOfClient.DCBC.Substring(InfoOfClient.DCBC.Length - 1, 1) : null,
                DCBC_BiloPoints = InfoOfClient != null ? InfoOfClient.DCBC_BiloPoints : null,
                DCBC_DobavilePoints = InfoOfClient != null ? InfoOfClient.DCBC_DobavilePoints : null,
                DCBC_OstalosPoints = InfoOfClient != null ? InfoOfClient.DCBC_OstalosPoints : null,
                DCBC_OtnayliP = InfoOfClient != null ? InfoOfClient.DCBC_OtnayliP : null,
                IncodeBarcode = incodeBarcode,
                Facture = ClassProMode.modePro ? "" : null
            };



            return DotLiquid.Hash.FromAnonymousObject(context);
        }


    }
}
