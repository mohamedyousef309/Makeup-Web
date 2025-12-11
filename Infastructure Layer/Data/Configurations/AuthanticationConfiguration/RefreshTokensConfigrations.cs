using Domain_Layer.Entites.Authantication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.Configurations.AuthanticationConfiguration
{
    public class RefreshTokensConfigrations : IEntityTypeConfiguration<RefreshTokens>
    {
        public void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");

            builder.Property(rt => rt.userid)
                  .IsRequired();

           
            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(500); 

            
            builder.Property(rt => rt.IsUsed)
                   .HasDefaultValue(false);

            builder.Property(rt => rt.ExpiresOn)
                   .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();

            builder.Property(rt => rt.RevokedOn)
                   .IsRequired(false);

            builder.HasIndex(rt => rt.Token)
                   .IsUnique();
        }
    }
}
