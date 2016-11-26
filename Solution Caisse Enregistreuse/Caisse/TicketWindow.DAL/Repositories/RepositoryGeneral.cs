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
    public class RepositoryGeneral
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\Generals.xml";

        public static List<GeneralType> Generals = new List<GeneralType>();
        public static bool IsOpen { get; set; }
        public static string Mess { get; set; }

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                Generals = connection.Query<GeneralType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("Generals");

            foreach (var actionCash in Generals)
                root.Add(GeneralType.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                Generals.Clear();
                foreach (var element in document.GetXElements("Generals", "rec"))
                    Generals.Add(GeneralType.FromXElement(element));
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

        public static void Set()
        {
            Sync();

            var currentGeneral = Generals.FirstOrDefault(g => g.EstablishmentCustomerId == Config.IdEstablishment);

            if (currentGeneral == null)
            {
                IsOpen = false;
                var newGeneral = new GeneralType(Guid.NewGuid(), Guid.Empty, false, Config.NameTicket, Config.User, DateTime.Now, Config.IdEstablishment);

                GlobalVar.TicketWindowG = newGeneral.TicketWindowGeneral;
                
                Add(newGeneral);
            }
            else
            {
                IsOpen = currentGeneral.IsOpen ?? false;
                GlobalVar.TicketWindowG = currentGeneral.TicketWindowGeneral;
            }
        }

        public static void Update(GeneralType general)
        {
            var document = XDocument.Load(Path);
            var element = document.GetXElements("Generals", "rec").First(el => el.GetXElementValue("Id").ToGuid() == general.Id);
            GeneralType.SetXmlValues(element, general);
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(UpdateQuery, general);

            var idx = Generals.FindIndex(g => g.Id == general.Id);
            Generals[idx] = general;
        }

        public static void Add(GeneralType general)
        {
            Generals.Add(general);

            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            document.GetXElement("Generals").Add(GeneralType.ToXElement(general));
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, general);
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
Id as id,
TicketWindowGeneral as ticketWindowGeneral,
[Open] as [open],
Name as name,
[User] as [user],
Date as date,
Establishment_CustomerId as establishmentCustomerId
    FROM General";

        private const string UpdateQuery = @"UPDATE General SET 
TicketWindowGeneral = @TicketWindowGeneral,
[Open] = @IsOpen,
Name = @Name,
[User] = @User,
[Date] = @Date
    WHERE Id = @Id";

        private const string InsertQuery = @"INSERT INTO General (
Id,
TicketWindowGeneral,
[Open],
Name,
[User],
[Date],
Establishment_CustomerId)
    VALUES (
@Id,
@TicketWindowGeneral,
@IsOpen,
@Name,
@User,
@Date,
@EstablishmentCustomerId)";

        #endregion
    }
}