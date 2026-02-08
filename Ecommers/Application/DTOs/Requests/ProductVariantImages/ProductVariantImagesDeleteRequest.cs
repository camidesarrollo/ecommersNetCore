using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesDeleteRequest
    {
        [Required]
        public long VarianteId { get; set; }
    }

}