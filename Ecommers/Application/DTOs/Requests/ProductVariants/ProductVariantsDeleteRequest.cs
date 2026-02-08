using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsDeleteRequest
    {
        [Required]
        public long VarianteId { get; set; }
    }
}