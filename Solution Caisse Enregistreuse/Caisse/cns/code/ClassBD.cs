using cns.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cns.code
{
    class ClassBD
    {
        prt_BDCAEntities bd;
        public ClassBD ()
        {
            bd = new prt_BDCAEntities();
        }
        public class ClassProduct : ClassBD
        {
            public void addDisc (string nc)
            {
                DiscountCards dc = new DiscountCards();

                dc.Active = true;

                dc.custumerId = Guid.NewGuid();

                dc.DateTimeLastAddProduct = DateTime.Now;

                dc.numberCard = nc;

                dc.points = 0;

              //  dc.InfoClients_custumerId = Guid.Empty;
              
               

                bd.DiscountCards.Add(dc);



                bd.SaveChanges();
            }








            /*
            public void add (Products p)
            {
             //   if (bd.Products.SingleOrDefault(l => l.CustumerId == p.CustumerId) == null)
                {
                    bd.Products.Add(p);
                    bd.SaveChanges();
                }
            }

            public Establishment sel(Guid g)
            {
                return bd.Establishment.FirstOrDefault(l => l.CustomerId == g);
            }

            public void del(Products p)
            {
                bd.Products.Remove(p);
                bd.SaveChanges();
            }
            public void mod(Products p)
            {
                Products g = bd.Products.SingleOrDefault(l => l.CustumerId == p.CustumerId);
                g.Balance = p.Balance;
                g.Chp_cat = p.Chp_cat;
                g.CodeBare = p.CodeBare;
                g.Contenance = p.Contenance;
                g.Date = DateTime.Now;
                g.Desc = "";
                g.Group = p.Group;
                g.Name = p.Name;
                g.Price = p.Price;
                g.QTY = p.QTY;
                g.SubGruop = p.SubGruop;
                g.Tare = p.Tare;
                g.Tva = p.Tva;
                g.UniteContenance = p.UniteContenance;
                bd.SaveChanges();
            }
            */
        }
        
    }
}
