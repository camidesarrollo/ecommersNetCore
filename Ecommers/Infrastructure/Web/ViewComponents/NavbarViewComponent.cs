using Ecommers.Application.DTOs.Requests;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Mvc;
namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class NavbarViewComponent(IConfiguracion configService) : ViewComponent
    {
        private readonly IConfiguracion _configService = configService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion = await _configService.GetCachedAsync(configuracionCacheRequest);

            ViewData["RequiresHeaderCss"] = true;
            ViewData["RequiresHeaderJs"] = true;

            var Navegacion = new NavViewModel
            {
                Items =
                [
                    new NavItem { Name = "Inicio", Path = "/", Icon = "fas fa-home" },

                    new NavItem
                    {
                        Name = "Productos",
                        Icon = "fas fa-box-open",
                        Children =
                        [
                            new() { Name = "Ver Todos", Path = "/productos", Icon = "fas fa-th-large" }
                        ]
                    },

                    new NavItem { Name = "Nosotros", Path = "/about", Icon = "fas fa-users" },
                    new NavItem { Name = "Contacto", Path = "/contacto", Icon = "fas fa-envelope" },
                    new NavItem { Name = "Términos y Condiciones", Path = "/terminos", Icon = "fas fa-file-alt" }
                ],

                Configuracion = configuracion ?? new Domain.Entities.ConfiguracionesD
                {
                    Id = 0, // o algún valor temporal/por defecto
                }

            };
            return View(Navegacion); // Remove the path completely
        }
    }
}