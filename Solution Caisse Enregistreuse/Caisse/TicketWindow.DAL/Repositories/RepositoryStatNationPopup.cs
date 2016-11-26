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
    public class RepositoryStatNationPopup
    {
        private const string Query = @"SELECT
IdCustomer as idCustomer,
NameNation as nameNation,
QTY as qty
    FROM StatNationPopup";

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\StatNationPopups.xml";

        public static List<StatNationPopup> StatNationPopups = new List<StatNationPopup>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                StatNationPopups = connection.Query<StatNationPopup>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("StatNationPopups");

            foreach (var statNationPopup in StatNationPopups)
                root.Add(StatNationPopup.ToXElement(statNationPopup));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                StatNationPopups.Clear();
                foreach (var element in document.GetXElements("StatNationPopups", "rec"))
                    StatNationPopups.Add(StatNationPopup.FromXElement(element));
            }
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

        public static void Add(StatNationPopup statNationPopup)
        {
            StatNationPopups.Add(statNationPopup);

            var document = XDocument.Load(Path);
            var statNationPopupsElement = document.GetXElement("StatNationPopups");
            statNationPopupsElement.Add(StatNationPopup.ToXElement(statNationPopup));

            const string query = "INSERT INTO StatNationPopup VALUES (@CustomerId, @NameNation, @Qty)";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, statNationPopup);
        }

        public static void Update(StatNationPopup statNationPopup)
        {
            var document = XDocument.Load(Path);
            var element = document.GetXElements("StatNationPopups", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == statNationPopup.CustomerId);
            StatNationPopup.SetXmlValues(element, statNationPopup);
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = @"UPDATE StatNationPopup SET NameNation = @NameNation, QTY = @Qty WHERE IdCustomer = @CustomerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new {statNationPopup.NameNation, statNationPopup.Qty, statNationPopup.CustomerId});
            }

            var idx = StatNationPopups.FindIndex(ds => ds.CustomerId == statNationPopup.CustomerId);
            StatNationPopups[idx] = statNationPopup;
        }

        public static void Delete(StatNationPopup statNationPopup)
        {
            var current = StatNationPopups.First(s => s.CustomerId == statNationPopup.CustomerId);
            StatNationPopups.Remove(current);

            var document = XDocument.Load(Path);
            var statNationPopupElement = document.GetXElements("StatNationPopups", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == statNationPopup.CustomerId);
            statNationPopupElement.Remove();
            document.Save(Path);

            const string query = "DELETE FROM StatNationPopup WHERE IdCustomer = @CustomerId";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, new {statNationPopup.CustomerId});
        }
    }
}