using AutoMapper;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductPriceHistoryProfile : Profile
    {
        public ProductPriceHistoryProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<ProductPriceHistory, ProductPriceHistoryD>()
                .ReverseMap();
        }
    }
}
