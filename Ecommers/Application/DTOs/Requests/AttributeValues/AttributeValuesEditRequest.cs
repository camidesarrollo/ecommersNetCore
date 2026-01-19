using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.AttributeValues
{
    public class AttributeValuesEditRequest : AttributeValuesBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }
    }
}
