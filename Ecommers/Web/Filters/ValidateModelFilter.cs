using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Ecommers.Web.Filters
{
    /// <summary>
    /// Filtro para validar automáticamente el ModelState en las acciones del controlador.
    /// Se puede omitir la validación usando el atributo [SkipModelValidation].
    /// </summary>
    public class ValidateModelFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar si la acción tiene el atributo SkipModelValidation
            var skipValidation = context.ActionDescriptor.EndpointMetadata
                .OfType<SkipModelValidationAttribute>()
                .Any();

            // Si tiene el atributo, omitir la validación
            if (skipValidation)
            {
                return;
            }

            // Validar el modelo solo si no está marcado para omitir
            if (!context.ModelState.IsValid)
            {
                var errores = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Determinar si es una petición AJAX o normal
                var isAjaxRequest = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjaxRequest)
                {
                    // Para peticiones AJAX, retornar JSON
                    context.Result = new BadRequestObjectResult(new
                    {
                        Success = false,
                        Message = string.Join(" ", errores)
                    });
                }
                else
                {
                    // Para peticiones normales de formulario, retornar a la vista
                    var controller = context.Controller as Controller;
                    if (controller != null)
                    {
                        // Agregar los errores a TempData para mostrarlos en la vista
                        controller.TempData["ErrorMessage"] = string.Join(" ", errores);

                        // Obtener el modelo del contexto de acción
                        var model = context.ActionArguments.Values.FirstOrDefault();

                        // Determinar la vista a retornar dinámicamente
                        var controllerName = context.ActionDescriptor.RouteValues["controller"];
                        var actionName = context.ActionDescriptor.RouteValues["action"];

                        string? viewName = actionName switch
                        {
                            "Crear" => $"~/Web/Views/{controllerName}/Create.cshtml",
                            "Editar" => $"~/Web/Views/{controllerName}/Edit.cshtml",
                            "Eliminar" => $"~/Web/Views/{controllerName}/Delete.cshtml",
                            _ => null
                        };

                        if (viewName != null && model != null)
                        {
                            context.Result = new ViewResult
                            {
                                ViewName = viewName,
                                ViewData = new ViewDataDictionary(
                                    controller.ViewData)
                                {
                                    Model = model
                                }
                            };
                        }
                        else
                        {
                            // Fallback: retornar BadRequest si no se puede determinar la vista
                            context.Result = new BadRequestObjectResult(new
                            {
                                Success = false,
                                Message = string.Join(" ", errores)
                            });
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se requiere lógica post-ejecución
        }
    }
}