using System.ComponentModel.DataAnnotations;
using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductVariantViewModel
    {
        public int Index { get; set; }

        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre de la variante es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El SKU es requerido")]
        [StringLength(100, ErrorMessage = "El SKU no puede exceder los 100 caracteres")]
        public string SKU { get; set; }

        [Required(ErrorMessage = "El precio de costo es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio de costo debe ser mayor o igual a 0")]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio de comparación debe ser mayor o igual a 0")]
        public decimal? CompareAtPrice { get; set; }

        [Required(ErrorMessage = "La cantidad en stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int StockQuantity { get; set; }

        public string StockStatus { get; set; } = "in_stock";

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ProductVariantImageViewModel> Images { get; set; } = new List<ProductVariantImageViewModel>();

        public List<ProductVariantAttributeViewModel> VariantAttributes { get; set; } = new List<ProductVariantAttributeViewModel>();
    }

    public class ProductVariantImageViewModel
    {
        public int? Id { get; set; }

        public IFormFile File { get; set; }

        public string Url { get; set; }

        public string AltText { get; set; }

        public int Order { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ProductVariantAttributeViewModel
    {
        public int AttributeId { get; set; }

        public string AttributeName { get; set; }

        public int? ValueId { get; set; }

        public string Value { get; set; }
    }
}
