using Ecommers.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Ecommers.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ICompositeViewEngine _viewEngine;

        protected BaseController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
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

        protected string RenderPartialViewToString(string viewPathOrName, object model)
        {
            ViewData.Model = model;

            using var writer = new StringWriter();

            var viewEngine = HttpContext.RequestServices
                .GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

            if (viewEngine == null)
                throw new InvalidOperationException("ICompositeViewEngine no está disponible.");

            ViewEngineResult viewResult;

            // 👉 Si viene una ruta (~/...), usar GetView
            if (viewPathOrName.StartsWith("~/") || viewPathOrName.EndsWith(".cshtml"))
            {
                viewResult = viewEngine.GetView(
                    executingFilePath: null,
                    viewPath: viewPathOrName,
                    isMainPage: false
                );
            }
            else
            {
                // 👉 Si es solo el nombre, usar FindView
                viewResult = viewEngine.FindView(
                    ControllerContext,
                    viewPathOrName,
                    isMainPage: false
                );
            }

            if (!viewResult.Success || viewResult.View == null)
            {
                throw new FileNotFoundException(
                    $"No se encontró la vista '{viewPathOrName}'. Buscadas: {string.Join(", ", viewResult.SearchedLocations ?? Enumerable.Empty<string>())}"
                );
            }

            var viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

            return writer.ToString();
        }


    }

}