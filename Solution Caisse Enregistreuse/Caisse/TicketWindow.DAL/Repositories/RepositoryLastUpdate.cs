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
    public class RepositoryLastUpdate
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\LastUpdates.xml";

        public static List<LastUpdateType> LastUpdates = new List<LastUpdateType>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                LastUpdates = connection.Query<LastUpdateType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("LastUpdates");

            foreach (var actionCash in LastUpdates)
                root.Add(LastUpdateType.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                LastUpdates.Clear();
                foreach (var element in document.GetXElements("LastUpdates", "rec"))
                    LastUpdates.Add(LastUpdateType.FromXElement(element));
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

        public static void Update(bool recall)
        {
            var notRecall = !recall;
            var date = DateTime.Now;

            if (SyncData.IsConnect)
            {
                var dateText = date.ToString(Config.DateFormat);
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(UpdateQuery, new { Config.NameTicket, dateText, Config.User, notRecall, Config.IdEstablishment, Config.CustomerId });
            }

            if (LastUpdates.Count == 0) Sync();
            var idx = LastUpdates.FindIndex(l => l.CustomerId == Config.CustomerId);

            LastUpdates[idx].NameTicket = Config.NameTicket;
            LastUpdates[idx].LastDate = date;
            LastUpdates[idx].User = Config.User;
            LastUpdates[idx].Upd = notRecall;
            LastUpdates[idx].IdEstablishment = Config.IdEstablishment;

            var document = XDocument.Load(Path);
            var element = document.GetXElements("LastUpdates", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == Config.CustomerId);
            LastUpdateType.SetXmlValues(element, LastUpdates[idx]);
            File.WriteAllText(Path, document.ToString());
        }

        public static DateTime? GetUpdateDate()
        {
            List<LastUpdateType> datas;

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    datas = connection.Query<LastUpdateType>(SelectQuery + " WHERE CustumerId = @CustomerId", new {Config.CustomerId}).ToList();
            else
            {
                if (LastUpdates.Count == 0) Sync();
                datas = LastUpdates.FindAll(l => l.CustomerId == Config.CustomerId);
            }

            if (datas.Count == 0)
            {
                var date = DateTime.Now.ToString(Config.DateFormat);

                if (SyncData.IsConnect)
                    using (var connection = ConnectionFactory.CreateConnection())
                        connection.Execute(InsertQuery, new {Config.CustomerId, Config.NameTicket, date, Config.User, Config.IdEstablishment});
                else LastUpdates.Add(new LastUpdateType(Config.CustomerId, Config.NameTicket, false, date, Config.User, Config.IdEstablishment));
            }
            else
            {
                var data = datas.First();
                if (data.Upd) return data.LastDate;
            }
            return null;
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
CustumerId as customerId,
NameTicket as nameTicket,
Upd as upd, 
DateLastUpd as date,
[User] as [user],
IdEstablishment as idEstablishment
    FROM LastUpd";

        private const string UpdateQuery = @"UPDATE LastUpd SET 
NameTicket = @NameTicket,
DateLastUpd = @dateText,
[User] = @User,
Upd = @notRecall,
IdEstablishment = IdEstablishment
    WHERE CustumerId = @CustomerId";

        private const string InsertQuery = @"INSERT INTO LastUpd (
CustumerId,
NameTicket,
DateLastUpd,
[User],
Upd,
IdEstablishment) 
    VALUES (
@CustomerId,
@NameTicket,
@date,
@User,
'false',
@IdEstablishment)";

        #endregion
    }
}