using System.Globalization;
using AutoMapper;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Infrastructure.Files;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            // Configurar cultura en español
            var supportedCultures = new[] { new CultureInfo("es-CL"), new CultureInfo("es") };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("es-CL");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // DbContext
            services.AddDbContext<EcommersContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EcommersContext")));

            // 📌 Registrar Identity (REQUERIDO)
            services.AddIdentity<AspNetUsers, AspNetRoles>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<EcommersContext>()
            .AddDefaultTokenProviders();

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<BannersProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<ConfiguracionProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<CategoriaProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<ServicioProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<MasterAttributesProfile>());

            // Repositorios
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ValidateModelFilter>();



            // Servicios
            services.AddScoped<IFileManagerService, FileManagerService>();
            services.AddScoped<IConfiguracionService, ConfiguracionService>();
            ervices.AddScoped<IBannersService, BannersService>();
            services.AddScoped<ICategoriasService, CategoriasService>();
            services.AddScoped<IServicioService,ServicioService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageStorageService, ImageStorageService>();

            // Cache
            services.AddMemoryCache();

            return services;
        }
    }
}