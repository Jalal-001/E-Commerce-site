using CommerceApp.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p=>p.ProductId);
            builder.Property(p=>p.Name).HasMaxLength(50).IsRequired();
            builder.Property(p=>p.Price).IsRequired();
            builder.Property(p=>p.Description).IsRequired();
            builder.Property(p=>p.ImgUrl).IsRequired();
            builder.Property(p=>p.Url).IsRequired();
            //builder.Property(o => o.OrderDate).HasDefaultValueSql("getdate()"); //for mssql
            builder.Property(o => o.DateAdded).HasDefaultValueSql("Date('now')"); //for sqlite
        }
    }
}
