using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Shared.Components
{
    public class SidebarViewModel
    {
        public required List<MenuItem> MenuItems { get; set; }
        public required List<Section> Sections { get; set; }

        public required ConfiguracionesD Configuracion { get; set; }
    }
}
