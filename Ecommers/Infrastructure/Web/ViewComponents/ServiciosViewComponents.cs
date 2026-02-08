using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class ServiciosViewComponent(IServicio serviciosService) : ViewComponent
    {
        private readonly IServicio _serviciosService = serviciosService;

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var servicios = await _serviciosService.GetAllActiveAsync();
            return View("~/Web/Views/Shared/Components/Servicios/Default.cshtml", servicios); // Remove the path completely
        }
    }
}
