using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.VariantAttributes
{
    public class VariantAttributesBaseRequest
    {
        [Required(ErrorMessage = "La variante es obligatoria.")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar una variante válida.")]
        public long VariantId { get; set; }

        [Required(ErrorMessage = "La característica es obligatoria.")]
        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar una característica válida.")]
        public long AttributeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Debe seleccionar un valor válido.")]
        public long? ValueId { get; set; }

        [MaxLength(500, ErrorMessage = "El valor personalizado no puede superar los 500 caracteres.")]
        public string? CustomValue { get; set; }

        [Required(ErrorMessage = "Debe indicar si esta característica está activa.")]
        public bool IsActive { get; set; }
    }
}
