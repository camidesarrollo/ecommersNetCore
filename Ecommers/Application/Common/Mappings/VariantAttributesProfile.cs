using AutoMapper;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class VariantAttributesProfile : Profile
    {
        public VariantAttributesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<VariantAttributes, VariantAttributesD>()
                .ReverseMap();
        }
    }
}
