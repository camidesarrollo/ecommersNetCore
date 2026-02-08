using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Web.Models;

namespace Ecommers.Infrastructure.Web.Models.Shared.Components
{
    public class SidebarViewModel
    {
        public required List<MenuItem> MenuItems { get; set; }
        public required List<Section> Sections { get; set; }

        public required ConfiguracionesD Configuracion { get; set; }
    }
}
