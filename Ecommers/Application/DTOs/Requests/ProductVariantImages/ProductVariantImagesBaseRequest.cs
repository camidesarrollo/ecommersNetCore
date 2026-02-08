using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesBaseRequest
    {
        public long VariantId { get; set; }

        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede superar los 500 caracteres.")]
        [Url(ErrorMessage = "Debes ingresar una URL válida.")]
        [Display(Name = "URL de la imagen")]
        public string? Url { get; set; } = null!;

        [Display(Name = "Imagen de la Categoría")]
        public IFormFile? ImageFile { get; set; }

        [MaxLength(255, ErrorMessage = "El texto alternativo no puede superar los 255 caracteres.")]
        [Display(Name = "Texto alternativo")]
        public string? AltText { get; set; }

        [Display(Name = "Imagen principal")]
        public bool IsPrimary { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden debe ser un número mayor o igual a 0.")]
        [Display(Name = "Orden de visualización")]
        public int SortOrder { get; set; }

        [Display(Name = "Imagen activa")]
        public bool IsActive { get; set; }
    }
}
