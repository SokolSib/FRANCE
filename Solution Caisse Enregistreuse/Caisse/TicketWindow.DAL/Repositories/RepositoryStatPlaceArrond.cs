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
    public class RepositoryStatPlaceArrond
    {
        private const string Query = @"SELECT
IdCustomer as idCustomer,
NamePlaceArrond as namePlaceArrond,
QTY as qty
    FROM StatPlaceArrond";

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\StatPlaceArronds.xml";

        public static List<StatPlaceArrond> StatPlaceArronds = new List<StatPlaceArrond>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                StatPlaceArronds = connection.Query<StatPlaceArrond>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("StatPlaceArronds");

            foreach (var actionCash in StatPlaceArronds)
                root.Add(StatPlaceArrond.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                StatPlaceArronds.Clear();
                foreach (var element in document.GetXElements("StatPlaceArronds", "rec"))
                    StatPlaceArronds.Add(StatPlaceArrond.FromXElement(element));
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

        public static void Add(StatPlaceArrond statPlaceArrond)
        {
            StatPlaceArronds.Add(statPlaceArrond);

            var document = XDocument.Load(Path);
            var statPlaceArrondsElement = document.GetXElement("StatPlaceArronds");
            statPlaceArrondsElement.Add(StatPlaceArrond.ToXElement(statPlaceArrond));

            const string query = "INSERT INTO StatPlaceArrond VALUES (@CustomerId, @NamePlaceArrond, @QTY)";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, statPlaceArrond);
        }

        public static void Update(StatPlaceArrond statPlaceArrond)
        {
            var document = XDocument.Load(Path);
            var element = document.GetXElements("StatPlaceArronds", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == statPlaceArrond.CustomerId);
            StatPlaceArrond.SetXmlValues(element, statPlaceArrond);
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = "UPDATE StatPlaceArrond SET NamePlaceArrond = @NamePlaceArrond, QTY = @QTY WHERE IdCustomer = @CustomerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, statPlaceArrond);
            }

            var idx = StatPlaceArronds.FindIndex(ds => ds.CustomerId == statPlaceArrond.CustomerId);
            StatPlaceArronds[idx] = statPlaceArrond;
        }

        public static void Delete(StatPlaceArrond statPlaceArrond)
        {
            var current = StatPlaceArronds.First(s => s.CustomerId == statPlaceArrond.CustomerId);
            StatPlaceArronds.Remove(current);

            var document = XDocument.Load(Path);
            var statPlaceArrondElement = document.GetXElements("StatPlaceArronds", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == statPlaceArrond.CustomerId);
            statPlaceArrondElement.Remove();
            document.Save(Path);

            const string query = "DELETE FROM StatPlaceArrond WHERE IdCustomer = @CustomerId";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, new {statPlaceArrond.CustomerId});
        }
    }
}