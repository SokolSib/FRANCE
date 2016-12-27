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
    public class RepositoryCloseTicketG
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\CloseTicketGs.xml";

        public static List<CloseTicketG> CloseTicketGs = new List<CloseTicketG>();
        public static string Mess { get; set; }

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                CloseTicketGs = connection.Query<CloseTicketG>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CloseTicketGs");

            foreach (var actionCash in CloseTicketGs)
                root.Add(CloseTicketG.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                CloseTicketGs.Clear();
                foreach (var element in document.GetXElements("CloseTicketGs", "rec"))
                    CloseTicketGs.Add(CloseTicketG.FromXElement(element));
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

        private static void Add(CloseTicketG closeTicketG)
        {
            CloseTicketGs.Add(closeTicketG);

            SaveFile();
       
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, closeTicketG);
        }

        public static List<CloseTicketG> Get(Guid customerId)
        {
            if (SyncData.IsConnect)
                if (customerId == Guid.Empty)
                    using (var connection = ConnectionFactory.CreateConnection())
                        return connection.Query<CloseTicketG>(SelectQuery).ToList();
                else
                    using (var connection = ConnectionFactory.CreateConnection())
                        return
                            connection.Query<CloseTicketG>(SelectQuery + " WHERE CustumerId = @customerId",
                                new {customerId}).ToList();

            return customerId == Guid.Empty
                ? CloseTicketGs.ToList()
                : CloseTicketGs.FindAll(ct => ct.CustomerId == customerId);
        }

        public static void Update(CloseTicketG closeTicketG)
        {
            var idx = CloseTicketGs.FindIndex(otw => otw.CustomerId == closeTicketG.CustomerId);
            if (idx >= 0)
                CloseTicketGs[idx] = closeTicketG;

            var document = XDocument.Load(Path);
            var element = document.GetXElements("CloseTicketGs", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == closeTicketG.CustomerId);
            CloseTicketG.SetXmlValues(element, closeTicketG);
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(UpdateQuery, closeTicketG);
        }

        public static Guid Create()
        {
            var g = new CloseTicketG(Guid.NewGuid(), DateTime.Now, Config.IdEstablishment);
            Add(g);
            return g.CustomerId;
        }
        
        #region Sqripts

        private const string SelectQuery = @"SELECT
CustumerId as customerId,
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
    FROM CloseTicketG";

        private const string InsertQuery = @"INSERT INTO CloseTicketG (
CustumerId,
EstablishmentCustomerId,
DateOpen,
DateClose,
PayBankChecks,
PayBankCards,
PayCash,
PayResto,
Pay1,
Pay2,
Pay3,
Pay4,
Pay5,
Pay6,
Pay7,
Pay8,
Pay9,
Pay10,
Pay11,
Pay12,
Pay13,
Pay14,
Pay15,
Pay16,
Pay17,
Pay18,
Pay19,
Pay20)
    VALUES (
@CustomerId,
@EstablishmentCustomerId,
@DateOpen,
@DateClose,
@PayBankChecks,
@PayBankCards,
@PayCash,
@PayResto,
@Pay1,
@Pay2,
@Pay3,
@Pay4,
@Pay5,
@Pay6,
@Pay7,
@Pay8,
@Pay9,
@Pay10,
@Pay11,
@Pay12,
@Pay13,
@Pay14,
@Pay15,
@Pay16,
@Pay17,
@Pay18,
@Pay19,
@Pay20)";

        private const string UpdateQuery = @"UPDATE CloseTicketG SET 
EstablishmentCustomerId = @EstablishmentCustomerId,
DateOpen = @DateOpen,
DateClose = @DateClose,
PayBankChecks = @PayBankChecks,
PayBankCards = @PayBankCards,
PayCash = @PayCash,
PayResto = @PayResto,
Pay1 = @Pay1,
Pay2 = @Pay2,
Pay3 = @Pay3,
Pay4 = @Pay4,
Pay5 = @Pay5,
Pay6 = @Pay6,
Pay7 = @Pay7,
Pay8 = @Pay8,
Pay9 = @Pay9,
Pay10 = @Pay10,
Pay11 = @Pay11,
Pay12 = @Pay12,
Pay13 = @Pay13,
Pay14 = @Pay14,
Pay15 = @Pay15,
Pay16 = @Pay16,
Pay17 = @Pay17,
Pay18 = @Pay18,
Pay19 = @Pay19,
Pay20 = @Pay20
    WHERE CustumerId = @CustomerId";

        #endregion
    }
}