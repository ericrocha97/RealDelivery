﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealDelivery
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_a464fd_realdevEntities : DbContext
    {
        public db_a464fd_realdevEntities()
            : base("name=db_a464fd_realdevEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<endereco> endereco { get; set; }
        public virtual DbSet<grupo> grupo { get; set; }
        public virtual DbSet<impress> impress { get; set; }
        public virtual DbSet<produto> produto { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
        public virtual DbSet<usuario_perm> usuario_perm { get; set; }
        public virtual DbSet<teste> teste { get; set; }
    }
}
