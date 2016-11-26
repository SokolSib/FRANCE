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
    public class RepositoryDevisId
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\DevisIds.xml";

        public static List<DevisIdType> DevisIds = new List<DevisIdType>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                DevisIds = connection.Query<DevisIdType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("DevisIds");

            foreach (var devisId in DevisIds)
                root.Add(DevisIdType.ToXElement(devisId));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                DevisIds.Clear();
                foreach (var element in document.GetXElements("DevisIds", "rec"))
                    DevisIds.Add(DevisIdType.FromXElement(element));
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

            foreach (var devisId in DevisIds)
                devisId.DivisWebs = RepositoryDevisWeb.DevisWebs.FindAll(d => d.IdDevis == devisId.Id);
        }

        public static int Add(DevisIdType di)
        {
            DevisIds.Add(di);

            var document = XDocument.Load(Path);
            document.GetXElement("DevisIds").Add(DevisIdType.ToXElement(di));
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Execute(InsertQuery, di);
            return 0;
        }

        public static int GetMaxId()
        {
            Sync();
            return DevisIds.Max(di => di.Id);
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
Id as id,
Date as date,
[Close] as [close],
infoClientsCustomerId,
Total as total
    FROM DevisId";

        private const string InsertQuery = @"INSERT INTO DevisId (
Date,
[Close],
infoClientsCustomerId,
Total)
    VALUES (
@Date,
@Close,
@InfoClientsCustomerId,
@Total)";

        #endregion
    }
}