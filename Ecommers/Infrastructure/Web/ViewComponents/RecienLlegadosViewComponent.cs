using Ecommers.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class RecienLlegadosViewComponent : ViewComponent
    {
        private readonly IProducts _products;

        public RecienLlegadosViewComponent(IProducts productoService)
        {
            _products = productoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recienLlegados = await _products.GetRecienLlegadosQueryAsync();

            return View(recienLlegados.Data);
        }
    }
}
