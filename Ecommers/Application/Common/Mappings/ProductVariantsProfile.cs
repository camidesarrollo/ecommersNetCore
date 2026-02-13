using AutoMapper;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductVariantsProfile : Profile
    {
        public ProductVariantsProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<ProductVariants, ProductVariantsD>()
                .ReverseMap();

            CreateMap<ProductVariantsCreateRequest, ProductVariantsD>();

            CreateMap<ProductVariants, ProductVariantsUpdateRequest>();
            CreateMap<ProductVariantsUpdateRequest, ProductVariants>();


            CreateMap<ProductVariantsCreateRequest, ProductVariantsUpdateRequest>();
            CreateMap<ProductVariantsUpdateRequest, ProductVariantsCreateRequest>();

        }
    }
}
