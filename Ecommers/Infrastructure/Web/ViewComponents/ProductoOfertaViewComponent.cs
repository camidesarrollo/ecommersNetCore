using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class ProductoOfertaViewComponent : ViewComponent
    {
        private readonly IProducts _products;

        public ProductoOfertaViewComponent(IProducts productoService)
        {
            _products = productoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productoOferta = await _products.GetProductosOferta();

            return View(productoOferta.Data);
        }
    }
}
