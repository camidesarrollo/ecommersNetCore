using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class CategoriasViewComponent(ICategorias categoriasService) : ViewComponent
    {
        private readonly ICategorias _categoriasService = categoriasService;

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var categorias = await _categoriasService.GetAllActiveAsync();
            return View("~/Web/Views/Shared/Components/Categorias/Default.cshtml", categorias); // Remove the path completely
        }
    }
}
