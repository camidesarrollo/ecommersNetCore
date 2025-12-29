using AutoMapper;
using Ecommers.Application.DTOs.Requests.MasterAttributes;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class MasterAttributesProfile : Profile
    {
        public MasterAttributesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<MasterAttributes, MasterAttributesD>()
                .ReverseMap();

            // ✅ Mapeo de DTOs a Dominio
            CreateMap<MasterAttributesCreateRequest, MasterAttributesD>();

            CreateMap<MasterAttributesUpdateRequest, MasterAttributesD>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
