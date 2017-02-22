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
    public class RepositoryDiscountCard
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\DiscountCards.xml";

        public static List<DiscountCard> DiscountCards = new List<DiscountCard>();

        private static List<DiscountCard> GetAllFromDb(string numberCard = null)
        {
            if (!string.IsNullOrEmpty(numberCard))
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<DiscountCard>(SelectQuery + " WHERE numberCard = @numberCard", new {numberCard}).ToList();

            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<DiscountCard>(SelectQuery).ToList();
        }

        private static void SetFromDb()
        {
            DiscountCards = GetAllFromDb();
        }

        private static void SaveFile()
        {
            var root = new XElement("DiscountCards");

            foreach (var discountCard in DiscountCards)
                root.Add(DiscountCard.ToXElement(discountCard));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                DiscountCards.Clear();
                foreach (var element in document.GetXElements("DiscountCards", "rec"))
                    DiscountCards.Add(DiscountCard.FromXElement(element));
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

        public static DiscountCard GetOneByNumber(string numberCard)
        {
            if (string.IsNullOrEmpty(numberCard)) return null;

            if (DiscountCards.Count == 0)
                Sync();

            var discountCards = DiscountCards.FindAll(dc => dc.NumberCard == numberCard);

            switch (discountCards.Count)
            {
                case 1:
                    return discountCards.First();
                case 0:
                    return null;
                default:
                    LogService.LogText(TraceLevel.Error, "Опасность повтор данных в дисконтных картах.");
                    return discountCards.Last();
            }
        }

        public static void Update(DiscountCard discountCard)
        {
            var date = DateTime.Now;

            var document = XDocument.Load(Path);
            var element = document.GetXElements("DiscountCards", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == discountCard.CustomerId);
            DiscountCard.SetXmlValues(element, discountCard);
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(UpdateQuery, new {discountCard.Points, date, discountCard.CustomerId});

            var idx = DiscountCards.FindIndex(ds => ds.CustomerId == discountCard.CustomerId);
            DiscountCards[idx] = discountCard;
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
    custumerId as customerId,
    numberCard as numberCard,
    points as points,
    Active as active,
    InfoClients_custumerId as infoClientsCustomerId,
    DateTimeLastAddProduct as dateTimeLastAddProduct
FROM DiscountCards";

        private const string UpdateQuery = @"UPDATE DiscountCards SET 
    points = @Points,
    DateTimeLastAddProduct = @date
WHERE custumerId = @CustomerId";

        #endregion
    }
}