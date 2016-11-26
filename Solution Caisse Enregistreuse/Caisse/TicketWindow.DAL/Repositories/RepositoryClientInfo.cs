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
    public class RepositoryClientInfo
    {
        #region sqripts

        private const string Query = @"SELECT
    custumerId as customerId,
    TypeClient as typeClient,
    Sex as sex,
    Name as name,
    Surname as surname,
    NameCompany as nameCompany,
    SIRET as siret,
    FRTVA as frtva,
    OfficeAddress as officeAddress,
    OfficeZipCode as officeZipCode,
    OfficeCity as officeCity,
    HomeAddress as homeAddress,
    HomeZipCode as homeZipCode,
    HomeCity as homeCity,
    Telephone as telephone,
    Mail as mail,
    Password as password,
    Countrys_CustomerId as countrysCustomerId,
    FavoritesProductAutoCustomerId as favoritesProductAutoCustomerId,
    Nclient as nclient,
    (SELECT custumerId FROM DiscountCards WHERE InfoClients_custumerId = InfoClients.custumerId) as idInfoClientsDiscount
FROM InfoClients WHERE TypeClient = 1";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        public static List<ClientInfo> ClientInfos = new List<ClientInfo>();
        private static readonly string Path = Config.AppPath + @"Data\ClientInfos.xml";

        private static List<ClientInfo> GetAllFromDb(Guid? customerId = null)
        {
            if (customerId.HasValue)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<ClientInfo>(Query + " WHERE custumerId = @customerId", new {customerId}).ToList();

            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<ClientInfo>(Query).ToList();
        }

        private static void SetFromDb()
        {
            ClientInfos = GetAllFromDb();
        }

        private static void SaveFile()
        {
            var root = new XElement("ClientInfos");

            foreach (var clientInfo in ClientInfos)
                root.Add(ClientInfo.ToXElement(clientInfo));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                ClientInfos.Clear();
                foreach (var element in document.GetXElements("ClientInfos", "rec"))
                    ClientInfos.Add(ClientInfo.FromXElement(element));
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

            if (RepositoryDiscountCard.DiscountCards.Count == 0)
                RepositoryDiscountCard.Sync();

            foreach (var info in ClientInfos)
            {
                var cards = RepositoryDiscountCard.DiscountCards.FindAll(dc => dc.InfoClientsCustomerId == info.CustomerId);
                info.DiscountCards.AddRange(cards);
            }
        }

        public static ClientInfo GetOneByNumber(Guid customerId)
        {
            if (customerId == Guid.Empty) return null;

            var clientInfos = SyncData.IsConnect
                ? GetAllFromDb(customerId)
                : ClientInfos.FindAll(ci => ci.CustomerId == customerId);

            switch (clientInfos.Count)
            {
                case 1:
                    return clientInfos.First();
                case 0:
                    return null;
                default:
                    LogService.LogText(TraceLevel.Error, "Опасность повтор данных в информации о клиентах.");
                    return clientInfos.First();
            }
        }
    }
}