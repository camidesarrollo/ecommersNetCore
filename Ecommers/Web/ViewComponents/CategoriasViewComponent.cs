using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.ViewComponents
{
    public class CategoriasViewComponent(ICategoriasService categoriasService) : ViewComponent
    {
        private readonly ICategoriasService _categoriasService = categoriasService;

        public async Task<IViewComponentResult> InvokeAsync(string seccion, bool activo)
        {

            var categorias = await _categoriasService.GetAllActiveAsync();
            return View("~/Web/Views/Shared/Components/Categorias/Default.cshtml", categorias); // Remove the path completely
        }
    }
}
