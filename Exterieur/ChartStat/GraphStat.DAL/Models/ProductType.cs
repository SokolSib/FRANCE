using System;

namespace ChartStat.Model.Models
{
    public class ProductType
    {
        public ProductType(Guid custumerId, string name, string codeBare)
        {
            Id = custumerId;
            Name = name;
            BarCode = codeBare;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string BarCode { get; private set; }

        public override string ToString()
        {
            return string.Concat(Name, " ", BarCode);
        }
    }
}