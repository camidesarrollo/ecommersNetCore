using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ConfiguracionesD : BaseEntity<int>
    {
        public string NombreAplicacion { get; set; } = null!;

        public string? NombreEmpresa { get; set; }

        public string? AbreviacionEmpresa { get; set; }

        public string? Slogan { get; set; }

        public string? Descripcion { get; set; }

        public string? EmailContacto { get; set; }

        public string? TelefonoContacto { get; set; }

        public string? Whatsapp { get; set; }

        public string? Direccion { get; set; }

        public string? Ciudad { get; set; }

        public string? Pais { get; set; }

        public IFormFile? LogoFile { get; set; }

        public IFormFile? FaviconFile { get; set; }      // 👈 NUEVO
        public IFormFile? BannerFile { get; set; }       // 👈 NUEVO

        public string? Logo { get; set; }

        public string? Favicon { get; set; }

        public string? BannerPrincipal { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        public string? Twitter { get; set; }

        public string? Linkedin { get; set; }

        public string? Tiktok { get; set; }

        public string? Youtube { get; set; }

        public string Moneda { get; set; } = null!;

        public string SimboloMoneda { get; set; } = null!;

        public string Idioma { get; set; } = null!;

        public bool ActivarCarrito { get; set; }

        public bool ActivarPagosOnline { get; set; }

        public string? MetaTitulo { get; set; }

        public string? MetaDescripcion { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaAutor { get; set; }

        public string? MetaRobots { get; set; }

        public string? MetaCanonical { get; set; }

        public string? MetaLanguage { get; set; }

        public string? MetaRevisitAfter { get; set; }

        public bool MetaNoindex { get; set; }

        public bool MetaNofollow { get; set; }

        public string? OgTitulo { get; set; }

        public string? OgDescripcion { get; set; }

        public string? OgImagen { get; set; }

        public string? OgUrl { get; set; }

        public string? OgTipo { get; set; }

        public string? OgSitename { get; set; }

        public string? TwitterCard { get; set; }

        public string? TwitterTitulo { get; set; }

        public string? TwitterDescripcion { get; set; }

        public string? TwitterImagen { get; set; }

        public string? TwitterSite { get; set; }

        public string? TwitterCreator { get; set; }

        public string? SchemaTipo { get; set; }

        public string? SchemaDatos { get; set; }

        public bool IncluirEnSitemap { get; set; }

        public DateTime? UltimaModificacion { get; set; }

        public int PrioridadSitemap { get; set; }

        public string? Slug { get; set; }

        public string? MetaImagenPredeterminada { get; set; }

        public string? ColorTemaNavegador { get; set; }

        public string? MetaViewport { get; set; }

        public string? MetaCharset { get; set; }
    }
}
