using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsBaseRequest
    {

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El SKU no puede superar los 100 caracteres.")]
        public string SKU { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio comparativo debe ser mayor a 0.")]
        public decimal? CompareAtPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0.")]
        public decimal? CostPrice { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, long.MaxValue, ErrorMessage = "La cantidad en stock no puede ser negativa.")]
        public long StockQuantity { get; set; }

        [Required(ErrorMessage = "Debe indicar si se gestiona el stock.")]
        public bool ManageStock { get; set; }

        [Required(ErrorMessage = "El estado de stock es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El estado de stock no puede superar los 50 caracteres.")]
        public string StockStatus { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar si esta variante es la predeterminada.")]
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Debe indicar si esta variante está activa.")]
        public bool IsActive { get; set; }
    }
}
