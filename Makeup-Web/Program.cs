
using Application_Layer.CQRS;
using Application_Layer.CQRS.Authantication.Commads.Login;
using Autofac.Core;
using FluentValidation;
using Infastructure_Layer.Data;
using Infastructure_Layer.Data.DependencyInjection;
using Makeup_Web.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
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

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddValidatorsFromAssemblyContaining<LoginCommendValidator>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


           // builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
           //option.TokenValidationParameters = new TokenValidationParameters()
           //{
           //    ValidateIssuer = true,
           //    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
           //    ValidateAudience = true,
           //    ValidAudience = builder.Configuration["JWT:ValidAudience"],
           //    ValidateLifetime = true,
           //    ClockSkew = TimeSpan.Zero,
           //    ValidateIssuerSigningKey = true,
           //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:AuthKey"] ?? string.Empty))

           //}
           //);



            builder.Services.AddTransient<GlobalExceptionHandler>();


            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionHandler>();


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



            app.UseAuthorization();
            app.UseAuthentication();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authantication}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
