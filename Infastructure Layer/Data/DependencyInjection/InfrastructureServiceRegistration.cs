using Application_Layer.CQRS.Authantication.Commads.Login;
using Application_Layer.Services;
using Application_Layer.Services.MailService;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure_Layer.Data.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(
                  config.GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddScoped<IunitofWork, UnitofWork>();
            services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher,PasswordHasher>();
            services.AddScoped<IAttachmentService, AttachmnetService>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            services.Configure<MailSettings>(config.GetSection("MailSettings"));


            services.AddScoped<IEMailService, EMailService>();


            return services;


        }
    }
}
