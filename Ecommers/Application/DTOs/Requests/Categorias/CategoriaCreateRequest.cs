using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.Categorias
{
    public class CategoriaCreateRequest
    {

        [Required(ErrorMessage = "Debe ingresar el nombre de la categoría.")]
        [MaxLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        [Display(Name = "Nombre de la Categoría")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar el slug de la categoría.")]
        [MaxLength(255, ErrorMessage = "El slug no puede superar los 255 caracteres.")]
        [Display(Name = "Slug URL")]
        public string Slug { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar una descripción corta.")]
        [MaxLength(150, ErrorMessage = "La descripción corta no puede superar los 150 caracteres.")]
        [Display(Name = "Descripción Corta")]
        public string ShortDescription { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar la descripción completa de la categoría.")]
        [Display(Name = "Descripción Completa")]
        public string Description { get; set; } = null!;

        [Display(Name = "Imagen Actual")]
        public string? Image { get; set; }

        [Display(Name = "Imagen de la Categoría")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un color o gradiente de fondo.")]
        [MaxLength(255, ErrorMessage = "La clase de fondo no puede superar los 255 caracteres.")]
        [Display(Name = "Color de Fondo")]
        public string? BgClass { get; set; }

        [Required(ErrorMessage = "Debe indicar el orden de visualización.")]
        [Range(0, 9999, ErrorMessage = "El orden debe ser un número mayor o igual a 0.")]
        [Display(Name = "Orden de Visualización")]
        public int SortOrder { get; set; }

        [Display(Name = "Categoría Activa")]
        public bool IsActive { get; set; }

        [Display(Name = "Categoría Padre")]
        public long? ParentId { get; set; }
    }
}
