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

            if (!appDbContext.Users.Any())
            {
                await seedAdminUserAsync(appDbContext);
            }
        }

        private static async Task seedRolsAsnyc(AppDbContext Context)
        {
            var roles = new List<Domain_Layer.Entites.Authantication.Role>
            {
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.Admin_id, Name = RoleConstants.AdminName },
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.SuperAdmin_id, Name=RoleConstants.SuperAdminName },
                new Domain_Layer.Entites.Authantication.Role { Id=RoleConstants.User_id, Name=RoleConstants.UserName }
            };

            await Context.Roles.AddRangeAsync(roles);
            await Context.SaveChangesAsync();
        }

        private static async Task seedAdminUserAsync(AppDbContext Context)
        {
            var users = new List<Domain_Layer.Entites.Authantication.User>
            {
                new Domain_Layer.Entites.Authantication.User
                {
                    Username = "admin",
                    Email = "Admin@Example.com",
                    PhoneNumber = "1234567890",
                    PasswordHash=BCrypt.Net.BCrypt.HashPassword("123456"), // In a real application, ensure to hash the password
                    UserAddress="AdminAddress",
                    UserRoles= new List<Domain_Layer.Entites.Authantication.UserRole>
                    {
                        new Domain_Layer.Entites.Authantication.UserRole { Roleid = RoleConstants.Admin_id, }
                    }
                },
                 new Domain_Layer.Entites.Authantication.User
                {
                    Username = "SuperAdmin",
                    Email = "SuperAdmin@Example.com",
                    PhoneNumber = "123457890",
                    PasswordHash=BCrypt.Net.BCrypt.HashPassword("1234567"), // In a real application, ensure to hash the password
                    UserAddress="Admin Address",

                    UserRoles= new List<Domain_Layer.Entites.Authantication.UserRole>
                    {
                        new Domain_Layer.Entites.Authantication.UserRole { Roleid = RoleConstants.Admin_id, },
                        new Domain_Layer.Entites.Authantication.UserRole { Roleid = RoleConstants.SuperAdmin_id, }
                    }
                }
            };

            await Context.Users.AddRangeAsync(users);

            await Context.SaveChangesAsync();
        }
    
    }
}       