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
    public class RepositoryActionHashBox
    {
        #region sqripts

        private const string Query = @"SELECT 
    CustomerId as customerId,
    NameAction as nameAction,
    Date as date,
    A as a,
    B as b,
    Est as est,
    Enabled as enabled,
    XML as xml
FROM ActionsCaisse
    WHERE (Enabled = 1) AND (Est = @idEstablishment) OR (Est IS NULL)";

        #endregion

        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);
        private static XDocument _documentA;
        private static readonly string PathA = Config.AppPath + @"Data\ActionsCaisse.xml";
        private static readonly string Path = Config.AppPath + @"Data\ActionHashBoxes.xml";

        public static List<ActionCashBox> ActionCashBoxes = new List<ActionCashBox>();

        private static void SetFromDb()
        {
            var idEstablishment = Config.IdEstablishment;

            using (var connection = ConnectionFactory.CreateConnection())
                ActionCashBoxes = connection.Query<ActionCashBox>(Query, new {idEstablishment}).ToList();
        }

        private static void SaveFile()
        {
            var rootA = new XElement("ActionsCaisse");
            var root = new XElement("ActionHashBoxes");

            foreach (var actionCash in ActionCashBoxes)
            {
                rootA.Add(new XElement("item", XElement.Parse(actionCash.Xml)));
                root.Add(ActionCashBox.ToXElement(actionCash));
            }

            File.WriteAllText(PathA, new XDocument(rootA).ToString());
            File.WriteAllText(Path, new XDocument(root).ToString());
        }

        private static void LoadFile()
        {
            if (File.Exists(PathA)) _documentA = XDocument.Load(PathA);

            if (File.Exists(Path))
            {
                var document = XDocument.Load(Path);

                ActionCashBoxes.Clear();
                foreach (var element in document.GetXElementsIfExistElement("ActionHashBoxes", "rec"))
                    ActionCashBoxes.Add(ActionCashBox.FromXElement(element));
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

        public static XDocument MergeProductsInCheck(XDocument check)
        {
            var resultProducts = new List<XElement>();

            foreach (var checkProduct in check.GetXElements("check", "product").Reverse())
            {
                var customerId = checkProduct.GetXElementValue("CustomerId");
                var price = checkProduct.GetXElementValue("price").ToDecimal();

                var mergedProduct = resultProducts.Find(p => p.GetXElementValue("CustomerId") == customerId && p.GetXElementValue("price").ToDecimal() == price);

                if (mergedProduct == null)
                {
                    checkProduct.GetXElement("ii").SetValue(resultProducts.Count);
                    resultProducts.Add(new XElement(checkProduct));
                }
                else
                {
                    mergedProduct.GetXElement("qty").SetValue(mergedProduct.GetXElementValue("qty").ToDecimal() + checkProduct.GetXElementValue("qty").ToDecimal());
                    mergedProduct.GetXElement("sumDiscount")
                        .SetValue(mergedProduct.GetXElementValue("sumDiscount").ToDecimal() + checkProduct.GetXElementValue("sumDiscount").ToDecimal());
                    mergedProduct.GetXElement("total").SetValue(mergedProduct.GetXElementValue("total").ToDecimal() + checkProduct.GetXElementValue("total").ToDecimal());
                }
            }
            check.GetXElements("check", "product").Remove();

            foreach (var product in resultProducts)
                check.GetXElement("check").Add(product);

            return GetActionsDocument(check);
        }

        private static XDocument GetActionsDocument(XDocument check)
        {
            var checkProducts = check.GetXElements("check", "product");

            if (!File.Exists(PathA)) SaveFile();

            _documentA = XDocument.Load(PathA);
            var actions = _documentA.GetXElements("ActionsCaisse", "item").ToArray();

            var resultProducts = new List<XElement>();

            foreach (var checkProduct in checkProducts)
            {
                var checkCustomerId = checkProduct.GetXElementValue("CustomerId").ToGuid();
                var flag = false;

                foreach (var action in actions)
                {
                    var actionType = action.GetXElementValue("DeActions", "type").ToInt();
                    var actionA = action.GetXElementValue("DeActions", "A").ToDateTime();
                    var actionB = action.GetXElementValue("DeActions", "B").ToDateTime();
                    var actionProductCustomerId = action.GetXElementValue("DeActions", "productCustomerId").ToGuid();
                    var actionQty = action.GetXElementValue("DeActions", "qty").ToDecimal();
                    var actionPrix = action.GetXElementValue("DeActions", "prix").ToDecimal();

                    var dateNow = DateTime.Now;

                    if (checkCustomerId == actionProductCustomerId && dateNow >= actionA && dateNow <= actionB && actionPrix != checkProduct.GetXElementValue("price").ToDecimal())
                    {
                        if (actionType == 1)
                        {
                            var qty = checkProduct.GetXElementValue("qty").ToDecimal();

                            if (actionQty == 1)
                            {
                                var n = new XElement(checkProduct);

                                var prix = checkProduct.GetXElementValue("price").ToDecimal();

                                n.GetXElement("qty").SetValue(qty);
                                n.GetXElement("price").SetValue(actionPrix);
                                n.GetXElement("total").SetValue(actionPrix*qty);
                                n.GetXElement("sumDiscount").SetValue(prix*qty - actionPrix*qty);
                                n.GetXElement("ii").SetValue(resultProducts.Count);
                                resultProducts.Add(n);
                                flag = true;
                            }
                            else if (actionQty > 1 && qty >= actionQty)
                            {
                                var n = new XElement(checkProduct);

                                var c = (int) (qty/actionQty);
                                var f = qty - actionQty*c;
                                var prix = checkProduct.GetXElementValue("price").ToDecimal();

                                n.GetXElement("qty").SetValue(actionQty*c);
                                n.GetXElement("total").SetValue(actionPrix*actionQty*c);
                                n.GetXElement("sumDiscount").SetValue((prix*actionQty*c - actionPrix*actionQty*c));
                                n.GetXElement("Discount").SetValue(Math.Truncate(100*actionPrix/prix));

                                if (f > 0)
                                {
                                    var n2 = new XElement(checkProduct);

                                    n2.GetXElement("qty").SetValue(f);
                                    n2.GetXElement("total").SetValue(prix*f);
                                    n2.GetXElement("sumDiscount").SetValue(0);
                                    n2.GetXElement("ii").SetValue(resultProducts.Count);
                                    resultProducts.Add(n2);
                                }
                                n.GetXElement("ii").SetValue(resultProducts.Count);
                                resultProducts.Add(n);
                                flag = true;
                            }
                        }
                    }
                }
                if (!flag)
                    resultProducts.Add(checkProduct);
            }

            check.GetXElements("check", "product").Remove();

            foreach (var product in resultProducts)
                check.GetXElement("check").AddFirst(product);

            return check;
        }
    }
}