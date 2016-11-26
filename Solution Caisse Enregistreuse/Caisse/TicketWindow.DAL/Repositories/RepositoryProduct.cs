using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class RepositoryProduct
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);

        public static readonly string Path = Config.AppPath + @"Data\Product.xml";
        public static List<string> ProductNames = new List<string>();
        public static List<ProductType> Products = new List<ProductType>();
        public static XDocument Document = new XDocument(new XElement("Product"));

        private static void LoadFile()
        {
            if (File.Exists(Path))
            {
                Products.Clear();

                Document = XDocument.Load(Path);
                var elements = Document.GetXElements("Product", "rec");
                Products.AddRange(elements.Select(ProductType.FromXElement));
            }
        }

        private static void SaveFile()
        {
            if (File.Exists(Path))
                Document = XDocument.Load(Path);

            var groupHistoryChangeProduct = RepositoryHistoryChangeProduct.GetGroup();

            XDocument old = null;

            if (Document == null)
                Document = new XDocument(new XElement("Product"));
            else
            {
                old = new XDocument(Document);
                Document.GetXElement("Product").RemoveAll();
            }

            foreach (var product in Products.Where(product => product != null))
            {
                RepositoryHistoryChangeProduct.Compare(old, product, groupHistoryChangeProduct);

                var productElement = Document.GetXElement("Product");
                productElement.Add(ProductType.ToXElement(product, productElement));
            }
            RepositoryHistoryChangeProduct.SaveFile();
            Document.Save(Path);
        }

        public static void AddRange(List<ProductType> products)
        {
            foreach (var product in products)
            {
                var productInBuffer = Products.FirstOrDefault(p => p.CustomerId == product.CustomerId);
                if (productInBuffer == null) Add(product);
            }
            Document.Save(Path);
        }

        public static List<ProductType> GetProductsByName(string name)
        {
            return Products.FindAll(p => p.Name == name);
        }

        public static void Add(ProductType product)
        {
            if (product.CustomerId == Guid.Empty) product.CustomerId = Guid.NewGuid();
            if (product.ProductsWebCustomerId == Guid.Empty) product.ProductsWebCustomerId = Guid.NewGuid();
            if (product.CusumerIdRealStock == Guid.Empty) product.CusumerIdRealStock = Guid.NewGuid();

            SetStockReal(product);

            AddToXml(product);
            //if (SyncData.IsConnect) AddToDb(product);
        }

        private static void AddToXml(ProductType product)
        {
            Products.Add(product);
            ProductNames.Add(product.Name.ToUpper());

            var productsElement = Document.GetXElement("Product");
            productsElement.Add(ProductType.ToXElement(product, productsElement));
            Document.Save(Path);
        }

        private static void SetStockReals(IEnumerable<ProductType> products)
        {
            foreach (var product in products)
                SetStockReal(product);
        }

        private static void SetStockReal(ProductType product)
        {
            var stockReals =
                RepositoryStockReal.StockReals.FindAll(
                    s => (s.ProductsCustomerId == product.CustomerId) && ((s.IdEstablishment == Config.IdEstablishment) || s.IdEstablishment == Config.IdEstablishmentGros));

            TestDubl(stockReals, product);
        }

        private static void SetFromDb()
        {
            using (var connection = ConnectionFactory.CreateConnection())
                Products = connection.Query<ProductType>(QuerySelect + " WHERE Chp_cat = 0").ToList();
        }

        public static int GetAbCountFromDb()
        {
            var lastDate = RepositoryLastUpdate.GetUpdateDate();

            if (lastDate != null)
            {
                var dateA = lastDate.Value.AddMinutes(-1);
                var dateB = DateTime.Now.AddMinutes(1);

                return GetByDates(dateA, dateB).Count;
            }
            return 0;
        }

        private static List<ProductType> GetByDates(DateTime dateA, DateTime dateB)
        {
            var products = Products.FindAll(p => p.Date >= dateA && p.Date <= dateB);
            return products;
        }

        private static void AddToDb(ProductType product)
        {
            product.Date = DateTime.Now;

            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();
                var trans = connection.BeginTransaction();

                var res = connection.Execute(QueryInsertWeb, new {product.ProductsWebCustomerId}, trans);
                if (res <= 0) trans.Rollback();

                res = connection.Execute(QueryInsertProd, product, trans);
                if (res <= 0) trans.Rollback();

                res = connection.Execute(QueryInsertStockReal, new {product.CusumerIdRealStock, product.Qty, product.Price, product.CustomerId, Config.IdEstablishment}, trans);
                if (res <= 0) trans.Rollback();

                trans.Commit();
            }

            RepositoryLastUpdate.Update(true);
        }

        private static void UpdateFromDb(ProductType product)
        {
            product.Date = DateTime.Now;
            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();
                var trans = connection.BeginTransaction();

                var res = connection.Execute(QueryUpdateProd, product, trans);
                if (res != 1)
                {
                    trans.Rollback();
                    return;
                }

                res = connection.Execute(QueryUpdateStockReal, new {product.Qty, product.Price, product.CustomerId, Config.IdEstablishment, product.CusumerIdRealStock}, trans);
                if (res != 1)
                {
                    trans.Rollback();
                    return;
                }

                trans.Commit();
            }

            RepositoryLastUpdate.Update(true);
        }

        public static void Update(ProductType product)
        {
            UpdateInXml(product);
            UpdateFromDb(product);
        }

        public static void UpdateInXml(ProductType product)
        {
            var prodElement = Document.GetXElements("Product", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == product.CustomerId);
            ProductType.SetXmlValues(prodElement, product);

            if (!ProductNames.Contains(product.Name)) ProductNames.Add(product.Name);

            Document.Save(Path);
        }

        public static void Delete(ProductType product)
        {
            DeleteInXml(product);
            DeleteFromDb(product);
        }

        public static void DeleteInXml(ProductType product)
        {
            var prodElement = Document.GetXElements("Product", "rec").First(p => p.GetXElementValue("CustomerId").ToGuid() == product.CustomerId);
            prodElement.Remove();

            if (!ProductNames.Contains(product.Name)) ProductNames.Remove(product.Name);

            Document.Save(Path);
        }

        public static XElement GetXElementByElementName(string elementName, string value)
        {
            return Document.GetXElements("Product", "rec").FirstOrDefault(p => p.GetXElementValue(elementName) == value);
        }

        public static XElement GetXElementByBarcode(string barcode)
        {
            return Document.GetXElements("Product", "rec").FirstOrDefault(p => p.GetXElementValue("CodeBare").IndexOf("[" + barcode + "]", StringComparison.Ordinal) != -1);
        }

        public static IEnumerable<XElement> FiltrXElementsByElementName(IEnumerable<XElement> elements, string elementName, string ballance)
        {
            return elements.Where(el => el.GetXElementValue(elementName) == ballance);
        }

        public static IEnumerable<XElement> FiltrXElementsByName(IEnumerable<XElement> elements, string name)
        {
            return elements.Where(el => el.GetXElementValue("Name").ToUpper().IndexOf(name.ToUpper(), StringComparison.Ordinal) != -1);
        }

        public static IEnumerable<XElement> FiltrXElementsByBarCode(IEnumerable<XElement> elements, string barCode)
        {
            var barCodeText = barCode.Trim();
            return
                elements.Where(
                    el => el.GetXElementValue("CodeBare").Length > barCodeText.Length && el.GetXElementValue("CodeBare").Trim().Substring(0, barCodeText.Length) == barCodeText);
        }

        public static IEnumerable<XElement> FiltrXElementsByElementNameMinMax(IEnumerable<XElement> elements, string elementName, decimal min, decimal max)
        {
            return elements.Where(el => (el.GetXElementValue(elementName).ToDecimal() >= min && el.GetXElementValue(elementName).ToDecimal() <= max));
        }

        private static void DeleteFromDb(ProductType product)
        {
            const string queryDeleteFromStockReal = "DELETE FROM StockReal WHERE ProductsCustumerId = @CustomerId";
            const string queryDeleteFromProducts = "DELETE FROM Products  WHERE CustumerId= @CustomerId";
            const string queryDeleteFromProductsWeb = "DELETE FROM ProductsWeb  WHERE CustomerId= @ProductsWebCustomerId";

            using (var connection = ConnectionFactory.CreateConnection())
            {
                connection.Open();
                var trans = connection.BeginTransaction();

                var res = connection.Execute(queryDeleteFromStockReal, product, trans);
                if (res < 0)
                {
                    trans.Rollback();
                    return;
                }

                res = connection.Execute(queryDeleteFromProducts, product, trans);
                if (res != 1)
                {
                    trans.Rollback();
                    return;
                }

                res = connection.Execute(queryDeleteFromProductsWeb, product, trans);
                if (res != 1)
                {
                    trans.Rollback();
                    return;
                }

                trans.Commit();
            }

            RepositoryLastUpdate.Update(true);
        }

        private static void TestDubl(List<StockReal> stockReals, ProductType product)
        {
            try
            {
                var sr1 = stockReals.FirstOrDefault(p => p.IdEstablishment == Config.IdEstablishment);
                var sr2 = stockReals.FirstOrDefault(p => p.IdEstablishment == Config.IdEstablishmentGros);

                if (sr1 == null && sr2 == null)
                    LogService.Log(TraceLevel.Error, 321);
                else if (sr1 == null || sr2 == null)
                    LogService.Log(TraceLevel.Error, 322);

                var sr1Id = sr1 != null ? sr1.CustomerId : Guid.Empty;
                var sr2Id = sr2 != null ? sr2.CustomerId : Guid.Empty;
                foreach (var sr in stockReals.Where(sr => sr.CustomerId != sr1Id && sr.CustomerId != sr2Id))
                    RepositoryStockReal.DeleteById(sr.CustomerId);

                if (sr1 == null) sr1 = RepositoryStockReal.AddAsNull(product.CustomerId, Config.IdEstablishment);
                if (sr2 == null) sr2 = RepositoryStockReal.AddAsNull(product.CustomerId, Config.IdEstablishmentGros);

                product.StockReal.Add(sr1);
                product.StockReal.Add(sr2);

                product.Price = sr1.Price;
                product.PriceGros = sr2.Price;
                product.Qty = sr1.Qty;
                product.CusumerIdRealStock = sr1.CustomerId;
            }
            catch (Exception ex)
            {
                LogService.Log(TraceLevel.Error, 0356, "Product : " + product.Name + " - " + product.CodeBare + " - " + product.CustomerId + " - " + ex.Message + ".");
            }
        }

        public static void ModifAddOnlyFile(ProductType product)
        {
            var element = Document.GetXElements("Product", "rec").FirstOrDefault(el => el.GetXElementValue("CustomerId").ToGuid() == product.CustomerId);

            if (element != null)
            {
                var name = element.GetXElementValue("Name");
                var idx = ProductNames.FindIndex(p => p == name);
                if (idx > 0) ProductNames[idx] = product.Name;

                ProductType.SetXmlValues(element, product);
            }
            else
            {
                var productElement = Document.GetXElement("Product");
                productElement.Add(ProductType.ToXElement(product, productElement));

                ProductNames.Add(product.Name);

                var existProduct = Products.FirstOrDefault(p => p.CustomerId == product.CustomerId);
                if (existProduct != null) Products.Add(existProduct);
            }
            Document.Save(Path);
        }

        public static void Sync()
        {
            RepositoryGroupProduct.Sync();
            RepositoryTva.Sync();
            RepositoryStockReal.Sync();

            if (SyncData.IsConnect)
            {
                SetFromDb();
                SetStockReals(Products);
                SaveFile();
            }
            else
            {
                LoadFile();
                SetStockReals(Products);
            }
        }

        public static void SyncSingleProduct()
        {
            var date = RepositoryLastUpdate.GetUpdateDate();

            if (date != null)
            {
                Sync();

                var productsAb = GetByDates(date.Value.AddMinutes(-1), DateTime.Now.AddMinutes(1));

                if (productsAb.Count > 0)
                {
                    var group = RepositoryHistoryChangeProduct.GetGroup();
                    RepositoryHistoryChangeProduct.LoadFile();

                    foreach (var product in productsAb)
                    {
                        RepositoryHistoryChangeProduct.Compare(Document, product, group);
                        ModifAddOnlyFile(product);
                    }

                    RepositoryHistoryChangeProduct.SaveFile();
                }
            }
        }

        public static void Set()
        {
            RepositoryHistoryChangeProduct.LoadFile();
            RepositoryActionHashBox.Sync();
            RepositoryProductBc.Sync();

            if (SyncData.IsConnect)
            {
                if (GetAbCountFromDb() > 1000 || Config.FromLoadSyncAll || !GlobalVar.IsOpen)
                    Config.FromLoadSyncAll = false;
                else SyncSingleProduct();
            }
            else
            {
                Sync();
                Config.FromLoadSyncAll = false;
            }

            Sync();
            RepositoryCountry.Sync();

            if (SyncData.IsConnect) RepositoryLastUpdate.Update(false);
        }

        #region queryes

        private const string QuerySelect = @"SELECT
CustumerId as customerId,
Name as name,
CodeBare as codeBare,
[Desc] as [desc],
Chp_cat as chpCat,
Balance as balance,
(SELECT ContenanceBox FROM ProductsWeb WHERE CustomerId = Products.ProductsWeb_CustomerId) as contenance,
UniteContenance as uniteContenance,
Tare as tare,
Date as date,
TVACustumerId as tvaCustomerId,
ProductsWeb_CustomerId as productsWebCustomerId,
SubGrpProduct_Id as subGrpProductId
    FROM Products";

        private const string QueryInsertWeb = @"INSERT INTO ProductsWeb (
CustomerId,
Visible,
Images,
ContenancePallet,
Weight,
Frozen)
    VALUES (
@ProductsWebCustomerId, 
0, 
'', 
0,
0,
0)";

        private const string QueryInsertProd = @"INSERT INTO Products (
CustumerId,
Name,
CodeBare,
[Desc],
Chp_cat,
Balance,
Contenance,
UniteContenance,
Tare,
[Date],
TVACustumerId,
ProductsWeb_CustomerId,
SubGrpProduct_Id) 
    VALUES (
@CustomerId,
@Name,
@CodeBare,
@Desc,
@ChpCat,
@Balance,
@Contenance,
@UniteContenance,
@Tare, 
@Date,
@TVACustomerId,
@ProductsWebCustomerId,
@CusumerIdSubGroup)";

        private const string QueryInsertStockReal = @"INSERT INTO StockReal (
CustomerId,
QTY,
MinQTY,
Price,
ProductsCustumerId,
Establishment_CustomerId)
    VALUES (
@CusumerIdRealStock,
@Qty,
10,
@Price,
@CustomerId,
@IdEstablishment)";

        private const string QueryUpdateProd = @"UPDATE Products SET 
[Name] = @Name, 
[CodeBare] = @CodeBare,
[Desc] = @Desc,
[TVACustumerId] = @TvaCustomerId,
[SubGrpProduct_Id] = @Sgrp,
[Balance] = @Balance,
[Contenance] = @Contenance,
[UniteContenance] = @UniteContenance,
[Tare] = @Tare,
[Date] = @Date
    WHERE CustumerId= @CustomerId";

        private const string QueryUpdateStockReal = @"UPDATE StockReal SET 
QTY = @Qty,
MinQTY = 0,
Price = @Price,
ProductsCustumerId = @CustomerId,
Establishment_CustomerId = @IdEstablishment
    WHERE CustomerId = @CusumerIdRealStock";

        #endregion
    }
}