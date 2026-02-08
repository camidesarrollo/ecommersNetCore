namespace Ecommers.Infrastructure.Web.Filters
{
    /// <summary>
    /// Atributo para marcar acciones que deben omitir la validación del modelo
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipModelValidationAttribute : Attribute
    {
    }
}
