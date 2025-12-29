using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Infrastructure.Files;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Web.Filters;
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

            // Pass the required 'configuration' parameter to the method
            services.AddDependencyInjectionConfiguration(configuration);

            // MVC
            services.AddControllersWithViews();

            return services;
        }

        // Recibe WebApplication para poder usar MapControllerRoute directamente
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

            // Ahora sí funciona porque 'app' es WebApplication
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }
    }
}
