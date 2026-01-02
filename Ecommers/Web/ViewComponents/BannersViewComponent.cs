using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.ViewComponents
{
    public class BannersViewComponent(IBanners bannersService) : ViewComponent
    {
        private readonly IBanners _bannersService = bannersService;

        public async Task<IViewComponentResult> InvokeAsync(string seccion, bool activo)
        {

            var banners = await _bannersService.GetBySeccionAsync(seccion, activo);
            return View("~/Web/Views/Shared/Components/Banners/Default.cshtml", banners); // Remove the path completely
        }
    }
}
