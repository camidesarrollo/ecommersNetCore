using AutoMapper;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<Categories, CategoriesD>()
                .ReverseMap();

            // ✅ Mapeo de DTOs a Dominio
            CreateMap<CategoriaCreateRequest, CategoriesD>();

            CreateMap<CategoriaUpdateRequest, CategoriesD>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcValue) => srcValue != null));
        }
    }
}
