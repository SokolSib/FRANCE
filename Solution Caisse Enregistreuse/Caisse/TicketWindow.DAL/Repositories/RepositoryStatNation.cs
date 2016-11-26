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
    public class RepositoryStatNation
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\StatNations.xml";

        public static List<StatNation> StatNations = new List<StatNation>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                StatNations = connection.Query<StatNation>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("StatNations");

            foreach (var statNation in StatNations)
                root.Add(StatNation.ToXElement(statNation));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                StatNations.Clear();
                foreach (var element in document.GetXElements("StatNations", "rec"))
                    StatNations.Add(StatNation.FromXElement(element));
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

        public static void Add(StatNation statNation)
        {
            StatNations.Add(statNation);

            var document = XDocument.Load(Path);
            var statNationsElement = document.GetXElement("StatNations");
            statNationsElement.Add(StatNation.ToXElement(statNation));

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(InsertQuery, statNation);
        }

        #region

        private const string SelectQuery = @"SELECT
CustomerId as customerId,
Date as date,
IdCaisse as idCaisse,
IdNation as idNation,
IdArrond as idArrond
    FROM StatNation";

        private const string InsertQuery = @"INSERT INTO StatNation VALUES (
@CustomerId, 
@Date,
@IdCaisse, 
@IdNation, 
@IdArrond)";

        #endregion
    }
}