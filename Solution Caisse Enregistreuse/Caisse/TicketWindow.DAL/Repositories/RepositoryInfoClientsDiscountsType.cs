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
    public class RepositoryInfoClientsDiscountsType
    {
        #region sqripts

        private const string SelectQuery = @"SELECT
Id as id,
Name as name,
Type as type,
[Value] as [value],
Description as description
    FROM InfoClientsDiscountsType";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        public static List<InfoClientsDiscountsTypeType> InfoClientsDiscounts = new List<InfoClientsDiscountsTypeType>();

        private static readonly string Path = Config.AppPath + @"Data\InfoClientsDiscountsData.xml";

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                InfoClientsDiscounts = connection.Query<InfoClientsDiscountsTypeType>(SelectQuery).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("InfoClientsDiscountsData");

            foreach (var checkTicketTmp in InfoClientsDiscounts)
                root.Add(InfoClientsDiscountsTypeType.ToXElement(checkTicketTmp));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                InfoClientsDiscounts.Clear();
                foreach (var element in document.GetXElements("InfoClientsDiscountsData", "rec"))
                    InfoClientsDiscounts.Add(InfoClientsDiscountsTypeType.FromXElement(element));
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