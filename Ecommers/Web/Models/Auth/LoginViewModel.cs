using System.ComponentModel.DataAnnotations;

namespace Ecommers.Web.Models.Auth
{
    /// <summary>
    /// ViewModel para el formulario de login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Por favor ingresa un correo electrónico válido")]
        [Display(Name = "Correo Electrónico")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public required string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

    }
}
