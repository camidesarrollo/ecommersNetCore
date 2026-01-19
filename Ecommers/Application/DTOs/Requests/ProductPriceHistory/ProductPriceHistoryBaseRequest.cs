using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductPriceHistory
{
    public class ProductPriceHistoryBaseRequest
    {
        [Required(ErrorMessage = "El VariantId es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "El VariantId debe ser mayor a 0.")]
        public long VariantId { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Price { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El CompareAtPrice debe ser mayor a 0.")]
        public decimal? CompareAtPrice { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(500, ErrorMessage = "La razón no puede superar los 500 caracteres.")]
        public string? Reason { get; set; }

        [Required(ErrorMessage = "El estado IsActive es obligatorio.")]
        public bool IsActive { get; set; }
    }
}
