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
    public class RepositoryCheckTicket : RepositoryCheckTicketBase<CheckTicket>
    {
        public static List<CheckTicket> CheckTickets = new List<CheckTicket>();
        private static readonly string Path = Config.AppPath + @"Data\CheckTickets.xml";

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                CheckTickets = connection.Query<CheckTicket>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CheckTickets");

            foreach (var checkTicket in CheckTickets)
                root.Add(CheckTicket.ToXElement(checkTicket));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                CheckTickets.Clear();
                foreach (var element in document.GetXElements("CheckTickets", "rec"))
                    CheckTickets.Add(CheckTicket.FromXElement(element));
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

        public static List<CheckTicket> GetByCloseTicketId(Guid customerId)
        {
            var checkTickets = SyncData.IsConnect
                ? GetFromDbByCloseTicketIdBase(customerId, SelectQuery + " WHERE CloseTicketCustumerId = @customerId").ToList()
                : CheckTickets.FindAll(c => c.CloseTicketCustomerId == customerId);

            if (RepositoryCloseTicketCheckDiscount.CloseTicketCheckDiscounts.Count == 0)
                RepositoryCloseTicketCheckDiscount.Sync();

            foreach (var checkTicket in checkTickets)
                checkTicket.CheckDiscount = RepositoryCloseTicketCheckDiscount.Get(checkTicket.CustomerId);

            return checkTickets;
        }

        public static int AddRange(List<CheckTicket> checks)
        {
            CheckTickets.AddRange(checks);
            SaveFile();

            var count = checks.Count;
            if (SyncData.IsConnect)
                count = AddRangeToDb(checks);

            return count;
        }

        private static int AddRangeToDb(List<CheckTicket> checks)
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Execute(InsertQuery, checks);
        }

        #region scripts

        private const string SelectQuery = @"SELECT
CustumerId as customerId,
BarCode as barCode,
Date as date,
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
CloseTicketCustumerId as closeTicketCustomerId,
TotalTTC as totalTtc,
Rendu as rendu
    FROM ChecksTicket";

        private const string InsertQuery = @"INSERT INTO ChecksTicket (
CustumerId,
BarCode,
[Date],
PayBankChecks,
PayBankCards,
PayCash,
PayResto,
CloseTicketCustumerId,
Rendu,
TotalTTC)
    VALUES (
@CustomerId,
@BarCode,
@Date,
@PayBankChecks,
@PayBankCards,
@PayCash,
@PayResto,
@CloseTicketCustomerId,
@Rendu, 
@TotalTtc)";

        #endregion
    }
}