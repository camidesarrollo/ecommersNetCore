using AutoMapper;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductVariantImagesProfile : Profile
    {
        public ProductVariantImagesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<ProductVariantImages, ProductVariantImagesD>()
                .ReverseMap();


            CreateMap<ProductVariantImagesCreateRequest, ProductVariantImagesD>();

            CreateMap<ProductVariantImages, ProductVariantImagesUpdateRequest>();
            CreateMap<ProductVariantImagesUpdateRequest, ProductVariantImages>();

            CreateMap<ProductVariantImagesCreateRequest, ProductVariantImagesUpdateRequest>();
            CreateMap<ProductVariantImagesUpdateRequest, ProductVariantImagesCreateRequest>();
        }
    }
}
