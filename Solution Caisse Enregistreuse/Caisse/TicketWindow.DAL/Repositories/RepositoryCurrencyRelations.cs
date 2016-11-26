using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.Extensions;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml.+
    /// </summary>
    public class RepositoryCurrencyRelations
    {
        public static XDocument Document;
        public static List<CountCurrency> CountCurencys = new List<CountCurrency>();
        public static List<Currency> Currencys = new List<Currency>();

        public static void SetMoneySum(decimal money)
        {
            Document = new XDocument(new XElement("MoneySum", new XAttribute("Money", money)));
        }

        public static bool IsExistDrawerTypeInDocument()
        {
            return Document.GetXElements("MoneySum", "pay").
                Any(el => bool.Parse(el.Attribute("typesPayTiroir").Value));
        }

        public static void Pay(TypePay typesPay, decimal money)
        {
            Document.GetXElement("MoneySum").Add(new XElement("pay",
                new XAttribute("typesPayName", typesPay.Name),
                new XAttribute("typesPayCodeCompta", typesPay.CodeCompta.HasValue ? typesPay.CodeCompta.Value.ToString() : string.Empty),
                new XAttribute("typesPayEtat", typesPay.Etat.HasValue ? typesPay.Etat.Value.ToString() : string.Empty),
                new XAttribute("typesPayNameCourt", typesPay.NameCourt),
                new XAttribute("typesPayRendu_Avoir", typesPay.RenduAvoir.HasValue ? typesPay.RenduAvoir.Value.ToString() : string.Empty),
                new XAttribute("typesPayTiroir", typesPay.Tiroir.HasValue ? typesPay.Tiroir.Value.ToString() : string.Empty),
                new XAttribute("typesPayCurMod", typesPay.CurMod.HasValue ? typesPay.CurMod.Value.ToString() : string.Empty),
                new XAttribute("typesPayId", typesPay.Id),
                new XAttribute("money", money)));
        }

        public static decimal GetSumMoney()
        {
            return Document != null
                ? Document.GetXAttributeValue("MoneySum", "Money").ToDecimal()
                : 0;
        }

        /// <summary>
        ///     Остаток.
        /// </summary>
        /// <returns>Остаток.</returns>
        public static decimal Residue()
        {
            var sum = RepositoryTypePay.TypePays.Sum(elm => GetMoneyFromType(elm));
            return GetSumMoney() - sum;
        }

        public static decimal AddCurrency(Currency currency, int count)
        {
            var countCurrency = new CountCurrency(count, currency);

            var searched = CountCurencys.Find(cc => cc.Currency.CustomerId == countCurrency.Currency.CustomerId);

            if (searched == null)
                CountCurencys.Add(countCurrency);
            else searched.Count += count;

            var h = 0;
            while (h < count)
            {
                h++;
                Currencys.Add(currency);
            }
            return Currencys.Sum(c => c.CurrencyMoney);
        }

        public static void ClearCurrency(TypePay typePay)
        {
            Currencys.RemoveAll(c => c.TypesPayId == typePay.Id);
            CountCurencys.Clear();
        }

        public static decimal GetMoneyFromType(TypePay typePay)
        {
            if (Document != null)
            {
                var elements = Document.GetXElements("MoneySum", "pay").
                    Where(l => l.GetXAttributeValue("typesPayId").ToInt() == typePay.Id).ToList();

                return elements.Sum(e => e.GetXAttributeValue("money").ToDecimal());
            }
            return -0.0001m;
        }

        public static decimal Calc()
        {
            var sum = Document.GetXAttributeValue("MoneySum", "Money").ToDecimal();
            var elements = Document.GetXElements("MoneySum", "pay");
            var sumMoney = elements.Sum(e => e.GetXAttributeValue("money").ToDecimal());
            return sum - sumMoney;
        }

        public static List<CountCurrency> Transform(decimal d, TypePay typePay)
        {
            var result = new List<CountCurrency>();

            var currencys = RepositoryCurrency.Currencys.Where(c => c.TypesPayId == typePay.Id).ToList();
            currencys.Sort((x, y) => decimal.Compare(-x.CurrencyMoney, -y.CurrencyMoney));

            foreach (var c in currencys)
                while (d >= 0)
                {
                    d = d - c.CurrencyMoney;

                    if (d >= 0)
                        result.Add(new CountCurrency(1, c));
                    else
                    {
                        d = d + c.CurrencyMoney;
                        break;
                    }
                }

            return result;
        }

        /* не используются
        private static void ClearTypesPay()
        {
            if (Document != null)
            {
                var elements = Document.GetXElements("MoneySum", "pay");

                if (elements != null)
                    elements.Remove();
            }
        }
         
        private static decimal ResidueDone()
        {
            var sum = GetSumMoney();
            var result = RepositoryTypePay.TypePays.Sum(elm => GetMoneyFromType(elm));
            
            var rendu = 0.0m;

            if (sum - result < 0)
            {
                foreach (var typePay in RepositoryTypePay.TypePays)
                    rendu += typePay.RenduAvoir ?? false ? GetMoneyFromType(typePay) : 0.0m;

                return -rendu - result;
            }
            return GetSumMoney() - result;
        }*/
    }
}