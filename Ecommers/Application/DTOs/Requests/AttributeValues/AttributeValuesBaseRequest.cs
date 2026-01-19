using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.AttributeValues
{
    public class AttributeValuesBaseRequest
    {
        [Required(ErrorMessage = "La característica es obligatoria.")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar una característica válida.")]
        public long AttributeId { get; set; }

        [MaxLength(255, ErrorMessage = "El valor no puede superar los 255 caracteres.")]
        public string? ValueString { get; set; }

        [MaxLength(1000, ErrorMessage = "El texto no puede superar los 1000 caracteres.")]
        public string? ValueText { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El valor decimal debe ser mayor a 0.")]
        public decimal? ValueDecimal { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El valor entero no puede ser negativo.")]
        public int? ValueInt { get; set; }

        public bool? ValueBoolean { get; set; }

        public DateOnly? ValueDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El orden de visualización no puede ser negativo.")]
        public int DisplayOrder { get; set; }

        [Required(ErrorMessage = "Debe indicar si está activo.")]
        public bool IsActive { get; set; }
    }
}
