using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsStateRequest
    {
        [Required]
        public long VarianteId { get; set; }
    }
}
