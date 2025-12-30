using Ecommers.Application.DTOs.Requests;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Web.Models;
using Ecommers.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.ViewComponents
{
    public class SidebarViewComponent(IConfiguracionService configService) : ViewComponent
    {
        private readonly IConfiguracionService _configService = configService;

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
                new() { Name = "Dashboard", Path = "/dashboard" },
                new() { Name = "Productos", Path = "/dashboard/productos" },
                new() { Name = "Pedidos", Path = "/dashboard/pedidos", Badge = "5" },
                new() { Name = "Clientes", Path = "/dashboard/clientes" },
                new() { Name = "Analíticas", Path = "/dashboard/analiticas" }
            };

            var sections = new List<Section>
            {
                new()
                {
                    Title = "Gestión",
                    Items =
                    [
                        new MenuItem { Name = "Banners", Path = "/gestion/banners" },
                        new MenuItem { Name = "Categorías", Path = "/gestion/categorias" },
                        new MenuItem { Name = "Configuraciones", Path = "/gestion/configuraciones" },
                        new MenuItem { Name = "Servicios", Path = "/gestion/servicios" },
                        new MenuItem { Name = "Inventario", Path = "/dashboard/inventario", Badge = "3" },
                        new MenuItem { Name = "Reportes", Path = "/dashboard/reportes" }
                    ]
                },
                new()
                {
                    Title = "Configuración",
                    Items =
                    [
                        new MenuItem { Name = "Ajustes", Path = "/dashboard/ajustes" }
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
                "~/Web/Views/Shared/Components/Sidebar/Default.cshtml",
                model
            );
        }

        private static void MarcarActivo(IEnumerable<MenuItem> items, string currentPath)
        {
            foreach (var item in items)
            {
                item.IsActive =
                    !string.IsNullOrWhiteSpace(item.Path) &&
                    currentPath.StartsWith(item.Path.ToLower());
            }
        }
    }
}
