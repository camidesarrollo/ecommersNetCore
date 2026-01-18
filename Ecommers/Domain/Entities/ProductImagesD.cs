using Ecommers.Domain.Common;
using Ecommers.Web.Models.Shared.TagHelpers;

namespace Ecommers.Domain.Entities
{
    public class ProductImagesD : BaseEntity<long>, IUiImage
    {
        public long ProductId { get; set; }

        public string Url { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }
        public string? AltText { get; set; }

        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }

    }
}
