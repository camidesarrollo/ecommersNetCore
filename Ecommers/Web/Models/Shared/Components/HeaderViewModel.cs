using Ecommers.Application.DTOs.Requests;
using Ecommers.Domain.Entities;
using Ecommers.Web.ViewComponents;

namespace Ecommers.Web.Models.Shared.Components
{
    public class HeaderViewModel
    {
        public required ConfiguracionesD ConfiguracionesD { get; set; }

        // Usuario logueado, puede ser null
        public LoggedUser? LoggedUser { get; set; }
    }
}
