﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BDCAtestEntities : DbContext
    {
        public BDCAtestEntities()
            : base("name=BDCAtestEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DevisId> DevisId { get; set; }
        public virtual DbSet<DevisWeb> DevisWeb { get; set; }
        public virtual DbSet<Establishment> Establishment { get; set; }
        public virtual DbSet<GrpProductSet> GrpProductSet { get; set; }
        public virtual DbSet<InfoClients> InfoClients { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsBC> ProductsBC { get; set; }
        public virtual DbSet<ProductsWeb> ProductsWeb { get; set; }
        public virtual DbSet<StockReal> StockReal { get; set; }
        public virtual DbSet<SubGrpNameSet> SubGrpNameSet { get; set; }
        public virtual DbSet<TranslateNameGroups> TranslateNameGroups { get; set; }
        public virtual DbSet<TranslateNameProductsSet> TranslateNameProductsSet { get; set; }
        public virtual DbSet<TranslateUniteContenance> TranslateUniteContenance { get; set; }
        public virtual DbSet<TVA> TVA { get; set; }
    }
}
