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
    public class RepositoryGroupProduct
    {
        #region sqripts

        private const string SelectQuery = @"SELECT
    Id as id,
    GroupName as name
FROM GrpProductSet";

        private const string InsertQuery = @"INSERT INTO GrpProductSet (
Id,
GroupName)
    VALUES (
@Id,
@Name)";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\GroupProduct.xml";
        private static XDocument _document;

        public static List<GroupProduct> GroupProducts = new List<GroupProduct>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                GroupProducts = connection.Query<GroupProduct>(SelectQuery).ToList();
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                GroupProducts.Clear();
                _document = XDocument.Load(Path);

                foreach (var el in _document.GetXElements("Palettes", "Palette"))
                    GroupProducts.Add(GroupProduct.FromXElement(el));
            }
        }

        private static void SaveFile()
        {
            var elements = new XElement("Palettes");
            _document = new XDocument(elements);

            foreach (var group in GroupProducts)
                elements.Add(GroupProduct.ToXElement(group));

            _document.Save(Path);
        }

        public static void Sync()
        {
            if (SyncData.IsConnect)
            {
                SetFromDb();

                if (RepositorySubGroupProduct.SubGroupProducts.Count == 0)
                    RepositorySubGroupProduct.SetFromDb();

                foreach (var g in GroupProducts)
                {
                    var subgroups = RepositorySubGroupProduct.SubGroupProducts.FindAll(k => k.GroupId == g.Id);
                    g.SubGroups.AddRange(subgroups);

                    foreach (var f in subgroups)
                        f.Group = g;
                }

                SaveFile();
            }
            else
            {
                LoadFile();

                foreach (var g in GroupProducts)
                    RepositorySubGroupProduct.SubGroupProducts.AddRange(g.SubGroups);
            }
        }

        public static void Add(GroupProduct group)
        {
            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            document.GetXElement("Palettes").Add(GroupProduct.ToXElement(group));
            File.WriteAllText(Path, document.ToString());

            GroupProducts.Add(group);

            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, group);
        }

        public static void Delete(GroupProduct group)
        {
            var current = GroupProducts.First(s => s.Id == group.Id);
            GroupProducts.Remove(current);

            var document = XDocument.Load(Path);
            var statNationPopupElement = document.GetXElements("Palettes", "Palette").First(p => p.GetXAttributeValue("Group", "ID").ToInt() == group.Id);
            statNationPopupElement.Remove();
            document.Save(Path);

            if (SyncData.IsConnect)
            {
                const string query = "DELETE FROM GrpProductSet WHERE Id = @Id";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new {group.Id});
            }
        }

        public static void AddSubgroup(SubGroupProduct subgroup)
        {
            subgroup.Group.SubGroups.Add(subgroup);

            if (!File.Exists(Path)) SaveFile();

            var document = XDocument.Load(Path);
            var element = document.GetXElements("Palettes", "Palette").First(p => p.GetXAttributeValue("Group", "ID").ToInt() == subgroup.Group.Id);
            GroupProduct.ToXElementSubGroups(element, subgroup.Group);
            
            File.WriteAllText(Path, document.ToString());

            RepositorySubGroupProduct.SubGroupProducts.Add(subgroup);
            RepositorySubGroupProduct.InsertToDb(subgroup);
        }

        public static void DeleteSubgroup(SubGroupProduct subgroup)
        {
            var current = subgroup.Group.SubGroups.First(s => s.Id == subgroup.Id);
            subgroup.Group.SubGroups.Remove(current);

            var document = XDocument.Load(Path);
            var groupElement = document.GetXElements("Palettes", "Palette").First(p => p.GetXAttributeValue("Group", "ID").ToInt() == subgroup.Group.Id);
            groupElement.GetXElements("SubGroup").First(s=>s.GetXAttributeValue("ID").ToInt()==subgroup.Id).Remove();
            document.Save(Path);

            RepositorySubGroupProduct.SubGroupProducts.Remove(subgroup);
            RepositorySubGroupProduct.DeleteFromDb(subgroup);
        }

        public static string GetGroupNameById(int id)
        {
            if (id == 0) id = 1;
            return GroupProducts.Find(g => g.Id == id).Name;
        }
    }
}