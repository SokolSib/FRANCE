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
    public class RepositoryCheckTicketTmp : RepositoryCheckTicketBase<CheckTicketTmp>
    {
        #region sqripts

        private const string SelectQuery = @"Select
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
CloseTicketTmpCustumerId as closeTicketCustomerId,
TotalTTC as totalTtc,
Rendu as rendu,
DCBC as dcbc,
DCBC_BiloPoints as dcbcBiloPoints,
DCBC_DobavilePoints as dcbcDobavilePoints,
DCBC_OtnayliPoints as dcbcOtnayliPoints,
DCBC_OstalosPoints as dcbcOstalosPoints,
DCBC_name as dcbcName
    FROM ChecksTicketTmp";

        #endregion

        private static List<CheckTicketTmp> _checkTicketsTmp = new List<CheckTicketTmp>();
        private static readonly string Path = Config.AppPath + @"Data\CheckTicketsTmp.xml";

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                _checkTicketsTmp = connection.Query<CheckTicketTmp>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("CheckTicketsTmp");

            foreach (var checkTicketTmp in _checkTicketsTmp)
                root.Add(CheckTicketTmp.ToXElement(checkTicketTmp));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                _checkTicketsTmp.Clear();
                foreach (var element in document.GetXElements("CheckTicketsTmp", "rec"))
                    _checkTicketsTmp.Add(CheckTicketTmp.FromXElement(element));
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

        public static List<CheckTicketTmp> GetByCloseTicketId(Guid customerId)
        {
            return SyncData.IsConnect
                ? GetFromDbByCloseTicketIdBase(customerId, SelectQuery + " WHERE CloseTicketTmpCustumerId = @customerId").ToList()
                : _checkTicketsTmp.FindAll(ct => ct.CloseTicketCustomerId == customerId);
        }

        public static List<CheckTicketTmp> GetByBarCode(string barCode)
        {
            List<CheckTicketTmp> checkTicketsTmp;

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    checkTicketsTmp = connection.Query<CheckTicketTmp>(SelectQuery + " WHERE BarCode = @barCode", new {barCode}).ToList();
            else
            {
                if (_checkTicketsTmp.Count == 0)
                    LoadFile();
                checkTicketsTmp = _checkTicketsTmp.FindAll(ct => ct.BarCode == barCode);
            }

            return checkTicketsTmp;
        }

        public static CheckTicketTmp GetFromDbByBarCodeWithPayProducts(string barCode)
        {
            var checkTicket = GetByBarCode(barCode).FirstOrDefault();

            if (checkTicket != null)
                checkTicket.PayProducts = RepositoryPayProductTmp.GetByCheckTicketId(checkTicket.CustomerId);

            return checkTicket;
        }
    }
}