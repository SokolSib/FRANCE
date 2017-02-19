using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class RepositoryStockReal
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\StockReals.xml";

        public static List<StockReal> StockReals = new List<StockReal>();

        private static void SetFromDb()
        {
            const string whereCondition = " WHERE Establishment_CustomerId = @IdEstablishment OR Establishment_CustomerId = @IdEstablishmentGros";

            using (var connection = ConnectionFactory.CreateConnection())
                StockReals = connection.Query<StockReal>(SelectQuery + whereCondition, new {Config.IdEstablishment, Config.IdEstablishmentGros}).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("StockReals");

            foreach (var stockReal in StockReals)
                root.Add(StockReal.ToXElement(stockReal));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                StockReals.Clear();
                foreach (var element in document.GetXElements("StockReals", "rec"))
                    StockReals.Add(StockReal.FromXElement(element));
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

        public static void UpdateProductCount(decimal qty, Guid customerId)
        {
            var stockReal = StockReals.First(sr => sr.CustomerId == customerId);
            stockReal.Qty = qty;

            var document = XDocument.Load(Path);
            var element =
                document.GetXElements("StockReals", "rec")
                    .First(el => el.GetXElementValue("CustomerId").ToGuid() == customerId);
            StockReal.SetXmlValues(element, stockReal);
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = "UPDATE StockReal SET QTY = @Qty WHERE CustomerId = @customerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new {stockReal.Qty, customerId});
            }
        }


        public static void AddProductCount(decimal qty, Guid customerId)
        {
            var stockReal = StockReals.First(sr => sr.CustomerId == customerId);
            stockReal.Qty += qty;

            var document = XDocument.Load(Path);
            var element =
                document.GetXElements("StockReals", "rec")
                    .First(el => el.GetXElementValue("CustomerId").ToGuid() == customerId);
            StockReal.SetXmlValues(element, stockReal);
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = "UPDATE StockReal SET QTY = @Qty WHERE CustomerId = @customerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new { stockReal.Qty, customerId });
            }
        }

        public static StockReal GetByProduct(ProductType product)
        {
            return
                StockReals.FirstOrDefault(
                    s => (s.ProductsCustomerId == product.CustomerId) && (s.IdEstablishment == Config.IdEstablishment));
        }

        public static void UpdatePrice(StockReal stockReal, decimal price)
        {
            stockReal.Price = price;

            var document = XDocument.Load(Path);
            var element = document.GetXElements("StockReals", "rec")
                    .First(el => el.GetXElementValue("CustomerId").ToGuid() == stockReal.CustomerId);
            StockReal.SetXmlValues(element, stockReal);
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = "UPDATE StockReal SET Price = @Price WHERE CustomerId = @CustomerId";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new { stockReal.Price, stockReal.CustomerId });
            }
        }
        
        public static void UpdateProductCountByEstablishment(decimal qty, Guid establishmentCustomerId, Guid productsCustomerId)
        {
            var stockReals = StockReals.FindAll(sr => sr.IdEstablishment == establishmentCustomerId && sr.ProductsCustomerId == productsCustomerId);

            foreach (var stockReal in stockReals)
                AddProductCount(qty, stockReal.CustomerId);
        }

        public static void DeleteById(Guid customerId)
        {
            var current = StockReals.First(s => s.CustomerId == customerId);
            StockReals.Remove(current);

            var document = XDocument.Load(Path);
            var statPlaceArrondElement = document.GetXElements("StockReals", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == customerId);
            statPlaceArrondElement.Remove();
            document.Save(Path);

            const string query = "DELETE FROM StockReal WHERE CustomerId = @customerId";

            using (var connection = ConnectionFactory.CreateConnection())
                connection.Execute(query, new {customerId});
        }

        public static StockReal AddAsNull(Guid productCustomerId, Guid idEstablishment)
        {
            var stockReals = StockReals.FindAll(sr => sr.IdEstablishment == idEstablishment && sr.ProductsCustomerId == productCustomerId);
            if (stockReals.Count == 0)
            {
                var stockReal = new StockReal(Guid.NewGuid(), 0, 10, 0, productCustomerId, idEstablishment);

                StockReals.Add(stockReal);

                var document = XDocument.Load(Path);
                var stockRealsElement = document.GetXElement("StockReals");
                stockRealsElement.Add(StockReal.ToXElement(stockReal));
                document.Save(Path);

                if (SyncData.IsConnect)
                {
                    int result;
                    using (var connection = ConnectionFactory.CreateConnection())
                        result = connection.Execute(InsertQuery, stockReal);

                    if (result == -1)
                    {
                        LogService.Log(TraceLevel.Error, 400);
                        LogService.SqlLog(TraceLevel.Error, stockReal.CustomerId.ToString());
                    }
                    else return stockReal;
                }
                return stockReal;
            }

            return null;
        }

        public static List<StockReal> AddOrUpdateCounts(Guid productCustomerId, Guid idEstablishment, decimal qty, decimal minQty, decimal price)
        {
            var stockReals = StockReals.FindAll(sr => sr.IdEstablishment == idEstablishment && sr.ProductsCustomerId == productCustomerId);
            if (stockReals.Count == 0)
            {
                var stockReal = new StockReal(Guid.NewGuid(), 0, 10, 0, productCustomerId, idEstablishment);

                StockReals.Add(stockReal);

                var document = XDocument.Load(Path);
                var stockRealsElement = document.GetXElement("StockReals");
                stockRealsElement.Add(StockReal.ToXElement(stockReal));
                document.Save(Path);
            }

            for (int i = 0; i < stockReals.Count; i++)
            {
                stockReals[i].Qty += qty;
                stockReals[i].MinQty = minQty;
                stockReals[i].Price = price;
            }

            SaveFile();
            if (SyncData.IsConnect)
            {
                int result;
                using (var connection = ConnectionFactory.CreateConnection())
                    result = connection.Execute(InsertQuery, stockReals);

                if (result == -1)
                {
                    LogService.Log(TraceLevel.Error, 400);
                    LogService.SqlLog(TraceLevel.Error, string.Join(", ", stockReals));
                }
            }

            return stockReals;
        }

        public static List<StockReal> GetFromDbByProductIds(Guid[] customerIdProducts)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return
                        connection.Query<StockReal>(SelectQuery + " WHERE [Establishment_CustomerId] = @CustomerId AND [ProductsCustumerId] IN @customerIdProducts",
                            new {RepositoryEstablishment.Establishment.CustomerId, customerIdProducts}).ToList();

            return StockReals.FindAll(sr => customerIdProducts.ToList().Contains(sr.ProductsCustomerId));
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
    CustomerId as customerId,
    QTY as qty,
    MinQTY as minQty,
    Price as price,
    ProductsCustumerId as productsCustomerId,
    Establishment_CustomerId as idEstablishment
FROM StockReal";

        private const string InsertQuery = @"INSERT INTO StockReal (
    CustomerId,
    QTY,
    MinQTY,
    Price,
    ProductsCustumerId,
    Establishment_CustomerId)
VALUES (
    @CustomerId,
    @Qty,
    @MinQty,
    @Price,
    @ProductsCustomerId,
    @IdEstablishment)";

        #endregion
    }
}