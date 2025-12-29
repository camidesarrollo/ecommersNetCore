using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.Banners
{
    public class BannersCreateRequest
    {
        [Required(ErrorMessage = "La sección es obligatoria.")]
        [MaxLength(255, ErrorMessage = "La sección no puede superar los 255 caracteres.")]
        public required string Seccion { get; set; }

        [Required(ErrorMessage = "El texto alternativo (Alt) es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El texto alternativo no puede superar los 255 caracteres.")]
        public required string AltText { get; set; }

        [Required(ErrorMessage = "El subtítulo es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El subtítulo no puede superar los 255 caracteres.")]
        public required string Subtitulo { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El título no puede superar los 255 caracteres.")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "El texto del botón es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El texto del botón no puede superar los 255 caracteres.")]
        public required string BotonTexto { get; set; }

        [Required(ErrorMessage = "El enlace del botón es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El enlace del botón no puede superar los 255 caracteres.")]
        public required string BotonEnlace { get; set; }

        public string? Image { get; set; }
        
        [Required(ErrorMessage = "La imagen del banner es obligatoria.")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "El orden de visualización es obligatorio.")]
        public int? SortOrder { get; set; } = 0;

    }
}
