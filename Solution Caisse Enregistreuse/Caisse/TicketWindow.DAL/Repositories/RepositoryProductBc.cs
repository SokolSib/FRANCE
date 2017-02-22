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
    public class RepositoryProductBc
    {
        #region sqripts

        private const string Query = @"SELECT
CustomerId as customerId,
CustomerIdProduct as customerIdProduct,
CodeBar as codeBar,
QTY as qty,
Description as description
  FROM ProductsBC";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static readonly string Path = Config.AppPath + @"Data\ProductsBc.xml";

        public static List<ProductBc> ProductsBc = new List<ProductBc>();

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                ProductsBc = connection.Query<ProductBc>(Query).ToList();
        }

        private static void SaveFile()
        {
            var root = new XElement("ProductsBc");

            foreach (var actionCash in ProductsBc)
                root.Add(ProductBc.ToXElement(actionCash));

            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                ProductsBc.Clear();
                foreach (var element in document.GetXElements("ProductsBc", "rec"))
                    ProductsBc.Add(ProductBc.FromXElement(element));
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
        }

        public static string GetAllBarCodes(string barCode, Guid customerIdProduct)
        {
            var res = barCode.Trim();

            if (!res.StartsWith("["))
                res = '[' + barCode.Trim() + ']';

            var products = ProductsBc.FindAll(l => l.CustomerIdProduct == customerIdProduct);

            if (products.Count > 0)
                res = products.Aggregate(res, (current, product) => current + string.Format("[{0}]^{1}", product.CodeBar.Trim(), product.Qty));

            return res;
        }
    }
}