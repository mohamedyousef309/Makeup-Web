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
    public class UserConfigrations:IEntityTypeConfiguration<User>
    {
      
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");


            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();


            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(u => u.Picture)
                   .HasMaxLength(250);

            builder.Property(u => u.UserAddress)
                   .HasMaxLength(250);

            builder.Property(u => u.EmailConfirmed)
                   .HasDefaultValue(false);

            
            builder.Property(u => u.FailedLoginAttempts)
                   .HasDefaultValue(0);

            builder.Property(u => u.LockoutEnd)
                   .IsRequired(false); 

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            builder.HasIndex(u => u.Username)
                   .IsUnique();



            builder.HasMany(x=>x.UserRoles).WithOne(x=>x.user).HasForeignKey(x=>x.Userid).OnDelete(DeleteBehavior.Cascade); 
            

           builder.HasMany(x=>x.userPermissions).WithOne(x=>x.user).HasForeignKey(x=>x.Userid).OnDelete(DeleteBehavior.Cascade);  

            builder.HasMany(x=>x.refreshTokens).WithOne(x=>x.User).HasForeignKey(x=>x.userid).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x=>x.userTokens).WithOne(x=>x.User).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);


        }
    }
}
