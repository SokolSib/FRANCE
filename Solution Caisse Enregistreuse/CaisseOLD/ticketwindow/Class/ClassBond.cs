using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ticketwindow.Class
{
    public class ClassBond
    {

        public static XDocument x;

        public class count_Currency
        {
            public int count { get; set; }

            public ClassSync.Currency currency { get; set; }
        }

        public static List<count_Currency> cc = new List<count_Currency>();

        public static ClassSync.TypesPayDB getTypesPayDBFromId(int Id)
        {
            ClassSync.TypesPayDB x = ClassSync.TypesPayDB.t.Where(l => l.Id == Id).SingleOrDefault();
            return x;
        }

        public static void setMoneySum(decimal money)
        {

            x = new XDocument();

            x.Add(new XElement("MoneySum", new XAttribute("Money", money)));
        }

        public static bool getType(string typesPayFromOption)
        {
            typesPayFromOption = typesPayFromOption ?? "typesPayTiroir";

            IEnumerable<XElement> elms = x.Element("MoneySum").Elements("pay")
               .Where(l => bool.Parse(
                   l.Attribute(typesPayFromOption).Value));
            return elms.Count() > 0 ;
          
        }
        public static void pay(ClassSync.TypesPayDB typesPay, decimal money)
        {
            x.Element("MoneySum").Add(new XElement("pay",
                new XAttribute("typesPayName", typesPay.Name),
                new XAttribute("typesPayCodeCompta", typesPay.CodeCompta),
                new XAttribute("typesPayEtat", typesPay.Etat),
                new XAttribute("typesPayNameCourt", typesPay.NameCourt),
                new XAttribute("typesPayRendu_Avoir", typesPay.Rendu_Avoir),
                new XAttribute("typesPayTiroir", typesPay.Tiroir),
                new XAttribute("typesPayCurMod", typesPay.CurMod),
                new XAttribute("typesPayId", typesPay.Id),
                new XAttribute("money", money)));

        }

        public static void clearTypesPay()
        {
            if (x != null)
            {
                IEnumerable<XElement> elms = x.Element("MoneySum").Elements("pay");
                if (elms != null)
                    elms.Remove();
            }
        }
        public static decimal getSumMoney()
        {

            return x != null ? decimal.Parse(x.Element("MoneySum").Attribute("Money").Value.Replace(".", ",")) : 0.0m;
        }
        public static decimal residue()
        {
            decimal m = 0.0m;

            foreach (var elm in ClassSync.TypesPayDB.t)
                m += getMoneyFromType(elm);


            return getSumMoney() - m;
        }

        public static List<ticketwindow.Class.ClassSync.Currency> List_Currency = new List<ClassSync.Currency>();

        public static decimal addCurrency(ticketwindow.Class.ClassSync.Currency c, int count)
        {
            count_Currency cc_ = new count_Currency();

            cc_.count = count;

            cc_.currency = c;

            int indx = cc.FindIndex(l => l.currency.CustomerId == cc_.currency.CustomerId);
            if (indx == -1)
                cc.Add(cc_);
            else cc[indx].count += count;

            int h = 0;
            while (h < count)
            {
                h++;
                List_Currency.Add(c);
            }
            return List_Currency.Sum(l => l.Currency_money);
        }

        public static void clearCurrency(Class.ClassSync.TypesPayDB tp)
        {
            List_Currency.RemoveAll(l => l.TypesPayId == tp.Id);
            cc.Clear();
        }

        public static decimal getMoneyFromType(ClassSync.TypesPayDB typesPay)
        {
            if (x != null)
            {
                List<XElement> elms = x.Element("MoneySum").Elements("pay").Where(l => int.Parse(l.Attribute("typesPayId").Value) == typesPay.Id).ToList();

                decimal r = 0.0m;

                foreach (XElement e in elms)
                {
                    r += decimal.Parse(e.Attribute("money").Value.Replace(".", ","));
                }

                return r;
            }
            return -0.0001m;
        }
        public static decimal calc()
        {
            decimal sum = decimal.Parse(x.Element("MoneySum").Attribute("Money").Value.Replace(".", ","));

            IEnumerable<XElement> elms = x.Element("MoneySum").Elements("pay");

            decimal getSum = 0.0m;

            foreach (XElement e in elms)
            {
                bool typesPayRendu_Avoir = bool.Parse(e.Attribute("typesPayRendu_Avoir").Value);
                decimal money = decimal.Parse(e.Attribute("money").Value.Replace(".", ","));
                getSum += money;
            }

            return sum - getSum;
        }

        public static decimal residue1()
        {
            decimal result = 0.0m;

            decimal sum = getSumMoney();

            foreach (var elm in ClassSync.TypesPayDB.t)
                result += getMoneyFromType(elm);

            decimal rendu = 0.0m;


            if (sum - result < 0)
            {

                foreach (var elm in ClassSync.TypesPayDB.t)
                    rendu += elm.Rendu_Avoir ?? false ? getMoneyFromType(elm) : 0.0m;

                result = -rendu - result;
            }

            else

                result = getSumMoney() - result;

            return result;
        }

        public static List<count_Currency> transform(decimal d, ClassSync.TypesPayDB typePay )
        {
            List<count_Currency> r = new List<count_Currency>();

            List<ClassSync.Currency> lc = ClassSync.Currency.List_Currency.Where(l=>l.TypesPayId == typePay.Id ).ToList();

            lc.Sort((x,y)=> decimal.Compare(-x.Currency_money,-y.Currency_money));
            
            List<ClassSync.Currency> lb  = new List<ClassSync.Currency> ();


            
            foreach (ClassSync.Currency c in lc)
            {
           
                while (d >= 0)
                {
                    d = d - c.Currency_money;
                    if (d >= 0)
                    {
                        count_Currency countCurrency = new count_Currency();
                        countCurrency.count = 1;
                        countCurrency.currency = c;
                        r.Add(countCurrency);
                    }
                    else
                    {
                        d = d + c.Currency_money;

                        break;
                    }
                }

            }



            return r;
        }
    }
}
