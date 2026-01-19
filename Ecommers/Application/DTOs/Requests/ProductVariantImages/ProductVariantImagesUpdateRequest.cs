using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesUpdateRequest : ProductVariantImagesBaseRequest
    {
        [Required(ErrorMessage = "El identificador de la imagen del la variante del producto es obligatorio.")]
        public long Id { get; set; }

    }
}
