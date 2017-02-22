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
    public class RepositoryTva
    {
        #region sqripts

        private const string SelectQuery = @"SELECT
CustumerId as customerId,
Id as id,
val as val
    FROM TVA";

        private const string InsertQuery = @"INSERT INTO TVA (
CustumerId,
Id,
val)
    VALUES (
@CustomerId,
@Id,
@Value)";
        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static XDocument _document;
        private static readonly string Path = Config.AppPath + @"Data\TVA.xml";

        public static List<Tva> Tvases = new List<Tva>();

        private static List<Tva> GetAllFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<Tva>(SelectQuery).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                _document = XDocument.Load(Path);

                Tvases.Clear();
                foreach (var element in _document.GetXElements("tva", "rec"))
                    Tvases.Add(Tva.FromXElement(element));
            }
        }

        private static void SaveFile()
        {
            _document = new XDocument(new XElement("tva"));

            foreach (var tva in Tvases)
                _document.GetXElement("tva").Add(Tva.ToXElement(tva));

            _document.Save(Path);
        }

        public static decimal GetById(int id)
        {
            var idx = Tvases.FindIndex(l => l.Id == id);
            return idx != -1 ? Tvases[idx].Value : 0;
        }

        public static void Sync()
        {
            if (SyncData.IsConnect)
            {
                Tvases = GetAllFromDb();
                SaveFile();
            }
            else LoadFile();
        }

        public static void Add(Tva tva)
        {
            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            document.GetXElement("tva").Add(Tva.ToXElement(tva));
            File.WriteAllText(Path, document.ToString());

            Tvases.Add(tva);

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, tva);
        }
    }
}