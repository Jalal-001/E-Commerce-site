﻿using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Concrete.EfCore
{
    public class CommerceAppContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlite("Data Source=CommerceAppDb");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<ProductCategory>()
            .HasKey(c => new {c.CategoryId,c.ProductId});
    }
}
