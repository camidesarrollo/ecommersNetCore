using AutoMapper;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<Products, ProductsD>()
                .ReverseMap();
        }
    }
}
