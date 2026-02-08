using Ecommers.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class NutDecorationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            int count = 6,
            string size = "2rem",
            double opacity = 0.6,
            string[]? nutTypes = null
        )
        {
            var types = nutTypes ?? ["🥜", "🌰", "🥥"];

            return View("~/Web/Views/Shared/Components/NutDecoration/Default.cshtml", new NutDecorationViewModel
            {
                Count = count,
                Size = size,
                Opacity = opacity,
                NutTypes = types
            });
        }
    }
}
