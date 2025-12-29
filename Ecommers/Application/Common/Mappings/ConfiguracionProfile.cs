using AutoMapper;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ConfiguracionProfile : Profile
    {
        public ConfiguracionProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<Configuraciones, ConfiguracionesD>()
                .ReverseMap();

            // ✅ Mapeo de DTOs a Dominio
            CreateMap<ConfiguracionCreateRequest, ConfiguracionesD>();

            CreateMap<ConfiguracionUpdateRequest, ConfiguracionesD>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
