using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.ViewComponents
{
    public class ServiciosViewComponent(IServicioService serviciosService) : ViewComponent
    {
        private readonly IServicioService _serviciosService = serviciosService;

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var servicios = await _serviciosService.GetAllActiveAsync();
            return View("~/Web/Views/Shared/Components/Servicios/Default.cshtml", servicios); // Remove the path completely
        }
    }
}
