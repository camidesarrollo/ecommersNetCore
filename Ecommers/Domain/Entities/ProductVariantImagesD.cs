using Ecommers.Domain.Common;
using Ecommers.Infrastructure.Web.Models.Shared.Interfaces;

namespace Ecommers.Domain.Entities
{
    public class ProductVariantImagesD : BaseEntity<long>, IUiImage
    {
        public long VariantId { get; set; }

        public string Url { get; set; } = null!;

        public string? AltText { get; set; }

        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }

    }
}
