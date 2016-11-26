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
    public class RepositoryCountry
    {
        #region sqripts

        private const string Query = @"SELECT
    CustomerId as customerId,
    NameCountry as nameCountry,
    Capital as capital,
    Continent as continent
FROM Countrys";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static XDocument _document;
        private static readonly string Path = Config.AppPath + @"Data\Countrys.xml";

        public static List<Country> Countrys = new List<Country>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                Countrys = connection.Query<Country>(Query).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                _document = XDocument.Load(Path);

                Countrys.Clear();
                foreach (var element in _document.GetXElements("Countrys", "rec"))
                    Countrys.Add(Country.FromXElement(element));
            }
        }

        private static void SaveFile()
        {
            _document = new XDocument(new XElement("Countrys"));

            foreach (var e in Countrys)
                _document.GetXElement("Countrys").Add(Country.ToXElement(e));

            _document.Save(Path);
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
    }
}