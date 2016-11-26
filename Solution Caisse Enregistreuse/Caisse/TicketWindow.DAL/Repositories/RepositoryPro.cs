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
    public class RepositoryPro
    {
        #region sqripts
        
        private const string SelectQuery = @"SELECT
    custumerId as customerId,
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
    Nclient as nclient
FROM InfoClients WHERE TypeClient = 2";

        private const string InsertQuery = @"INSERT INTO InfoClients (
custumerId,
TypeClient,
Sex,
Name,
Surname,
NameCompany,
SIRET,
FRTVA,
OfficeAddress,
OfficeZipCode,
OfficeCity,
HomeAddress,
HomeZipCode,
HomeCity,
Telephone,
Mail,
Password,
Countrys_CustomerId,
FavoritesProductAutoCustomerId,
Nclient)
     VALUES (
NEWID(),
2,
@Sexint,
@Name,
@SurName,
@NameCompany,
@Siret,
@Frtva,
@OfficeAddress,
@OfficeZipCode,
@OfficeCity,
@HomeAddress,
@HomeZipCode,
@HomeCity,
@Telephone,
@Mail,
@Password,
@CountrysCustomerId,
@FavoritesProductAutoCustomerId,
@Nclient)";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        public static List<Pro> Pros = new List<Pro>();
        private static readonly string Path = Config.AppPath + @"Data\Pros.xml";

        private static List<Pro> GetAllFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<Pro>(SelectQuery).ToList();
        }

        private static void SetFromDb()
        {
            Pros = GetAllFromDb();
        }

        private static void SaveFile()
        {
            var root = new XElement("Pros");

            foreach (var pro in Pros)
                root.Add(Pro.ToXElement(pro));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                Pros.Clear();
                foreach (var element in document.GetXElements("Pros", "rec"))
                    Pros.Add(Pro.FromXElement(element));
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

            if (RepositoryProDiscount.ProDiscounts.Count == 0)
                RepositoryProDiscount.Sync();
            if (RepositoryInfoClientsDiscountsType.InfoClientsDiscounts.Count == 0)
                RepositoryInfoClientsDiscountsType.Sync();

            foreach (var pro in Pros)
            {
                var proDiscounts = RepositoryProDiscount.ProDiscounts.FindAll(pd => pd.CustomerIdInfoClients == pro.CustomerId);
                pro.ProDiscounts.AddRange(proDiscounts);

                if (proDiscounts.Count > 0)
                {
                    var infoClientDiscountTypes =
                        RepositoryInfoClientsDiscountsType.InfoClientsDiscounts.FindAll(i => proDiscounts.Select(pd => pd.IdInfoClientsDiscountsType).Contains(i.Id));

                    if (infoClientDiscountTypes.Count > 0)
                    {
                        pro.DiscountName = string.Join(", ", infoClientDiscountTypes.Select(i => i.Name.Trim()));
                        pro.DiscountValue = infoClientDiscountTypes.First().Value;
                    }
                }
            }
        }

        public static int InsertToDb(Pro pro)
        {
            if (RepositoryClientInfo.ClientInfos.Count == 0)
                RepositoryClientInfo.Sync();

            pro.Nclient = GetMaxNclientWithTypeClient2() + 10;
            pro.Password = string.Empty;
            pro.CountrysCustomerId = new Guid("F3D5541D-9860-4D93-ADCF-18F631ADAD74");
            pro.FavoritesProductAutoCustomerId = Guid.Empty;

            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Execute(InsertQuery, pro);
        }

        public static int GetMaxNclientWithTypeClient2()
        {
            int maxNclient;

            if (SyncData.IsConnect)
            {
                const string query = @"SELECT MAX(CAST(Nclient AS int)) FROM InfoClients WHERE TypeClient = 2";

                using (var connection = ConnectionFactory.CreateConnection())
                    maxNclient = (int) connection.ExecuteScalar(query);
            }
            else maxNclient = Pros.Max(ci => Convert.ToInt32(ci.Nclient));

            return maxNclient;
        }
    }
}