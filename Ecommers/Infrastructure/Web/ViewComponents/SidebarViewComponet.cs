using Ecommers.Application.DTOs.Requests;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Web.Models;
using Ecommers.Infrastructure.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class SidebarViewComponent(IConfiguracion configService) : ViewComponent
    {
        private readonly IConfiguracion _configService = configService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentPath =
                ViewContext.HttpContext.Request.Path.Value?.ToLower() ?? "";

            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion =
                await _configService.GetCachedAsync(configuracionCacheRequest);

            var menuItems = new List<MenuItem>
            {
                new() { Name = "Dashboard", Path = "/Dashboard" },
                new() { Name = "Pedidos", Path = "/Dashboard/pedidos", Badge = "5" },
                new() { Name = "Clientes", Path = "/Dashboard/clientes" },
                new() { Name = "Analíticas", Path = "/Dashboard/analiticas" }
            };

            var sections = new List<Section>
            {
                new()
                {
                    Title = "Catálogo",
                    Items =
                    [
                        new() { Name = "Productos", Path = "/Gestion/Products" },
                        new() { Name = "Categorías", Path = "/Gestion/Categorias" },
                        new() { Name = "Maestro de atributos", Path = "/Gestion/MasterAttributes" },
                        new() { Name = "Inventario", Path = "/Gestion/inventario", Badge = "3" }
                    ]
                },
                new()
                {
                    Title = "Operación",
                    Items =
                    [
                        new() { Name = "Reportes", Path = "/Gestion/reportes" }
                    ]
                },
                new()
                {
                    Title = "Configuración",
                    Items =
                    [
                        new() { Name = "Configuraciones", Path = "/Configuracion/Configuraciones" },
                        new() { Name = "Banners", Path = "/Configuracion/Banners" },
                        new() { Name = "Servicios", Path = "/Configuracion/Servicios" },
                        new() { Name = "Ajustes", Path = "/Dashboard/ajustes" }
                    ]
                }
            };


            // Marcar activos
            MarcarActivo(menuItems, currentPath);

            foreach (var section in sections)
            {
                MarcarActivo(section.Items, currentPath);
                section.IsOpen = section.Items.Any(i => i.IsActive);
            }

            var model = new SidebarViewModel
            {
                MenuItems = menuItems,
                Sections = sections,
                Configuracion = configuracion ?? new Domain.Entities.ConfiguracionesD { Id = 0 }
            };

            return View(
                model
            );
        }

        private static void MarcarActivo(IEnumerable<MenuItem> items, string currentPath)
        {
            foreach (var item in items)
            {
                item.IsActive =
                    !string.IsNullOrWhiteSpace(item.Path) &&
                    currentPath.StartsWith(item.Path, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
