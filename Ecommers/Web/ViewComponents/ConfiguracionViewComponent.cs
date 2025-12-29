using Ecommers.Application.Interfaces;
using Ecommers.Application.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Ecommers.Application.DTOs.Requests.Configuracion;

namespace Ecommers.Web.ViewComponents
{
    public class FooterViewComponent(IConfiguracionService configService) : ViewComponent
    {
        private readonly IConfiguracionService _configService = configService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion = await _configService.GetCachedAsync(configuracionCacheRequest);
            return View("~/Web/Views/Shared/Components/Footer/Default.cshtml", configuracion); // Remove the path completely
        }
    }
}
