using AutoMapper;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class BannersProfile : Profile
    {
        public BannersProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<Banners, BannersD>()
                .ReverseMap();

            // ✅ Mapeo de DTOs a Dominio
            CreateMap<BannersCreateRequest, BannersD>();

            CreateMap<BannersUpdateRequest, BannersD>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
