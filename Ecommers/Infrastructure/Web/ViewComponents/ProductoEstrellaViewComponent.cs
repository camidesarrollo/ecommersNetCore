using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class ProductoEstrellaViewComponent : ViewComponent
    {
        private readonly IProducts _products;

        public ProductoEstrellaViewComponent(IProducts productoService)
        {
            _products = productoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productoEstrella = await _products.GetProductoStartQueryAsync();

            return View(productoEstrella.Data);
        }
    }
}
