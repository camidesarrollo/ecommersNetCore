namespace Ecommers.Application.DTOs.Requests.VariantAttributes
{
    public class VariantAttributesCreateRequest
    {
        public long VariantId { get; set; }

        public long AttributeId { get; set; }

        public long? ValueId { get; set; }

        public string? CustomValue { get; set; }

        public bool IsActive { get; set; }
    }
}
