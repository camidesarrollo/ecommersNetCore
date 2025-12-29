using AutoMapper;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Application.DTOs.Requests.Servicios;

namespace Ecommers.Application.Common.Mappings
{
    public class ServicioProfile : Profile
    {
        public ServicioProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<Servicios, ServiciosD>()
                .ReverseMap();

            // ✅ Mapeo de DTOs a Dominio
            CreateMap<ServiciosCreateRequest, ServiciosD>();

            CreateMap<ServiciosUpdateRequest, ServiciosD>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
