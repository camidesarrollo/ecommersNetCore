using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommers.Application.DTOs.Requests.Categorias
{
    public class CategoriaUpdateRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El slug es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El slug no puede superar los 255 caracteres.")]
        public required string Slug { get; set; }

        [Required(ErrorMessage = "La descripción completa es obligatoria.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La descripción corta es obligatoria.")]
        [MaxLength(150, ErrorMessage = "La descripción corta no puede superar los 150 caracteres.")]
        public string? ShortDescription { get; set; }

        /// <summary>
        /// Imagen subida por el usuario
        /// </summary>
        public string? Image { get; set; }

        [Required(ErrorMessage = "La imagen de la categoría es obligatoria.")]
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// Clase de fondo
        /// </summary>
        [Required(ErrorMessage = "Debe seleccionar un color de fondo.")]
        [MaxLength(255, ErrorMessage = "La clase de fondo no puede superar los 255 caracteres.")]
        public string? BgClass { get; set; }

        /// <summary>
        /// Orden de visualización
        /// </summary>
        [Required(ErrorMessage = "El orden de visualización es obligatorio.")]
        public int? SortOrder { get; set; } = 0;

        /// <summary>
        /// Estado activo
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Categoría padre
        /// </summary>
        public long? ParentId { get; set; }
    }
}


