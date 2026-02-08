using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Shared.Components
{
    public class NavViewModel
    {
        public List<NavItem> Items { get; set; } = [];
        public required ConfiguracionesD Configuracion { get; set; }
    }

    public class NavItem
    {
        public required string Name { get; set; }      // Ej: "Inicio"
        public string? Path { get; set; }      // Ej: "/about"
        public string? Icon { get; set; }      // Ej: "fas fa-home"
        public List<NavItem>? Children { get; set; } // Submenús (Ej: categorías)
    }
}
