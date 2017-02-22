using System;

namespace ChartStat.Model.Models
{
    public class StatSalesType
    {
        public StatSalesType(Guid custumerId, string barCode, DateTime date, string name, decimal qty, decimal priceHt, decimal tva, decimal sumDiscount, decimal total, int number)
        {
            CustumerId = custumerId;
            BarCode = barCode;
            Date = date;
            Name = name;
            Qty = qty;
            PriceHt = priceHt;
            Tva = tva;
            Total = total;
            SumDiscount = sumDiscount;
            FullName = string.Format("{0} ({1})", name, barCode);
            Number = number;
        }

        public StatSalesType(Guid custumerId, string barCode, DateTime date, string name, decimal qty, decimal priceHt, decimal tva, decimal sumDiscount, decimal total, int number, decimal prix)
        {
            CustumerId = custumerId;
            BarCode = barCode;
            Date = date;
            Name = name;
            Qty = qty;
            PriceHt = priceHt;
            Prix = prix;
            Tva = tva;
            Total = total;
            SumDiscount = sumDiscount;
            FullName = string.Format("{0} ({1})", name, barCode);
            Number = number;
        }

        public StatSalesType(Guid custumerId, DateTime date, string name, string barCode, decimal priceHt, decimal qty, decimal prix, decimal tva, decimal sumDiscount, decimal total, int number)
        {
            CustumerId = custumerId;
            Date = date;
            Name = name;
            BarCode = barCode;
            PriceHt = priceHt;
            Qty = qty;
            Prix = prix;
            Tva = tva;
            SumDiscount = sumDiscount;
            Total = total;
            Number = number;
            FullName = string.Format("{0} ({1})", name, barCode);
        }

        public Guid CustumerId { get; private set; }
        public string BarCode { get; private set; }
        public DateTime Date { get; private set; }
        public string Name { get; private set; }
        public decimal Qty { get; private set; }
        public decimal PriceHt { get; private set; }
        public decimal Prix { get; private set; }
        public decimal Tva { get; private set; }
        public decimal SumDiscount { get; private set; }
        public decimal Total { get; private set; }
        public string FullName { get; private set; }
        public int Number { get; private set; }

        public override string ToString()
        {
            return CustumerId.ToString();
        }
    }
}