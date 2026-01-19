using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductAttributes
{
    public class ProductAttributesBaseRequest
    {
        [Required(ErrorMessage = "El id del producto es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "El id del producto debe ser mayor a 0.")]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "El id del atributo es obligatorio.")]
        [Range(1, long.MaxValue, ErrorMessage = "El id del atributo debe ser mayor a 0.")]
        public long AttributeId { get; set; }

        // ValueId puede ser null, pero si existe debe ser > 0
        [Range(1, long.MaxValue, ErrorMessage = "El id del valor debe ser mayor a 0.")]
        public long? ValueId { get; set; }

        // CustomValue puede ser null, pero si existe no debe estar vacío
        [MaxLength(500, ErrorMessage = "El valor personalizado no puede superar los 500 caracteres.")]
        public string? CustomValue { get; set; }

        [Required(ErrorMessage = "El estado IsActive es obligatorio.")]
        public bool IsActive { get; set; }
    }
}
