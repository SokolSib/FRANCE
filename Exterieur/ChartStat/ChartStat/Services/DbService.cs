using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ChartStat.Model.Models;
using ChartStat.Model.Repositories;

namespace ChartStat.Services
{
    public class DbService
    {
        public static bool IsDbAvaliable(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static ICollection<StatSalesType> GetStatSalesTypesByProductType(string connectionString, DateTime? startDate, DateTime? endDate, ICollection<Guid> productId, bool includeSum, bool isStatSales)
        {
            var repozitory = new StatSalesRepozitory(new SqlConnection(connectionString));
            return repozitory.GetAllEntitiesByProductType(startDate, endDate, productId, includeSum, isStatSales);
        }

        public static ICollection<StatSalesType> GetStatSalesTypesByGroup(string connectionString, DateTime? startDate, DateTime? endDate, int groupId, bool includeSum, bool isStatSales)
        {
            var repozitory = new StatSalesRepozitory(new SqlConnection(connectionString));
            return repozitory.GetAllEntitiesByGroup(startDate, endDate, groupId, includeSum, isStatSales);
        }

        public static ICollection<StatSalesType> GetStatSalesTypesBySubgroup(string connectionString, DateTime? startDate, DateTime? endDate, ICollection<int> subgroupIds, bool includeSum, bool isStatSales)
        {
            var repozitory = new StatSalesRepozitory(new SqlConnection(connectionString));
            return repozitory.GetAllEntitiesBySubgroup(startDate, endDate, subgroupIds, includeSum, isStatSales);
        }

        public static ICollection<GroupType> GetGroupTypes(string connectionString)
        {
            var repozitory = new GroupRepozitory(new SqlConnection(connectionString));
            return repozitory.GetAllEntities();
        }

        public static ICollection<ProductType> GetProductTypes(string connectionString)
        {
            var repozitory = new ProductRepozitory(new SqlConnection(connectionString));
            return repozitory.GetAllEntities();
        }
    }
}