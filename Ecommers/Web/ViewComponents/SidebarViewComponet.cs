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
            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion = await _configService.GetCachedAsync(configuracionCacheRequest);

            // Menú principal
            var menuItems = new List<MenuItem>
            {
                new() { Name = "Dashboard", Path = "/Dashboard/Index", Badge = null },
                new() { Name = "Productos", Path = "/Dashboard/productos", Badge = null },
                new() { Name = "Pedidos", Path = "/Dashboard/pedidos", Badge = "5" },
                new() { Name = "Clientes", Path = "/Dashboard/clientes", Badge = null },
                new() { Name = "Analíticas", Path = "/Dashboard/analiticas", Badge = null }
            };

            // Secciones con subcategorías
            var sections = new List<Section>
            {
                new() {
                    Title = "Gestión",
                    Items =
                    [
                        new MenuItem { Name = "Banners", Path = "/Gestion/Banners", Badge = null },
                        new MenuItem { Name = "Categorías", Path = "/Gestion/Categorias", Badge = null },
                        new MenuItem { Name = "Configuraciones", Path = "/Gestion/Configuraciones", Badge = null },
                        new MenuItem { Name = "Servicios", Path = "/Gestion/Servicios", Badge = null },
                        new MenuItem { Name = "Inventario", Path = "/Dashboard/inventario", Badge = "3" },
                        new MenuItem { Name = "Reportes", Path = "/Dashboard/reportes", Badge = null }
                    ]
                },
                new() {
                    Title = "Configuración",
                    Items =
                    [
                        new MenuItem { Name = "Ajustes", Path = "/Dashboard/ajustes", Badge = null }
                    ]
                }
            };

            // Pasar datos a la vista
            var model = new SidebarViewModel
            {
                MenuItems = menuItems,
                Sections = sections,
                Configuracion = configuracion ?? new Domain.Entities.ConfiguracionesD { Id = 0 }
            };

            return View("~/Web/Views/Shared/Components/Sidebar/Default.cshtml", model);
        }
    }
}
