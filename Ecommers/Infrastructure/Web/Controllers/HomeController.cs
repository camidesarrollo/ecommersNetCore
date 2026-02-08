using System.Diagnostics;
using Ecommers.Infrastructure.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.Controllers
{
    public class HomeController : Controller
    {
 
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View(); // Si tienes Views/Home/Privacy.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
