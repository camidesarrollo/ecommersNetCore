namespace Ecommers.Application.DTOs.Requests.ProductAttributes
{
    public class ProductAttributesCreateRequest
    {
         public long ProductId { get; set; }

        public long AttributeId { get; set; }

        public long? ValueId { get; set; }

        public string? CustomValue { get; set; }

        public bool IsActive { get; set; }

    }
}
