//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Devis
{
    using System;
    using System.Collections.Generic;
    
    public partial class Establishment
    {
        public Establishment()
        {
            this.StockReal = new HashSet<StockReal>();
        }
    
        public System.Guid CustomerId { get; set; }
        public Nullable<int> Type { get; set; }
        public string Name { get; set; }
        public string CP { get; set; }
        public string Ville { get; set; }
        public string Adresse { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string SIRET { get; set; }
        public string NTVA { get; set; }
        public string CodeNAF { get; set; }
        public string Fax { get; set; }
    
        public virtual ICollection<StockReal> StockReal { get; set; }
    }
}
