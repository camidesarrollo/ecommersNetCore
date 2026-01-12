using Ecommers.Domain.Common;
using Ecommers.Web.Models.Shared.TagHelpers;

namespace Ecommers.Domain.Entities
{
    public class ProductVariantImagesD : BaseEntity<long>, IUiImage
    {
        public long VariantId { get; set; }

        public string Url { get; set; } = null!;

        public string? AltText { get; set; }

        public bool IsPrimary { get; set; }

        public int Order { get; set; }

    }
}
