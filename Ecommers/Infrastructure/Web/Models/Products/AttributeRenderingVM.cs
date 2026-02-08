using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    /// <summary>
    /// Modelo unificado para renderizar atributos
    /// </summary>
    public class AttributeRenderingVM
    {
        public List<MasterAttributesD> MasterAttributes { get; set; } = [];
        public List<AttributeValuesD> AttributeValues { get; set; } = [];

        /// <summary>
        /// Función para obtener los valores del producto/variante
        /// </summary>
        public Func<int, List<string>> GetProductValues { get; set; } = _ => [];

        /// <summary>
        /// Prefijo del nombre del input (ej: "ProductsAttributes" o "ProductVariants[0].Attributes")
        /// </summary>
        public string InputNamePrefix { get; set; } = string.Empty;

        /// <summary>
        /// Si es true, renderiza con estilo más compacto (sin mb-4)
        /// </summary>
        public bool CompactMode { get; set; } = false;
    }
}