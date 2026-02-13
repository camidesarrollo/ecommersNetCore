using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductAttributes
{
    public class ProductAttributesUpdateRequest : ProductAttributesBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }
    }

}
