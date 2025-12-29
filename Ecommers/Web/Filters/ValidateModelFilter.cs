using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

                context.Result = new BadRequestObjectResult(new
                {
                    Success = false,
                    Message = string.Join("<br>", errores)
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se requiere lógica post-ejecución
        }
    }
}