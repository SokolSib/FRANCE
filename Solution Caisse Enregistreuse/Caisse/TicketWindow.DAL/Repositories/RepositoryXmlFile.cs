using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db. пустота в режиме локальной работы
    /// </summary>
    public class RepositoryXmlFile
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);

        private static void InsertToDb(XmlFile xmlFile)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Query<XmlFile>(InsertQuery, xmlFile);
        }

        private static void UpdateFromDb(XmlFile xmlFile)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Query<XmlFile>(UpdateQuery, xmlFile);
        }

        private static XmlFile GetFromDb(string fileName, string userName)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    return connection.Query<XmlFile>(SelectQuery + " WHERE UserName = @userName AND FileName = @fileName", new {userName, fileName}).FirstOrDefault();

            return null;
        }

        private static void SetFromDb(string path)
        {
            var xmlFile = GetFromDb(Path.GetFileName(path), Config.User);
            if (xmlFile != null) File.WriteAllText(path, xmlFile.Data);
        }

        public static void SaveToDb(string path, XDocument document)
        {
            var fileName = Path.GetFileName(path);
            if (SyncData.IsConnect)
            {
                var worker = new BackgroundWorker();
                worker.DoWork +=
                    (s, e) =>
                    {
                        SyncData.SetSunc(true);
                        var xmlFile = GetFromDb(fileName, Config.User);

                        if (xmlFile == null)
                            InsertToDb(new XmlFile(Guid.NewGuid(), fileName, DateTime.Now, true, Config.User, document.ToString(), Config.IdEstablishment));
                        else
                        {
                            xmlFile.Upd = true;
                            xmlFile.FileName = fileName;
                            xmlFile.Date = DateTime.Now;
                            xmlFile.Data = document.ToString();
                            UpdateFromDb(xmlFile);
                        }
                    };
                worker.RunWorkerCompleted += (s, e) => SyncData.SetSunc(false);
                worker.RunWorkerAsync();
            }
        }

        public static void SetAllFromDb()
        {
            if (SyncData.IsConnect)
            {
                // директории взять из ClassGridGroup и ClassGridProduct
                SetFromDb(AppDomain.CurrentDomain.BaseDirectory + @"Data\GridGroup.xml");
                SetFromDb(AppDomain.CurrentDomain.BaseDirectory + @"Data\GridLeft.xml");
                SetFromDb(AppDomain.CurrentDomain.BaseDirectory + @"Data\GridTypePay.xml");
                SetFromDb(AppDomain.CurrentDomain.BaseDirectory + @"Data\GridRigthBottom.xml");
                SetFromDb(AppDomain.CurrentDomain.BaseDirectory + @"Data\GridProduct.xml");
            }
        }

        #region sqripts

        private const string SelectQuery = @"SELECT
CustomerId as customerId,
FileName as fileName,
Date as date,
Upd as upd,
UserName as userName,
Data as data,
Establishment_CustomerId as establishmentCustomerId
    FROM XML_File";

        private const string UpdateQuery = @"UPDATE XML_File SET 
CustomerId = @CustomerId, 
FileName = @FileName, 
Date = @Date,
Upd = @Upd,
UserName = @UserName,
Data = @Data, 
Establishment_CustomerId = @EstablishmentCustomerId
    WHERE CustomerId = @CustomerId";

        private const string InsertQuery = @"INSERT INTO XML_File VALUES (
@CustomerId,
@FileName,
@Date,
@Upd, 
@UserName, 
@Data,
@EstablishmentCustomerId)";

        #endregion
    }
}