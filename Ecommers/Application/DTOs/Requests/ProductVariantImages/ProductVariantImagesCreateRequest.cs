namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesCreateRequest
    {
        public long VariantId { get; set; }

        public string Url { get; set; } = null!;

        public string? AltText { get; set; }

        public bool IsPrimary { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
