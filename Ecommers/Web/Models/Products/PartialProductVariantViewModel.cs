using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Web.Models.Products
{
    public class PartialProductVariantViewModel
    {
        public int Index { get; set; }

        public required ProductVariantsCreateRequest ProductVariant { get; set; }

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];

        public IEnumerable<AttributeValuesD> AtrributeValue { get; set; } = [];

        public IEnumerable<ProductVariantImagesCreateRequest> ProductVariantImages { get; set; } = [];


    }

}
