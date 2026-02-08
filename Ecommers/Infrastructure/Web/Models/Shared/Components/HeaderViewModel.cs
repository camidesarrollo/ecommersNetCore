using Ecommers.Application.DTOs.Requests;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Web.ViewComponents;

namespace Ecommers.Infrastructure.Web.Models.Shared.Components
{
    public class HeaderViewModel
    {
        public required ConfiguracionesD ConfiguracionesD { get; set; }

        // Usuario logueado, puede ser null
        public LoggedUser? LoggedUser { get; set; }
    }
}
