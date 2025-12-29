using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class BannersD : BaseEntity<long>
    {
        public string Seccion { get; set; } = null!;

        public string Imagen { get; set; } = null!;

        public string AltText { get; set; } = null!;

        public string Subtitulo { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string BotonTexto { get; set; } = null!;

        public string BotonEnlace { get; set; } = null!;

        public bool? CanDelete { get; set; }
    }
}
