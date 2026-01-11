using AutoMapper;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class AttributeValuesProfile : Profile
    {
        public AttributeValuesProfile()
        {
            // ✅ Mapeo bidireccional entre Infraestructura y Dominio
            CreateMap<AttributeValues, AttributeValuesD>()
                .ReverseMap();

        }
    }
}
