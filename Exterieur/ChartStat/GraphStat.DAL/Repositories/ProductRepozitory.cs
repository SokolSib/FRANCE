using System.Data;
using ChartStat.Model.Models;

namespace ChartStat.Model.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProductRepozitory : RepositoryBase<ProductType>
    {
        public ProductRepozitory(IDbConnection connection)
            : base(connection)
        {
        }

        protected override string GetCommandText()
        {
            return @"SELECT 
      CustumerId
      ,Name
      ,CodeBare
  FROM dbo.Products ORDER BY Name";
        }
    }
}
