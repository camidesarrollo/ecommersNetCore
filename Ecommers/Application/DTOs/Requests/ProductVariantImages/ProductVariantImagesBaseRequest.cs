using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesBaseRequest
    {
        [Required(ErrorMessage = "El id de la variante  es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "El id de la variante  debe ser mayor a 0.")]
        public long VariantId { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La URL no puede superar los 500 caracteres.")]
        public string Url { get; set; } = null!;

        [MaxLength(255, ErrorMessage = "El texto alternativo no puede superar los 255 caracteres.")]
        public string? AltText { get; set; }

        [Required(ErrorMessage = "El campo principal es obligatorio.")]
        public bool IsPrimary { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser 0 o mayor.")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "El campo activo es obligatorio.")]
        public bool IsActive { get; set; }
    }
}
