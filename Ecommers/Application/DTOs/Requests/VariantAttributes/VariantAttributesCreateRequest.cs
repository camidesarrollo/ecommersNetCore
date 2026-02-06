namespace Ecommers.Application.DTOs.Requests.VariantAttributes
{
    public class VariantAttributesCreateRequest : VariantAttributesBaseRequest
    {
        
    }

    public class ProductVariantAttributeVM
    {
        public int MasterAttributeId { get; set; }

        public string? Value { get; set; }
    }
}
