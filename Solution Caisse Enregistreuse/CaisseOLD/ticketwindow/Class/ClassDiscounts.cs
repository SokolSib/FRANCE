using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ticketwindow.Class
{

    class ClassDiscounts
    {
        public class Discount
        {
            public DateTime dt = new DateTime().AddHours(24);

            public DateTime lastDateUpd { get; set; }

            public string barcode { get; set; }

            public string nameFirst { get; set; }

            public string nameLast { get; set; }

            public decimal procent = 0.0m;

            public decimal procent_default = 0.0m;

            public int points = 0;

            public int maxPoints = 8;

            public bool addPoints = false;

            public decimal moneyMax = 20.0m;

            public bool discountSet = false;

            public bool showMessaget = true;

            public Guid InfoClients_customerId;
        }

        public static Discount client = new Discount();

        public static void restoreDiscount()
        {
            client.lastDateUpd = new DateTime() ;
            client.barcode = null;
            client.nameLast = null;
            client.nameFirst = null;
            client.points = 0;
            client.procent = 0.0m;
            client.addPoints = false;
            client.discountSet = false;
            client.showMessaget = true;
            ClassCheck.discountCalc();
            new ClassFunctuon().writeToatl();
        }

        public static ClassSync.Discount.DiscountCards getDiscount(string codebar)
        {
            ClassSync.Discount.DiscountCards dc = ClassSync.Discount.DiscountCards.OneDiscountCards(codebar);

            if (dc == null)
            {
                new ClassFunctuon().showMessageSB("La carte n'existe pas ou il y a peut-être des problèmes avec l'Internet");
            }
            else
            {
                if (dc.active)
                {
                    client.barcode = codebar;
                    client.points = dc.points;
                    client.InfoClients_customerId = dc.InfoClients_custumerId;
                    client.procent = 0.0m;
                    client.lastDateUpd = dc.DateTimeLastAddProduct;
                    
                    new ClassFunctuon().writeToatl();
                }

                else
                {
                    new ClassFunctuon().showMessageSB(" La carte est bloquée ");
                }

            }
            return dc;
        }

        public static bool setDiscountPoint(string codebar, int points, bool set)
        {

            ClassSync.Discount.DiscountCards dc = ClassSync.Discount.DiscountCards.OneDiscountCards(codebar);

            if (dc != null)
            {
                if (points >= ClassDiscounts.client.maxPoints) points = ClassDiscounts.client.maxPoints;

                dc.points = set ? points : dc.points ;
               
                if (set)
                    return ClassSync.Discount.DiscountCards.upd(dc) > 0 ? true : false;
              
            }
            return false;
        }

        public static ClassSync.Discount.InfoClients getInfClt(Guid InfoClients_customerId)
        {
            if (InfoClients_customerId != null)
            {
                ClassSync.Discount.InfoClients inf = ClassSync.Discount.InfoClients.sel(InfoClients_customerId);
             //   ClassSync.Discount.InfoClients.LInfoClients.Find(l => l.DiscountCards.ToList().Find(l2 => l2.numberCard.TrimEnd().TrimStart()
             //       == codebar.TrimEnd().TrimStart()) != null);
                if (inf != null)
                {
                    client.nameFirst = inf.Name;
                    client.nameLast = inf.Surname;
                }
                else
                {
                    client.nameFirst = " Inconnue ";
                    client.nameLast = "";
                }
                return inf;
            }
            return null;
        }
    }
}
