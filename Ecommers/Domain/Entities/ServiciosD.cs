using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ServiciosD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int? SortOrder { get; set; }


        public bool ? CanDelete { get; set; }
    }
}
