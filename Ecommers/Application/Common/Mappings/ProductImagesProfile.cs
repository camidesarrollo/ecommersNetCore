using AutoMapper;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductImagesProfile : Profile
    {
        public ProductImagesProfile()
        {
            // Mapeo de dominio a infraestructura
            CreateMap<ProductImages, ProductImagesD>()
                .ReverseMap();

            // Mapeo de request a entidad de infraestructura
            CreateMap<ProductImagesCreateRequest, ProductImagesD>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<ProductImages, ProductImagesUpdateRequest>();
            CreateMap<ProductImagesUpdateRequest, ProductImages>();
        }
    }
}
