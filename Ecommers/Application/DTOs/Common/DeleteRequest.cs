using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Common
{
    public class DeleteRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int Id { get; set; }
    }
}
