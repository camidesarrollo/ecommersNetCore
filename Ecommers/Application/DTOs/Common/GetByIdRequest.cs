using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Common
{
    public class GetByIdRequest<TId>
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public required TId Id { get; set; }
    }
}
