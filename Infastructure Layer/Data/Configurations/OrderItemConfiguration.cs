using Domain_Layer.Entites;
using Domain_Layer.Entites.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItems>
    {
        public void Configure(EntityTypeBuilder<OrderItems> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");

            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);

            builder.Property(oi => oi.ProductName)
              .IsRequired()
              .HasMaxLength(200);

            builder.Property(oi => oi.PictureUrl)
                   .IsRequired();

            builder.Property(oi => oi.Quantity)
                   .IsRequired();

            builder.Property(oi => oi.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();


        }
    }
}
