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
    public class UserPermissionsConfigrations : IEntityTypeConfiguration<UserPermissions>
    {

        public void Configure(EntityTypeBuilder<UserPermissions> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnType("int");

            builder.HasKey(x => new { x.Userid, x.PermissionId });
        }
    }
}
