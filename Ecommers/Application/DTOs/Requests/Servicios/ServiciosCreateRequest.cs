using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.Servicios
{
    public class ServiciosCreateRequest
    {

        [Required(ErrorMessage = "El nombre del servicio es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La descripción del servicio es obligatorio.")]
        [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
        public required string Description { get; set; }
        /// <summary>
        /// Imagen subida por el usuario
        /// </summary>
        public string? Image { get; set; }

        [Required(ErrorMessage = "La imagen de la categoría es obligatoria.")]
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        [Required(ErrorMessage = "El orden de visualización es obligatorio.")]
        public int? SortOrder { get; set; } = 0;

        /// <summary>
        /// Estado activo
        /// </summary>
        public bool IsActive { get; set; } = true;


    }
}
