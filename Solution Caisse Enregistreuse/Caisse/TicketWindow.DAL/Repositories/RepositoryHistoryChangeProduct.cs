using System;
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
    public class RepositoryHistoryChangeProduct
    {
        public static readonly string Path = Config.AppPath + @"Data\HistoryChangeProducts.xml";
        public static XDocument Document;

        private static void AddToXml(Guid customerId, string barcode, string name, decimal price, decimal priceGros, decimal price2, decimal priceGros2, DateTime dt,
            int type, int group)
        {
            var elm = new XElement("rec");

            elm.Add(
                new XElement("date", DateTime.Now),
                new XElement("CustomerId", customerId),
                new XElement("barcode", barcode),
                new XElement("name", name),
                new XElement("price", price),
                new XElement("price_", price2),
                new XElement("priceGros", priceGros),
                new XElement("priceGros_", priceGros2),
                new XElement("dateTime", dt),
                new XElement("type", type),
                new XElement("group", group),
                new XElement("print", ""));

            if (Document.GetXElements("HistoryChangeProducts", "rec").Any())
                Document.GetXElement("HistoryChangeProducts").AddFirst(elm);
            else
                Document.GetXElement("HistoryChangeProducts").Add(elm);
        }

        private static string GetArhDirName()
        {
            return System.IO.Path.Combine(Config.AppPath, "data", "arh", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString());
        }

        private static string GetArhFileName()
        {
            return GetArhDirName() + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".xml";
        }

        public static void ToArh()
        {
            var dir = GetArhDirName();

            if (!File.Exists(Path)) SaveFile();

            Directory.CreateDirectory(dir);
            File.Move(Path, GetArhFileName());

            Document = new XDocument(new XElement("HistoryChangeProducts"));
            Document.Save(Path);
        }

        public static void SaveFile()
        {
            Document.Save(Path);
        }

        public static void LoadFile()
        {
            if (Document == null)
            {
                if (File.Exists(Path))
                {
                    try
                    {
                        Document = XDocument.Load(Path);
                    }
                    catch
                    {
                        ToArh();
                        LogService.Log(TraceLevel.Error, 9092);
                    }
                }
                else Document = new XDocument(new XElement("HistoryChangeProducts"));
            }
        }

        public static int GetGroup()
        {
            LoadFile();

            var firstElement = Document.GetXElements("HistoryChangeProducts", "rec").FirstOrDefault();

            if (firstElement != null)
                return firstElement.GetXElementValue("group").ToInt() + 1;

            return 0;
        }

        public static void Compare(XDocument document, ProductType product, int group)
        {
            var elements = document.GetXElements("Product", "rec");
            var element = elements.FirstOrDefault(l => l.GetXElementValue("CustomerId") == product.CustomerId.ToString());

            if (element != null)
            {
                var customerId = element.GetXElementValue("CustomerId").ToGuid();
                var price = element.GetXElementValue("price").ToDecimal();
                var priceGros = element.GetXElementValue("priceGros").ToDecimal();

                if ((price != product.Price) && product.Price > 0)
                {
                    AddToXml(customerId, product.CodeBare, product.Name, price, priceGros, product.Price, product.PriceGros, product.Date ?? DateTime.Now, 0, group);
                }
            }
            else if (product.Price > 0)
                AddToXml(product.CustomerId, product.CodeBare, product.Name, 0, 0, product.Price, product.PriceGros, product.Date ?? DateTime.Now, 1, group);
        }
    }
}