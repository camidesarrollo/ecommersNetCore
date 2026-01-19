using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommers.Application.DTOs.Requests.Configuracion
{
    public class ConfiguracionUpdateRequest : ConfiguracionBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int Id { get; set; }

    }
}
