using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Application.Validator;
using Ecommers.Infrastructure.Files;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Infrastructure.Web.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Configuration
{
    public static class ApplicationSetup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<EcommersContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EcommersContext")));

            services.AddControllersWithViews()
               .AddRazorOptions(options =>
               {
                   options.ViewLocationFormats.Clear();

                   options.ViewLocationFormats.Add("/Infrastructure/Web/Views/{1}/{0}.cshtml");
                   options.ViewLocationFormats.Add("/Infrastructure/Web/Views/Shared/{0}.cshtml");
               });

            // Dependency Injection
            services.AddDependencyInjectionConfiguration(configuration);

            // MVC
            services.AddControllersWithViews();

            // FluentValidation (nueva sintaxis)
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ProductsCreateRequestValidator>();

            return services;
        }

        public static WebApplication ConfigureMiddlewares(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }
    }
}