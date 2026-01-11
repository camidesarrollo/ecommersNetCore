using AutoMapper;
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
        }
    }
}
