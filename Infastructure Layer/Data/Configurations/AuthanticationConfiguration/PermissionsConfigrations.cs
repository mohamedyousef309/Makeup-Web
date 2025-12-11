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
    public class PermissionsConfigrations : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");

            builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(x=>x.userPermissions).WithOne(x=>x.permission).HasForeignKey(x=>x.PermissionId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
