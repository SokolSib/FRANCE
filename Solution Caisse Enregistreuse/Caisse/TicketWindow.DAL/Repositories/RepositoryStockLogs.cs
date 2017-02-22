using TicketWindow.DAL.Models;

// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Не используется.+
    /// </summary>
    public class RepositoryStockLogs
    {
        // изменить использование
        public static string InsQuery(StockLogs o)
        {
            const string cmdInsProduct = "INSERT INTO StockLogs (CustomerId,[DateTime],TypeOperation,Name,Barcode,QTY,[User],Details)"
                                         + "VALUES ('{CustumerId}','{DateTime}','{TypeOperation}','{Name}','{Barcode}',{QTY},'{User}','{Details}')";

            var c = cmdInsProduct
                .Replace("{CustumerId}", o.CustomerId.ToString())
                .Replace("{DateTime}", o.DateTime.ToString())
                .Replace("{TypeOperation}", o.TypeOperation.ToString())
                .Replace("{Name}", o.Name.Replace("'", "''"))
                .Replace("{Barcode}", o.Barcode)
                .Replace("{QTY}", o.Qty.ToString().Replace(",", "."))
                .Replace("{User}", o.User.Replace("'", "''"))
                .Replace("{Details}", o.Details.Replace("'", "''"));

            return c;
        }
    }
}