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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderNumber).IsRequired();
            builder.Property(o => o.OrderDate).HasDefaultValueSql("getdate()"); //for mssql
            //builder.Property(o => o.OrderDate).HasDefaultValueSql("Date('now')"); //for sqlite
            builder.Property(o => o.UserId).IsRequired();
            builder.Property(o => o.FirstName).HasMaxLength(30).IsRequired();
            builder.Property(o => o.LastName).HasMaxLength(30).IsRequired();
            builder.Property(o => o.Address).HasMaxLength(500).IsRequired();
            builder.Property(o => o.City).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Phone).HasMaxLength(20).IsRequired();
            builder.Property(o => o.Email).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Note).HasMaxLength(200).IsRequired();
            builder.Property(o => o.PaymentId).IsRequired();
            builder.Property(o => o.ConversationId).IsRequired();
            builder.Property(o => o.OrderState).IsRequired();
            builder.Property(o => o.EnumPaymentType).IsRequired();
        }
    }
}
