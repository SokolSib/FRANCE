using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories.Base;
using TicketWindow.Extensions;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db Sunc.+
    /// </summary>
    public class RepositoryPayProduct : RepositoryPayProductBase<PayProduct>
    {
        #region sqripts
        private const string Query = @"SELECT
IdCheckTicket as idCheckTicket,
ProductId as productId,
Name as name,
Barcode as barcode,
QTY as qty,
TVA as tva,
PriceHT as priceHt,
Total as total,
ChecksTicketCustumerId as checksTicketCustomerId,
ChecksTicketCloseTicketCustumerId as checksTicketCloseTicketCustomerId,
Discount as discount,
sumDiscount as sumDiscount
    FROM PayProducts";

        private const string InsertQuery = @"INSERT INTO PayProducts (
ProductId,
Name,
Barcode,
QTY,
TVA,
PriceHT,
Total,
ChecksTicketCustumerId,
ChecksTicketCloseTicketCustumerId,
IdCheckTicket,
Discount,
sumDiscount)
    VALUES (
@ProductId,
@Name,
@Barcode,
@Qty,
@Tva,
@PriceHt,
@Total,
@ChecksTicketCustomerId,
@ChecksTicketCloseTicketCustomerId,
@IdCheckTicket,
@Discount,
@SumDiscount)";
        #endregion

        private static readonly string Path = Config.AppPath + @"Data\PayProducts.xml";

        public static List<PayProduct> PayProducts = new List<PayProduct>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                PayProducts = connection.Query<PayProduct>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("PayProducts");

            foreach (var actionCash in PayProducts)
                root.Add(PayProduct.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                PayProducts.Clear();
                foreach (var element in document.GetXElements("PayProducts", "rec"))
                    PayProducts.Add(PayProduct.FromXElement(element));
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

        public static List<PayProduct> GetByCheckTicketId(Guid checkTicketId)
        {
            if (SyncData.IsConnect)
                return GetFromDbByCheckTicketIdBase(checkTicketId, Query + " WHERE ChecksTicketCustumerId = @checkTicketId");
            
            return PayProducts.FindAll(p => p.ChecksTicketCustomerId == checkTicketId);
        }

        public static int AddRange(List<PayProduct> payProducts)
        {
            PayProducts.AddRange(payProducts);

            SaveFile();
            int count = payProducts.Count;

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    count = connection.Execute(InsertQuery, payProducts);

            return count;
        }
    }
}