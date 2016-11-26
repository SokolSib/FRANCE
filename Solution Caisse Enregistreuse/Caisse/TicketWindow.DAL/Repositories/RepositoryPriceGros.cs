using System;
using System.Collections.Generic;
using System.Linq;
using TicketWindow.DAL.Additional;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     не используется.+
    /// </summary>
    public class RepositoryPriceGros
    {
        public static List<PriceGrosType> GetFromDbByProductIds(Guid[] productIds)
        {
            var stockReals = RepositoryStockReal.GetFromDbByProductIds(productIds);
            return stockReals.Select(sr => new PriceGrosType(sr.CustomerId, sr.ProductsCustomerId, sr.Price)).ToList();
        }
    }
}