using System.Collections.Generic;
using System.Linq;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db Sunc.+ (загрузка идет в Groups)
    /// </summary>
    public class RepositorySubGroupProduct
    {
        #region sqripts

        private const string SelectQuery = @"SELECT 
    Id as id,
    SubGroupName as name,
    GrpProductId as groupId
FROM SubGrpNameSet";

        private const string InsertQuery = @"INSERT INTO SubGrpNameSet (
Id,
SubGroupName,
GrpProductId)
    VALUES (
@Id,
@Name,
@GroupId)";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);

        public static List<SubGroupProduct> SubGroupProducts = new List<SubGroupProduct>();

        private static List<SubGroupProduct> GetAllFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<SubGroupProduct>(SelectQuery).ToList();
        }

        public static void SetFromDb()
        {
            SubGroupProducts = GetAllFromDb();
        }

        public static void DeleteFromDb(SubGroupProduct subgroup)
        {
            if (SyncData.IsConnect)
            {
                const string query = "DELETE FROM SubGrpNameSet WHERE Id = @Id";

                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(query, new { subgroup.Id });
            }
        }

        public static void InsertToDb(SubGroupProduct subgroup)
        {
            if (SyncData.IsConnect)
                using (var connection = ConnectionFactory.CreateConnection())
                    connection.Execute(InsertQuery, subgroup);
        }
    }
}