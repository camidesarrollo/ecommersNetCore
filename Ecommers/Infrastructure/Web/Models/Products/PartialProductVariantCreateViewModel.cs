using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class PartialProductVariantCreateViewModel : PartialBaseProductVariantViewModel
    {
        public required ProductVariantsCreateRequest ProductVariant { get; set; }

        public IEnumerable<ProductVariantImagesCreateRequest> ProductVariantImages { get; set; } = [];


    }

}
