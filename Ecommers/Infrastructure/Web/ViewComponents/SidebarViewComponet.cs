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
                    Title = "Gestión",
                    Items =
                    [
                        new() { Name = "Productos", Path = "/Gestion/Products" },
                        new MenuItem { Name = "Categorías", Path = "/Gestion/Categorias" },
                        new MenuItem { Name = "MaestroAtributos", Path = "/Gestion/MasterAttributes"},
                        new MenuItem { Name = "Inventario", Path = "/Gestion/inventario", Badge = "3" },
                        new MenuItem { Name = "Reportes", Path = "/Gestion/reportes" }
                    ]
                },
                new()
                {
                    Title = "Configuración",
                    Items =
                    [
                        new MenuItem { Name = "Configuraciones", Path = "/Configuracion/Configuraciones" },
                        new MenuItem { Name = "Banners", Path = "/Configuracion/Banners" },
                        new MenuItem { Name = "Servicios", Path = "/Configuracion/Servicios" },
                        new MenuItem { Name = "Ajustes", Path = "/Dashboard/ajustes" }
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
                    currentPath.StartsWith(item.Path, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
