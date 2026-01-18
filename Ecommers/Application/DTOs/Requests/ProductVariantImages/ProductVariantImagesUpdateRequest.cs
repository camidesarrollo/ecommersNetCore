using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesUpdateRequest
    {
        [Required(ErrorMessage = "El identificador de la imagen del la variante del producto es obligatorio.")]
        public long Id { get; set; }

        public long VariantId { get; set; }

        public string Url { get; set; } = null!;

        public string? AltText { get; set; }

        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
