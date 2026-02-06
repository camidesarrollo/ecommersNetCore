namespace Ecommers.Application.DTOs.Requests.ProductVariantImages
{
    public class ProductVariantImagesCreateRequest : ProductVariantImagesBaseRequest
    {
       
    }

    public class ProductVariantImageVM
    {
        public int? Id { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
