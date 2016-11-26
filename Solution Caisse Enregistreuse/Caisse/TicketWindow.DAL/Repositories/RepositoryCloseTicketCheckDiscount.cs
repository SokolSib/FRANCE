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
    public class RepositoryCloseTicketCheckDiscount
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\CloseTicketCheckDiscounts.xml";

        public static List<CloseTicketCheckDiscount> CloseTicketCheckDiscounts = new List<CloseTicketCheckDiscount>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                CloseTicketCheckDiscounts = connection.Query<CloseTicketCheckDiscount>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CloseTicketCheckDiscounts");

            foreach (var closeTicketCheckDiscount in CloseTicketCheckDiscounts)
                root.Add(CloseTicketCheckDiscount.ToXElement(closeTicketCheckDiscount));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                CloseTicketCheckDiscounts.Clear();
                foreach (var element in document.GetXElements("CloseTicketCheckDiscounts", "rec"))
                    CloseTicketCheckDiscounts.Add(CloseTicketCheckDiscount.FromXElement(element));
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

        public static CloseTicketCheckDiscount Get(Guid customerId)
        {
            if (CloseTicketCheckDiscounts.Count == 0) Sync();

            return CloseTicketCheckDiscounts.FirstOrDefault(c => c.CloseTicketCheckcCustomer == customerId);
        }

        public static int AddRange(List<CloseTicketCheckDiscount> closeTicketCheckDiscount)
        {
            int count;

            using (var connection = ConnectionFactory.CreateConnection())
                count = connection.Execute(InsertQuery, closeTicketCheckDiscount);

            if (count == closeTicketCheckDiscount.Count)
                CloseTicketCheckDiscounts.AddRange(closeTicketCheckDiscount);

            return count;
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
CustumerId as customerId,
CloseTicketCheckcCustomer as closeTicketCheckcCustomer,
DiscountCardsCustumerId as discountCardsCustomerId,
DCBC as dcbc,
DCBC_BiloPoints as dcbcBiloPoints,
DCBC_DobavilePoints as dcbcDobavilePoints,
DCBC_OtnayliPoints as dcbcOtnayliPoints,
DCBC_OstalosPoints as dcbcOstalosPoints,
DCBC_name as dcbcname
    FROM CloseTicketCheckDiscount";

        private const string InsertQuery = @"INSERT INTO CloseTicketCheckDiscount (
CustumerId,
CloseTicketCheckcCustomer,
DiscountCardsCustumerId,
DCBC,
DCBC_BiloPoints,
DCBC_DobavilePoints,
DCBC_OtnayliPoints,
DCBC_OstalosPoints,
DCBC_name)
    VALUES (
@CustomerId,
@CloseTicketCheckcCustomer,
@DiscountCardsCustomerId,
@Dcbc,
@DcbcBiloPoints,
@DcbcDobavilePoints,
@DcbcOtnayliPoints,
@DcbcOstalosPoints,
@DcbcName)";

        #endregion
    }
}