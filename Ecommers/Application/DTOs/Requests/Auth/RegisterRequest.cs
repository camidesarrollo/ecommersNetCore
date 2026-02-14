using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool TermsAccepted { get; set; }
    }
}
