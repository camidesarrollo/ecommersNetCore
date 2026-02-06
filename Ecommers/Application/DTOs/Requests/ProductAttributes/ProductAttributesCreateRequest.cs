namespace Ecommers.Application.DTOs.Requests.ProductAttributes
{
    public class ProductAttributesCreateRequest : ProductAttributesBaseRequest
    {


    }

    public class ProductAttributeVM
    {
        public int MasterAttributeId { get; set; }

        // string porque:
        // - text
        // - number
        // - checkbox
        // - select
        // - multiselect (CSV)
        public string? Value { get; set; }
    }
}
