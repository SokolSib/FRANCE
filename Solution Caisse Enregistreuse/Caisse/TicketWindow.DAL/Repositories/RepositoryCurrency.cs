using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db Sunc.+
    /// </summary>
    public class RepositoryCurrency
    {
        private const string Query = @"SELECT
    CustomerId as customerId,
    Currency_money as currencyMoney,
    [Desc] as 'desc',
    TypesPayId as typesPayId
FROM Currency";

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\Currency.xml";

        public static List<Currency> Currencys = new List<Currency>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                Currencys = connection.Query<Currency>(Query).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);
                var elements = document.GetXElements("Currency", "rec");

                Currencys.Clear();
                foreach (var c in elements)
                    Currencys.Add(Currency.FromXElement(c));
            }
        }

        private static void SaveFile()
        {
            var document = new XDocument(new XElement("Currency"));

            document.GetXElements("Currency", "rec").Remove();

            foreach (var c in Currencys)
                document.GetXElement("Currency").Add(Currency.ToXElement(c));

            document.Save(Path);
        }

        public static void Sync()
        {
            if (SyncData.IsConnect)
            {
                SetFromDb();
                SaveFile();
            }
            else LoadFile();
        }
    }
}