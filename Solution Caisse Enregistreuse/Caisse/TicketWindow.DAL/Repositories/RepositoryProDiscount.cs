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
    public class RepositoryProDiscount
    {
        #region sqripts

        private const string Query = @"SELECT 
CustomerId as customerId,
CustomerIdInfoClients as customerIdInfoClients,
IdInfoClientsDiscountsType as idInfoClientsDiscountsType
  FROM InfoCliientsDisconts";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\ProDiscount.xml";

        public static List<ProDiscount> ProDiscounts = new List<ProDiscount>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                ProDiscounts = connection.Query<ProDiscount>(Query).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                ProDiscounts.Clear();
                foreach (var element in document.GetXElements("ProDiscounts", "rec"))
                    ProDiscounts.Add(ProDiscount.FromXElement(element));
            }
        }

        private static void SaveFile()
        {
            var document = new XDocument(new XElement("ProDiscounts"));

            foreach (var proDiscount in ProDiscounts)
                document.GetXElement("ProDiscounts").Add(ProDiscount.ToXElement(proDiscount));

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