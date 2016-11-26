using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChartStat.Model.Models;
using Dapper;

namespace ChartStat.Model.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StatSalesRepozitory
    {
        /// <summary>
        /// Объект соединения с БД.
        /// </summary>
        private readonly IDbConnection _connection;

        private readonly int _connectionTimeout = 600;

        public StatSalesRepozitory(IDbConnection connection)
        {
            _connection = connection;
            if (_connectionTimeout < connection.ConnectionTimeout)
                _connectionTimeout = _connection.ConnectionTimeout;
        }

        public ICollection<StatSalesType> GetAllEntitiesByGroup(DateTime? startDate, DateTime? endDate, int groupId, bool includeSum, bool isStatSales)
        {
            // query parameters
            var parameters = new DynamicParameters();

             parameters.Add("groupId", groupId);
            if (startDate.HasValue) parameters.Add("startDate", startDate.Value);
            if (endDate.HasValue) parameters.Add("endDate", endDate.Value);

            using (_connection)
                return _connection.Query<StatSalesType>(GetCommandText(startDate.HasValue, endDate.HasValue, null, groupId, null, includeSum, isStatSales), parameters, commandTimeout: _connectionTimeout).ToArray();
        }

        public ICollection<StatSalesType> GetAllEntitiesByProductType(DateTime? startDate, DateTime? endDate, ICollection<Guid> productId, bool includeSum, bool isStatSales)
        {
            // query parameters
            var parameters = new DynamicParameters();

            if (startDate.HasValue) parameters.Add("startDate", startDate.Value);
            if (endDate.HasValue) parameters.Add("endDate", endDate.Value);

            using (_connection)
                return _connection.Query<StatSalesType>(GetCommandText(startDate.HasValue, endDate.HasValue, null, null, productId, includeSum, isStatSales), parameters, commandTimeout: _connectionTimeout).ToArray();
        }

        public ICollection<StatSalesType> GetAllEntitiesBySubgroup(DateTime? startDate, DateTime? endDate, ICollection<int> subgroupIds, bool includeSum, bool isStatSales)
        {
            // query parameters
            var parameters = new DynamicParameters();

            if (startDate.HasValue) parameters.Add("startDate", startDate.Value);
            if (endDate.HasValue) parameters.Add("endDate", endDate.Value);

            using (_connection)
                return _connection.Query<StatSalesType>(GetCommandText(startDate.HasValue, endDate.HasValue, subgroupIds, null, null, includeSum, isStatSales), parameters, commandTimeout: _connectionTimeout).ToArray();
        }

        private static string GetCommandText(bool isExistStartDate, bool isExistEndDate, ICollection<int> subGroupIds, int? groupId, ICollection<Guid> productIds, bool includeSum, bool isStatSales)
        {
            StringBuilder query;
            if (isStatSales)
                query = new StringBuilder(string.Format(StatSalesQuery,
                    includeSum ? @",ISNULL (
				(SELECT TOP 1 prix.Prix FROM dbo.PrixDaChatHistory AS prix WHERE ProductCustomerId = p.ProductId AND c.Date >= prix.Date ORDER BY prix.Date),
				(SELECT TOP 1 prix.Prix FROM dbo.PrixDaChatHistory AS prix WHERE ProductCustomerId = p.ProductId AND (SELECT TOP 1 prix.Date FROM dbo.PrixDaChatHistory AS prix WHERE ProductCustomerId = p.ProductId ORDER BY prix.Date) = prix.Date ORDER BY prix.Date)) AS Prix" : string.Empty,
                    string.Empty));
                    //includeSum ? "LEFT JOIN dbo.PrixDaChatHistory AS prix ON p.ProductId = prix.ProductCustomerId AND c.Date >= prix.Date " : string.Empty));
            else query =new StringBuilder( CloseTicketQuery);

            var isExistWhere = false;

            if (subGroupIds != null)
            {
                if (subGroupIds.Count == 1)
                    query.Append(string.Format(@" WHERE products.SubGrpProduct_Id = {0}", subGroupIds.First()));
                else
                {
                    query.Append(string.Format(@" WHERE products.SubGrpProduct_Id IN({0})", string.Join(",", subGroupIds)));
                    query.Replace(",products.Name", ",subgroups.SubGroupName AS Name");
                    query.Replace(",1 as number", ",subgroups.Id AS number");
                }
                
                isExistWhere = true;
            }

            if (groupId.HasValue)
            {
                query.Append(string.Format(@" {0} subgroups.GrpProductId = @groupId", isExistWhere ? "AND" : "WHERE"));
                query.Replace(",products.Name", ",subgroups.SubGroupName AS Name");
                query.Replace(",1 as number", ",subgroups.Id AS number");
                isExistWhere = true;
            }

            if (productIds != null)
            {
                query.Append(productIds.Count == 1
                    ? string.Format(@" {0} p.ProductId = '{1}'", isExistWhere ? "AND" : "WHERE", productIds.First())
                    : string.Format(@" {0} p.ProductId IN({1})", isExistWhere ? "AND" : "WHERE", string.Join(",", productIds.Select(p => string.Format("'{0}'", p)))));

                isExistWhere = true;
            }

            if (isExistStartDate)
            {
                query.Append(string.Format(" {0} c.Date >= @startDate", isExistWhere ? "AND" : "WHERE"));
                isExistWhere = true;
            }

            if (isExistEndDate)
            {
                query.Append(string.Format(" {0} c.Date <= @endDate", isExistWhere ? "AND" : "WHERE"));
            }

            return query.ToString();
        }

        private const string StatSalesQuery = @"SELECT
      c.CustumerId
      ,products.CodeBare AS barCode
      ,c.Date
      ,products.Name
      ,p.QTY
      ,p.PriceHT
      ,p.TVA
      ,p.sumDiscount
      ,p.total
      ,1 as number
      {0}
  FROM dbo.ChecksTicket AS c
  LEFT JOIN dbo.PayProducts AS p ON c.CustumerId = p.ChecksTicketCustumerId
  LEFT JOIN dbo.Products AS products ON p.ProductId = products.CustumerId
  LEFT JOIN dbo.SubGrpNameSet AS subgroups ON subgroups.Id = products.SubGrpProduct_Id
  {1}";

        private const string CloseTicketQuery = @"SELECT
      ct.CustumerId
      ,c.BarCode AS barCode
      ,c.Date
      ,p.Name
      ,p.QTY
      ,p.PriceHT
  FROM dbo.CloseTicket AS ct
  LEFT JOIN dbo.CloseTicketG AS g ON ct.CloseTicketGCustumerId = g.CustumerId
  LEFT JOIN dbo.ChecksTicket AS c ON ct.CustumerId = c.CustumerId
  LEFT JOIN dbo.PayProducts AS p ON ct.CustumerId = p.ChecksTicketCustumerId
";
    }
}