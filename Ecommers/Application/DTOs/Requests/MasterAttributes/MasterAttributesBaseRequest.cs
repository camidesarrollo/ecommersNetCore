using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.MasterAttributes
{
    public class MasterAttributesBaseRequest
    {
        [Required(ErrorMessage = "El nombre de la caracteristica es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El  nombre de la caracteristica no puede superar los 255 caracteres.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "El slug de la caracteristica es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El  slug de la caracteristica no puede superar los 255 caracteres.")]
        public string Slug { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de dato caracteristica es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El tipo de dato no puede superar los 255 caracteres.")]
        public string DataType { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de dato caracteristica es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El tipo de dato no puede superar los 255 caracteres.")]
        public string InputType { get; set; } = null!;
    }
}
