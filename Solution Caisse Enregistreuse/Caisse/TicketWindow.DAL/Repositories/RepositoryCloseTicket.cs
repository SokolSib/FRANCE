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
    public class RepositoryCloseTicket
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        public static string PathCheck = Config.AppPath + @"Data\check.xml";
        public static string Path = Config.AppPath + @"Data\CloseTickets.xml";
        public static List<CloseTicket> CloseTickets = new List<CloseTicket>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                CloseTickets = connection.Query<CloseTicket>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CloseTickets");

            foreach (var closeTicket in CloseTickets)
                root.Add(CloseTicket.ToXElement(closeTicket));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                CloseTickets.Clear();
                foreach (var element in document.GetXElements("CloseTickets", "rec"))
                    CloseTickets.Add(CloseTicket.FromXElement(element));
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

        public static List<CloseTicket> GetByCloseTicketGId(Guid closeTicketGCustomerId)
        {
            if (CloseTickets.Count == 0) Sync();

            var closeTickets = CloseTickets.FindAll(ct => ct.CloseTicketGCustomerId == closeTicketGCustomerId);

            if (RepositoryCheckTicket.CheckTickets.Count == 0) RepositoryCheckTicket.Sync();

            foreach (var closeTicket in closeTickets)
            {
                closeTicket.ChecksTicket = RepositoryCheckTicket.GetByCloseTicketId(closeTicket.CustomerId);
                foreach (var checkTicket in closeTicket.ChecksTicket)
                    checkTicket.PayProducts = RepositoryPayProduct.GetByCheckTicketId(checkTicket.CustomerId);
            }
            return closeTickets;
        }

        public static int Add(CloseTicket closeTicket)
        {
            CloseTickets.Add(closeTicket);

            SaveFile();

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Execute(InsertQuery, closeTicket);
            return 0;
        }

        public static CloseTicket Close()
        {
            var document = XDocument.Load(PathCheck);
            var closeTicket = CloseTicket.FromCheckXElement(document.GetXElement("checks"));
            closeTicket.DateClose = DateTime.Now;
            document.GetXAttribute("checks", "closeDate").SetValue(closeTicket.DateClose.ToString(Config.DateFormat));
            File.WriteAllText(PathCheck, document.ToString());

            var payProducts = new List<PayProduct>();
            var closeTicketCheckDiscounts = new List<CloseTicketCheckDiscount>();
            foreach (var check in closeTicket.ChecksTicket)
            {
                payProducts.AddRange(check.PayProducts);
                if (check.CheckDiscount != null) closeTicketCheckDiscounts.Add(check.CheckDiscount);
            }

            Add(closeTicket);
            RepositoryCheckTicket.AddRange(closeTicket.ChecksTicket);
            RepositoryPayProduct.AddRange(payProducts);
            RepositoryCloseTicketCheckDiscount.AddRange(closeTicketCheckDiscounts);
            return closeTicket;
        }

        #region scripts

        private const string SelectQuery = @"SELECT
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
CloseTicketGCustumerId as closeTicketGCustomerId
    FROM CloseTicket";

        private const string InsertQuery = @"INSERT INTO CloseTicket
           ([CustumerId]
           ,[NameTicket]
           ,[DateOpen]
           ,[DateClose]
           ,[PayBankChecks]
           ,[PayBankCards]
           ,[PayCash]
           ,[PayResto]
           ,[Pay1]
           ,[Pay2]
           ,[Pay3]
           ,[Pay4]
           ,[Pay5]
           ,[Pay6]
           ,[Pay7]
           ,[Pay8]
           ,[Pay9]
           ,[Pay10]
           ,[Pay11]
           ,[Pay12]
           ,[Pay13]
           ,[Pay14]
           ,[Pay15]
           ,[Pay16]
           ,[Pay17]
           ,[Pay18]
           ,[Pay19]
           ,[Pay20]
           ,[CloseTicketGCustumerId])
     VALUES (
@CustomerId
,@NameTicket
,@DateOpen
,@DateClose
,@PayBankChecks
,@PayBankCards
,@PayCash
,@PayResto
,@Pay1
,@Pay2
,@Pay3
,@Pay4
,@Pay5
,@Pay6
,@Pay7
,@Pay8
,@Pay9
,@Pay10
,@Pay11
,@Pay12
,@Pay13
,@Pay14
,@Pay15
,@Pay16
,@Pay17
,@Pay18
,@Pay19
,@Pay20
,@CloseTicketGCustomerId)";

        #endregion
    }
}