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
    public class RepositoryDevisWeb
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\DevisWebs.xml";

        public static List<DevisWebType> DevisWebs = new List<DevisWebType>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                DevisWebs = connection.Query<DevisWebType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("DevisWebs");

            foreach (var devisWeb in DevisWebs)
                root.Add(DevisWebType.ToXElement(devisWeb));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                DevisWebs.Clear();
                foreach (var element in document.GetXElements("DevisWebs", "rec"))
                    DevisWebs.Add(DevisWebType.FromXElement(element));
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

        public static int AddRange(List<DevisWebType> devisWebs)
        {
            var count = 0;

            DevisWebs.AddRange(devisWebs);

            var document = XDocument.Load(Path);
            foreach (var devisWeb in devisWebs)
                document.GetXElement("DevisWebs").Add(DevisWebType.ToXElement(devisWeb));
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                {
                    connection.Open();
                    var trans = connection.BeginTransaction();

                    count = connection.Execute(InsertQuery, devisWebs, trans);

                    trans.Commit();
                }
            return count;
        }

        #region sqripts

        private const string InsertQuery = @"INSERT INTO DevisWeb (
CustomerId,
IdDevis,
PrixHT,
MonPrixHT,
QTY,
TotalHT,
PayementType,
Operator,
ProductsCustumerId,
InfoClients_custumerId)
    VALUES(
@CustomerId,
@IdDevis,
@PrixHt,
@MonPrixHt,
@Qty,
@TotalHt,
@PayementType,
@Operator,
@ProductsCustumerId,
@InfoClientsCustumerId)";

        private const string SelectQuery = @"SELECT
CustomerId as customerId,
IdDevis as idDevis,
PrixHT as prixHt,
MonPrixHT as monPrixHt,
QTY as qty,
TotalHT as totalHt,
PayementType as payementType,
Operator as operatorData,
ProductsCustumerId as productsCustomerId,
InfoClients_custumerId as infoClientsCustomerId
    FROM DevisWeb";

        #endregion
    }
}