using AutoMapper;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductAttributesProfile : Profile
    {
        public ProductAttributesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<ProductAttributes, ProductAttributesD>()
                .ReverseMap();

            CreateMap<ProductAttributesCreateRequest, ProductAttributesD>()
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
