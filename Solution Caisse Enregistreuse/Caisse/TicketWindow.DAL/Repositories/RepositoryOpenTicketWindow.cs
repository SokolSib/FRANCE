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
    public class RepositoryOpenTicketWindow
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\OpenTicketWindows.xml";

        public static List<OpenTicketWindow> OpenTicketWindows = new List<OpenTicketWindow>();
        public static bool IsOpen { get; set; }

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                OpenTicketWindows = connection.Query<OpenTicketWindow>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("OpenTicketWindows");

            foreach (var openTicketWindow in OpenTicketWindows)
                root.Add(OpenTicketWindow.ToXElement(openTicketWindow));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                OpenTicketWindows.Clear();
                foreach (var element in document.GetXElements("OpenTicketWindows", "rec"))
                    OpenTicketWindows.Add(OpenTicketWindow.FromXElement(element));
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

            var openTicketWindow = OpenTicketWindows.FirstOrDefault(otw => otw.EstablishmentCustomerId == Config.IdEstablishment);
            IsOpen = openTicketWindow != null && openTicketWindow.IsOpen;
            CreateIfNotExist();

            openTicketWindow = GetCurrent();
            GlobalVar.TicketWindow = openTicketWindow.IdTicketWindow;
            GlobalVar.TicketWindowG = openTicketWindow.IdTicketWindowG ?? Guid.Empty;
            GlobalVar.IsOpen = openTicketWindow.IsOpen;
        }

        private static void CreateIfNotExist()
        {
            if (GetCurrent() == null)
                Add(new OpenTicketWindow(Config.CustomerId, Guid.Empty, Config.NameTicket, Config.User, Config.NumberTicket, false, DateTime.Now, Guid.Empty,
                    Config.IdEstablishment));
        }

        private static void Add(OpenTicketWindow openTicketWindow)
        {
            OpenTicketWindows.Add(openTicketWindow);

            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            document.GetXElement("OpenTicketWindows").Add(OpenTicketWindow.ToXElement(openTicketWindow));
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, openTicketWindow);
        }

        public static OpenTicketWindow GetCurrent()
        {
            return OpenTicketWindows.FirstOrDefault(otw => otw.CustomerId == Config.CustomerId);
        }

        public static void Update(OpenTicketWindow openTicketWindow)
        {
            var idx = OpenTicketWindows.FindIndex(otw => otw.CustomerId == openTicketWindow.CustomerId);
            if (idx >= 0)
                OpenTicketWindows[idx] = openTicketWindow;

            var document = XDocument.Load(Path);
            var element = document.GetXElements("OpenTicketWindows", "rec").First(el => el.GetXElementValue("CustomerId").ToGuid() == openTicketWindow.CustomerId);
            OpenTicketWindow.SetXmlValues(element, openTicketWindow);
            File.WriteAllText(Path, document.ToString());

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(UpdateQuery, openTicketWindow);
        }

        public static bool Open()
        {
            var otw = GetCurrent();
            GlobalVar.TicketWindow = Guid.NewGuid();

            if (otw == null)
                Add(new OpenTicketWindow(Config.CustomerId, GlobalVar.TicketWindow, Config.NameTicket, Config.User, Config.NumberTicket, true, DateTime.Now,
                    GlobalVar.TicketWindowG, Guid.Empty));
            else
            {
                otw.IdTicketWindow = GlobalVar.TicketWindow;
                otw.NameTicket = Config.NameTicket;
                otw.User = Config.User;
                otw.NumberTicket = Config.NumberTicket;
                otw.DateOpen = DateTime.Now;
                otw.IsOpen = true;
                otw.IdTicketWindowG = GlobalVar.TicketWindowG;

                Update(otw);
            }

            RepositoryCheck.OpenTicket();
            GlobalVar.IsOpen = true;
            return true;
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
custumerId as customerId,
idTicketWindow,
nameTicket,
[user],
numberTicket,
isOpen,
dateOpen,
IdTicketWindowG as idTicketWindowG,
Establishment_CustomerId as establishmentCustomerId
    FROM OpenTicketWindow";

        private const string InsertQuery = @"INSERT INTO OpenTicketWindow (
custumerId,
idTicketWindow,
nameTicket,
[user],
numberTicket,
isOpen,
dateOpen,
Establishment_CustomerId)
    VALUES (
@CustomerId,
@IdTicketWindow,
@NameTicket,
@User,
@NumberTicket,
@IsOpen,
@DateOpen,
@EstablishmentCustomerId)";

        private const string UpdateQuery = @"UPDATE OpenTicketWindow SET 
idTicketWindow = @IdTicketWindow,
nameTicket = @NameTicket,
[user] = @User,
numberTicket = @NumberTicket,
isOpen = @IsOpen,
dateOpen = @DateOpen,
IdTicketWindowG = @IdTicketWindowG, 
Establishment_CustomerId = @EstablishmentCustomerId 
    WHERE custumerId = @CustomerId";

        #endregion
    }
}