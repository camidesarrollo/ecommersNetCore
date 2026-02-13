using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductAttributes;

namespace Ecommers.Application.DTOs.Requests.VariantAttributes
{
    public class VariantAttributesUpdateRequest : VariantAttributesBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }
    }
}
