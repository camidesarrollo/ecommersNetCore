using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsBaseRequest
    {
        public long ProductId { get; set; }

        [Required(ErrorMessage = "El SKU es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El SKU no puede superar los 100 caracteres.")]
        public string SKU { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        public decimal? CompareAtPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe ser mayor a 0.")]
        public decimal? CostPrice { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
        [Range(0, long.MaxValue, ErrorMessage = "La cantidad en stock no puede ser negativa.")]
        public long StockQuantity { get; set; }

        public bool ManageStock { get; set; }

        [MaxLength(50)]
        public string? StockStatus { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Debe indicar si esta variante está activa.")]
        public bool IsActive { get; set; }

        public int Id { get; set; }

    }
}
