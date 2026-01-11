using AutoMapper;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductImagesProfile : Profile
    {
        public ProductImagesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<ProductImages, ProductImagesD>()
                .ReverseMap();
        }
    }
}
