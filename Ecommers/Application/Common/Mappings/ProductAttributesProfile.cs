using AutoMapper;
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
        }
    }
}
