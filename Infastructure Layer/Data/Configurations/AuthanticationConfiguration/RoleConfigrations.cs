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
    public class RoleConfigrations : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasMany(x => x.UserRoles).WithOne(x => x.role).HasForeignKey(x => x.Roleid).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
