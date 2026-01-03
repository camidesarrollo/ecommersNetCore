using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class CategoriesD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        public string? BgClass { get; set; }

        public int SortOrder { get; set; }

        public long? ParentId { get; set; }

        public int? CantidadProductos { get; set; }

        public bool ? CanDelete { get; set; }

        public bool EsNuevo  { get; set; } 
    }
}
