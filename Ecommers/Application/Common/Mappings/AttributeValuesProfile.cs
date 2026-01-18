using AutoMapper;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
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


            CreateMap<AttributeValuesCreateRequest, AttributeValuesD>()
                    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        }
    }
}
