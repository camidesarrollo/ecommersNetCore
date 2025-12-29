using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Ecommers.Infrastructure.Persistence.Entities
{
    public partial class AspNetUsers : IdentityUser
    {
        // NO redeclares las propiedades de IdentityUser, ya están heredadas
        // Estas líneas están causando el problema:
        // public string Id { get; set; } = null!;
        // public string? UserName { get; set; }
        // public string? NormalizedUserName { get; set; }
        // etc.

        // Solo agrega las propiedades ADICIONALES que no están en IdentityUser:
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool TermsAccepted { get; set; }
        public bool PrivacyAccepted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Orders> Orders { get; set; } = [];
    }
}