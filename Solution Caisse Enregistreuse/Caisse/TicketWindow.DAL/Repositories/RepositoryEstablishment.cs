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
    public class RepositoryEstablishment
    {
        #region sqripts

        private const string Query = @"SELECT
    CustomerId as customerId,
    Type as type,
    Name as name,
    CP as cp,
    Ville as ville,
    Adresse as adress,
    Phone as phone,
    Mail as mail,
    SIRET as siret,
    NTVA as ntva,
    CodeNAF as codeNaf,
    Fax as fax
FROM Establishment";

        #endregion

        private static Establishment _establishment;
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\Establishments.xml";

        public static List<Establishment> Establishments = new List<Establishment>();

        public static Establishment Establishment
        {
            get
            {
                if (_establishment == null)
                {
                    if (SyncData.IsConnect)
                    {
                        using (var connection = ConnectionFactory.CreateConnection())
                            _establishment = connection.Query<Establishment>(Query, new {customerId = Config.IdEstablishment}).First();
                    }
                    else
                    {
                        if (Establishments.Count == 0) Sync();
                        _establishment = Establishments.FirstOrDefault(e => e.CustomerId == Config.IdEstablishment);
                    }
                    Config.Name = _establishment != null ? _establishment.Name : string.Empty;
                }
                return _establishment;
            }
        }

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                Establishments = connection.Query<Establishment>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("Establishments");

            foreach (var establishment in Establishments)
                root.Add(Establishment.ToXElement(establishment));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                Establishments.Clear();
                foreach (var element in document.GetXElements("Establishments", "rec"))
                    Establishments.Add(Establishment.FromXElement(element));
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
    }
}