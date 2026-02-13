using AutoMapper;
using Ecommers.Application.DTOs.Requests.Products;
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

            CreateMap<ProductsCreateRequest, ProductsD>();

            CreateMap<Products, ProductsUpdateRequest>();
            CreateMap<ProductsUpdateRequest, Products>();

            CreateMap<ProductsD, ProductsUpdateRequest>();
            CreateMap<ProductsUpdateRequest, ProductsD>();

        }
    }

   
}
