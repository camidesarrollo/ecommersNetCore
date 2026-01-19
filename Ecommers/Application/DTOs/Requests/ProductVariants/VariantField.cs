namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class VariantField
    {
        public int VariantIndex { get; set; }
        public string Property { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
