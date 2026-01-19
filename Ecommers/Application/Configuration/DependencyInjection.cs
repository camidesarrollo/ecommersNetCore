using System.Globalization;
using AutoMapper;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Domain.Entities;
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
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BannersProfile>();
                cfg.AddProfile<ConfiguracionProfile>();
                cfg.AddProfile<CategoriaProfile>();
                cfg.AddProfile<ServicioProfile>();
                cfg.AddProfile<MasterAttributesProfile>();
                cfg.AddProfile<ProductAttributesProfile>();
                cfg.AddProfile<ProductImagesProfile>();
                cfg.AddProfile<ProductPriceHistoryProfile>();
                cfg.AddProfile<ProductsProfile>();
                cfg.AddProfile<ProductVariantImagesProfile>();
                cfg.AddProfile<ProductVariantsProfile>();
                cfg.AddProfile<VariantAttributesProfile>();
                cfg.AddProfile<AttributeValuesProfile>();
            });
            // Repositorios
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ValidateModelFilter>();



            // Servicios
            services.AddScoped<IFileManager, FileManagerService>();
            services.AddScoped<IConfiguracion, ConfiguracionService>();
            services.AddScoped<IBanners, BannersService>();
            services.AddScoped<ICategorias, CategoriasService>();
            services.AddScoped<IServicio, ServicioService>();
            services.AddScoped<IMasterAttributes,MasterAttributesService>();
            services.AddScoped<IProducts, ProductsService>();
            services.AddScoped<IProductAttributes, ProductAttributesService>();
            services.AddScoped<IProductImages, ProductImagesService>();
            services.AddScoped<IProductPriceHistory, ProductPriceHistoryService>();
            services.AddScoped<IProductVariantImages, ProductVariantImagesService>();
            services.AddScoped<IProductVariants, ProductVariantsService>();
            services.AddScoped<IVariantAttributes, VariantAttributesService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageStorage, ImageStorageService>();
            services.AddScoped<IAttributeValues, AttributeValuesService>();

        
            // Cache
            services.AddMemoryCache();

            return services;
        }
    }
}