
using Infastructure_Layer.Data;
using Infastructure_Layer.Data.DependencyInjection;
using Makeup_Web.Middlewares;
using Microsoft.EntityFrameworkCore;
namespace Makeup_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<TransactionMiddleWare>();


            var app = builder.Build();

            app.UseStaticFiles();

            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var _dbcontext = service.GetRequiredService<AppDbContext>();
            try
            {
                _dbcontext.Database.Migrate();

            }
            catch (Exception ex)
            {

                var logger = service.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An Error Occurred During Apply the Migration");
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<TransactionMiddleWare>();


            app.UseAuthorization();
            app.UseAuthentication();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
