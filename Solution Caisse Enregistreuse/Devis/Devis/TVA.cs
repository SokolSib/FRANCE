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
    
    public partial class TVA
    {
        public TVA()
        {
            this.Products = new HashSet<Products>();
        }
    
        public System.Guid CustumerId { get; set; }
        public string Id { get; set; }
        public string val { get; set; }
    
        public virtual ICollection<Products> Products { get; set; }
    }
}
