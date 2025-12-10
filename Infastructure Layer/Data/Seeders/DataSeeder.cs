using Domain_Layer.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.Seeders
{
    public class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext appDbContext)
        {
            await appDbContext.Database.MigrateAsync();

            if (!await appDbContext.Roles.AnyAsync())
            {
                 await seedRolsAsnyc(appDbContext);
            }
        }

        private static async Task seedRolsAsnyc(AppDbContext Context) 
        {
            var roles = new List<Domain_Layer.Entites.Authantication.Role>
            {
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.Admin_id,Name = RoleConstants.AdminName },
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.SuperAdmin_id,Name=RoleConstants.SuperAdminName },
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.User_id,Name=RoleConstants.UserName }
            };

            await Context.Roles.AddRangeAsync(roles);
            await Context.SaveChangesAsync();
        }
    }
}
