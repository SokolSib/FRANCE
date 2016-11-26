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
    public class RepositoryPayProductTmp : RepositoryPayProductBase<PayProductTmp>
    {
        #region sqripts

        private const string SelectQuery = @"SELECT
IdCheckTicket as idCheckTicket,
ProductId as productId,
Name as name,
Barcode as barcode,
QTY as qty,
TVA as tva,
PriceHT as priceHt,
Total as total,
ChecksTicketTmpCustumerId as checksTicketCustomerId,
Discount as discount,
sumDiscount as sumDiscount
    FROM PayProductsTmp";

        #endregion

        private static List<PayProductTmp> _payProductsTmp = new List<PayProductTmp>();
        private static readonly string Path = Config.AppPath + @"Data\PayProductsTmp.xml";

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                _payProductsTmp = connection.Query<PayProductTmp>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("PayProducts");

            foreach (var checkTicketTmp in _payProductsTmp)
                root.Add(PayProductTmp.ToXElement(checkTicketTmp));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                _payProductsTmp.Clear();
                foreach (var element in document.GetXElements("PayProducts", "rec"))
                    _payProductsTmp.Add(PayProductTmp.FromXElement(element));
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

        public static List<PayProductTmp> GetByCheckTicketId(Guid checkTicketId)
        {
            return SyncData.IsConnect
                ? GetFromDbByCheckTicketIdBase(checkTicketId, SelectQuery + " WHERE ChecksTicketTmpCustumerId = @checkTicketId")
                : _payProductsTmp.FindAll(p => p.ChecksTicketCustomerId == checkTicketId);
        }
    }
}