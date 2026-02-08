using Ecommers.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
