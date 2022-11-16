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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(ot => ot.Id);
            builder.Property(ot => ot.OrderId).IsRequired();
            builder.Property(ot => ot.ProductId).IsRequired();
            builder.Property(ot => ot.Price).IsRequired();
            builder.Property(ot => ot.Quantity).IsRequired();
        }
    }
}
