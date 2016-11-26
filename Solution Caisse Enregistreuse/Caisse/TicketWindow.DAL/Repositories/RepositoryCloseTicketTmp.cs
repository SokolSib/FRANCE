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
    public class RepositoryCloseTicketTmp
    {
        #region sqripts

        private const string Query = @"SELECT
CustumerId as customerId,
NameTicket as nameTicket,
DateOpen as dateOpen,
DateClose as dateClose,
PayBankChecks as payBankChecks,
PayBankCards as payBankCards,
PayCash as payCash,
PayResto as payResto,
Pay1 as pay1,
Pay2 as pay2,
Pay3 as pay3,
Pay4 as pay4,
Pay5 as pay5,
Pay6 as pay6,
Pay7 as pay7,
Pay8 as pay8,
Pay9 as pay9,
Pay10 as pay10,
Pay11 as pay11,
Pay12 as pay12,
Pay13 as pay13,
Pay14 as pay14,
Pay15 as pay15,
Pay16 as pay16,
Pay17 as pay17,
Pay18 as pay18,
Pay19 as pay19,
Pay20 as pay20,
EstablishmentCustomerId as establishmentCustomerId
    FROM CloseTicketTmp ORDER BY DateOpen DESC";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\CloseTicketTmps.xml";

        public static List<CloseTicketTmp> CloseTicketTmps = new List<CloseTicketTmp>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                CloseTicketTmps = connection.Query<CloseTicketTmp>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CloseTicketTmps");

            foreach (var actionCash in CloseTicketTmps)
                root.Add(CloseTicketTmp.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                CloseTicketTmps.Clear();
                foreach (var element in document.GetXElements("CloseTicketTmps", "rec"))
                    CloseTicketTmps.Add(CloseTicketTmp.FromXElement(element));
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

        public static void MergeProductsInCloseTicket(CloseTicketTmp closeTicket)
        {
            foreach (var checkTicket in closeTicket.ChecksTicket)
            {
                checkTicket.CloseTicketCustomerId = closeTicket.CustomerId;
                var tempProducts = new List<PayProductTmp>(checkTicket.PayProducts);

                foreach (var product in tempProducts)
                {
                    var mergedProducts = checkTicket.PayProducts.Where(p => p.ProductId == product.ProductId).ToList();

                    if (mergedProducts.Count > 1)
                    {
                        for (var i = 1; i < mergedProducts.Count; i++)
                            checkTicket.PayProducts.Remove(mergedProducts[i]);

                        var oneProduct = checkTicket.PayProducts.First(p => p.ProductId == product.ProductId);
                        oneProduct.Qty = mergedProducts.Sum(l => l.Qty);
                        oneProduct.Total = mergedProducts.Sum(l => l.Total);
                    }
                }
            }
        }
    }
}