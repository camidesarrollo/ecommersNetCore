using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariants;

namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsCreateRequest
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;


        [Required(ErrorMessage = "La descripción completa es obligatoria.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "La descripción corta es obligatoria.")]
        [MaxLength(150, ErrorMessage = "La descripción corta no puede superar los 150 caracteres.")]
        public required string ShortDescription { get; set; }

        public bool IsActive { get; set; }

        public long CategoryId { get; set; }

        public ICollection<ProductVariantsCreateRequest> ProductVariants { get; set; } = [];
    }
}
