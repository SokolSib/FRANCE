using System;
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
    public class RepositorySyncPlus
    {
        private const string Query = @"SELECT
    customerId,
    date,
    nameCasse
FROM SyncPlus";

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\SyncPluses.xml";

        public static List<SyncPlus> SyncPluses = new List<SyncPlus>();

        private static void SetFromDb()
        {
            var idEstablishment = Config.IdEstablishment;

            using (var connection = ConnectionFactory.CreateConnection())
                SyncPluses = connection.Query<SyncPlus>(Query, new {idEstablishment}).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("SyncPluses");

            foreach (var syncPlus in SyncPluses)
                root.Add(SyncPlus.ToXElement(syncPlus));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                SyncPluses.Clear();
                foreach (var element in document.GetXElements("SyncPluses", "rec"))
                    SyncPluses.Add(SyncPlus.FromXElement(element));
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

        public static void Add(SyncPlus syncPlus)
        {
            SyncPluses.Add(syncPlus);
            SaveFile();

            if (SyncData.IsConnect)
            {
                const string query = "INSERT INTO SyncPlus VALUES (@CustomerId, @Date, @NameCashBox)";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new {syncPlus.CustomerId, syncPlus.Date, syncPlus.NameCashBox});
            }
        }

        public static void Delete(Guid customerId)
        {
            var current = SyncPluses.First(s => s.CustomerId == customerId);
            SyncPluses.Remove(current);

            var document = XDocument.Load(Path);
            var statNationPopupElement =
                document.GetXElements("SyncPluses", "rec")
                    .First(p => p.GetXElementValue("CustomerId").ToGuid() == customerId);
            statNationPopupElement.Remove();
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
            {
                const string query = "DELETE FROM SyncPlus WHERE customerId = @customerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new {customerId});
            }
        }

        public static SyncPlus GetById(Guid customerId)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<SyncPlus>(Query + " WHERE customerId = @customerId", new {customerId}).FirstOrDefault();

            return SyncPluses.FirstOrDefault(sp => sp.CustomerId == customerId);
        }
    }
}