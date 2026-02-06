using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductImages
{
    public class ProductImagesCreateRequest : ProductImagesBaseRequest
    {

    }

    public class ProductImageVM
    {
        public int? Id { get; set; }

        public int SortOrder { get; set; }

        public bool IsPrimary { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
