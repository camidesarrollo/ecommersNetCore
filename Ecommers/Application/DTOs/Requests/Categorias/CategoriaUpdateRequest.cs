using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommers.Application.DTOs.Requests.Categorias
{
    public class CategoriaUpdateRequest : CategoriaBaseRequest
    {

        [Required(ErrorMessage = "El identificador de la categoría es obligatorio.")]
        public long Id { get; set; }

    }
}


