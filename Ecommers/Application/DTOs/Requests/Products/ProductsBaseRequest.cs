using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsBaseRequest
    {
        [Required(ErrorMessage = "Debes ingresar el nombre del producto.")]
        [MaxLength(255, ErrorMessage = "El nombre del producto no puede superar los 255 caracteres.")]
        [Display(Name = "Nombre del producto")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar el slug del producto.")]
        [MaxLength(255, ErrorMessage = "El slug no puede superar los 255 caracteres.")]
        [Display(Name = "Slug URL")]
        public string Slug { get; set; } = null!;

        [Required(ErrorMessage = "Debes ingresar la descripción del producto.")]
        [Display(Name = "Descripción")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Debes ingresar una descripción corta del producto.")]
        [MaxLength(150, ErrorMessage = "La descripción corta no puede superar los 150 caracteres.")]
        [Display(Name = "Descripción corta")]
        public required string ShortDescription { get; set; }

        [Display(Name = "Producto activo")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una categoría para el producto.")]
        [Display(Name = "Categoría")]
        public long CategoryId { get; set; }


        [Required(ErrorMessage = "Debes singresar el precio base del producto.")]
        [Display(Name = "BasePrice")]
        public decimal? BasePrice { get; set; }
    }
}
