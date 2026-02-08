using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesUploadImageRequest
    {
        public long VariantId { get; set; }
        public required string Slug { get; set; }
        public List<ProductVariantImagesCreateRequest> Imagenes { get; set; } = new();
    }

}