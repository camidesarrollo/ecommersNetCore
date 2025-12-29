using Ecommers.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Web/Views/Dashboard/Index.cshtml");
        }
    }
}
