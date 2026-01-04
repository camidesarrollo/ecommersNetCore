using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductImagesD : BaseEntity<long>
    {
        public long ProductId { get; set; }

        public string Url { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public int Order { get; set; }
    }
}
