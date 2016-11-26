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
    public class RepositorySyncPlusProduct
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\ActionHashBoxes.xml";

        public static List<SyncPlusProductType> SyncPlusProducts = new List<SyncPlusProductType>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                SyncPlusProducts = connection.Query<SyncPlusProductType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("SyncPlusProducts");

            foreach (var syncPlusProduct in SyncPlusProducts)
                root.Add(SyncPlusProductType.ToXElement(syncPlusProduct));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                SyncPlusProducts.Clear();
                foreach (var element in document.GetXElements("SyncPlusProducts", "rec"))
                    SyncPlusProducts.Add(SyncPlusProductType.FromXElement(element));
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

        public static void Delete(Guid customerId)
        {
            var current = SyncPlusProducts.First(s => s.CustomerId == customerId);
            SyncPlusProducts.Remove(current);

            var document = XDocument.Load(Path);
            var statNationPopupElement = document.GetXElements("SyncPlusProducts", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == customerId);
            statNationPopupElement.Remove();
            File.WriteAllText(Path, document.ToString());

            const string query = "DELETE FROM SyncPlusProducts WHERE customerId = @customerId";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, new {customerId});
        }

        public static void Add(SyncPlusProductType syncPlusProduct)
        {
            SyncPlusProducts.Add(syncPlusProduct);

            var document = XDocument.Load(Path);
            var syncPlusProductsElement = document.GetXElement("SyncPlusProducts");
            syncPlusProductsElement.Add(SyncPlusProductType.ToXElement(syncPlusProduct));
            File.WriteAllText(Path, document.ToString());

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(InsertQuery, syncPlusProduct);
        }

        public static List<SyncPlusProductType> GetByIdSyncPlus(Guid customerIdSyncPlus)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<SyncPlusProductType>(SelectQuery + " WHERE customerIdSyncPlus = @customerIdSyncPlus", new {customerIdSyncPlus}).ToList();

            return SyncPlusProducts.FindAll(spp => spp.CustomerIdSyncPlus == customerIdSyncPlus);
        }

        public static SyncPlusProductType GetById(Guid customerId)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<SyncPlusProductType>(SelectQuery + " WHERE customerId = @customerId", new {customerId}).FirstOrDefault();

            return SyncPlusProducts.FirstOrDefault(spp => spp.CustomerId == customerId);
        }

        public static void SetCheckToDb(XDocument check)
        {
            var suncPlus = RepositorySyncPlus.GetById(Config.CustomerId);

            if (suncPlus == null)
            {
                suncPlus = new SyncPlus(Config.CustomerId, DateTime.Now, Config.NameTicket);
                RepositorySyncPlus.Add(suncPlus);
            }

            var suncPlusProduct = new SyncPlusProductType(Guid.NewGuid(), check.ToString(), suncPlus.Date, suncPlus.CustomerId);

            Add(suncPlusProduct);
        }

        public static XDocument GetCheck(Guid customerId)
        {
            var suncPlusProduct = GetById(customerId);
            Delete(customerId);

            var suncPlusProducts = GetByIdSyncPlus(suncPlusProduct.CustomerIdSyncPlus);

            if (suncPlusProducts.Count == 0)
                RepositorySyncPlus.Delete(suncPlusProduct.CustomerIdSyncPlus);

            return suncPlusProduct.Check;
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
customerId,
[check],
date,
customerIdSyncPlus
    FROM SyncPlusProducts";

        private const string InsertQuery = @"INSERT INTO SyncPlusProducts VALUES (
@CustomerId, 
@CheckText, 
@Date,
@CustomerIdSyncPlus)";

        #endregion
    }
}