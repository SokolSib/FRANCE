using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml.+
    /// </summary>
    public class RepositoryCheck
    {
        public static XDocument C;
        public static XDocument Document;
        public static XDocument DocumentEnAttenete;
        public static XDocument DocumentProductCheck;
        public static string Path = Config.AppPath + @"Data\check.xml";
        public static string PathEnAttenete = Config.AppPath + @"Data\en_attenete.xml";
        public static string PathProductCheck = AppDomain.CurrentDomain.BaseDirectory + @"Data\b.xml";

        private static decimal GetPay(IEnumerable<XAttribute> attributes)
        {
            return attributes.Sum(a => a.Value.ToDecimal());
        }

        public static bool OpenTicket()
        {
            if (!File.Exists(Path))
            {
                Document = new XDocument(new XElement("checks",
                    new XAttribute("ticket", Config.NameTicket),
                    new XAttribute("openDate", DateTime.Now.ToString(Config.DateFormat)),
                    new XAttribute("closeDate", ""),
                    new XAttribute("idTicketWindowG", GlobalVar.TicketWindowG),
                    new XAttribute("idTicketWindow", GlobalVar.TicketWindow)));

                Document.Save(Path);
                return true;
            }

            return false;
        }

        public static void GetDucument()
        {
            if (Document == null) Document = XDocument.Load(Path);
        }

        public static void CloseTicket()
        {
            if (File.Exists(Path))
            {
                GetDucument();

                Document.GetXAttribute("checks", "closeDate").Value = DateTime.Now.ToString(Config.DateFormat);

                var checkElement = Document.GetXElementOrNull("checks", "check");

                foreach (var typePay in RepositoryTypePay.TypePays)
                    Document.GetXElement("checks")
                        .Add(new XAttribute(typePay.NameCourt, checkElement != null ? GetPay(Document.GetXAttributes("checks", "check", typePay.NameCourt)) : 0));

                Document.GetXElement("checks").Add(new XAttribute("Rendu", checkElement != null ? GetPay(Document.GetXAttributes("checks", "check", "Rendu")) : 0));
                Document.GetXElement("checks").Add(new XAttribute("sum", checkElement != null ? GetPay(Document.GetXAttributes("checks", "check", "sum")) : 0));
                Document.Save(Path);

                var dir = Config.AppPath + @"\Data\" + DateTime.Now.Year + @"\" + DateTime.Now.Month;
                Directory.CreateDirectory(dir);

                var file = dir + @"\" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xml";

                File.Move(Path, file);
            }
        }

        public static void ReturnCheck(CheckTicket check, List<PayProduct> payProducts)
        {
            var returnCheck = new CheckTicket(check.CustomerId,
                check.BarCode,
                DateTime.Now, //check.Date,
                0, //check.PayBankChecks,
                0, //check.PayBankCards,
                0, //check.PayCash,
                0, //check.PayResto,
                0, //check.Pay1,
                0, //check.Pay2,
                0, //check.Pay3,
                0, //check.Pay4,
                0, //check.Pay5,
                0, //check.Pay6,
                0, //check.Pay7,
                0, //check.Pay8,
                0, //check.Pay9,
                0, //check.Pay10,
                0, //check.Pay11,
                0, //check.Pay12,
                0, //check.Pay13,
                0, //check.Pay14,
                0, //check.Pay15,
                0, //check.Pay16,
                0, //check.Pay17,
                0, //check.Pay18,
                0, //check.Pay19,
                0, //check.Pay20,
                check.CloseTicketCustomerId,
                -payProducts.Sum(p => p.Total), //check.TotalTtc,
                0 //check.Rendu
                );

            returnCheck.BarCode = GetBarCodeCheck();
            returnCheck.ReturnBarCode = check.BarCode;

            foreach (var n in payProducts.Select(p => new PayProduct(
                p.IdCheckTicket,
                p.ProductId,
                p.Name,
                p.Barcode,
                -p.Qty,
                p.Tva,
                -p.PriceHt,
                -p.Total,
                p.ChecksTicketCustomerId,
                p.ChecksTicketCloseTicketCustomerId,
                0,
                0)))
                returnCheck.PayProducts.Add(n);

            var checkelement = CheckTicket.ToCheckXElement(returnCheck);
            Document.GetXElement("checks").Add(checkelement);

            Document.Save(Path);
        }

        public static CloseTicketTmp GetCloseTicketTmp()
        {
            var closeTicket = CloseTicketTmp.FromCheckXElement(Document);
            RepositoryCloseTicketTmp.MergeProductsInCloseTicket(closeTicket);
            return closeTicket;
        }

        public static int GetCountOfProductInCheckFromBuf()
        {
            try
            {
                return DocumentProductCheck.GetXElements("check", "product").Count();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetCountOfProductInCheckFromEnAttenete()
        {
            try
            {
                return DocumentEnAttenete.GetXElements("checks", "check").Count();
            }
            catch
            {
                return 0;
            }
        }


        public static string GetBarCodeCheck()
        {
            var date = DateTime.Now;
            GetDucument();

            try
            {
                var count = Document.GetXElements("checks", "check").Count() + 1;
                return Config.NumberTicket + date.ToString("HHmmss") + string.Format("{0:00000}", count);
            }
            catch
            {
                if (OpenTicket())
                {
                    const string message = "Не кореектный файл check.xml , испр.";
                    LogService.Log(TraceLevel.Error, 3457, message);

                    return Config.NumberTicket + date.ToString("HHmmss") + string.Format("{0:00000}", Document.GetXElements("checks", "check").Count() + 1);
                }
                const string message2 = "Не кореектный файл check.xml , не испр, тк файл сущ-ет.";
                LogService.Log(TraceLevel.Error, 3458, message2);
                return string.Empty;
            }
        }

        public static Guid GetTicketWindow()
        {
            if (!File.Exists(Path)) return Guid.Empty;

            if (Document == null) Document = XDocument.Load(Path);
            return new Guid(Document.GetXAttribute("checks", "idTicketWindow").Value);
        }
    }
}