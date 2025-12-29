using Ecommers.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Maneja Result sin datos, redirige a una acción
        /// </summary>
        protected IActionResult HandleResult(Result result, string redirect)
        {
            TempData["mensaje"] = result.Message;
            TempData["estado"] = result.Success ? "Exito" : "Error";
            return RedirectToAction(redirect);
        }

        /// <summary>
        /// Maneja Result con datos, muestra vista en caso de error o redirige en éxito
        /// </summary>
        protected IActionResult HandleResult<T>(
            Result<T> result,
            string viewPath,
            string? redirectAction = null)
        {
            if (!result.Success)
            {
                TempData["mensaje"] = result.Message;
                TempData["estado"] = "Error";
                return View(viewPath, result.Data);
            }

            TempData["mensaje"] = result.Message;
            TempData["estado"] = "Exito";
            return RedirectToAction(redirectAction ?? nameof(Index));
        }

        /// <summary>
        /// Maneja Result con datos, siempre muestra la vista (para GET)
        /// </summary>
        protected IActionResult HandleResultView<T>(
            Result<T> result,
            string viewPath)
        {
            if (!result.Success)
            {
                TempData["mensaje"] = result.Message;
                TempData["estado"] = "Error";
                return RedirectToAction(nameof(Index));
            }

            return View(viewPath, result.Data);
        }
    }
}