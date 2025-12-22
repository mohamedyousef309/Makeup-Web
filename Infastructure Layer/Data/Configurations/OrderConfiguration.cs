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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");

            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            builder.Property(o => o.BuyerEmail)
                    .IsRequired()
                    .HasMaxLength(256);

            builder.Property(o => o.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(o => o.Address)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(o => o.orderDate)
                   .IsRequired();

            builder.Property(o => o.status)
                  .HasConversion<string>()
                  .IsRequired();


            builder.Property(x => x.subTotal)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.HasMany(x => x.Items)
                   .WithOne(x => x.order)
                   .HasForeignKey(x => x.orderid)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    }
