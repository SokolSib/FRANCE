using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db Sunc.+
    /// </summary>
    public class RepositoryTypePay
    {
        private const string Query = @"SELECT 
    Id as id,
    Etat as etat,
    Name as name,
    NameCourt as nameCourt,
    CodeCompta as codeCompta,
    Rendu_Avoir as renduAvoir,
    Tiroir as tiroir,
    CurMod as curMod
FROM TypesPay";

        private static readonly string Path = Global.Config.AppPath + @"Data\TypePay.xml";
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Global.Config.ConnectionString);

        public static List<TypePay> TypePays = new List<TypePay>();

        private static List<TypePay> GetAllFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<TypePay>(Query).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                TypePays.Clear();
                foreach (var tp in document.GetXElements("typesPay", "rec"))
                    TypePays.Add(TypePay.FromXElement(tp));
            }
        }

        private static void SaveFile()
        {
            XDocument document;
            if (File.Exists(Path))
            {
                document = XDocument.Load(Path);
                document.GetXElement("typesPay").RemoveAll();
            }
            else
            {
                document = new XDocument();
                document.Add(new XElement("typesPay"));
            }

            foreach (var tp in TypePays)
                document.GetXElement("typesPay").Add(TypePay.ToXElement(tp));

            document.Save(Path);
        }

        public static TypePay GetById(int id)
        {
            return TypePays.SingleOrDefault(tp => tp.Id == id);
        }

        public static void Sync()
        {
            if (Global.SyncData.IsConnect)
            {
                TypePays = GetAllFromDb();
                SaveFile();
            }
            else LoadFile();
        }
    }
}