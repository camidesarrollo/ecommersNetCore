using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class UnirceComunidad : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View(); // Remove the path completely
        }
    }
}
