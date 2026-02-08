using Ecommers.Application.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class ConfiguracionViewComponent(IConfiguracion configService) : ViewComponent
    {
        private readonly IConfiguracion _configService = configService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion = await _configService.GetCachedAsync(configuracionCacheRequest);
            return View("~/Web/Views/Shared/Components/Configuracion/Default.cshtml", configuracion); // Remove the path completely
        }
    }
}
